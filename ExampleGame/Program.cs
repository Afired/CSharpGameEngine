using System;
using GameEngine;
using GameEngine.Core;
using GameEngine.Geometry;
using GameEngine.Rendering.Camera2D;

namespace ExampleGame;

internal class Program {
    
    public static int Main(string[] args) {
        Game game = new Game();
        
        Game.OnUpdate += deltaTime => {
            // do something in update
        };
        
        Game.OnFixedUpdate += fixedDeltaTime => {
            // do something in fixed update
        };
        
        game.Initialize();

        Camera2D camera = new Camera2D(10.0f);
        camera.Transform.Position.X = 0;
        Game.SetActiveCamera(camera);

        Plane plane = new Plane();
        
        
        Game.OnUpdate += deltaTime => {
            //Game.CurrentCamera.Transform.Position.X += deltaTime * 1;
            //Console.WriteLine(camera.Transform.Position.X);
            //Game.CurrentCamera.Zoom += deltaTime * 5.0f;
            Game.CurrentCamera.Transform.Position.X += deltaTime;
        };
        
        game.Start();
        
        return 0;
    }
    
}
