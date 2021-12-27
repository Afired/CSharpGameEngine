using ImGuiNET;

namespace GameEngine.Editor; 

public class EditorGui {
    
    public EditorGui() {
        Initialize();
    }
    
    private void Initialize() {
        OnImGui += OnImGuiDraw;
    }
    
    private void OnImGuiDraw() {
        //ImGui.DockSpaceOverViewport();
        ImGui.ShowDemoWindow();
    }
    
}
