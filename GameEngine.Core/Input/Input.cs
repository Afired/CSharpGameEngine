﻿using System;
using System.Collections.Generic;
using System.Reflection;
using GameEngine.Core.Numerics;
using Silk.NET.GLFW;

namespace GameEngine.Core.Input; 

public static class Input {
    
    public static Vector2 MouseDelta { get; internal set; }
    private static Dictionary<Keys, bool> _inputData;
    
    
    static Input() {
        _inputData = new Dictionary<Keys, bool>();

        foreach (Keys keyCode in GetEnumValues<Keys>()) {
            if(!_inputData.ContainsKey(keyCode))
                _inputData.Add(keyCode, false);
        }
    }
    
    public static bool IsKeyDown(KeyCode key) {
        return _inputData[(Keys) key];
    }
    
    internal static void SetKeyState(Keys key, bool state) {
        _inputData[key] = state;
    }
    
    private static IEnumerable<T> GetEnumValues<T>() where T : Enum {
        FieldInfo[] fieldInfos = typeof(T).GetFields();
        
        foreach (FieldInfo fieldInfo in fieldInfos) {
            if (fieldInfo.IsLiteral)
                yield return (T) fieldInfo.GetValue(null);
        }
    }
    
}
