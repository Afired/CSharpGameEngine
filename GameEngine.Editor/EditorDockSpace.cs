using ImGuiNET;

namespace GameEngine.Editor; 

public class EditorDockSpace {

    public EditorDockSpace() {
        EditorApplication.Instance.EditorLayer.OnDraw += Draw;
    }

    private void Draw() {
        ImGui.DockSpaceOverViewport();
    }
    
}
