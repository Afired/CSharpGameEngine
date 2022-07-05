using System.Diagnostics;
using GameEngine.Core.Core;
using GameEngine.Core.Layers;
using GameEngine.Core.Physics;
using GameEngine.Core.Rendering;
using GameEngine.Core.SceneManagement;

namespace GameEngine.Editor;

public unsafe class EditorApplication : Application<EditorApplication> {
    
    internal EditorLayer EditorLayer { get; private set; }
    public EditorMode Mode { get; set; } = EditorMode.Editing;
    
    public override void Initialize() {
        base.Initialize();
        InitializeEditor();
    }
    
    private void InitializeEditor() {
        EditorLayer = new EditorLayer();
        RenderingEngine.LayerStack.Push(EditorLayer, LayerType.Overlay);
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
            
            if(Mode == EditorMode.Playing) {
                Hierarchy.Awake();
                Hierarchy.Update(updateTime);
            }
            
            RenderingEngine.InputHandler.ResetMouseDelta();
            
            float physicsTime = (float) physicsTimer.Elapsed.TotalSeconds;
            if(physicsTime > Configuration.FixedTimeStep) {
                if(Mode == EditorMode.Playing)
                    PhysicsEngine.DoStep();
                physicsTimer.Restart();
            }
            
            RenderingEngine.Render();
            
            // handle input
            RenderingEngine.Glfw.PollEvents();
            RenderingEngine.InputHandler.HandleMouseInput(RenderingEngine.WindowHandle);
            
            if(RenderingEngine.Glfw.WindowShouldClose(RenderingEngine.WindowHandle))
                Terminate();
        }
        
    }
    
}
