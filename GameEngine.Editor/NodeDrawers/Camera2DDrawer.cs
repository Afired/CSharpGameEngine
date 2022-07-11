using GameEngine.Core.Nodes;
using GameEngine.Core.Rendering;
using ImGuiNET;
using Renderer = GameEngine.Core.Rendering.Renderer;

namespace GameEngine.Editor.NodeDrawers; 

public class BaseCameraDrawer : NodeDrawer<BaseCamera> {
    
    protected override void DrawNode(BaseCamera node) {
        DrawDefaultDrawers(node, typeof(BaseCamera));
//        ImGui.Spacing();
        if(ImGui.Button("Set active"))
            Renderer.SetActiveCamera(node);
    }
    
}
