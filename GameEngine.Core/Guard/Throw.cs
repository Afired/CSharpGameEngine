using System;

namespace GameEngine.Core.Guard; 

public static class Throw {
    
    public static void If(bool condition, string message = "") {
        if(condition)
            throw new Exception(message);
    }
    
    public static void IfNull(object value, string message = "") {
        if(value is null)
            throw new Exception(message);
    }
    
    public static void IfNullOrEmpty(string value, string message = "") {
        if(string.IsNullOrEmpty(value))
            throw new Exception(message);
    }
    
}
