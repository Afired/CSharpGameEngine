using ExampleGame.Entities;
using GameEngine;
using GameEngine.Numerics;
using GameEngine.Rendering;

namespace ExampleGame.Scenes; 

public static class TestScene {
    
    public static Scene Get() {
        Scene scene = new Scene();
        scene.Name = "Test Scene";
                
        scene.AddEntity(new PhysicsQuad() {
            Transform = { Position = new Vector3(0, 10, 0)},
            Renderer = { Texture = "Box", Shader = "default"}
        });
        scene.AddEntity(new PhysicsQuad() {
            Transform = { Position = new Vector3(0.5f, 11, 0)},
            Renderer = { Texture = "Checkerboard", Shader = "default"}
        });
        scene.AddEntity(new PhysicsQuad() {
            Transform = { Position = new Vector3(-0.25f, 12, 0)},
            Renderer = { Texture = "Checkerboard", Shader = "default"}
        });
        scene.AddEntity(new Quad() {
            Transform = { Position = new Vector3(0, 2, 0)} ,
            Renderer = { Texture = "Checkerboard", Shader = "default" }
        });
        Player player = new Player();
        scene.AddEntity(player);
        
        return scene;
    }
    
}
