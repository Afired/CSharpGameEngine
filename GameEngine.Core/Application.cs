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
using GameEngine.Numerics;

namespace GameEngine.Core;

public delegate void OnFinishedInit(Application application);

public abstract class Application : IDisposable {
    
    public event OnFinishedInit? OnFinishedInit;
    public static Application? Instance { get; private set; }
    
    public bool IsRunning { get; protected set; }
    public Configuration Config { get; }
    public Renderer Renderer { get; private set; }
    public PhysicsEngine PhysicsEngine { get; private set; }
    public static readonly ConcurrentQueue<Action> TaskQueue = new();
    
    public readonly ExternalAssemblyLoadContextManager AssemblyLoadContextManager = new();
    
    public bool IsReloadingExternalAssemblies { get; private set; }

    private Vec2<float> __inGameResolution;
    public Vec2<float> InGameResolution {
        get => __inGameResolution;
        set {
            if(__inGameResolution != InGameResolution) {
                __inGameResolution = value;
                foreach(WeakReference<FrameBuffer> frameBufferRef in InGameFrameBuffers) {
                    if(frameBufferRef.TryGetTarget(out FrameBuffer? frameBuffer)) {
                        frameBuffer.Resize((uint) __inGameResolution.X, (uint) __inGameResolution.Y);
                    } 
                }
                InGameFrameBuffers.RemoveWhere(weakRef => !weakRef.TryGetTarget(out _));
            }
        }
    }
    public HashSet<WeakReference<FrameBuffer>> InGameFrameBuffers = new();
    
    public void RegisterReloadOfExternalAssemblies() => IsReloadingExternalAssemblies = true;
    
    protected Application(Configuration config) {
        Instance = this;
        Config = config;
        
        Console.Log("[0/3] Initializing...");
        
        PhysicsEngine = new PhysicsEngine();
        Console.LogSuccess("[1/3] Initialized physics engine");
        
        Renderer = new Renderer(this);
        Console.LogSuccess("[2/3] Initialized render engine");
        
        Console.LogSuccess("[3/3] Initialized engine");
        LoadExternalAssemblies();
    }
    
    public void InvokeFinishedInit() {
        OnFinishedInit?.Invoke(this);
    }
    
    protected void ExecuteQueuedTasks() {
        while(TaskQueue.TryDequeue(out Action? task)) {
            task.Invoke();
        }
    }
    
    public virtual void LoadExternalAssemblies() {
//        _ealcm.LoadExternalAssembly(EXTERNAL_ASSEMBLY_DLL, true);
        Serializer.LoadAssemblyIfNotLoadedAlready();
        // Hierarchy.SetRootNode(Serializer.Deserialize("Test"));
        
        AssemblyLoadContextManager.AddUnloadTask(() => {
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
        AssemblyLoadContextManager.Unload();
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

        process.OutputDataReceived += (_, dataArgs) => {
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

        process.ErrorDataReceived += (_, dataArgs) => {
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
                PhysicsEngine.DoStep(Config.FixedTimeStep);
                Hierarchy.PhysicsUpdate(Config.FixedTimeStep);
                physicsTimer.Restart();
            }
            
            Renderer.Render();
            
            // handle input
            Renderer.MainWindow.Glfw.PollEvents();
            
            Renderer.InputHandler.HandleMouseInput(Renderer.WindowHandle);
            
            if(Renderer.MainWindow.Glfw.WindowShouldClose(Renderer.WindowHandle))
                Terminate();
        }
        
    }
    
    public virtual void Terminate() {
        IsRunning = false;
        Console.Log("Is terminating...");
    }
    
    public virtual void Dispose() {
        AssemblyLoadContextManager.Dispose();
        Renderer.Dispose();
        PhysicsEngine.Dispose();
    }
    
}

public abstract class Application<T> : Application where T : Application<T> {
    
    public new static T Instance => (Application.Instance as T) ?? throw new Exception("wrong application type");
    
    protected Application(Configuration config) : base(config) { }
    
}

public readonly record struct DotnetBuildProperty(string Name, string Value);
