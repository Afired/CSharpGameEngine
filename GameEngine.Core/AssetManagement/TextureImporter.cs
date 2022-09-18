using System.Collections.Generic;
using GameEngine.Core.Rendering.Textures;

namespace GameEngine.Core.AssetManagement; 

public class TextureImporter : AssetImporter<Texture2D> {
    
    public override string[] GetExtensions() => new[] { "png" };
    
    public override Texture2D Import(string path) {
        return new Texture2D(path);
    }
    
    public override IEnumerable<Texture2D?> Import(string[] paths) {
        for(int i = 0; i < paths.Length; i++) {
            yield return Import(paths[i]);
        }
    }
    
}
