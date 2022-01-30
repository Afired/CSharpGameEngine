using System;
using GameEngine.Components;
using GameEngine.Entities;

namespace ExampleGame.Entities; 

[Obsolete("Has to be edited to fit the new vertex data format")]
public partial class Pyramid : Entity, ITransform, IRenderer, IRigidBody {
    
    protected override void OnAwake() {
        base.OnAwake();
        
        float[] vertexData = {
            //walls
            0, 1, 0,   // top
            1, -1, 1,  // bottom right
            -1, -1, 1, // bottom left
            
            0, 1, 0,    // top
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

        //Geometry.VertexData = vertexData;
    }
    
}
