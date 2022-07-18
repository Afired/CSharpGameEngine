using GameEngine.Core.Numerics;
using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerVector3 : PropertyDrawer<Vector3> {
    
    protected override void DrawProperty(ref Vector3 value, Property property) {
        System.Numerics.Vector3 v3 = new System.Numerics.Vector3(value.X, value.Y, value.Z);
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();
        
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X / 3 - 20);
        ImGui.PushID(property.Name);
        ImGui.Text("X");
        ImGui.SameLine();
        ImGui.DragFloat("##X", ref v3.X);
        ImGui.SameLine();
        ImGui.Text("Y");
        ImGui.SameLine();
        ImGui.DragFloat("##Y", ref v3.Y);
        ImGui.SameLine();
        ImGui.Text("Z");
        ImGui.SameLine();
        ImGui.DragFloat("##Z", ref v3.Z);
//        ImGui.DragFloat3("", ref v3, 0.01f);
        value = new Vector3(v3.X, v3.Y, v3.Z);
        
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}
