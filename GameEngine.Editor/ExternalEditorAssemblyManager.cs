using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using GameEngine.Core;
using GameEngine.Editor.PropertyDrawers;

namespace GameEngine.Editor; 

public static class ExternalEditorAssemblyManager {

    // private const string EXTERNAL_EDITOR_ASSEMBLY_PROJ_DIR = @"D:\Dev\C#\CSharpGameEngine\ExampleProject\src\ExampleGame.Editor";
    // private const string EXTERNAL_EDITOR_ASSEMBLY_DLL = @"D:\Dev\C#\CSharpGameEngine\ExampleProject\bin\Debug\net6.0\ExampleGame.Editor.dll";
    //
    // private static readonly List<Assembly> _externalEditorAssemblies = new();
    // private static List<WeakReference> _oldExternalEditorAssembliesRefs = new();
    // private static ExternalEditorAssemblyLoadContext? _editorAssemblyLoadContext;
    // public static bool IsReloadingEditorAssemblies { get; private set; }
    //
    // public static void RegisterReloadOfEditorAssemblies() => IsReloadingEditorAssemblies = true;
    
    // public static IEnumerable<Assembly> EditorAssemblies() {
    //     yield return Assembly.GetAssembly(typeof(EditorApplication))!;
    //     foreach(Assembly editorAssembly in _externalEditorAssemblies) {
    //         yield return editorAssembly;
    //     }
    // }
    
    // public static void ReloadEditorAssemblies() {
    //     ClearEditorResources();
    //     if(TryToUnloadEditorAssemblies()) {
    //         if(!CompileExternalEditorAssemblies())
    //             Console.LogWarning("Reloading latest editor assemblies...");
    //         LoadEditorAssemblies();
    //     } else {
    //         RecoverLatestEditorAssemblies();
    //     }
    //     IsReloadingEditorAssemblies = false;
    //     GenerateEditorResources();
    // }

    // private static bool CompileExternalEditorAssemblies() {
    //     Console.Log("Compiling external editor assemblies...");
    //     Console.Log("**********************************************************************************************************************");
    //     
    //     ProcessStartInfo processInfo = new() {
    //         WorkingDirectory = EXTERNAL_EDITOR_ASSEMBLY_PROJ_DIR,
    //         FileName = "cmd.exe",
    //         Arguments = "/c dotnet build",
    //         CreateNoWindow = true,
    //         UseShellExecute = false,
    //         RedirectStandardError = true,
    //         RedirectStandardOutput = true,
    //     };
    //     
    //     Process process = Process.Start(processInfo) ?? throw new Exception();
    //
    //     process.OutputDataReceived += (sender, dataArgs) => {
    //         string? data = dataArgs.Data;
    //         
    //         if(data is null)
    //             return;
    //         
    //         if(data.Contains("Warning", StringComparison.CurrentCultureIgnoreCase))
    //             Console.LogWarning(data);
    //         else if(data.Contains("Error", StringComparison.CurrentCultureIgnoreCase))
    //             Console.LogError(data);
    //         else
    //             Console.Log(data);
    //     };
    //     process.BeginOutputReadLine();
    //
    //     process.ErrorDataReceived += (sender, dataArgs) => {
    //         if(dataArgs.Data is not null)
    //             Console.LogError(dataArgs.Data);
    //     };
    //     process.BeginErrorReadLine();
    //     
    //     process.WaitForExit();
    //     
    //     int exitCode = process.ExitCode;
    //     process.Close();
    //     
    //     Console.Log($"Exit Code: '{exitCode}'");
    //     Console.Log("**********************************************************************************************************************");
    //     
    //     if(exitCode != 0) {
    //         Console.LogError("Failed to compile external editor assemblies!");
    //         return false;
    //     }
    //     
    //     Console.LogSuccess("Successfully compiled external editor assemblies!");
    //     return true;
    // }
    
    // private static void RecoverLatestEditorAssemblies() {
    //     Console.Log($"Trying to recover latest external editor assemblies...");
    //     Console.LogWarning($"Note that after a recovery, the editor is unable to reload assemblies! Exit the editor gracefully and restart it!");
    //     _externalEditorAssemblies.Clear();
    //     foreach(WeakReference externalAssemblyRef in _oldExternalEditorAssembliesRefs) {
    //         if(externalAssemblyRef.IsAlive) {
    //             Assembly assembly = (externalAssemblyRef.Target as Assembly)!;
    //             Console.Log($"Restoring old editor assembly: '{assembly.Location}'...");
    //             _externalEditorAssemblies.Add(assembly);
    //         } else {
    //             Console.LogWarning($"Failed to recover old editor assembly");
    //         }
    //     }
    // }
    //
    // public static void GenerateEditorResources() {
    //     Console.Log($"Generating editor resources...");
    //     PropertyDrawer.GenerateLookUp();
    //     Console.LogSuccess($"Successfully generated editor resources!");
    // }
    //
    // public static void ClearEditorResources() {
    //     Console.Log($"Clearing editor resources...");
    //     PropertyDrawer.ClearLookUp();
    //     Console.LogSuccess($"Successfully cleared editor resources!");
    // }
    //
    // [MethodImpl(MethodImplOptions.NoInlining)]
    // public static bool TryToUnloadEditorAssemblies() {
    //     if(_editorAssemblyLoadContext is null) {
    //         Console.Log($"There are no external editor assemblies loaded!");
    //         Console.LogSuccess($"Successfully unloaded external editor assemblies!");
    //         return true;
    //     }
    //     
    //     UnloadEditorAssemblies(out WeakReference editorAssemblyLoadContextRef, out _oldExternalEditorAssembliesRefs);
    //     
    //     const int MAX_GC_ATTEMPTS = 10;
    //     for(int i = 0; editorAssemblyLoadContextRef.IsAlive; i++) {
    //         if(i >= MAX_GC_ATTEMPTS) {
    //             Console.LogError($"Failed to unload external editor assemblies!");
    //             _editorAssemblyLoadContext = (editorAssemblyLoadContextRef.Target as ExternalEditorAssemblyLoadContext)!;
    //             return false;
    //         }
    //         Console.Log($"GC Attempt ({i + 1}/{MAX_GC_ATTEMPTS})...");
    //         GC.Collect();
    //         GC.WaitForPendingFinalizers();
    //     }
    //     
    //     Console.LogSuccess($"Successfully unloaded external editor assemblies!");
    //     return true;
    // }
    //
    // [MethodImpl(MethodImplOptions.NoInlining)]
    // private static void UnloadEditorAssemblies(out WeakReference editorAssemblyLoadContextRef, out List<WeakReference> externalAssemblyRefs) {
    //     externalAssemblyRefs = new List<WeakReference>();
    //     foreach(Assembly assembly in _externalEditorAssemblies) {
    //         externalAssemblyRefs.Add(new WeakReference(assembly));
    //     }
    //     _externalEditorAssemblies.Clear();
    //     
    //     foreach(Assembly assembly in _editorAssemblyLoadContext.Assemblies) {
    //         Console.Log($"Unloading external editor assembly from: '{assembly.Location}'...");
    //     }
    //     
    //     //crashes when unloading fails, editor recovers and then user tries to unload again
    //     _editorAssemblyLoadContext.Unload();
    //     
    //     editorAssemblyLoadContextRef = new WeakReference(_editorAssemblyLoadContext);
    //     _editorAssemblyLoadContext = null;
    // }
    //
    // public static void LoadEditorAssemblies() {
    //     Assembly loadedAssembly = LoadEditorAssembly(EXTERNAL_EDITOR_ASSEMBLY_DLL);
    //     _externalEditorAssemblies.Add(loadedAssembly);
    //     
    //     Console.LogSuccess("Successfully loaded external editor assemblies!");
    // }
    //
    // [MethodImpl(MethodImplOptions.NoInlining)]
    // private static Assembly LoadEditorAssembly(string assemblyPath) {
    //     Console.Log($"Loading external editor assembly from: '{assemblyPath}'...");
    //     _editorAssemblyLoadContext = new(assemblyPath);
    //     Assembly assembly = _editorAssemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
    //     
    //     return assembly;
    // }
    //
}
//
// public class ExternalEditorAssemblyLoadContext : AssemblyLoadContext {
//     
//     private const string EXTERNAL_GAME_ASSEMBLY_DLL = @"D:\Dev\C#\CSharpGameEngine\ExampleProject\bin\Debug\net6.0\ExampleGame.dll";
//     private readonly AssemblyDependencyResolver _resolver;
//     private readonly AssemblyDependencyResolver _gameAssemblyDependencyResolver;
//     
//     public ExternalEditorAssemblyLoadContext(string pluginPath) : base(true) {
//         _resolver = new AssemblyDependencyResolver(pluginPath);
//         _gameAssemblyDependencyResolver = new AssemblyDependencyResolver(EXTERNAL_GAME_ASSEMBLY_DLL);
//     }
//     
//     protected override Assembly? Load(AssemblyName assemblyName) {
//         string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
//         assemblyPath ??= _gameAssemblyDependencyResolver.ResolveAssemblyToPath(assemblyName);
//         if(assemblyPath != null) {
//             return LoadFromAssemblyPath(assemblyPath);
//         }
//         return null;
//     }
//     
// }
