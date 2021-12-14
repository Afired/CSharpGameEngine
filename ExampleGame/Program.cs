﻿using GameEngine.Core;
using GameEngine.Geometry;
using GameEngine.Numerics;
using GameEngine.Rendering.Cameras;

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
        Camera3D camera3d = new Camera3D(75);
        camera3d.Transform.Position = new Vector3(0, 0, -5f);
        Game.SetActiveCamera(camera3d);
        new CameraController(camera3d);
        
        Pyramid sprite2 = new Pyramid();
        sprite2.Transform.Position = new Vector3(0f, 0f, 0f);
        
        Game.OnUpdate += deltaTime => sprite2.Transform.Rotation += new Vector3(deltaTime * 4, 0, 0);
        //new PlayerController(sprite2);
    }
    
}
