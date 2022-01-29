using GameEngine.Core;
using GameEngine.Layers;
using GameEngine.Rendering;

namespace GameEngine.Editor;

public static class Program {
    
    internal static EditorLayer EditorLayer;

    public static int Main(string[] args) {

        Application application = new Application();
        SetConfig();
        
        application.Initialize();
        application.Start();
        
        RenderingEngine.OnLoad += InitializeEditor;
        
        return 0;
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
