using System.Collections.Generic;
using GameEngine.Core.Rendering.Shaders;

namespace GameEngine.Core.AssetManagement; 

public class ShaderImporter : AssetImporter<Shader> {
    
    public override string[] GetExtensions() => new[] { "glsl" };
    
    public override Shader Import(string path) {
        return new Shader(path);
    }
    
    public override IEnumerable<Shader?> Import(string[] paths) {
        for(int i = 0; i < paths.Length; i++) {
            yield return Import(paths[i]);
        }
    }
    
}
