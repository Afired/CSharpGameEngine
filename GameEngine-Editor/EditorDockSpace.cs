using GameEngine.Rendering;
using ImGuiNET;

namespace GameEngine.Editor; 

public class EditorDockSpace {

    public EditorDockSpace() {
        RenderingEngine.LayerStack.DefaultOverlayLayer.OnDraw += Draw;
    }

    private void Draw() {
        ImGui.DockSpaceOverViewport();
    }
    
}
