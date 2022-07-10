using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using GameEngine.Core.Nodes;

namespace GameEngine.Core.Serialization; 

public static class Serializer {

    private const string SERIALIZATION_ASSEMBLY_DLL_PATH = @"D:\Dev\C#\CSharpGameEngine\GameEngine.Serialization\bin\Debug\net6.0\GameEngine.Serialization.dll";
    private static ExternalAssemblyLoadContext? _serializationAssemblyLoadContext;
    private static Assembly? _serializationAssembly;
    
    public static void UnloadResources() {
        TryToUnloadExternalAssemblies();
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool TryToUnloadExternalAssemblies() {
        if(_serializationAssemblyLoadContext is null) {
            Console.LogSuccess($"Successfully serialization assembly");
            return true;
        }
        
        UnloadExternalAssemblies(out WeakReference externalAssemblyLoadContextRef);
        
        const int MAX_GC_ATTEMPTS = 10;
        for(int i = 0; externalAssemblyLoadContextRef.IsAlive; i++) {
            if(i >= MAX_GC_ATTEMPTS) {
                Console.LogError($"Failed to unload serialization assembly!");
                return false;
            }
            Console.Log($"GC Attempt ({i + 1}/{MAX_GC_ATTEMPTS})...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        
        Console.LogSuccess($"Successfully unloaded serialization assembly!");
        return true;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void UnloadExternalAssemblies(out WeakReference editorAssemblyLoadContextRef) {
        _serializationAssembly = null;
        
        //crashes when unloading fails, editor recovers and then user tries to unload again
        _serializationAssemblyLoadContext.Unload();
        
        editorAssemblyLoadContextRef = new WeakReference(_serializationAssemblyLoadContext);
        _serializationAssemblyLoadContext = null;
    }
    
    public static void LoadAssemblyIfNotLoadedAlready() {
        if(_serializationAssemblyLoadContext is not null)
            return;
        _serializationAssemblyLoadContext = new ExternalAssemblyLoadContext(SERIALIZATION_ASSEMBLY_DLL_PATH);
        _serializationAssembly = _serializationAssemblyLoadContext.LoadFromAssemblyPath(SERIALIZATION_ASSEMBLY_DLL_PATH);
        
        // load references
        _serializationAssemblyLoadContext.LoadFromAssemblyPath(@"D:\Dev\C#\CSharpGameEngine\GameEngine.Serialization\bin\Debug\net6.0\Newtonsoft.Json.dll");
    }
    
    public static Node Deserialize(string fileName) {
        LoadAssemblyIfNotLoadedAlready();
        Type? serializerType = _serializationAssembly.GetType("GameEngine.Serialization.Serializer");
        MethodInfo? deserializeMethod = serializerType.GetMethod("Deserialize", BindingFlags.Static | BindingFlags.Public);
        object? result = deserializeMethod.Invoke(null, new object?[] { fileName });
        
        return result as Node;
    }
    
    public static void Serialize<T>(T node, string fileName) where T : Node {
        LoadAssemblyIfNotLoadedAlready();
        Type? serializerType = _serializationAssembly.GetType("GameEngine.Serialization.Serializer");
        MethodInfo? serializeMethod = serializerType.GetMethod("Serialize", BindingFlags.Static | BindingFlags.Public);
        serializeMethod.Invoke(null, new object?[] { node, fileName });
    }
    
}
