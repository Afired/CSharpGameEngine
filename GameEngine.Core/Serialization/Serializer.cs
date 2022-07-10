using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GameEngine.Core.Nodes;

namespace GameEngine.Core.Serialization; 

public static class Serializer {

    private const string SERIALIZATION_ASSEMBLY_DLL_PATH = @"D:\Dev\C#\CSharpGameEngine\GameEngine.Serialization\bin\Debug\net6.0\GameEngine.Serialization.dll";

    private static readonly ExternalAssemblyLoadContextManager _ealcm = new();
    
    public static void UnloadResources() {
        _ealcm.Unload();
    }
    
    public static void LoadAssemblyIfNotLoadedAlready() {
        // if(_serializationAssemblyLoadContext is not null)
        //     return;
        _ealcm.LoadExternalAssembly(SERIALIZATION_ASSEMBLY_DLL_PATH, true);
        
        // load references
        _ealcm.LoadExternalAssembly(@"D:\Dev\C#\CSharpGameEngine\GameEngine.Serialization\bin\Debug\net6.0\Newtonsoft.Json.dll", true);
    }
    
    public static Node Deserialize(string fileName) {
        LoadAssemblyIfNotLoadedAlready();
        Type? serializerType = _ealcm.ExternalAssemblies.First().GetType("GameEngine.Serialization.Serializer");
        MethodInfo? deserializeMethod = serializerType.GetMethod("Deserialize", BindingFlags.Static | BindingFlags.Public);
        object? result = deserializeMethod.Invoke(null, new object?[] { fileName });
        
        return result as Node;
    }
    
    public static void Serialize<T>(T node, string fileName) where T : Node {
        LoadAssemblyIfNotLoadedAlready();
        Type? serializerType = _ealcm.ExternalAssemblies.First().GetType("GameEngine.Serialization.Serializer");
        MethodInfo? serializeMethod = serializerType.GetMethod("Serialize", BindingFlags.Static | BindingFlags.Public);
        serializeMethod.Invoke(null, new object?[] { node, fileName });
    }
    
}
