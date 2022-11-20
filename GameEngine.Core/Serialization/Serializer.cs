using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GameEngine.Core.Nodes;

namespace GameEngine.Core.Serialization; 

public static class Serializer {
    
    private const string SERIALIZATION_ASSEMBLY_DLL_PATH = @"D:\Dev\C#\CSharpGameEngine\GameEngine.Serialization\bin\Debug\net7.0\GameEngine.Serialization.dll";
    private const string NEWTONSOFT_JSON_DLL_PATH = @"D:\Dev\C#\CSharpGameEngine\GameEngine.Serialization\bin\Debug\net7.0\Newtonsoft.Json.dll";
    
    private static readonly ExternalAssemblyLoadContextManager _ealcm = new();
    
    public static void UnloadResources() {
        _ealcm.Unload();
    }
    
    public static void LoadAssemblyIfNotLoadedAlready() {
        // if(_serializationAssemblyLoadContext is not null)
        //     return;
        _ealcm.LoadExternalAssembly(SERIALIZATION_ASSEMBLY_DLL_PATH, true);
        
        // load references
        _ealcm.LoadExternalAssembly(NEWTONSOFT_JSON_DLL_PATH, true);
    }
    
    public static Node DeserializeNode(string filePath) {
        LoadAssemblyIfNotLoadedAlready();
        Type? serializerType = _ealcm.ExternalAssemblies.First().GetType("GameEngine.Serialization.Serializer");
        MethodInfo? deserializeMethod = serializerType.GetMethod("DeserializeNode", BindingFlags.Static | BindingFlags.Public);
        object? result = deserializeMethod.Invoke(null, new object?[] { filePath });
        
        return result as Node;
    }
    
    public static string SerializeNode<T>(T node) where T : Node {
        LoadAssemblyIfNotLoadedAlready();
        Type? serializerType = _ealcm.ExternalAssemblies.First().GetType("GameEngine.Serialization.Serializer");
        MethodInfo? serializeMethod = serializerType.GetMethod("SerializeNode", BindingFlags.Static | BindingFlags.Public);
        return (string) serializeMethod.Invoke(null, new object?[] { node });
    }
    
}
