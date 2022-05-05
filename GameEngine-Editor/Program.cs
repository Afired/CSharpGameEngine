using GameEngine.Core.Core;
using GameEngine.Core.Layers;
using GameEngine.Core.Rendering;

namespace GameEngine.Editor;

public static class Program {
    
    internal static EditorLayer EditorLayer;

    public static int Main(string[] args) {
        
        SetConfig();
        
        Application.Initialize();
        InitializeEditor();
        Application.Run();
        
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
