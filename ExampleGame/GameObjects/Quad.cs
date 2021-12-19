using GameEngine.Components;

namespace ExampleGame.GameObjects; 

public class Quad : GameObject, ITransform, IGeometry, IRenderer {
    
    public Transform Transform { get; set; }
    public Geometry Geometry { get; set; }
    public Renderer Renderer { get; set; }
    
    
    public Quad(string texture) {
        
        float[] vertexData = {
            -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,   // top left
            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,    // top right
            -0.5f, -0.5f, 0.0f, 0.0f , 0.0f, // bottom left

            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,    // top right
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f,   // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,  // bottom left
        };
        
        Transform = new Transform(this);
        Geometry = new Geometry(this, vertexData);
        Renderer = new Renderer(this, texture);
    }
    
}
