using System;
using System.IO;
using GameEngine.SceneManagement;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GameEngine.Serialization; 

public static class SceneSerializer {
//
//    public static Scene Load(string path) {
//        var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
//            .WithNamingConvention(CamelCaseNamingConvention.Instance)
//            .Build();
//
//        var myConfig = deserializer.Deserialize<Scene>(File.ReadAllText("Test.scene"));
//        return new Scene();
//    }
//
//    public static bool SaveOpenedScene() {
//        if(Hierarchy.Scene is null)
//            return false;
//
//        return SaveScene("Test", Hierarchy.Scene);
//    }
//    
//    private static bool SaveScene(string path, Scene scene) {
//        ISerializer serializer = new SerializerBuilder()
//            .WithNamingConvention(CamelCaseNamingConvention.Instance)
//            //.IncludeNonPublicProperties()
//            .IgnoreFields()
//            .WithTypeResolver()
//            .Build();
//        
//        string stringResult = serializer.Serialize(scene);
//        
//        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
//        File.WriteAllText(desktopPath + "\\" + "Test.scene", stringResult);
//        return true;
//    }
    
}
