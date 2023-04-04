using System.Numerics;
using GameEngine.Core.Nodes;
using GameEngine.Editor.PropertyDrawers;
using ImGuiNET;

namespace GameEngine.Editor.NodeDrawers; 

public class Transform3DDrawer : NodeDrawer<Transform3D> {
    
    private bool _local = true;
    
    protected override bool DrawHeader(Transform3D node) {
        bool opened = ImGui.CollapsingHeader(nameof(Transform3D), ImGuiTreeNodeFlags.DefaultOpen | ImGuiTreeNodeFlags.AllowItemOverlap);
        
            ImGui.PushStyleVar(ImGuiStyleVar.Alpha, _local ? 1.0f : 0.5f);
            float width = ImGui.GetContentRegionAvail().X;
            ImGui.SameLine(width - 35);
            if(ImGui.ImageButton((IntPtr) EditorResources.GetIcon("MinimizeIcon").Id, new Vector2(10, 10)))
                _local = true;
            ImGui.PopStyleVar();
            
            ImGui.PushStyleVar(ImGuiStyleVar.Alpha, _local ? 0.5f : 1.0f);
            ImGui.SameLine();
            if(ImGui.ImageButton((IntPtr)EditorResources.GetIcon("MaximizeIcon").Id, new Vector2(10, 10)))
                _local = false;
            ImGui.PopStyleVar();
        
        return opened;
    }
    
    protected override void DrawNode(Transform3D node) {
        if(_local) {
            DrawDefaultDrawers(node, typeof(Transform3D));
        } else {
//            PropertyDrawer.Draw(node, typeof(Transform3D).GetProperty(nameof(node.WorldPosition)));
//            PropertyDrawer.Draw(node, typeof(Transform3D).GetProperty(nameof(node.WorldRotation)));
//            PropertyDrawer.Draw(node, typeof(Transform3D).GetProperty(nameof(node.WorldScale)));
        }
    }
    
}
