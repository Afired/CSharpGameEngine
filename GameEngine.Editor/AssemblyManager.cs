using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using GameEngine.Editor.PropertyDrawers;

namespace GameEngine.Editor; 

public static class AssemblyManager {
    
    private static List<Assembly> _externalEditorAssemblies = new();
    private static WeakReference? _weak;
    private static WeakReference<EditorAssemblyLoadContext>? _weakLoadContext;
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
        _externalEditorAssemblies = new List<Assembly>();
        if(_weak is null || _weakLoadContext is null)
            return;
        
        //UNLOAD
        PropertyDrawer.UnloadLookUp();
        
        UnloadEditorAssemblies();
        
        const int MAX_GC_ATTEMPTS = 10;
        for(int i = 0; _weak.IsAlive && i < MAX_GC_ATTEMPTS; i++) {
            Console.Log($"GC Attempt ({i + 1}/{MAX_GC_ATTEMPTS})");
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        
        _weak = null;
        _weakLoadContext = null;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void UnloadEditorAssemblies() {
        if(_weakLoadContext is null)
            return;
        
        if(_weakLoadContext.TryGetTarget(out EditorAssemblyLoadContext? loadContext)) {
            foreach(Assembly assembly in loadContext.Assemblies) {
                Console.Log($"Unloading editor assembly from: {assembly.Location}");
            }
            loadContext.Unload();
        }
    }
    
    public static void LoadEditorAssemblies() {
        #if DEBUG
            string configName = "Debug";
        #else
            string configName = "Release";
        #endif
        string editorAssemblyDir = Path.GetDirectoryName(Assembly.GetAssembly(typeof(EditorApplication))!.Location)!;
        string pluginRelativePath = $"ExampleGame.Editor\\bin\\{configName}\\net6.0\\ExampleGame.Editor.dll";
        string pluginFullPath = Path.Combine(editorAssemblyDir, $"..\\..\\..\\..\\{pluginRelativePath}");
        // Assembly loadedAssembly = LoadEditorAssembly(@"ExampleGame.Editor\bin\Debug\net6.0\ExampleGame.Editor.dll");
        Assembly loadedAssembly = LoadEditorAssembly(pluginFullPath);
        _externalEditorAssemblies.Add(loadedAssembly);

        IsReloadingEditorAssemblies = false;
        
        //LOAD
        PropertyDrawer.LoadLookUp();
    }
    
    // private static Assembly LoadEditorAssemblies(string relativePath) {
    //     string editorAssemblyLocation = typeof(Program).Assembly.Location;
    //     // Navigate up to the solution root
    //     string root = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(editorAssemblyLocation)))))));
    //
    //     string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
    //     Console.Log($"Loading editor assembly from: {pluginLocation}");
    //     EditorAssemblyLoadContext loadContext = new(pluginLocation);
    //     return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
    // }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Assembly LoadEditorAssembly(string assemblyPath) {
        Console.Log($"Loading editor assembly from: {assemblyPath}");
        EditorAssemblyLoadContext loadContext = new(assemblyPath);
        Assembly assembly = loadContext.LoadFromAssemblyPath(assemblyPath);
        _weak = new WeakReference(loadContext);
        _weakLoadContext = new WeakReference<EditorAssemblyLoadContext>(loadContext);
        return assembly;
    }
    
    // private static IEnumerable<ICommand> CreateCommands(Assembly assembly) {
    //     int count = 0;
    //
    //     foreach (Type type in assembly.GetTypes())
    //     {
    //         if (typeof(ICommand).IsAssignableFrom(type))
    //         {
    //             ICommand result = Activator.CreateInstance(type) as ICommand;
    //             if (result != null)
    //             {
    //                 count++;
    //                 yield return result;
    //             }
    //         }
    //     }
    //     
    // }
    
}

public class EditorAssemblyLoadContext : AssemblyLoadContext {
    
    private AssemblyDependencyResolver _resolver;
    
    public EditorAssemblyLoadContext(string pluginPath) : base(true) {
        _resolver = new AssemblyDependencyResolver(pluginPath);
    }
    
    // protected override Assembly? Load(AssemblyName assemblyName) {
    //     string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
    //     if(assemblyPath != null) {
    //         return LoadFromAssemblyPath(assemblyPath);
    //     }
    //     return null;
    // }
    
    protected override Assembly? Load(AssemblyName assemblyName) {
        string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        
        if(assemblyPath != null) {
            //Using  MemoryStream to prevent attach dll to this .exe
            MemoryStream ms = new();
            
            using(FileStream fs = File.Open(assemblyPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                fs.CopyTo(ms);
            }
            
            ms.Position = 0;
            return LoadFromStream(ms);
        }
        
        return null;
    }
    
}