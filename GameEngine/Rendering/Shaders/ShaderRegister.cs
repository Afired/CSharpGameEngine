using System.Collections.Generic;
using System.IO;
using GameEngine.AssetManagement;
using GameEngine.Debugging;
using GameEngine.Guard;
using GameEngine.Rendering.Textures;

namespace GameEngine.Rendering.Shaders; 

public static class ShaderRegister {
    
    internal static Dictionary<string, Shader> _shaderRegister;

    private static Shader _invalidShaderShader;
    
    
    static ShaderRegister() {
        _shaderRegister = new Dictionary<string, Shader>();
    }

    public static void Register(string name, Shader shader) {
        Throw.If(_shaderRegister.ContainsKey(name), "duplicate shader");
        _shaderRegister.Add(name, shader);
    }

    public static Shader Get(string name) {
        if(name is null) {
            Console.LogWarning($"Shader not found 'null'");
            return _invalidShaderShader;
        }
        name = name.ToLower();
        if(_shaderRegister.TryGetValue(name, out Shader shader))
            return shader;
        Console.LogWarning($"Shader not found '{name}'");
        return _invalidShaderShader;
    }
    
    public static void Load() {
        Console.Log($"Compiling shaders...");
        _invalidShaderShader = InvalidShader.Create();
        DefaultShader.Initialize();
        string[] paths = AssetManager.GetAllShaderPaths();
        for (int i = 0; i < paths.Length; i++) {
            Register(Path.GetFileNameWithoutExtension(paths[i]).ToLower(), new Shader(paths[i]));
            Console.LogSuccess($"Compiling shaders ({i + 1}/{paths.Length}) '{paths[i]}'");
        }
    }
    
}
