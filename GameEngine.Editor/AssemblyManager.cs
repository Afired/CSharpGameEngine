using System.Reflection;
using System.Runtime.Loader;

namespace GameEngine.Editor; 

public static class AssemblyManager {
    
    private static List<Assembly> _externalEditorAssemblies = new();
    
    public static IEnumerable<Assembly> EditorAssemblies() {
        yield return Assembly.GetAssembly(typeof(EditorApplication))!;
        foreach(Assembly editorAssembly in _externalEditorAssemblies) {
            yield return editorAssembly;
        }
    }
    
    public static void ReloadEditorAssemblies() {
        Assembly loadedAssembly = LoadEditorAssemblies(@"ExampleGame.Editor\bin\Debug\net6.0\ExampleGame.Editor.dll");
        _externalEditorAssemblies.Add(loadedAssembly);
    }
    
    private static Assembly LoadEditorAssemblies(string relativePath) {
        string editorAssemblyLocation = typeof(Program).Assembly.Location;
        // Navigate up to the solution root
        string root = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(editorAssemblyLocation)))))));

        string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
        Console.Log($"Loading editor assembly from: {pluginLocation}");
        EditorAssemblyLoadContext loadContext = new(pluginLocation);
        return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
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
    
    public EditorAssemblyLoadContext(string pluginPath) {
        _resolver = new AssemblyDependencyResolver(pluginPath);
    }
    
    protected override Assembly Load(AssemblyName assemblyName) {
        string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        if(assemblyPath != null) {
            return LoadFromAssemblyPath(assemblyPath);
        }
        
        return null;
    }
    
    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName) {
        string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (libraryPath != null) {
            return LoadUnmanagedDllFromPath(libraryPath);
        }
        
        return IntPtr.Zero;
    }
    
}