using GameEngine.Core;
using GameEngine.Core.Nodes;
using ImGuiNET;

namespace GameEngine.Editor.NodeDrawers; 

public class BaseCameraDrawer : NodeDrawer<BaseCamera> {
    
    protected override void DrawNode(BaseCamera node) {
        DrawDefaultDrawers(node, typeof(BaseCamera));
//        ImGui.Spacing();
        if(ImGui.Button("Set active"))
            Application.Instance.Renderer.SetActiveCamera(node);
    }
    
}
