using GameEngine.Components;
using GameEngine.Entities;

namespace ExampleGame.Entities; 

public partial class Quad : Entity, ITransform, IGeometry, IRenderer {
    
    protected override void OnAwake() {
        base.OnAwake();
        float[] vertexData = {
            -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,   // top left
            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,    // top right
            -0.5f, -0.5f, 0.0f, 0.0f , 0.0f, // bottom left

            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,   // top right
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        };

        Geometry.VertexData = vertexData;
    }
    
}
