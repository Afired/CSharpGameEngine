using System.Reflection;
using GameEngine.Editor.NodeDrawers;
using ImGuiNET;
using JetBrains.Annotations;

namespace GameEngine.Editor.PropertyDrawers;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class PropertyDrawer {
    
    private static readonly Dictionary<Type, PropertyDrawer> _propertyDrawerLookup = new();
    protected internal abstract Type PropertyType { get; }
    protected internal abstract void DrawPropertyInternal(object container, FieldInfo fieldInfo);
    protected internal abstract void DrawPropertyInternal(object container, PropertyInfo propertyInfo);
    
    internal static void Draw(object container, FieldInfo fieldInfo) {
        if(_propertyDrawerLookup.TryGetValue(fieldInfo.FieldType, out PropertyDrawer? propertyDrawer)) {
            propertyDrawer.DrawPropertyInternal(container, fieldInfo);
        } else { //todo: arrays
            Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {fieldInfo.FieldType}");
        }
    }
    
    internal static void Draw(object container, PropertyInfo propertyInfo) {
        if(_propertyDrawerLookup.TryGetValue(propertyInfo.PropertyType, out PropertyDrawer? propertyDrawer)) {
            propertyDrawer.DrawPropertyInternal(container, propertyInfo);
        } else if(propertyInfo.PropertyType.IsArray) {
            DrawArray(container, propertyInfo);
        } else {
            Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {propertyInfo.PropertyType}");
        }
    }
    
    private static void DrawArray(object container, PropertyInfo propertyInfo) {
        Type type = propertyInfo.PropertyType;
        Type elementType = type.GetElementType()!;
        
        Array? array = (Array?) propertyInfo.GetValue(container);
        
        if(array is null) {
            ImGui.Text("Array is null");
        } else {
            
            ImGui.Text("Start Array");
            for(int i = 0; i < array.Length; i++) {

                object? element = array.GetValue(i);

                if(element is null) {
                    ImGui.Text("Element is null");
                    continue;
                }
                
                Type explicitElementType = element.GetType();
                
                if(_propertyDrawerLookup.TryGetValue(explicitElementType, out PropertyDrawer? propertyDrawer)) {
                    array.SetValue(propertyDrawer.DrawPropertyDirect(element, propertyInfo), i);
                } else { //todo: nested arrays
                    Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {explicitElementType}");
                }
                
            }
            ImGui.Text("End Array");
            
        }
        
        if(propertyInfo.CanWrite)
            propertyInfo.SetValue(container, array);
        
    }
    
    private static Dictionary<Type, PropertyDrawer> BuildPropertyDrawerLookup() {
        Dictionary<Type, PropertyDrawer> propertyDrawerLookup = new();
        
        foreach(Assembly editorAssembly in EditorApplication.Instance.ExternalAssemblies.Append(typeof(EditorApplication).Assembly)) {
            List<Type> derivedTypes = ReflectionHelper.GetDerivedTypes(typeof(PropertyDrawer<>), editorAssembly);
            foreach(Type type in derivedTypes) {
                PropertyDrawer propertyDrawer = Activator.CreateInstance(type) as PropertyDrawer ?? throw new NullReferenceException();
                if(!propertyDrawerLookup.TryAdd(propertyDrawer.PropertyType, propertyDrawer))
                    Console.LogWarning($"Failed to register property drawer for {type.ToString()}");
            }
        }
        return propertyDrawerLookup;
    }
    
    public static void ClearLookUp() {
        _propertyDrawerLookup.Clear();
    }
    
    public static void GenerateLookUp() {
        _propertyDrawerLookup.Clear();
        foreach(Assembly editorAssembly in EditorApplication.Instance.ExternalAssemblies.Append(typeof(EditorApplication).Assembly)) {
            List<Type> derivedTypes = ReflectionHelper.GetDerivedTypes(typeof(PropertyDrawer<>), editorAssembly);
            foreach(Type type in derivedTypes) {
                PropertyDrawer propertyDrawer = Activator.CreateInstance(type) as PropertyDrawer ?? throw new NullReferenceException();
                if(!_propertyDrawerLookup.TryAdd(propertyDrawer.PropertyType, propertyDrawer))
                    Console.LogWarning($"Failed to register property drawer for {type.ToString()}");
            }
        }
    }

    protected internal abstract object? DrawPropertyDirect(object? value, PropertyInfo propertyInfo);

}

public abstract class PropertyDrawer<TProperty> : PropertyDrawer {
    
    protected internal sealed override Type PropertyType => typeof(TProperty);
    
    protected internal sealed override void DrawPropertyInternal(object container, FieldInfo fieldInfo) {
        TProperty? value = (TProperty?) fieldInfo.GetValue(container);
        DrawProperty(ref value, new Property(fieldInfo));
        fieldInfo.SetValue(container, value);
    }
    
    protected internal sealed override void DrawPropertyInternal(object container, PropertyInfo propertyInfo) {
        TProperty? value = (TProperty?) propertyInfo.GetValue(container);
        DrawProperty(ref value, new Property(propertyInfo));
        if(propertyInfo.CanWrite)
            propertyInfo.SetValue(container, value);
    }
    
    protected abstract void DrawProperty(ref TProperty? value, Property property);
    
    protected internal sealed override object? DrawPropertyDirect(object? value, PropertyInfo propertyInfo) {
        TProperty? castValue = (TProperty?) value;
        DrawProperty(ref castValue, new Property(propertyInfo));
        return castValue;
    }
    
}
