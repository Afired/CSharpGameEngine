using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameEngine.Core.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GameEngine.Serialization; 

public class SerializedContractResolver : DefaultContractResolver {
    
    protected override List<MemberInfo> GetSerializableMembers(Type type) {
        List<MemberInfo> members = new List<MemberInfo>();
        for(Type? currentType = type; currentType is not null; currentType = currentType.BaseType) {
            
            PropertyInfo[] propertyInfos = currentType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach(PropertyInfo propertyInfo in propertyInfos) {
                object[] customAttributes = propertyInfo.GetCustomAttributes(false);
                foreach(object customAttribute in customAttributes) {
                    bool isSerializedAttribute = customAttribute.GetType().GUID == typeof(Serialized).GUID;
                    if(isSerializedAttribute)
                        members.Add(propertyInfo);
                }
            }
            
            FieldInfo[] fieldInfos = currentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach(FieldInfo fieldInfo in fieldInfos) {
                object[] customAttributes = fieldInfo.GetCustomAttributes(false);
                foreach(object customAttribute in customAttributes) {
                    bool isSerializedAttribute = customAttribute.GetType().GUID == typeof(Serialized).GUID;
                    if(isSerializedAttribute)
                        members.Add(fieldInfo);
                }
            }
            
        }
        return members;
    }
    
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
        JsonProperty jProp = base.CreateProperty(member, memberSerialization);

        if(member is PropertyInfo propertyInfo) {
            
            // include all properties even if they have a non public setter
            if(!jProp.Writable) {
                bool hasNonePublicSetter = propertyInfo?.GetSetMethod(true) is not null;
                jProp.Writable = hasNonePublicSetter;
            }
            
            // include all properties if they have a getter method
            if(!jProp.Readable) {
                bool hasGetterMethod = propertyInfo?.GetGetMethod(true) is not null;
                jProp.Readable = hasGetterMethod;
            }
            
        } else if(member is FieldInfo fieldInfo) {
            jProp.Readable = true;
            jProp.Writable = true;
        }
        
        return jProp;
    }
    
}

