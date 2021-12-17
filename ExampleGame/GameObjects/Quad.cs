namespace GameEngine.Components; 

public class Quad : GameObject, ITransform, IGeometry, IRenderer, IRigidBody {
    
    public Transform Transform { get; set; }
    public Geometry Geometry { get; set; }
    public Renderer Renderer { get; set; }
    public RigidBody RigidBody { get; set; }


    public Quad() {
        Transform = new Transform(this);
        Geometry = new Geometry(this);
        Renderer = new Renderer(this);
        RigidBody = new RigidBody(this);
    }
    
}