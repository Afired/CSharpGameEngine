using GameEngine.Components;

namespace ExampleGame.GameObjects; 

public class Pyramid : GameObject, ITransform, IGeometry, IRenderer, IRigidBody {
    
    public Transform Transform { get; set; }
    public Geometry Geometry { get; set; }
    public Renderer Renderer { get; set; }
    public RigidBody RigidBody { get; set; }
    
    
    public Pyramid(string texture, string shader) {
        
        float[] vertices = {
            //walls
            0, 1, 0,   // top
            1, -1, 1,  // bottom right
            -1, -1, 1, // bottom left
            
            0, 1, 0,   // top
            -1, -1, 1,  // bottom right
            -1, -1, -1, // bottom left
            
            0, 1, 0,    // top
            -1, -1, -1, // bottom right
            1, -1, -1,  // bottom left
            
            0, 1, 0,   // top
            1, -1, -1, // bottom right
            1, -1, 1,  // bottom left
            //base
            -1, -1, -1,
            1, -1, -1,
            -1, -1, 1,
            
            1, -1, 1,
            1, -1, -1,
            -1, -1, 1,
        };
        
        Transform = new Transform(this);
        Geometry = new Geometry(this, vertices);
        Renderer = new Renderer(this, texture, shader);
        RigidBody = new RigidBody(this);
    }
    
}
