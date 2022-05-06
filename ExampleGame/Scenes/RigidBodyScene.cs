using System.Collections.Generic;
using ExampleGame.Entities;
using GameEngine.Core.Entities;
using GameEngine.Core.Numerics;
using GameEngine.Core.SceneManagement;

namespace ExampleGame.Scenes; 

public class RigidBodyScene : Scene {

    public RigidBodyScene() {
        Name = "RigidBody Scene";
        
        _entities = new List<Entity>() {
            
            new PhysicsQuad() {
                Transform = { Position = new Vector3(0, 10, 0) },
                Renderer = { Texture = "Checkerboard", Shader = "default" }
            },
            new PhysicsQuad() {
                Transform = { Position = new Vector3(0.5f, 11, 0) },
                Renderer = { Texture = "Checkerboard", Shader = "default" }
            },
            new PhysicsQuad() {
                Transform = { Position = new Vector3(-0.25f, 12, 0) },
                Renderer = { Texture = "Checkerboard", Shader = "default" }
            },
            new DynamicCamera() {
                Transform = { Position = new Vector3(0, 10, 0) },
                Camera2D = { Zoom = 50f }
            }
            
        };
    }
    
}
