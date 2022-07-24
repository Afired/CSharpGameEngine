using System.Reflection;

namespace GameEngine.Editor.NodeDrawers; 

public static class ReflectionHelper {
    
    public static List<Type> GetDerivedTypes(Type baseType, Assembly assembly) {
        // Get all types from the given assembly
        Type[] types = assembly.GetTypes();
        List<Type> derivedTypes = new List<Type>();
        
        for (int i = 0, count = types.Length; i < count; i++) {
            Type type = types[i];
            if (IsSubclassOf(type, baseType)) {
                // The current type is derived from the base type,
                // so add it to the list
                derivedTypes.Add(type);
            }
        }
        
        return derivedTypes;
    }
    
    public static bool IsSubclassOf(Type type, Type baseType) {
        if (type == null || baseType == null || type == baseType)
            return false;
        
        if (baseType.IsGenericType == false) {
            if (type.IsGenericType == false)
                return type.IsSubclassOf(baseType);
        } else {
            baseType = baseType.GetGenericTypeDefinition();
        }
        
        type = type.BaseType;
        Type objectType = typeof(object);
        
        while (type != objectType && type != null) {
            Type curentType = type.IsGenericType ?
                type.GetGenericTypeDefinition() : type;
            if (curentType == baseType)
                return true;

            type = type.BaseType;
        }
        return false;
    }
    
}