using System.Collections.Generic;
using GameEngine.Core.Rendering.Geometry;

namespace GameEngine.Core.AssetManagement; 

public class ModelImporter : AssetImporter<Model> {
    
    public override string[] GetExtensions() => new[] { "fbx", "obj" };
    
    public override Model Import(string path) {
        return new Model(path);
    }
    
}
