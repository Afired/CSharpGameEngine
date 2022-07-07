using System.Reflection;

namespace GameEngine.Editor.PropertyDrawers; 

public record class Property {
    
    private readonly MemberInfo _memberInfo;
    
    public Property(PropertyInfo propertyInfo) {
        _memberInfo = propertyInfo;
    }
    
    public Property(FieldInfo fieldInfo) {
        _memberInfo = fieldInfo;
    }
    
    public bool IsReadonly {
        get {
            return _memberInfo switch {
                FieldInfo => false,
                PropertyInfo propertyInfo => !propertyInfo.CanWrite,
                _ => throw new InvalidCastException()
            };
        }
    }
    
    public string Name => _memberInfo.Name;
    
    // todo: get attributes
}
