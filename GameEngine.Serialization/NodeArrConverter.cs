using System;
using GameEngine.Core.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameEngine.Serialization; 

public class NodeArrConverter : JsonConverter {
    
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(NodeArr<>);
    }

    private Type GetConcreteType(JObject obj) {
        // if(obj.GetValue(nameof(ContentA.SomePropertyOfContentA), StringComparison.OrdinalIgnoreCase) != null)
        //     return typeof(ContentA);
        // obj.Get
        // return obj.GetValue(nameof(INodeArr.GetNodeType)).
        
        // Add other tests for other content types.
        // Return a default type or throw an exception if a unique type cannot be found.
        throw new JsonSerializationException("Cannot determine concrete type for IContentType");
    }
    
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
        throw new NotImplementedException();
    }
    
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.Null)
            return null;
        var obj = JObject.Load(reader);
        var concreteType = GetConcreteType(obj);
        return obj.ToObject(concreteType, serializer);
    }
    
}