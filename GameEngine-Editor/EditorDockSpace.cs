using GameEngine.Layers;
using ImGuiNET;

namespace GameEngine.Editor; 

public class EditorDockSpace {

    public EditorDockSpace() {
        DefaultOverlayLayer.OnDraw += Draw;
    }

    private void Draw() {
        ImGui.DockSpaceOverViewport();
    }
    
}
