using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using GameEngine.Core.Nodes;
using Newtonsoft.Json;

namespace GameEngine.Serialization; 

public static class Serializer {
    
    private static JsonSerializerSettings SerializerSettings => _cachedSerializerSettingsWithCachedContractResolver ??= new JsonSerializerSettings {
        ContractResolver = new SerializedContractResolver(),
        TypeNameHandling = TypeNameHandling.Auto,
        // TypeNameHandling = TypeNameHandling.All,
        // MaxDepth = 10,
        ObjectCreationHandling = ObjectCreationHandling.Replace,
        DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
        PreserveReferencesHandling = PreserveReferencesHandling.All,
        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
        ConstructorHandling = ConstructorHandling.Default,
    };
    private static JsonSerializerSettings? _cachedSerializerSettingsWithCachedContractResolver;
    
    // public static T Deserialize<T>(string fileName) where T : Node {
    //     string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
    //     Directory.CreateDirectory($"{desktopPath}\\Project");
    //     T t = JsonConvert.DeserializeObject<T>(File.ReadAllText($"{desktopPath}\\Project\\{fileName}.node"), SerializerSettings)!;
    //     return t;
    // }
    
    public static string SerializeNode(object obj) {
        string json = JsonConvert.SerializeObject(obj, Formatting.Indented, SerializerSettings);
        Type nodeType = obj.GetType();
        return $"{nodeType.Assembly.GetName().Name}::{nodeType.Namespace}.{nodeType.Name}\n{json}";
    }
    
    public static Node DeserializeNode(string filePath) {
        TrimFirstLine(File.ReadAllText(filePath), out string assemblyTypeDefAsString, out string objAsString);
        SplitAssemblyTypeDefString(assemblyTypeDefAsString, out string assemblyName, out string typeName);
        Type nodeType = GetTypeFromString(assemblyName, typeName);
        Node node = (Node) JsonConvert.DeserializeObject(objAsString, nodeType, SerializerSettings)!;
        return node;
    }
    
    public static Type GetTypeFromString(string assemblyName, string typeAsString) {
        foreach(TypeInfo typeInfo in Assembly.GetAssembly(typeof(Node))!.DefinedTypes) {
            if(typeInfo.FullName == typeAsString)
                return typeInfo.AsType();
        }
        throw new Exception();
    }
    
    public static void SplitAssemblyTypeDefString(string assemblyTypeDefAsString, out string assemblyName, out string typeName) {
        for(int i = 0; i < assemblyTypeDefAsString.Length; i++) {
            if(assemblyTypeDefAsString[i] == ':') {
                assemblyName = assemblyTypeDefAsString[..i];
                typeName = assemblyTypeDefAsString[(i + 2)..];
                return;
            }
        }
        throw new Exception();
    }
    
    public static int GetIndexOfFirstOccurrenceOfCharacter(string str, char character) {
        for(int i = 0; i < str.Length; i++) {
            if(str[i] == character)
                return i;
        }

        return -1;
    }
    
    public static void TrimFirstLine(string str, out string firstLine, out string other) {
        int index = Math.Min(GetIndexOfFirstOccurrenceOfCharacter(str, '\n'), GetIndexOfFirstOccurrenceOfCharacter(str, '\r'));
        if(index == -1) {
            firstLine = str;
            other = string.Empty;
            return;
        }
        firstLine = str[..index];
        other = str[(index + 1)..];
    }
    
}
