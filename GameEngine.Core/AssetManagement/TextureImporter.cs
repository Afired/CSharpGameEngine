using System.Collections.Generic;
using GameEngine.Core.Rendering.Textures;

namespace GameEngine.Core.AssetManagement; 

public class TextureImporter : AssetImporter<Texture2D> {
    
    public override string[] GetExtensions() => new[] { "png", "hdr" };
    
    public override Texture2D? Import(string path) {
        return new Texture2D(Application.Instance.Renderer.MainWindow.Gl, path);
    }
    
}
