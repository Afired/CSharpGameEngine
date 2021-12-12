using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameEngine.Input; 

public static class Input {
    
    private static Dictionary<KeyCode, bool> _inputData;
    
    
    static Input() {
        _inputData = new Dictionary<KeyCode, bool>();
        List<KeyCode> keyCodes = GetEnumValues<KeyCode>();
        for(int i = 0; i < keyCodes.Count; i++) {
            _inputData.Add(keyCodes[i], false);
        }
    }
    
    public static bool IsKeyDown(KeyCode key) {
        return _inputData[key];
    }
    
    internal static void SetKeyState(KeyCode key, bool state) {
        _inputData[key] = state;
    }
    
    // Return a list of an enumerated type's values.
    private static List<T> GetEnumValues<T>() where T : Enum {
        // Get the type's Type information.
        Type t_type = typeof(T);

        // Enumerate the Enum's fields.
        FieldInfo[] field_infos = t_type.GetFields();

        // Loop over the fields.
        List<T> results = new List<T>();
        foreach (FieldInfo field_info in field_infos) {
            // See if this is a literal value (set at compile time).
            if (field_info.IsLiteral) {
                // Add it.
                T value = (T)field_info.GetValue(null);
                results.Add(value);
            }
        }

        return results;
    }
    
}
