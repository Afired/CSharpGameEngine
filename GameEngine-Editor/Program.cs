﻿using ExampleGame.Entities;
using GameEngine.Core;
using GameEngine.Layers;
using GameEngine.Numerics;
using GameEngine.Rendering;
using GameEngine.SceneManagement;

namespace GameEngine.Editor;

public static class Program {
    
    internal static EditorLayer EditorLayer;

    public static int Main(string[] args) {

        Application application = new Application();

        SetConfig();
        
        application.Initialize();

        InitializeWorld();
        
        application.Start();
        
        RenderingEngine.OnLoad += InitializeEditor;
        
        return 0;
    }

    private static void InitializeWorld() {
        Hierarchy.Add(new PhysicsQuad() {
            Transform = { Position = new Vector3(0, 10, 0)},
            Renderer = { Texture = "Box", Shader = "default"}
        });
        Hierarchy.Add(new PhysicsQuad() {
            Transform = { Position = new Vector3(0.5f, 11, 0)},
            Renderer = { Texture = "Checkerboard", Shader = "default"}
        });
        Hierarchy.Add(new PhysicsQuad() {
            Transform = { Position = new Vector3(-0.25f, 12, 0)},
            Renderer = { Texture = "Checkerboard", Shader = "default"}
        });
        Hierarchy.Add(new Quad() {
            Transform = { Position = new Vector3(0, 2, 0)} ,
            Renderer = { Texture = "Checkerboard", Shader = "default" }
        });
        Player player = new Player();
        RenderingEngine.SetActiveCamera(player.Camera2D);
        Hierarchy.Add(player);
    }

    private static void InitializeEditor() {
        EditorLayer = new EditorLayer();
        RenderingEngine.LayerStack.Push(EditorLayer, LayerType.Overlay);
        EditorGui editorGui = new EditorGui();
    }

    private static void SetConfig() {
        Configuration.TargetFrameRate = -1;
        Configuration.WindowTitle = "GameEngine-Editor";
        Configuration.DoUseVsync = false;
    }
    
}
