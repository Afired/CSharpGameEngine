using GameEngine.AutoGenerator;
using GameEngine.Components;

namespace ExampleGame.Components; 

[RequireComponent(typeof(Geometry))]
public partial class Quad : Renderer {
    
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
