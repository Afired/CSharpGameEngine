﻿using System;
using GameEngine.Core;
using GameEngine.Geometry;
using GameEngine.Numerics;
using GameEngine.Rendering.Cameras;
using Console = GameEngine.Debugging.Console;

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
        Camera2D camera2d = new Camera2D(10.0f, true);
        Camera3D camera3d = new Camera3D();
        camera3d.Transform.Position = new Vector3(0, 0, -10);
        Game.SetActiveCamera(camera3d);
        
        Sprite sprite1 = new Sprite();
        Game.OnUpdate += deltaTime => sprite1.Transform.Position += new Vector3(1f, 1f, 0) * deltaTime;
        
        Sprite sprite2 = new Sprite();
        sprite2.Transform.Position = new Vector3(0f, 0f, 0f);
        new PlayerController(sprite2);
        
        Sprite sprite3 = new Sprite();
        Game.OnUpdate += deltaTime => sprite3.Transform.Position = new Vector3((float) Math.Sin(Time.TotalTimeElapsed * 5f), 0, 0);
    }
    
}
