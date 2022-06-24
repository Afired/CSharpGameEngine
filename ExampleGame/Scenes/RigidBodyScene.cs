using System.Collections.Generic;
using ExampleGame.Nodes;
using GameEngine.Core.Nodes;
using GameEngine.Core.Numerics;
using GameEngine.Core.SceneManagement;

namespace ExampleGame.Scenes; 

public class RigidBodyScene : Scene {

    public RigidBodyScene() {
        Name = "RigidBody Scene";
        
        _entities = new List<Node>() {
            
            // new PhysicsQuad() {
            //     LocalPosition = new Vector3(0, 10, 0),
            //     Renderer = { Texture = "Checkerboard", Shader = "default" }
            // },
            // new PhysicsQuad() {
            //     LocalPosition = new Vector3(0.5f, 11, 0),
            //     Renderer = { Texture = "Checkerboard", Shader = "default" },
            // },
            // new PhysicsQuad() {
            //     LocalPosition = new Vector3(-0.25f, 12, 0),
            //     Renderer = { Texture = "Checkerboard", Shader = "default" }
            // },
            // new Camera2D() {
            //     LocalPosition = new Vector3(0, 10, -5),
            //     Zoom = 10f
            // }
            
        };
    }
    
}
