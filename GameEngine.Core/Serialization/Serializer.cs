using System;
using System.IO;
using GameEngine.Core.Nodes;
using Newtonsoft.Json;

namespace GameEngine.Core.Serialization; 

public static class Serializer {
    
    private static readonly JsonSerializerSettings SerializerSettings = new() {
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
    
    public static T Deserialize<T>(string fileName) where T : Node {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        Directory.CreateDirectory($"{desktopPath}\\Project");
        T scene = JsonConvert.DeserializeObject<T>(File.ReadAllText($"{desktopPath}\\Project\\{fileName}.node"), SerializerSettings)!;
        return scene;
    }
    
    public static void Serialize<T>(T node, string fileName) where T : Node {
        string stringResult = JsonConvert.SerializeObject(node, Formatting.Indented, SerializerSettings);
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        Directory.CreateDirectory($"{desktopPath}\\Project");
        File.WriteAllText($"{desktopPath}\\Project\\{fileName}.node", stringResult);
    }
    
}
