using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using GameEngine.Editor.PropertyDrawers;

namespace GameEngine.Editor; 

public static class AssemblyManager {
    
    private static readonly List<Assembly> _externalEditorAssemblies = new();
    private static List<WeakReference> _oldExternalEditorAssembliesRefs = new();
    private static EditorAssemblyLoadContext? _editorAssemblyLoadContext;
    public static bool IsReloadingEditorAssemblies { get; private set; }

    public static void RegisterReloadOfEditorAssemblies() => IsReloadingEditorAssemblies = true;
    
    public static IEnumerable<Assembly> EditorAssemblies() {
        yield return Assembly.GetAssembly(typeof(EditorApplication))!;
        foreach(Assembly editorAssembly in _externalEditorAssemblies) {
            yield return editorAssembly;
        }
    }
    
    public static void ReloadEditorAssemblies() {
        ClearEditorResources();
        if(TryToUnloadEditorAssemblies()) {
            LoadEditorAssemblies();
        } else {
            Console.Log($"Trying to recover latest external editor assemblies...");
            Console.LogWarning($"Note that after a recovery, the editor is unable to reload assemblies! Exit the editor gracefully and restart it!");
            _externalEditorAssemblies.Clear();
            foreach(WeakReference externalAssemblyRef in _oldExternalEditorAssembliesRefs) {
                if(externalAssemblyRef.IsAlive) {
                    Assembly assembly = (externalAssemblyRef.Target as Assembly)!;
                    Console.Log($"Restoring old editor assembly: '{assembly.Location}'...");
                    _externalEditorAssemblies.Add(assembly);
                } else {
                    Console.LogWarning($"Failed to recover old editor assembly");
                }
            }
        }
        IsReloadingEditorAssemblies = false;
        GenerateEditorResources();
    }
    
    public static void GenerateEditorResources() {
        Console.Log($"Generating editor resources...");
        PropertyDrawer.GenerateLookUp();
        Console.LogSuccess($"Successfully generated editor resources!");
    }
    
    public static void ClearEditorResources() {
        Console.Log($"Clearing editor resources...");
        PropertyDrawer.ClearLookUp();
        Console.LogSuccess($"Successfully cleared editor resources!");
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool TryToUnloadEditorAssemblies() {
        if(_editorAssemblyLoadContext is null) {
            Console.Log($"There are no external editor assemblies loaded!");
            Console.LogSuccess($"Successfully unloaded external editor assemblies!");
            return true;
        }
        
        UnloadEditorAssemblies(out WeakReference editorAssemblyLoadContextRef, out _oldExternalEditorAssembliesRefs);
        
        const int MAX_GC_ATTEMPTS = 10;
        for(int i = 0; editorAssemblyLoadContextRef.IsAlive; i++) {
            if(i >= MAX_GC_ATTEMPTS) {
                Console.LogError($"Failed to unload external editor assemblies!");
                _editorAssemblyLoadContext = (editorAssemblyLoadContextRef.Target as EditorAssemblyLoadContext)!;
                return false;
            }
            Console.Log($"GC Attempt ({i + 1}/{MAX_GC_ATTEMPTS})...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        
        Console.LogSuccess($"Successfully unloaded external editor assemblies!");
        return true;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void UnloadEditorAssemblies(out WeakReference editorAssemblyLoadContextRef, out List<WeakReference> externalAssemblyRefs) {
        externalAssemblyRefs = new List<WeakReference>();
        foreach(Assembly assembly in _externalEditorAssemblies) {
            externalAssemblyRefs.Add(new WeakReference(assembly));
        }
        _externalEditorAssemblies.Clear();
        
        foreach(Assembly assembly in _editorAssemblyLoadContext.Assemblies) {
            Console.Log($"Unloading external editor assembly from: '{assembly.Location}'...");
        }
        
        //crashes when unloading fails, editor recovers and then user tries to unload
        _editorAssemblyLoadContext.Unload();
        
        editorAssemblyLoadContextRef = new WeakReference(_editorAssemblyLoadContext);
        _editorAssemblyLoadContext = null;
    }
    
    public static void LoadEditorAssemblies() {
        #if DEBUG
            string configName = "Debug";
        #else
            string configName = "Release";
        #endif
        string editorAssemblyDir = Path.GetDirectoryName(Assembly.GetAssembly(typeof(EditorApplication))!.Location)!;
        string projectDir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(editorAssemblyDir))))!;
        string pluginRelativePath = $"ExampleGame.Editor\\bin\\{configName}\\net6.0\\ExampleGame.Editor.dll";
        string pluginFullPath = Path.Combine(projectDir, pluginRelativePath);
        // Assembly loadedAssembly = LoadEditorAssembly(@"ExampleGame.Editor\bin\Debug\net6.0\ExampleGame.Editor.dll");
        Assembly loadedAssembly = LoadEditorAssembly(pluginFullPath);
        _externalEditorAssemblies.Add(loadedAssembly);
        
        Console.LogSuccess("Successfully loaded external editor assemblies!");
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Assembly LoadEditorAssembly(string assemblyPath) {
        Console.Log($"Loading external editor assembly from: '{assemblyPath}'...");
        _editorAssemblyLoadContext = new(assemblyPath);
        Assembly assembly = _editorAssemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
        
        return assembly;
    }
    
}

public class EditorAssemblyLoadContext : AssemblyLoadContext {
    
    private AssemblyDependencyResolver _resolver;
    
    public EditorAssemblyLoadContext(string pluginPath) : base(true) {
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
