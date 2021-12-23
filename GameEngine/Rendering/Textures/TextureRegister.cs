using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GameEngine.AssetManagement;
using GameEngine.Rendering.Textures;

namespace GameEngine.Rendering.Shaders; 

public static class TextureRegister {
    
    private static Dictionary<string, Texture> _textureRegister;
    
    
    static TextureRegister() {
        _textureRegister = new Dictionary<string, Texture>();
    }
    
    private static void Register(string name, Texture texture) {
        Debug.Assert(!_textureRegister.ContainsKey(name), "duplicate shader");
        _textureRegister.Add(name, texture);
    }
    
    public static Texture Get(string name) {
        name = name.ToLower();
        if(_textureRegister.TryGetValue(name, out Texture texture))
            return texture;
        Console.LogWarning($"Texture not found '{name}'");
        //todo: return missing texture texture
        throw new Exception(name);
    }
    
    public static void Load() {
        Console.Log($"Loading textures...");
        string[] paths = AssetManager.GetAllTexturePaths();
        for (int i = 0; i < paths.Length; i++) {
            Register(Path.GetFileNameWithoutExtension(paths[i]).ToLower(), new Texture2D(paths[i]));
            Console.LogSuccess($"Loading textures ({i + 1}/{paths.Length}) '{paths[i]}'");
        }
    }
    
}
