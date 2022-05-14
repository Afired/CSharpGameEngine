using GameEngine.Core.Nodes;
using GameEngine.Core.Rendering;
using ImGuiNET;

namespace GameEngine.Editor.NodeDrawers; 

public class Camera2DDrawer : NodeDrawer<Camera2D> {
    
    protected override void DrawNode(Camera2D node) {
        DrawDefaultHeader(node);
        DrawDefaultDrawers(node);
        ImGui.Spacing();
        if(ImGui.Button("Set as active Camera"))
            RenderingEngine.SetActiveCamera(node);
    }
    
}
