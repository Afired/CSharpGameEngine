using System.Collections.Generic;
using GameEngine.Debugging;
using GameEngine.Rendering.Textures;

namespace GameEngine.Rendering.Shaders; 

public static class TextureRegister {
    
    private static Dictionary<string, Texture> _textureRegister;
    
    
    static TextureRegister() {
        _textureRegister = new Dictionary<string, Texture>();
    }

    public static void Register(string name, Texture texture) {
        //todo: throw new duplicate shader exception
        _textureRegister.Add(name, texture);
    }

    public static Texture Get(string name) {
        if(_textureRegister.TryGetValue(name, out Texture texture))
            return texture;
        else
            throw new ShaderNotFoundException(name);
    }
    
}
