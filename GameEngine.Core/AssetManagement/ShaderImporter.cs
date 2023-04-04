using System.Collections.Generic;
using GameEngine.Core.Rendering.Shaders;

namespace GameEngine.Core.AssetManagement; 

public class ShaderImporter : AssetImporter<Shader> {
    
    public override string[] GetExtensions() => new[] { "glsl" };
    
    public override Shader Import(string path) {
        return new Shader(Application.Instance.Renderer.MainWindow.Gl, path);
    }
    
}
