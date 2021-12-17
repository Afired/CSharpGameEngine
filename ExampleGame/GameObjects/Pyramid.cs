namespace GameEngine.Components; 

public class Pyramid : GameObject, ITransform, IGeometry, IRenderer, IRigidBody {
    
    public Transform Transform { get; set; }
    public Geometry Geometry { get; set; }
    public Renderer Renderer { get; set; }
    public RigidBody RigidBody { get; set; }


    public Pyramid() {
        
        float[] vertices = {
            //walls
            0, 1, 0, 0.5f, 0.5f, 1.5f,   // top
            1, -1, 1, 0.5f, 0.5f, 1.5f,  // bottom right
            -1, -1, 1, 0.5f, 0.5f, 1.5f, // bottom left
            
            0, 1, 0, 0.5f, 1.5f, 0.5f,   // top
            -1, -1, 1, 0.5f, 1.5f, 0.5f,  // bottom right
            -1, -1, -1, 0.5f, 1.5f, 0.5f, // bottom left
            
            0, 1, 0, 1.5f, 0.5f, 0.5f,    // top
            -1, -1, -1, 1.5f, 0.5f, 0.5f, // bottom right
            1, -1, -1, 1.5f, 0.5f, 0.5f,  // bottom left
            
            0, 1, 0, 0.5f, 0.5f, 0.5f,   // top
            1, -1, -1, 0.5f, 0.5f, 0.5f, // bottom right
            1, -1, 1, 0.5f, 0.5f, 0.5f,  // bottom left
            //base
            -1, -1, -1, 0.5f, 0.5f, 0.5f,
            1, -1, -1, 0.5f, 0.5f, 0.5f,
            -1, -1, 1, 0.5f, 0.5f, 0.5f,
            
            1, -1, 1, 0.5f, 0.5f, 0.5f,
            1, -1, -1, 0.5f, 0.5f, 0.5f,
            -1, -1, 1, 0.5f, 0.5f, 0.5f,
        };
        
        Transform = new Transform(this);
        Geometry = new Geometry(this, vertices);
        Renderer = new Renderer(this);
        RigidBody = new RigidBody(this);
    }
    
}
