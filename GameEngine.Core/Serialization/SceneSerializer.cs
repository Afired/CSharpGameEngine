using System;
using System.IO;
using System.Reflection;
using GameEngine.Core.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GameEngine.Core.Serialization; 

public static class SceneSerializer {
    
    public static Scene LoadJson(string path) {
        
        JsonSerializerSettings settings = new JsonSerializerSettings() {
            ContractResolver = new SerializedPropertiesResolver(),
            TypeNameHandling = TypeNameHandling.Auto,
            MaxDepth = 10,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
//            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        };
        
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        Scene scene = JsonConvert.DeserializeObject<Scene>(File.ReadAllText(desktopPath + "\\" + "Test.scene"), settings)!;
        return scene;
    }
    
    public static bool SaveOpenedScene() {
        if(Hierarchy.Scene is null)
            return false;
        
        return SaveSceneJson("Test", Hierarchy.Scene);
    }
    
    private static bool SaveSceneJson(string path, Scene scene) {
        JsonSerializerSettings settings = new JsonSerializerSettings() {
            ContractResolver = new SerializedPropertiesResolver(),
            TypeNameHandling = TypeNameHandling.Auto,
            MaxDepth = 10,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
//            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        };
        string stringResult = JsonConvert.SerializeObject(scene, Formatting.Indented, settings);
        
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        File.WriteAllText(desktopPath + "\\" + "Test.scene", stringResult);
        return true;
    }
    
}

public class SerializedPropertiesResolver : DefaultContractResolver {
    
    private static readonly Serialized _serializedAttribute = new();
    
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
        JsonProperty jProp = base.CreateProperty(member, memberSerialization);
        
//        Console.LogSuccess(jProp.PropertyName ?? "null");
        
        // include all properties even if they have a none public setter
        if(!jProp.Writable) {
            PropertyInfo? propertyInfo = member as PropertyInfo;
            bool hasNonePublicSetter = propertyInfo?.GetSetMethod(true) is not null;
            jProp.Writable = hasNonePublicSetter;
        }
        
        // include all properties if they have a getter method
        if(!jProp.Readable) {
            PropertyInfo? propertyInfo = member as PropertyInfo;
            bool hasGetterMethod = propertyInfo?.GetGetMethod(true) is not null;
            jProp.Readable = hasGetterMethod;
        }
        
        // only include if property is decorated with [Serialized] attribute
        jProp.ShouldSerialize = _ => jProp.AttributeProvider.GetAttributes(false).Contains(_serializedAttribute);
        jProp.ShouldDeserialize = _ => jProp.AttributeProvider.GetAttributes(false).Contains(_serializedAttribute);
        return jProp;
    }
    
}
