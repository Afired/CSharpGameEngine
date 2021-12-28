using ImGuiNET;

namespace GameEngine.Editor; 

public class EditorDockSpace {

    public EditorDockSpace() {
        OnImGui += Draw;
    }

    private void Draw() {
        ImGui.DockSpaceOverViewport();
    }
    
}
