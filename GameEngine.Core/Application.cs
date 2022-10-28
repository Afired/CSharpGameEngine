using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using GameEngine.Core.Guard;
using GameEngine.Core.Physics;
using GameEngine.Core.Rendering;
using GameEngine.Core.SceneManagement;
using GameEngine.Core.Serialization;

namespace GameEngine.Core;

public abstract class Application : IDisposable {
    
    private static Application? _instance;
    public static Application Instance => _instance ?? throw new Exception();
    
    public bool IsRunning { get; protected set; }
    public Configuration Config { get; }
    public Renderer Renderer { get; private set; }
    public PhysicsEngine PhysicsEngine { get; private set; }
    public static readonly ConcurrentQueue<Action> TaskQueue = new();
    
    protected readonly ExternalAssemblyLoadContextManager _ealcm = new();
    public IEnumerable<Assembly> ExternalAssemblies => _ealcm.ExternalAssemblies;
    
    public static IEnumerable<Assembly> GetExternalAssembliesStatic => Instance._ealcm.ExternalAssemblies;
    
    public bool IsReloadingExternalAssemblies { get; private set; }
    public void RegisterReloadOfExternalAssemblies() => IsReloadingExternalAssemblies = true;
    
    protected Application(Configuration config) {
        _instance = this;
        Config = config;
    }
    
    protected void ExecuteQueuedTasks() {
        while(TaskQueue.TryDequeue(out Action? task)) {
            task.Invoke();
        }
    }
    
    public virtual void Initialize() {
        Console.Log("Initializing...");
        Console.Log("Initializing engine...");
        Console.LogSuccess("Initialized engine (1/3)");
        
        Console.Log("Initializing physics engine...");
        PhysicsEngine = new PhysicsEngine();
        PhysicsEngine.Initialize();
        Console.LogSuccess("Initialized physics engine (2/3)");
        
        Console.Log("Initializing render engine...");
        Renderer = new Renderer();
        Renderer.Initialize();
        Console.LogSuccess("Initialized render engine (3/3)");
        
        Console.LogSuccess("Initialization complete");
        
        // LoadExternalAssemblies();
    }
    
    public virtual void LoadExternalAssemblies() {
//        _ealcm.LoadExternalAssembly(EXTERNAL_ASSEMBLY_DLL, true);
        Serializer.LoadAssemblyIfNotLoadedAlready();
        // Hierarchy.SetRootNode(Serializer.Deserialize("Test"));
        
        _ealcm.AddUnloadTask(() => {
            Hierarchy.SaveCurrentRootNode();
            Hierarchy.Clear();
            Renderer.SetActiveCamera(null);
            Serializer.UnloadResources();
            ClearTypeDescriptorCache();
            PhysicsEngine.World = null;
            return true;
        });
    }
    
    public static void ClearTypeDescriptorCache() {
        var typeConverterAssembly = typeof(TypeConverter).Assembly;
        
        var reflectTypeDescriptionProviderType = typeConverterAssembly.GetType("System.ComponentModel.ReflectTypeDescriptionProvider");
        var reflectTypeDescriptorProviderTable = reflectTypeDescriptionProviderType.GetField("s_attributeCache", BindingFlags.Static | BindingFlags.NonPublic);
        var attributeCacheTable = (Hashtable) reflectTypeDescriptorProviderTable.GetValue(null);
        attributeCacheTable?.Clear();
        
        var reflectTypeDescriptorType = typeConverterAssembly.GetType("System.ComponentModel.TypeDescriptor");
        var reflectTypeDescriptorTypeTable = reflectTypeDescriptorType.GetField("s_defaultProviders", BindingFlags.Static | BindingFlags.NonPublic);
        var defaultProvidersTable = (Hashtable) reflectTypeDescriptorTypeTable.GetValue(null);
        defaultProvidersTable?.Clear();
        
        var providerTableWeakTable = (Hashtable) reflectTypeDescriptorType.GetField("s_providerTable", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
        providerTableWeakTable?.Clear();
    }
    
    protected virtual void ReloadExternalAssemblies() {
        _ealcm.Unload();
        CompileExternalAssemblies();
        LoadExternalAssemblies();
        IsReloadingExternalAssemblies = false;
    }
    
    protected virtual void CompileExternalAssemblies() {
//        CompileExternalAssembly(EXTERNAL_ASSEMBLY_PROJ_DIR);
    }
    
    protected static bool CompileExternalAssembly(string dir, DotnetBuildProperty[] dotnetBuildProperties) {
        Console.Log("");
        Console.Log($"Compiling external assembly in {dir}...");
        Console.Log("**********************************************************************************************************************");

        string propertiesAsArgument = dotnetBuildProperties.Length > 0 ? " /p:" : string.Empty;
        foreach(DotnetBuildProperty property in dotnetBuildProperties) {
            propertiesAsArgument += property.Name;
            propertiesAsArgument += "=";
            propertiesAsArgument += property.Value;
            propertiesAsArgument += ";";
        }
        if(propertiesAsArgument.Length > 0)
            propertiesAsArgument = propertiesAsArgument[..^1];
        
        ProcessStartInfo processInfo = new() {
            WorkingDirectory = dir,
            FileName = "cmd.exe",
            Arguments = $"/c dotnet build {propertiesAsArgument}",
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
        };
        
        Process process = Process.Start(processInfo) ?? throw new Exception();

        process.OutputDataReceived += (sender, dataArgs) => {
            string? data = dataArgs.Data;
            
            if(data is null)
                return;
            
            if(data.Contains("Warning", StringComparison.CurrentCultureIgnoreCase))
                Console.LogWarning(data);
            else if(data.Contains("Error", StringComparison.CurrentCultureIgnoreCase))
                Console.LogError(data);
            else
                Console.Log(data);
        };
        process.BeginOutputReadLine();

        process.ErrorDataReceived += (sender, dataArgs) => {
            if(dataArgs.Data is not null)
                Console.LogError(dataArgs.Data);
        };
        process.BeginErrorReadLine();
        
        process.WaitForExit();
        
        int exitCode = process.ExitCode;
        process.Close();
        
        Console.Log($"Exit Code: '{exitCode}'");
        Console.Log("**********************************************************************************************************************");
        
        if(exitCode != 0) {
            Console.LogError("Failed to compile external assembly!");
            Console.Log("");
            return false;
        }
        
        Console.LogSuccess("Successfully compiled external assembly!");
        Console.Log("");
        return true;
    }
    
    public virtual void Run() {
        if(IsRunning)
            throw new Exception("Application is already running!");
        IsRunning = true;
        // starts loops on all threads
        Throw.If(!Renderer.IsInit, "rendering engine has not yet been initialized or initialization has not been fully completed");
        Throw.If(!PhysicsEngine.IsInit, "physics engine has not yet been initialized or initialization has not been fully completed");
        
        Loop();
    }
    
    protected virtual unsafe void Loop() {
        Stopwatch updateTimer = new();
        Stopwatch physicsTimer = new();
        updateTimer.Start();
        physicsTimer.Start();
        
        while(IsRunning) {
            
            float updateTime = (float) updateTimer.Elapsed.TotalSeconds;
            if(Config.TargetFrameRate > 0) {
                TimeSpan timeOut = TimeSpan.FromSeconds(1 / Config.TargetFrameRate - updateTime);
                if(timeOut.TotalSeconds > 0) {
                    Thread.Sleep(timeOut);
                    updateTime = (float) updateTimer.Elapsed.TotalSeconds;
                }
            }
            Time.TotalTimeElapsed += (float) updateTimer.Elapsed.TotalSeconds;
            updateTimer.Restart();
            
            Hierarchy.Awake();
            Hierarchy.Update(updateTime);
            
            Renderer.InputHandler.ResetMouseDelta(Renderer.WindowHandle);
            
            float physicsTime = (float) physicsTimer.Elapsed.TotalSeconds;
            if(physicsTime > Config.FixedTimeStep) {
                Hierarchy.PrePhysicsUpdate();
                PhysicsEngine.DoStep();
                Hierarchy.PhysicsUpdate(Config.FixedTimeStep);
                physicsTimer.Restart();
            }
            
            Renderer.Render();
            
            // handle input
            Renderer.Glfw.PollEvents();
            
            Renderer.InputHandler.HandleMouseInput(Renderer.WindowHandle);
            
            if(Renderer.Glfw.WindowShouldClose(Renderer.WindowHandle))
                Terminate();
        }
        
    }
    
    public virtual void Terminate() {
        IsRunning = false;
        Console.Log("Is terminating...");
    }
    
    public virtual void Dispose() {
        _ealcm.Dispose();
        Renderer.Dispose();
        PhysicsEngine.Dispose();
    }
    
}

public abstract class Application<T> : Application where T : Application<T> {
    
    public new static T? Instance { get; private set; }
    
    protected Application(Configuration config) : base(config) {
        Instance = (this as T)!;
    }
    
}

public readonly record struct DotnetBuildProperty(string Name, string Value);
