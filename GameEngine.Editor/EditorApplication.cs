using System.Diagnostics;
using GameEngine.Core.Core;
using GameEngine.Core.Layers;
using GameEngine.Core.Physics;
using GameEngine.Core.Rendering;
using GameEngine.Core.SceneManagement;

namespace GameEngine.Editor;

public unsafe class EditorApplication : Application<EditorApplication> {
    
    internal EditorLayer EditorLayer { get; private set; }
    
    public override void Initialize() {
        base.Initialize();
        InitializeEditor();
    }
    
    private void InitializeEditor() {
        EditorLayer = new EditorLayer();
        Renderer.LayerStack.Push(EditorLayer, LayerType.Overlay);
        EditorGui editorGui = new();
    }
    
    protected override void Loop() {
        Stopwatch updateTimer = new();
        Stopwatch physicsTimer = new();
        updateTimer.Start();
        physicsTimer.Start();
        
        while(IsRunning) {
            
            float updateTime = (float) updateTimer.Elapsed.TotalSeconds;
            if(Configuration.TargetFrameRate > 0) {
                TimeSpan timeOut = TimeSpan.FromSeconds(1 / Configuration.TargetFrameRate - updateTime);
                if(timeOut.TotalSeconds > 0) {
                    Thread.Sleep(timeOut);
                    updateTime = (float) updateTimer.Elapsed.TotalSeconds;
                }
            }
            updateTimer.Restart();
            
            if(PlayMode.Current == PlayMode.Mode.Playing) {
                Hierarchy.Awake();
                Hierarchy.Update(updateTime);
            }
            
            Renderer.InputHandler.ResetMouseDelta();
            
            float physicsTime = (float) physicsTimer.Elapsed.TotalSeconds;
            if(physicsTime > Configuration.FixedTimeStep) {
                if(PlayMode.Current == PlayMode.Mode.Playing)
                    PhysicsEngine.DoStep();
                physicsTimer.Restart();
            }
            
            Renderer.Render();
            
            // handle input
            Renderer.Glfw.PollEvents();
            Renderer.InputHandler.HandleMouseInput(Renderer.WindowHandle);
            
            if(Renderer.Glfw.WindowShouldClose(Renderer.WindowHandle))
                Terminate();
        }
        
    }
    
}
