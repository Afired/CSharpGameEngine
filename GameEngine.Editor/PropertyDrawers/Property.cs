using System.Reflection;

namespace GameEngine.Editor.PropertyDrawers; 

public readonly record struct Property {
    
    public string Name { get; init; }
    public bool IsReadonly { get; init; }
    // todo: get attributes
    
    public Property(PropertyInfo propertyInfo) {
        Name = propertyInfo.Name;
        IsReadonly = !propertyInfo.CanWrite;
    }
    
    public Property(FieldInfo fieldInfo) {
        Name = fieldInfo.Name;
        IsReadonly = false;
    }
    
}
