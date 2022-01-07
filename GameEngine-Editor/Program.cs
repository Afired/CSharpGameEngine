using ExampleGame.Entities;
using GameEngine.Core;
using GameEngine.Layers;
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
        Hierarchy.Instance.Add(new PhysicsCheckerboard());
        
        Player player = new Player();
        SetActiveCamera(player.Camera2D);
        Hierarchy.Instance.Add(player);
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
