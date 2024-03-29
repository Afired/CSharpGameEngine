﻿using System.Collections.Generic;
using System.IO;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Debugging;
using GameEngine.Core.Guard;
using GameEngine.Core.Rendering.Textures;

namespace GameEngine.Core.Rendering.Shaders; 

public static class ShaderRegister {
    
    private static readonly Dictionary<string, Shader> _shaderRegister = new();
    private static Shader _invalidShaderShader;
    
    public static void Register(string name, Shader shader) {
        name = name.ToLower();
        Throw.If(_shaderRegister.ContainsKey(name), "duplicate shader");
        _shaderRegister.Add(name, shader);
    }

    public static Shader Get(string name) {
        name = name.ToLower();
        if(_shaderRegister.TryGetValue(name, out Shader shader))
            return shader;
        Console.LogWarning($"Shader not found '{name}'");
        return _invalidShaderShader;
    }
    
    public static void Reload() {
        _shaderRegister.Clear();
        Console.Log($"Compiling shaders...");
        _invalidShaderShader = InvalidShader.Create();
        DefaultShader.Initialize();
        DiffuseShader.Initialize();
        string[] paths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension("glsl");
        for (int i = 0; i < paths.Length; i++) {
            Register(Path.GetFileNameWithoutExtension(paths[i]).ToLower(), new Shader(paths[i]));
            Console.LogSuccess($"Compiling shaders ({i + 1}/{paths.Length}) '{paths[i]}'");
        }
    }
        
}
