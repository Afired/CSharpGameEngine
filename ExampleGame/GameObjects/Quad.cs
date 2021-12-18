using GameEngine.Components;

namespace ExampleGame.GameObjects; 

public class Quad : GameObject, ITransform, IGeometry, IRenderer, IRigidBody {
    
    public Transform Transform { get; set; }
    public Geometry Geometry { get; set; }
    public Renderer Renderer { get; set; }
    public RigidBody RigidBody { get; set; }


    public Quad() {
        
        float[] vertices = {
            -0.5f, 0.5f, 1f,  // top left
            0.5f, 0.5f, 0f,   // top right
            -0.5f, -0.5f, 0f, // bottom left

            0.5f, 0.5f, 0f,   // top right
            0.5f, -0.5f, 0f,  // bottom right
            -0.5f, -0.5f, 0f, // bottom left
        };
        
        Transform = new Transform(this);
        Geometry = new Geometry(this, vertices);
        Renderer = new Renderer(this);
        RigidBody = new RigidBody(this);
    }
    
}