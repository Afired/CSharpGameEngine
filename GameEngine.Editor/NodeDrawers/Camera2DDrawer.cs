using GameEngine.Core.Nodes;
using ImGuiNET;

namespace GameEngine.Editor.NodeDrawers; 

public class Camera2DDrawer : NodeDrawer<Camera2D> {
    
    public override void DrawNode(Camera2D node) {
        ImGui.Text("Camera2D");
        ImGui.Spacing();
        DrawDefault(node);
    }
    
}
