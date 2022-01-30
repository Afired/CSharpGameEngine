using System.Collections.Generic;
using ExampleGame.Entities;
using GameEngine;
using GameEngine.Entities;
using GameEngine.Numerics;
using GameEngine.Rendering;

namespace ExampleGame.Scenes; 

public static class TestScene {
    
    public static Scene Get() {
        
        string name = "Test Scene";

        List<Entity> _entities = new List<Entity>() {

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
            new Quad() {
                Transform = { Position = new Vector3(0, 2, 0) },
                Renderer = { Texture = "Checkerboard", Shader = "default" }
            },
            new Bullet() {
                Transform = { Position = new Vector3(-5, 0, 0) },
                Renderer = { Texture = "Box", Shader = "default" }
            },
            new Bullet() {
                Transform = { Position = new Vector3(5, 0, 0) },
                Renderer = { Texture = "Box", Shader = "default" }
            },
            new Player() {
                Transform = { Position = new Vector3(0, 2, 0) },
                Renderer = { Texture = "Box", Shader = "default" },
                PlayerController = { Speed = 10f }
            },
            new StaticCamera() {
                Transform = { Position = new Vector3(0, 0, 0) },
                Camera2D = { Zoom = 50f }
            }
            
        };
        
        return new Scene(name, _entities);
    }
    
}
