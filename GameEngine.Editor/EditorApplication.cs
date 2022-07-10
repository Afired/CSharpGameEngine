using System.Diagnostics;
using GameEngine.Core;
using GameEngine.Core.Layers;
using GameEngine.Core.Physics;
using GameEngine.Core.Rendering;
using GameEngine.Core.SceneManagement;
using GameEngine.Editor.PropertyDrawers;

namespace GameEngine.Editor;

public unsafe class EditorApplication : Application<EditorApplication> {
    
    private const string EXTERNAL_EDITOR_ASSEMBLY_PROJ_DIR = @"D:\Dev\C#\CSharpGameEngine\ExampleProject\src\ExampleGame.Editor";
    private const string EXTERNAL_EDITOR_ASSEMBLY_DLL = @"D:\Dev\C#\CSharpGameEngine\ExampleProject\bin\Debug\net6.0\ExampleGame.Editor.dll";
    
    internal EditorLayer EditorLayer { get; private set; }
    
    public override void Initialize() {
        base.Initialize();
        EditorLayer = new EditorLayer();
        Renderer.LayerStack.Push(EditorLayer, LayerType.Overlay);
        EditorGui editorGui = new();
    }
    
    protected override void CompileExternalAssemblies() {
        base.CompileExternalAssemblies();
        CompileExternalAssembly(EXTERNAL_EDITOR_ASSEMBLY_PROJ_DIR);
    }
    
    public override void LoadExternalAssemblies() {
        _ealcm.LoadExternalAssembly(EXTERNAL_EDITOR_ASSEMBLY_DLL, true);
        base.LoadExternalAssemblies();
        PropertyDrawer.GenerateLookUp();
        _ealcm.AddUnloadTask(() => {
            Selection.Clear();
            PropertyDrawer.ClearLookUp();
            return true;
        });
    }
    
    protected override void Loop() {
        Stopwatch updateTimer = new();
        Stopwatch physicsTimer = new();
        updateTimer.Start();
        physicsTimer.Start();
        
        while(IsRunning) {
            
            if(IsReloadingExternalAssemblies)
                ReloadExternalAssemblies();
            
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
                if(PlayMode.Current == PlayMode.Mode.Playing) {
                    Hierarchy.PrePhysicsUpdate();
                    PhysicsEngine.DoStep();
                    Hierarchy.PhysicsUpdate(Configuration.FixedTimeStep);
                }
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
