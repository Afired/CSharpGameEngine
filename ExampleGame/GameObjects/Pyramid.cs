namespace GameEngine.Components; 

public class Pyramid : GameObject, ITransform, IGeometry, IRenderer {
    public Transform Transform { get; set; }
    public Geometry Geometry { get; set; }
    public Renderer Renderer { get; set; }


    public Pyramid() {
        Transform = new Transform(this);
        Geometry = new Geometry(this);
        Renderer = new Renderer(this);
    }
    
}