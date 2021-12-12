using GameEngine.Rendering.Shaders;

namespace GameEngine.Geometry; 

public interface IRendered : IGeometry {

    Shader Shader { get; set; }
    
    
    public void OnDraw();

}
