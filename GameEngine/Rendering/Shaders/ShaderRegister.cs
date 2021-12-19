using System.Collections.Generic;
using System.IO;
using GameEngine.AssetManagement;
using GameEngine.Debugging;
using GameEngine.Rendering.Textures;

namespace GameEngine.Rendering.Shaders; 

public static class ShaderRegister {
    
    private static Dictionary<string, Shader> _shaderRegister;

    private static Shader _invalidShaderShader;
    
    
    static ShaderRegister() {
        _shaderRegister = new Dictionary<string, Shader>();
    }

    public static void Register(string name, Shader shader) {
        //todo: throw new duplicate shader exception
        _shaderRegister.Add(name, shader);
    }

    public static Shader Get(string name) {
        name = name.ToLower();
        if(_shaderRegister.TryGetValue(name, out Shader shader))
            return shader;
        else {
            Console.LogWarning($"Shader not found '{name}'");
            return _invalidShaderShader;
        }
    }

    public static void Load() {
        DefaultShader.Initialize();
        foreach(string path in AssetManager.GetAllShaderPaths()) {
            Register(Path.GetFileNameWithoutExtension(path).ToLower(), new Shader(path));
        }
        _invalidShaderShader = InvalidShader.Create();
    }
    
}
