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
            
            new PhysicsQuad() {
                Position = new Vector3(0, 10, 0),
                Renderer = { Texture = "Checkerboard", Shader = "default" }
            },
            new PhysicsQuad() {
                Position = new Vector3(0.5f, 11, 0),
                Renderer = { Texture = "Checkerboard", Shader = "default" },
            },
            new PhysicsQuad() {
                Position = new Vector3(-0.25f, 12, 0),
                Renderer = { Texture = "Checkerboard", Shader = "default" }
            },
            new Camera2D() {
                Position = new Vector3(0, 10, 0),
                Zoom = 50f
            }
            
        };
    }
    
}
