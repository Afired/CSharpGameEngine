using System.Reflection;
using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering;
using GameEngine.Editor.NodeDrawers;
using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers;


public abstract class PropertyDrawer {
    
    private static readonly Dictionary<Type, PropertyDrawer> _propertyDrawerLookup = new();
    protected internal abstract Type PropertyType { get; }
    protected internal abstract void DrawPropertyInternal(object container, FieldInfo fieldInfo);
    protected internal abstract void DrawPropertyInternal(object container, PropertyInfo propertyInfo);
    
    internal static void Draw(object container, FieldInfo fieldInfo) {
        if(_propertyDrawerLookup.TryGetValue(fieldInfo.FieldType, out PropertyDrawer? propertyDrawer)) {
            propertyDrawer.DrawPropertyInternal(container, fieldInfo);
        } else {
            Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {fieldInfo.FieldType}");
        }
    }
    
    internal static void Draw(object container, PropertyInfo propertyInfo) {
        if(_propertyDrawerLookup.TryGetValue(propertyInfo.PropertyType, out PropertyDrawer? propertyDrawer)) {
            propertyDrawer.DrawPropertyInternal(container, propertyInfo);
        } else {
            Console.LogWarning($"Property can't be drawn because there is no property drawer defined for {propertyInfo.PropertyType}");
        }
    }
    
    private static Dictionary<Type, PropertyDrawer> BuildPropertyDrawerLookup() {
        Dictionary<Type, PropertyDrawer> propertyDrawerLookup = new();
        
        foreach(Assembly editorAssembly in AssemblyManager.EditorAssemblies()) {
            List<Type> derivedTypes = ReflectionHelper.GetDerivedTypes(typeof(PropertyDrawer<>), editorAssembly);
            foreach(Type type in derivedTypes) {
                PropertyDrawer propertyDrawer = Activator.CreateInstance(type) as PropertyDrawer ?? throw new NullReferenceException();
                if(!propertyDrawerLookup.TryAdd(propertyDrawer.PropertyType, propertyDrawer))
                    Console.LogWarning($"Failed to register property drawer for {type.ToString()}");
            }
        }
        return propertyDrawerLookup;
        
        // List<Type> derivedTypes = ReflectionHelper.GetDerivedTypes(typeof(PropertyDrawer<>), typeof(PropertyDrawer<>).Assembly);
        // foreach(Type type in derivedTypes) {
        //     PropertyDrawer propertyDrawer = Activator.CreateInstance(type) as PropertyDrawer ?? throw new NullReferenceException();
        //     if(!propertyDrawerLookup.TryAdd(propertyDrawer.PropertyType, propertyDrawer))
        //         Console.LogWarning($"Failed to register property drawer for {type.ToString()}");
        // }
        // return propertyDrawerLookup;
    }
    
    public static void ClearLookUp() {
        _propertyDrawerLookup.Clear();
    }
    
    public static void GenerateLookUp() {
        _propertyDrawerLookup.Clear();
        foreach(Assembly editorAssembly in AssemblyManager.EditorAssemblies()) {
            List<Type> derivedTypes = ReflectionHelper.GetDerivedTypes(typeof(PropertyDrawer<>), editorAssembly);
            foreach(Type type in derivedTypes) {
                PropertyDrawer propertyDrawer = Activator.CreateInstance(type) as PropertyDrawer ?? throw new NullReferenceException();
                if(!_propertyDrawerLookup.TryAdd(propertyDrawer.PropertyType, propertyDrawer))
                    Console.LogWarning($"Failed to register property drawer for {type.ToString()}");
            }
        }
    }
    
}

public abstract class PropertyDrawer<TProperty> : PropertyDrawer {
    
    protected internal sealed override Type PropertyType => typeof(TProperty);
    
    protected internal sealed override void DrawPropertyInternal(object container, FieldInfo fieldInfo) {
        TProperty value = (TProperty) fieldInfo.GetValue(container);
        DrawProperty(ref value, new Property(fieldInfo));
        fieldInfo.SetValue(container, value);
    }
    
    protected internal sealed override void DrawPropertyInternal(object container, PropertyInfo propertyInfo) {
        TProperty value = (TProperty) propertyInfo.GetValue(container);
        DrawProperty(ref value, new Property(propertyInfo));
        if(propertyInfo.CanWrite)
            propertyInfo.SetValue(container, value);
    }
    
    protected abstract void DrawProperty(ref TProperty value, Property property);
    
}

public class PropertyDrawerFloat : PropertyDrawer<float> {
    
    protected override void DrawProperty(ref float value, Property property) {
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();

        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);
        ImGui.PushID(property.Name);
        ImGui.DragFloat("", ref value, 0.01f, float.MinValue, float.MaxValue, "%g");
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}

public class PropertyDrawerBool : PropertyDrawer<bool> {
    
    protected override void DrawProperty(ref bool value, Property property) {
        ImGui.Checkbox(property.Name, ref value);
    }
    
}

public class PropertyDrawerColor : PropertyDrawer<Color> {
    
    protected override void DrawProperty(ref Color value, Property property) {
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();
        
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);
        ImGui.PushID(property.Name);
        
        System.Numerics.Vector4 v4 = new(value.R, value.G, value.B, value.A);
        ImGui.ColorEdit4("", ref v4);
        value = new Color(v4.X, v4.Y, v4.Z, v4.W);
        
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}

public class PropertyDrawerString : PropertyDrawer<string?> {
    
    protected override void DrawProperty(ref string? value, Property property) {
        
        value ??= string.Empty;
        
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();

        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);
        ImGui.PushID(property.Name);
        ImGui.InputText("", ref value, 30);
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}

public class PropertyDrawerVector3 : PropertyDrawer<Vector3> {
    
    protected override void DrawProperty(ref Vector3 value, Property property) {
        System.Numerics.Vector3 v3 = new System.Numerics.Vector3(value.X, value.Y, value.Z);
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();
        
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X / 3 - 20);
        ImGui.PushID(property.Name);
        ImGui.Text("X");
        ImGui.SameLine();
        ImGui.DragFloat("##X", ref v3.X);
        ImGui.SameLine();
        ImGui.Text("Y");
        ImGui.SameLine();
        ImGui.DragFloat("##Y", ref v3.Y);
        ImGui.SameLine();
        ImGui.Text("Z");
        ImGui.SameLine();
        ImGui.DragFloat("##Z", ref v3.Z);
//        ImGui.DragFloat3("", ref v3, 0.01f);
        value = new Vector3(v3.X, v3.Y, v3.Z);
        
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}
