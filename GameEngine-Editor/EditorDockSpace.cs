using GameEngine.Core.Rendering;
using ImGuiNET;

namespace GameEngine.Editor; 

public class EditorDockSpace {

    public EditorDockSpace() {
        Program.EditorLayer.OnDraw += Draw;
    }

    private void Draw() {
        ImGui.DockSpaceOverViewport();
    }
    
}
