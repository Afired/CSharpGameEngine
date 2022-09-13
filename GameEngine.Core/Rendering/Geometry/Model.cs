using GameEngine.Core.AssetManagement;

namespace GameEngine.Core.Rendering.Geometry; 

public class Model : IAsset {
    
    public Mesh[] Meshes { get; }
    
    public Model(Mesh[] meshes) {
        Meshes = meshes;
    }
    
}
