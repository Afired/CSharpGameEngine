using System;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Rendering.Textures;

namespace GameEngine.Core.Rendering.Materials; 


public class Material : IAsset {
    
    public Shader Shader { get; }
    public Texture Texture { get; }
    
    public Material(Shader shader, Texture texture) {
        Shader = shader;
        Texture = texture;
    }
    
    public void Dispose() {
        //TODO: dispose
    }
    
}
