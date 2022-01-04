using GameEngine.Editor.EditorWindows;
using GameEngine.Layers;
using ImGuiNET;

namespace GameEngine.Editor; 

public class EditorGui {
    
    public EditorGui() {
        Initialize();
    }
    
    private void Initialize() {
        new EditorMenubar();
        new EditorDockSpace();
        new HierarchyWindow();
        new ViewportWindow();
        new InspectorWindow();
        new ConsoleWindow();
        new AssetBrowserWindow();
        new ProjectSettingsWindow();
        DefaultOverlayLayer.OnDraw += RenderDemoWindow;
    }

    private void RenderDemoWindow() {
        ImGui.ShowDemoWindow();
    }
    
}
