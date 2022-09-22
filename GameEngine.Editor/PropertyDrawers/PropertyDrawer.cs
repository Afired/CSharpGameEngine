using System.Reflection;
using GameEngine.Core;
using GameEngine.Editor.NodeDrawers;
using ImGuiNET;
using JetBrains.Annotations;

namespace GameEngine.Editor.PropertyDrawers;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class PropertyDrawer {
    
    private static readonly Dictionary<Type, PropertyDrawer> _propertyDrawerCache = new();  // <PropertyType, PropertyDrawer>
    private static readonly Dictionary<Type, Type> _genericPropertyDrawerTypeCache = new(); // <PropertyType, PropertyDrawerType<>>
    protected internal abstract Type PropertyType { get; }
    protected internal abstract void DrawInternal(object container, FieldInfo fieldInfo);
    protected internal abstract void DrawInternal(object container, PropertyInfo propertyInfo);
    protected internal abstract object? DrawDirectInternal(Type type, object? value, Property property);
    
    public static bool DrawNull(object container, FieldInfo fieldInfo) {
        if(ImGui.Button($"init<{fieldInfo.FieldType}>")) {
            if(fieldInfo.FieldType == typeof(string)) {
                fieldInfo.SetValue(container, string.Empty);
                return true;
            }
            try {
                fieldInfo.SetValue(container, Activator.CreateInstance(fieldInfo.FieldType));
                return true;
            } catch(Exception e) {
                Console.LogWarning(e.Message);
                return false;
            }
        }
        return false;
    }
    
    public static bool DrawNull(object container, PropertyInfo propertyInfo) {
        if(ImGui.Button($"init<{propertyInfo.PropertyType}>")) {
            if(propertyInfo.PropertyType == typeof(string)) {
                propertyInfo.SetValue(container, string.Empty);
                return true;
            }
            try {
                propertyInfo.SetValue(container, Activator.CreateInstance(propertyInfo.PropertyType));
                return true;
            } catch(Exception e) {
                Console.LogWarning(e.Message);
                return false;
            }
        }
        return false;
    }
    
    public static object? DrawNullDirect(Type type) {
        if(ImGui.Button($"init<{type}>")) {
            if(type == typeof(string))
                return string.Empty;
            try {
                return Activator.CreateInstance(type);
            } catch(Exception e) {
                Console.LogWarning(e.Message);
                return null;
            }
        }
        return null;
    }
    
    public static void Draw(object container, FieldInfo fieldInfo) {
        
        if(fieldInfo.GetValue(container) is null)
            if(!DrawNull(container, fieldInfo))
                return;
        
        if(_propertyDrawerCache.TryGetValue(fieldInfo.FieldType, out PropertyDrawer? propertyDrawer))
            propertyDrawer.DrawInternal(container, fieldInfo);
        
        else if(fieldInfo.FieldType.IsGenericType) {
            if(_genericPropertyDrawerTypeCache.TryGetValue(fieldInfo.FieldType.GetGenericTypeDefinition(), out Type? propertyDrawerType)) {
                Type genericPropertyDrawerType = propertyDrawerType.MakeGenericType(fieldInfo.FieldType.GetGenericArguments());
                PropertyDrawer genericPropertyDrawer = Activator.CreateInstance(genericPropertyDrawerType) as PropertyDrawer ?? throw new Exception();
                genericPropertyDrawer.DrawInternal(container, fieldInfo);
            }
            
        } else if(fieldInfo.FieldType.IsArray)
            new PropertyDrawerArray().DrawInternal(container, fieldInfo);
        
        else if(fieldInfo.FieldType.IsEnum)
            new PropertyDrawerEnum().DrawInternal(container, fieldInfo);
        
        else {
            if(fieldInfo.FieldType.IsSerializable) {
                ImGui.Text(fieldInfo.Name + ":");
                ImGui.Indent();
                List<MemberInfo> memberInfos = NodeDrawer.GetSerializedMembersDisplayedInInspector(fieldInfo.FieldType);
                if(memberInfos.Count == 0) {
                    ImGui.Unindent();
                    return;
                }
                foreach(MemberInfo memberInfo in memberInfos) {
                    if(memberInfo is PropertyInfo nestedPropertyInfo)
                        Draw(fieldInfo.GetValue(container), nestedPropertyInfo);
                    else if(memberInfo is FieldInfo nestedFieldInfo)
                        Draw(fieldInfo.GetValue(container), nestedFieldInfo);
                }
                ImGui.Unindent();
            } else {
                Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {fieldInfo.FieldType}, neither is it marked as Serializable");
            }
        }
    }
    
    public static void Draw(object container, PropertyInfo propertyInfo) {
        if(!propertyInfo.CanWrite) {
            Console.LogWarning("Readonly serialized properties aren't supported at this time. They can't be drawn by any property drawers");
            return;
        }
        
        if(propertyInfo.GetValue(container) is null)
            if(!DrawNull(container, propertyInfo))
                return;
        
        if(_propertyDrawerCache.TryGetValue(propertyInfo.PropertyType, out PropertyDrawer? propertyDrawer))
            propertyDrawer.DrawInternal(container, propertyInfo);
        
        else if(propertyInfo.PropertyType.IsGenericType) {
            if(_genericPropertyDrawerTypeCache.TryGetValue(propertyInfo.PropertyType.GetGenericTypeDefinition(), out Type? propertyDrawerType)) {
                Type genericPropertyDrawerType = propertyDrawerType.MakeGenericType(propertyInfo.PropertyType.GetGenericArguments());
                PropertyDrawer genericPropertyDrawer = Activator.CreateInstance(genericPropertyDrawerType) as PropertyDrawer ?? throw new Exception();
                genericPropertyDrawer.DrawInternal(container, propertyInfo);
            }
            
        } else if(propertyInfo.PropertyType.IsArray)
            new PropertyDrawerArray().DrawInternal(container, propertyInfo);
        
        else if(propertyInfo.PropertyType.IsEnum)
            new PropertyDrawerEnum().DrawInternal(container, propertyInfo);
        
        else {
            if(propertyInfo.PropertyType.IsSerializable) {
                ImGui.Text(propertyInfo.Name + ":");
                ImGui.Indent();
                List<MemberInfo> memberInfos = NodeDrawer.GetSerializedMembersDisplayedInInspector(propertyInfo.PropertyType);
                if(memberInfos.Count == 0) {
                    ImGui.Unindent();
                    return;
                }
                foreach(MemberInfo memberInfo in memberInfos) {
                    if(memberInfo is PropertyInfo nestedPropertyInfo)
                        Draw(propertyInfo.GetValue(container), nestedPropertyInfo);
                    else if(memberInfo is FieldInfo nestedFieldInfo)
                        Draw(propertyInfo.GetValue(container), nestedFieldInfo);
                }
                ImGui.Unindent();
            } else {
                Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {propertyInfo.PropertyType}, neither is it marked as Serializable");
            }
        }
    }
    
    public static object? DrawDirect(Type type, object? value, Property property) {
        
        if(value is null)
            return DrawNullDirect(type);
        
        if(_propertyDrawerCache.TryGetValue(value.GetType(), out PropertyDrawer? propertyDrawer)) {
            return propertyDrawer.DrawDirectInternal(type, value, property);
            
        } else if(value.GetType().IsGenericType) {
            if(_genericPropertyDrawerTypeCache.TryGetValue(value.GetType().GetGenericTypeDefinition(), out Type? propertyDrawerType)) {
                Type genericPropertyDrawerType = propertyDrawerType.MakeGenericType(value.GetType().GetGenericArguments());
                PropertyDrawer genericPropertyDrawer = Activator.CreateInstance(genericPropertyDrawerType) as PropertyDrawer ?? throw new Exception();
                return genericPropertyDrawer.DrawDirectInternal(type, value, property);
            }
            
        } else if(value.GetType().IsArray) {
            return new PropertyDrawerArray().DrawDirectInternal(type, value, property);
            
        } else {
            if(type.IsSerializable) {
                ImGui.Text(property.Name + ":");
                ImGui.Indent();
                List<MemberInfo> memberInfos = NodeDrawer.GetSerializedMembersDisplayedInInspector(type);
                if(memberInfos.Count == 0) {
                    ImGui.Unindent();
                    return value;
                }
                foreach(MemberInfo memberInfo in memberInfos) {
                    if(memberInfo is PropertyInfo nestedPropertyInfo)
                        Draw(value, nestedPropertyInfo);
                    else if(memberInfo is FieldInfo nestedFieldInfo)
                        Draw(value, nestedFieldInfo);
                }
                ImGui.Unindent();
            } else {
                Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {type}, neither is it marked as Serializable");
            }
        }
        return value;
    }
    
    public static void ClearLookUp() {
        _propertyDrawerCache.Clear();
        _genericPropertyDrawerTypeCache.Clear();
    }
    
    public static void GenerateLookUp() {
        foreach(Assembly editorAssembly in EditorApplication.Instance.ExternalAssemblies.Append(typeof(EditorApplication).Assembly)) {
            List<Type> derivedTypes = typeof(PropertyDrawer<>).GetDerivedTypes(editorAssembly);
            foreach(Type type in derivedTypes) {
                
                if(type.IsGenericType) {
                    Type basePropertyDrawerType = type.BaseType!;
                    Type[] basePropertyDrawersGenericTypes = basePropertyDrawerType.GetGenericArguments();
                    
                    if(!_genericPropertyDrawerTypeCache.TryAdd(basePropertyDrawersGenericTypes[0].GetGenericTypeDefinition(), type))
                        Console.LogWarning($"Failed to register property drawer for {type}");
                    continue;
                }
                
                PropertyDrawer propertyDrawer = Activator.CreateInstance(type) as PropertyDrawer ?? throw new Exception();
                if(!_propertyDrawerCache.TryAdd(propertyDrawer.PropertyType, propertyDrawer))
                    Console.LogWarning($"Failed to register property drawer for {type}");
            }
        }
    }
    
}

public abstract class PropertyDrawer<TProperty> : PropertyDrawer where TProperty : notnull {
    
    protected abstract void DrawProperty(ref TProperty value, Property property);
    
    protected internal sealed override Type PropertyType => typeof(TProperty);
    
    protected internal sealed override void DrawInternal(object container, FieldInfo fieldInfo) {
        TProperty? value = (TProperty?) fieldInfo.GetValue(container);
        DrawProperty(ref value, new Property(fieldInfo));
        fieldInfo.SetValue(container, value);
    }
    
    protected internal sealed override void DrawInternal(object container, PropertyInfo propertyInfo) {
        TProperty? value = (TProperty?) propertyInfo.GetValue(container);
        DrawProperty(ref value, new Property(propertyInfo));
        if(propertyInfo.CanWrite)
            propertyInfo.SetValue(container, value);
    }
    
    protected internal sealed override object? DrawDirectInternal(Type type, object? value, Property property) {
        TProperty? castValue = (TProperty?) value;
        DrawProperty(ref castValue, property);
        return castValue;
    }
    
}
