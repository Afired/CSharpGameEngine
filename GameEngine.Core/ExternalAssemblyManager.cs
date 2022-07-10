using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using GameEngine.Core.Physics;
using GameEngine.Core.Rendering;
using GameEngine.Core.SceneManagement;
using GameEngine.Core.Serialization;

namespace GameEngine.Core; 

public delegate void OnClearGameResources();

public static class ExternalAssemblyManager {

    private const string EXTERNAL_ASSEMBLY_PROJ_DIR = @"D:\Dev\C#\CSharpGameEngine\ExampleProject\src\ExampleGame";
    private const string EXTERNAL_ASSEMBLY_DLL = @"D:\Dev\C#\CSharpGameEngine\ExampleProject\bin\Debug\net6.0\ExampleGame.dll";
    
    private static readonly List<Assembly> _externalAssemblies = new();
    private static List<WeakReference> _oldExternalAssembliesRefs = new();
    private static ExternalAssemblyLoadContext? _externalAssemblyLoadContext;
    public static bool IsReloadingExternalAssemblies { get; private set; }

    public static void RegisterReloadOfExternalAssemblies() => IsReloadingExternalAssemblies = true;
    
    public static IEnumerable<Assembly> Assemblies() {
        yield return Assembly.GetAssembly(typeof(Application<>))!;
        foreach(Assembly editorAssembly in _externalAssemblies) {
            yield return editorAssembly;
        }
    }
    
    public static void ReloadExternalAssemblies() {
        ClearGameResources();
        if(TryToUnloadExternalAssemblies()) {
            if(!CompileExternalAssemblies())
                Console.LogWarning("Reloading latest external assemblies...");
            LoadExternalAssemblies();
        } else {
            RecoverLatestExternalAssemblies();
        }
        IsReloadingExternalAssemblies = false;
        GenerateGameResources();
    }
    
    private static bool CompileExternalAssemblies() {
        Console.Log("Compiling external assemblies...");
        Console.Log("**********************************************************************************************************************");
        
        ProcessStartInfo processInfo = new() {
            WorkingDirectory = EXTERNAL_ASSEMBLY_PROJ_DIR,
            FileName = "cmd.exe",
            Arguments = "/c dotnet build",
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
            Console.LogError("Failed to compile external assemblies!");
            return false;
        }
        
        Console.LogSuccess("Successfully compiled external assemblies!");
        return true;
    }
    
    private static void RecoverLatestExternalAssemblies() {
        Console.Log($"Trying to recover latest external assemblies...");
        Console.LogWarning($"Note that after a recovery, the application is unable to reload external assemblies! Exit the application gracefully and restart it!");
        _externalAssemblies.Clear();
        foreach(WeakReference externalAssemblyRef in _oldExternalAssembliesRefs) {
            if(externalAssemblyRef.IsAlive) {
                Assembly assembly = (externalAssemblyRef.Target as Assembly)!;
                Console.Log($"Restoring latest external assembly: '{assembly.Location}'...");
                _externalAssemblies.Add(assembly);
            } else {
                Console.LogWarning($"Failed to recover latest external assembly!");
            }
        }
    }
    
    public static void GenerateGameResources() {
        Console.Log($"Generating game resources...");
        Hierarchy.SetRootNode(Serializer.Deserialize("Test"));                          // <-- probably causes some caching of types, resulting in assembly not being able to be unloaded
        Console.LogSuccess($"Successfully generated game resources!");                  // https://stackoverflow.com/questions/33557737/does-json-net-cache-types-serialization-information
    }                                                                                           // https://github.com/dotnet/runtime/issues/30656
                                                                                                // https://github.com/dotnet/runtime/issues/13283
    public static event OnClearGameResources? OnClearGameResources;                             // even System.Text.Json https://github.com/dotnet/runtime/issues/65323
    public static void ClearGameResources() {
        Console.Log($"Clearing game resources...");
        Hierarchy.SaveCurrentRootNode();                                                        // <-- probably causes some caching of types, resulting in assembly not being able to be unloaded
        Hierarchy.Clear();                                                                      // https://stackoverflow.com/questions/33557737/does-json-net-cache-types-serialization-information
        Renderer.SetActiveCamera(null);                                                         // https://github.com/dotnet/runtime/issues/30656
        OnClearGameResources?.Invoke();                                                         // https://github.com/dotnet/runtime/issues/13283
        Serializer.UnloadResources();                                                                // even System.Text.Json https://github.com/dotnet/runtime/issues/65323
        ClearTypeDescriptorCache();
        PhysicsEngine.World = null;
        Console.LogSuccess($"Successfully cleared game resources!");
    }
    
    public static void ClearTypeDescriptorCache() {
        var typeConverterAssembly = typeof(TypeConverter).Assembly;
        
        var reflectTypeDescriptionProviderType = typeConverterAssembly.GetType("System.ComponentModel.ReflectTypeDescriptionProvider");
        var reflectTypeDescriptorProviderTable = reflectTypeDescriptionProviderType.GetField("s_attributeCache", BindingFlags.Static | BindingFlags.NonPublic);
        var attributeCacheTable = (Hashtable)reflectTypeDescriptorProviderTable.GetValue(null);
        attributeCacheTable?.Clear();
        
        var reflectTypeDescriptorType = typeConverterAssembly.GetType("System.ComponentModel.TypeDescriptor");
        var reflectTypeDescriptorTypeTable = reflectTypeDescriptorType.GetField("s_defaultProviders", BindingFlags.Static | BindingFlags.NonPublic);
        var defaultProvidersTable = (Hashtable)reflectTypeDescriptorTypeTable.GetValue(null);
        defaultProvidersTable?.Clear();
        
        var providerTableWeakTable = (Hashtable)reflectTypeDescriptorType.GetField("s_providerTable", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
        providerTableWeakTable?.Clear();
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool TryToUnloadExternalAssemblies() {
        if(_externalAssemblyLoadContext is null) {
            Console.Log($"There are no external assemblies loaded!");
            Console.LogSuccess($"Successfully unloaded external assemblies!");
            return true;
        }
        
        UnloadExternalAssemblies(out WeakReference externalAssemblyLoadContextRef, out _oldExternalAssembliesRefs);
        
        const int MAX_GC_ATTEMPTS = 10;
        for(int i = 0; externalAssemblyLoadContextRef.IsAlive; i++) {
            if(i >= MAX_GC_ATTEMPTS) {
                Console.LogError($"Failed to unload external assemblies!");
                _externalAssemblyLoadContext = (externalAssemblyLoadContextRef.Target as ExternalAssemblyLoadContext)!;
                return false;
            }
            Console.Log($"GC Attempt ({i + 1}/{MAX_GC_ATTEMPTS})...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        
        Console.LogSuccess($"Successfully unloaded external assemblies!");
        return true;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void UnloadExternalAssemblies(out WeakReference editorAssemblyLoadContextRef, out List<WeakReference> externalAssemblyRefs) {
        externalAssemblyRefs = new List<WeakReference>();
        foreach(Assembly assembly in _externalAssemblies) {
            externalAssemblyRefs.Add(new WeakReference(assembly));
        }
        _externalAssemblies.Clear();
        
        foreach(Assembly assembly in _externalAssemblyLoadContext.Assemblies) {
            Console.Log($"Unloading external assembly from: '{assembly.Location}'...");
        }
        
        //crashes when unloading fails, editor recovers and then user tries to unload again
        _externalAssemblyLoadContext.Unload();
        
        editorAssemblyLoadContextRef = new WeakReference(_externalAssemblyLoadContext);
        _externalAssemblyLoadContext = null;
    }
    
    public static void LoadExternalAssemblies() {
        Assembly loadedAssembly = LoadExternalAssembly(EXTERNAL_ASSEMBLY_DLL);
        _externalAssemblies.Add(loadedAssembly);
        
        Console.LogSuccess("Successfully loaded external assemblies!");
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Assembly LoadExternalAssembly(string assemblyPath) {
        Console.Log($"Loading external assembly from: '{assemblyPath}'...");
        _externalAssemblyLoadContext = new(assemblyPath);
        Assembly assembly = _externalAssemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
        
        return assembly;
    }
    
}


public class ExternalAssemblyLoadContext : AssemblyLoadContext {
    
    private readonly AssemblyDependencyResolver _resolver;
    
    public ExternalAssemblyLoadContext(string pluginPath) : base(true) {
        _resolver = new AssemblyDependencyResolver(pluginPath);
    }
    
    protected override Assembly? Load(AssemblyName assemblyName) {
        string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        if(assemblyPath != null) {
            return LoadFromAssemblyPath(assemblyPath);
        }
        return null;
    }
    
}





public class ExternalAssemblyLoadContextManager {
    
    private ExternalAssemblyLoadContext1? _externalAssemblyLoadContext;
    private List<(WeakReference lifetimeDependency, MulticastDelegate @delegate)> _unloadLifetimeDelegates = new();
    
    public IEnumerable<Assembly> ExternalAssemblies {
        get {
            if(_externalAssemblyLoadContext is null)
                yield break;
            foreach(Assembly assembly in _externalAssemblyLoadContext.Assemblies) {
                yield return assembly;
            }
        }
    }
    
    public void LoadExternalAssembly(string assemblyPath, bool isDependency) {
        _externalAssemblyLoadContext ??= new ExternalAssemblyLoadContext1();
        _externalAssemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
        if(isDependency)
            _externalAssemblyLoadContext.AddDependency(assemblyPath);
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Unload() {
        if(_externalAssemblyLoadContext is null)
            return;
        
        InvokeUnloadDelegate();
        
        UnloadInternal(out WeakReference externalAssemblyLoadContextRef);
        
        const int MAX_GC_ATTEMPTS = 10;
        for(int i = 0; externalAssemblyLoadContextRef.IsAlive; i++) {
            if(i >= MAX_GC_ATTEMPTS) {
                Console.LogError($"Failed to unload external assemblies!");
                _externalAssemblyLoadContext = externalAssemblyLoadContextRef.Target as ExternalAssemblyLoadContext1;
                return;
            }
            Console.Log($"GC Attempt ({i + 1}/{MAX_GC_ATTEMPTS})...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        
        Console.LogSuccess($"Successfully unloaded external assemblies!");
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void UnloadInternal(out WeakReference externalAssemblyLoadContextRef) {
        foreach(Assembly assembly in ExternalAssemblies) {
            Console.Log($"Unloading external assembly from: '{assembly.Location}'...");
        }
        
        // crashes after recovery and attempted unloading for the second time
        _externalAssemblyLoadContext.Unload();
        
        externalAssemblyLoadContextRef = new WeakReference(_externalAssemblyLoadContext);
        _externalAssemblyLoadContext = null;
    }
    
    public void AddUnloadTask<T>(T lifetimeDependency, Func<T, bool> @delegate) {
        _unloadLifetimeDelegates.Add((new WeakReference(lifetimeDependency), @delegate));
    }
    
    private void InvokeUnloadDelegate() {
        foreach((WeakReference lifetimeDependency, MulticastDelegate @delegate) in _unloadLifetimeDelegates) {
            if(!lifetimeDependency.IsAlive)
                continue;
            
            bool result = (bool) @delegate.DynamicInvoke(new object?[] { lifetimeDependency.Target })!;
            if(!result)
                Console.LogError("some unload delegate returned with failure");
        }
        
        _unloadLifetimeDelegates = new();
    }
    
    private class ExternalAssemblyLoadContext1 : AssemblyLoadContext {
        
        private readonly List<AssemblyDependencyResolver> _assemblyDependencyResolvers;
        
        public ExternalAssemblyLoadContext1() : base(true) {
            _assemblyDependencyResolvers = new List<AssemblyDependencyResolver>();
        }
        
        public void AddDependency(string assemblyPath) {
            _assemblyDependencyResolvers.Add(new AssemblyDependencyResolver(assemblyPath));
        }
        
        protected override Assembly? Load(AssemblyName assemblyName) {
            foreach(AssemblyDependencyResolver assemblyDependencyResolver in _assemblyDependencyResolvers) {
                if(assemblyDependencyResolver.ResolveAssemblyToPath(assemblyName) is {} resolvedAssemblyPath)
                    return LoadFromAssemblyPath(resolvedAssemblyPath);
            }
            return null;
        }
        
    }
    
}
