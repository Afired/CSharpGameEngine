using System.Collections.Generic;
using ExampleGame.Entities;
using GameEngine.Core;
using GameEngine.Core.Entities;
using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering;

namespace ExampleGame.Scenes; 

public class TestScene : Scene {
    
    public TestScene() {
        Name = "Test Scene";
        
        _entities = new List<Entity>() {
            
            new Bullet() {
                Transform = { Position = new Vector3(-5, 0, 0) },
                Renderer = { Texture = "Box", Shader = "default" }
            },
            new Bullet() {
                Transform = { Position = new Vector3(5, 0, 0) },
                Renderer = { Texture = "Box", Shader = "default" }
            },
            new Enemy() {
                Transform = { Position = new Vector3(0, 5, 0) },
                Renderer = { Texture = "Checkerboard", Shader = "default" }
            },
            new Player() {
                Transform = { Position = new Vector3(0, 2, 0) },
                Renderer = { Texture = "Box", Shader = "default" },
                Movable = { Speed = 20f }
            },
            new DynamicCamera() {
                Transform = { Position = new Vector3(0, 10, 0) },
                Camera2D = { Zoom = 50f }
            }
            
        };
    }
    
}
