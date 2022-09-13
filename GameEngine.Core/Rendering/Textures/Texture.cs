using GameEngine.Core.AssetManagement;

namespace GameEngine.Core.Rendering.Textures; 

public abstract class Texture : IAsset {
    
    public abstract void Bind(uint slot = 0);

}
