using System.Reflection;
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
            DrawArray(container, fieldInfo);
        
        else
            Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {fieldInfo.FieldType}");
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
            DrawArray(container, propertyInfo);
        
        else
            Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {propertyInfo.PropertyType}");
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
            return DrawArrayDirect(value.GetType(), (Array) value, property);
            
        }  else {
            Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {value.GetType()}");
            
        }
        return value;
    }
    
    private static Array? DrawArrayDirect(Type type, Array? array, Property property) {
        
        if(array is null)
            return (Array) DrawNullDirect(type);
        
        ImGui.Text("Start Array");
        for(int i = 0; i < array.Length; i++) {

            object? element = array.GetValue(i);

            if(element is null) {
                ImGui.Text("Element is null");
                continue;
            }
                
            Type explicitElementType = element.GetType();
                
            if(_propertyDrawerCache.TryGetValue(explicitElementType, out PropertyDrawer? propertyDrawer)) {
                array.SetValue(propertyDrawer.DrawDirectInternal(explicitElementType, element, property), i);
            } else { //todo: nested arrays
                Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {explicitElementType}");
            }
                
        }
        ImGui.Text("End Array");
        return array;
    }
    
    private static void DrawArray(object container, PropertyInfo propertyInfo) {
        
        if(!propertyInfo.CanWrite) {
            Console.LogWarning("Readonly serialized properties aren't supported at this time. They can't be drawn by any property drawers");
            return;
        }
        
        Array? array = (Array?) propertyInfo.GetValue(container);
        
        if(array is null)
            if(!DrawNull(container, propertyInfo))
                return;
        
        ImGui.Text("Start Array");
        for(int i = 0; i < array.Length; i++) {
            
            object? element = array.GetValue(i);
            
            if(element is null) {
                ImGui.Text("Element is null");
                continue;
            }
            
            Type explicitElementType = element.GetType();
            
            if(_propertyDrawerCache.TryGetValue(explicitElementType, out PropertyDrawer? propertyDrawer)) {
                array.SetValue(propertyDrawer.DrawDirectInternal(explicitElementType, element, new Property(propertyInfo)), i);
            } else { //todo: nested arrays
                Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {explicitElementType}");
            }
            
        }
        ImGui.Text("End Array");
        
        if(propertyInfo.CanWrite)
            propertyInfo.SetValue(container, array);
    }
    
    private static void DrawArray(object container, FieldInfo fieldInfo) {
        
        Array? array = (Array?) fieldInfo.GetValue(container);
        
        if(array is null)
            if(!DrawNull(container, fieldInfo))
                return;
        
        ImGui.Text("Start Array");
        for(int i = 0; i < array.Length; i++) {
            
            object? element = array.GetValue(i);
            
            if(element is null) {
                ImGui.Text("Element is null");
                continue;
            }
            
            Type explicitElementType = element.GetType();
                
            if(_propertyDrawerCache.TryGetValue(explicitElementType, out PropertyDrawer? propertyDrawer)) {
                array.SetValue(propertyDrawer.DrawDirectInternal(explicitElementType, element, new Property(fieldInfo)), i);
            } else { //todo: nested arrays
                Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {explicitElementType}");
            }
            
        }
        ImGui.Text("End Array");
        
        fieldInfo.SetValue(container, array);
    }
    
    public static void ClearLookUp() {
        _propertyDrawerCache.Clear();
        _genericPropertyDrawerTypeCache.Clear();
    }
    
    public static void GenerateLookUp() {
        foreach(Assembly editorAssembly in EditorApplication.Instance.ExternalAssemblies.Append(typeof(EditorApplication).Assembly)) {
            List<Type> derivedTypes = ReflectionHelper.GetDerivedTypes(typeof(PropertyDrawer<>), editorAssembly);
            foreach(Type type in derivedTypes) {
                
                if(type.IsGenericType) {
                    Type basePropertyDrawerType = type.BaseType!;
                    Type[] basePropertyDrawersGenericTypes = basePropertyDrawerType.GetGenericArguments();
                    
                    if(!_genericPropertyDrawerTypeCache.TryAdd(basePropertyDrawersGenericTypes[0].GetGenericTypeDefinition(), type))
                        Console.LogWarning($"Failed to register property drawer for {type.ToString()}");
                    continue;
                }
                
                PropertyDrawer propertyDrawer = Activator.CreateInstance(type) as PropertyDrawer ?? throw new Exception();
                if(!_propertyDrawerCache.TryAdd(propertyDrawer.PropertyType, propertyDrawer))
                    Console.LogWarning($"Failed to register property drawer for {type.ToString()}");
                
            }
        }
    }
    
    protected internal abstract object? DrawDirectInternal(Type type, object? value, Property property);
}

public abstract class PropertyDrawer<TProperty> : PropertyDrawer {
    
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
    
    protected abstract void DrawProperty(ref TProperty value, Property property);
    
    protected internal sealed override object? DrawDirectInternal(Type type, object? value, Property property) {
        TProperty? castValue = (TProperty?) value;
        DrawProperty(ref castValue, property);
        return castValue;
    }
    
}
