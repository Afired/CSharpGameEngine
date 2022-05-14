using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GameEngine.Core.Nodes;
using GameEngine.Core.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GameEngine.Core.Serialization; 

public static class SceneSerializer {
    
    public static Scene Load(string path) {

        DeserializerBuilder deserializerBuilder = new();
        deserializerBuilder.WithTypeInspector(descriptor => new SerializationTypeInspector(descriptor)).IncludeNonPublicProperties();
        IDeserializer deserializer = deserializerBuilder.Build();
        
//        var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
//            .WithNamingConvention(CamelCaseNamingConvention.Instance)
//            .Build();
        
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        Scene scene = deserializer.Deserialize<Scene>(File.ReadAllText(desktopPath + "\\" + "Test.scene"));
        return scene;
    }
    
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
    
    private static bool SaveScene(string path, Scene scene) {

        SerializerBuilder serializerBuilder = new();
        serializerBuilder.WithTypeInspector(descriptor => new SerializationTypeInspector(descriptor)).IncludeNonPublicProperties();
        ISerializer serializer = serializerBuilder.Build();
        
//        ISerializer serializer = new SerializerBuilder()
//            .WithNamingConvention(CamelCaseNamingConvention.Instance)
//            //.IncludeNonPublicProperties()
//            .IgnoreFields()
//            .WithTypeResolver()
//            .Build();
        
        string stringResult = serializer.Serialize(scene);
        
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        File.WriteAllText(desktopPath + "\\" + "Test.scene", stringResult);
        return true;
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

//public class IgnoreParentPropertiesResolver : DefaultContractResolver {
//    
//    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
//        var allProps = base.CreateProperties(type, memberSerialization);
//        
//        //Choose the properties you want to serialize/deserialize
//        var props = type.GetProperties(~BindingFlags.FlattenHierarchy); 
//        
//        return 
//        
//        return allProps.Where(p => props.Any(a => a.Name == p.PropertyName)).ToList();
//    }
//    
//}
//
//public class MyResolver : IContractResolver {
//    
//    public JsonContract ResolveContract(Type type) {
//        new JsonContainerContract()
//    }
//    
//}

public class SerializedPropertiesResolver : DefaultContractResolver {
    
    private static readonly Serialized _serializedAttribute = new();
    
//    private readonly HashSet<string> _ignoreProps;
//    public IgnorePropertiesResolver(IEnumerable<string> propNamesToIgnore) {
//        _ignoreProps = new HashSet<string>(propNamesToIgnore);
//    }

//    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
//        JsonProperty property = base.CreateProperty(member, memberSerialization);
//        if(_ignoreProps.Contains(property.PropertyName))
//            property.ShouldSerialize = _ => false;
//        return property;
//    }
    
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
        JsonProperty property = base.CreateProperty(member, memberSerialization);
        property.ShouldSerialize = _ => property.AttributeProvider.GetAttributes(false).Contains(_serializedAttribute);
        property.ShouldDeserialize = _ => property.AttributeProvider.GetAttributes(false).Contains(_serializedAttribute);
        return property;
    }
    
}
