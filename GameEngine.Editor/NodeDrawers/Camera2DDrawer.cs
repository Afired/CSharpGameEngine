using GameEngine.Core.Nodes;
using GameEngine.Core.Rendering;
using ImGuiNET;
using Renderer = GameEngine.Core.Rendering.Renderer;

namespace GameEngine.Editor.NodeDrawers; 

public class Camera2DDrawer : NodeDrawer<Camera2D> {
    
    protected override void DrawNode(Camera2D node) {
        DrawDefaultDrawers(node, typeof(Camera2D));
//        ImGui.Spacing();
        if(ImGui.Button("Set as active Camera"))
            Renderer.SetActiveCamera(node);
    }
    
}
