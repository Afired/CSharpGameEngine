using System.Collections.Generic;
using ExampleGame.Entities;
using GameEngine;
using GameEngine.Entities;
using GameEngine.Numerics;

namespace ExampleGame.Scenes; 

public static class RigidBodyScene {

    public static Scene Get() {
        
        string name = "RigidBody Scene";

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
            new DynamicCamera() {
                Transform = { Position = new Vector3(0, 10, 0) },
                Camera2D = { Zoom = 50f }
            }
            
        };
        
        return new Scene(name, _entities);
    }
    
}