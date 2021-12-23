using GameEngine.Components;
using GameEngine.Entities;

namespace ExampleGame.Entities; 

public class Quad : Entity, ITransform, IGeometry, IRenderer {
    
    public Transform Transform { get; set; }
    public Geometry Geometry { get; set; }
    public Renderer Renderer { get; set; }
    
    
    public Quad(string texture, string shader) {
        
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
        Renderer = new Renderer(this, texture, shader);
    }
    
}
