using System;
using System.IO;
using GameEngine.Core.SceneManagement;
using Newtonsoft.Json;

namespace GameEngine.Core.Serialization; 

public static class SceneSerializer {
    
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
    
    public static Scene LoadJson(string path) {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        Scene scene = JsonConvert.DeserializeObject<Scene>(File.ReadAllText(desktopPath + "\\" + "Test.scene"), SerializerSettings)!;
        return scene;
    }
    
    public static bool SaveOpenedScene() {
        if(Hierarchy.Scene is null)
            return false;
        
        return SaveSceneJson("Test", Hierarchy.Scene);
    }
    
    private static bool SaveSceneJson(string path, Scene scene) {
        string stringResult = JsonConvert.SerializeObject(scene, Formatting.Indented, SerializerSettings);
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        File.WriteAllText(desktopPath + "\\" + "Test.scene", stringResult);
        return true;
    }
    
}
