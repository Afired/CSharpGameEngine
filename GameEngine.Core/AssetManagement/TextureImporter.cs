using System.Collections.Generic;
using GameEngine.Core.Rendering.Textures;

namespace GameEngine.Core.AssetManagement; 

public class TextureImporter : AssetImporter<Texture2D> {
    
    public override string[] GetExtensions() => new[] { "png" };
    
    public override Texture2D? Import(string path) {
        return Texture2D.Create(path);
    }
    
}
