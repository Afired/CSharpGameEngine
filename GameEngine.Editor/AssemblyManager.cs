using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using GameEngine.Editor.PropertyDrawers;

namespace GameEngine.Editor; 

public static class AssemblyManager {
    
    private static readonly List<Assembly> _externalEditorAssemblies = new();
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
        TryToUnloadEditorAssemblies();
        LoadEditorAssemblies();
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void TryToUnloadEditorAssemblies() {
        if(_editorAssemblyLoadContext is null)
            return;
        
        //UNLOAD
        _externalEditorAssemblies.Clear(); //= new List<Assembly>();
        PropertyDrawer.UnloadLookUp();
        
        UnloadEditorAssemblies(out WeakReference weak);
        
        const int MAX_GC_ATTEMPTS = 10;
        for(int i = 0; weak.IsAlive && i < MAX_GC_ATTEMPTS; i++) {
            Console.Log($"GC Attempt ({i + 1}/{MAX_GC_ATTEMPTS})");
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void UnloadEditorAssemblies(out WeakReference weak) {
        foreach(Assembly assembly in _editorAssemblyLoadContext.Assemblies) {
            Console.Log($"Unloading editor assembly from: {assembly.Location}");
        }
        _editorAssemblyLoadContext.Unload();
        weak = new WeakReference(_editorAssemblyLoadContext);
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

        IsReloadingEditorAssemblies = false;
        
        //LOAD
        PropertyDrawer.LoadLookUp();
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Assembly LoadEditorAssembly(string assemblyPath) {
        Console.Log($"Loading editor assembly from: {assemblyPath}");
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
