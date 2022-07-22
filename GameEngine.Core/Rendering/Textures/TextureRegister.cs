using System;
using System.Collections.Generic;
using System.IO;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Guard;

namespace GameEngine.Core.Rendering.Textures;

public static class TextureRegister {
    
    private static readonly Dictionary<string, Texture> _textureRegister = new();
    
    private static void Register(string name, Texture texture) {
        name = name.ToLower();
        Throw.If(_textureRegister.ContainsKey(name), "duplicate texture");
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
