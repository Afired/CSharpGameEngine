using System;
using GameEngine.Core;
using GameEngine.Geometry;
using GameEngine.Rendering.Camera2D;

namespace ExampleGame;

internal class Program {
    
    public static int Main(string[] args) {
        Game game = new Game();
        
        game.Initialize();
        
        InitializeWorld();
        
        game.Start();
        
        return 0;
    }
    
    private static void InitializeWorld() {
        Camera2D camera = new Camera2D(10.0f);
        Game.SetActiveCamera(camera);

        Sprite sprite1 = new Sprite();
        Game.OnUpdate += deltaTime => sprite1.Transform.Position.X += deltaTime;
        Game.OnUpdate += deltaTime => sprite1.Transform.Position.Y += deltaTime;
        Sprite sprite2 = new Sprite();
        sprite2.Transform.Position.Y = -4;
        Sprite sprite3 = new Sprite();
        Game.OnUpdate += deltaTime => sprite3.Transform.Position.X = (float) Math.Sin(Time.TotalTimeElapsed * 5f);
    }
    
}
