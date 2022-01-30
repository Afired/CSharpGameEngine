using GameEngine.Editor.EditorWindows;
using GameEngine.Rendering;
using ImGuiNET;

namespace GameEngine.Editor; 

public class EditorGui {
    
    public EditorGui() {
        Initialize();
    }
    
    private void Initialize() {
        // todo: make windows stay docked https://github.com/mellinoe/ImGui.NET/issues/202
        ImGui.GetIO().ConfigFlags = ImGuiConfigFlags.DockingEnable;
        ImGui.GetIO().BackendFlags = ImGuiBackendFlags.HasMouseCursors;
        ImGui.GetIO().ConfigInputTextCursorBlink = true;
        ImGui.GetIO().ConfigWindowsResizeFromEdges = true;
        ImGui.GetIO().ConfigWindowsMoveFromTitleBarOnly = true;
        ImGui.GetIO().MouseDrawCursor = true;
        
        new EditorDockSpace();
        new EditorMenubar();
        new HierarchyWindow();
        new ViewportWindow();
        new InspectorWindow();
        new ConsoleWindow();
        new AssetBrowserWindow();
        new SceneSelectWindow();
        //Program.EditorLayer.OnDraw += RenderDemoWindow;
    }

    private void RenderDemoWindow() {
        ImGui.ShowDemoWindow();
    }
    
}
