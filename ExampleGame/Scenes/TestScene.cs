using System.Collections.Generic;
using ExampleGame.Nodes;
using GameEngine.Core.Nodes;
using GameEngine.Core.Numerics;
using GameEngine.Core.SceneManagement;

namespace ExampleGame.Scenes; 

public class TestScene : Scene {
    
    public TestScene() {
        Name = "Test Scene";
        
        _entities = new List<Node>() {
            
            new Bullet() {
                LocalPosition = new Vector3(-5, 0, 0),
                Renderer = { Texture = "Box", Shader = "default" }
            },
            new Bullet() {
                LocalPosition = new Vector3(5, 0, 0),
                Renderer = { Texture = "Box", Shader = "default" }
            },
            new Enemy() {
                LocalPosition = new Vector3(0, 5, 0),
                Renderer = { Texture = "Checkerboard", Shader = "default" }
            },
            new Player() {
                LocalPosition = new Vector3(0, 2, 0),
                Renderer = { Texture = "Box", Shader = "default" },
                Speed = 20f
            },
            new Camera2D() {
                LocalPosition = new Vector3(0, 10, -5),
                Zoom = 10f
            },
            new Renderer() {
                LocalPosition = new Vector3(2, 10, 0),
                Texture = "Box",
                Shader = "default",
            }
            
        };
    }
    
}
