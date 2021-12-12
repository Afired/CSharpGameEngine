using System;
using GameEngine;
using GameEngine.Core;
using GameEngine.Geometry;
using GameEngine.Rendering.Camera2D;

namespace ExampleGame;

internal class Program {
    
    public static int Main(string[] args) {
        Game game = new Game();

        SetConfig();
        
        game.Initialize();
        
        InitializeWorld();
        
        game.Start();
        
        return 0;
    }

    private static void SetConfig() {
        Configuration.TargetFrameRate = 144;
        Configuration.WindowTitle = "Example Game";
        Configuration.DoUseVsync = true;
    }
    
    private static void InitializeWorld() {
        Camera2D camera = new Camera2D(10.0f);
        Game.SetActiveCamera(camera);
        
        Sprite sprite1 = new Sprite();
        Game.OnUpdate += deltaTime => sprite1.Transform.Position += new Vector3(1f, 1f, 0) * deltaTime;
        Sprite sprite2 = new Sprite();
        sprite2.Transform.Position = new Vector3(0f, -4f, 0f);
        PlayerController playerController = new PlayerController(sprite2);
        Sprite sprite3 = new Sprite();
        Game.OnUpdate += deltaTime => sprite3.Transform.Position = new Vector3((float) Math.Sin(Time.TotalTimeElapsed * 5f), 0, 0);
    }
    
}
