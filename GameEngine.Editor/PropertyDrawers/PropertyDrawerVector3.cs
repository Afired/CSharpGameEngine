using GameEngine.Core.Numerics;
using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerVector3 : PropertyDrawer<Vector3> {
    
    protected override void DrawProperty(ref Vector3 vector3, Property property) {
        System.Numerics.Vector3 newVector3 = new System.Numerics.Vector3(vector3.X, vector3.Y, vector3.Z);
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();
        
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X / 3 - 20);
        ImGui.PushID(property.Name);
        ImGui.Text("X");
        ImGui.SameLine();
        ImGui.DragFloat($"##X", ref newVector3.X);
        ImGui.SameLine();
        ImGui.Text("Y");
        ImGui.SameLine();
        ImGui.DragFloat($"##Y", ref newVector3.Y);
        ImGui.SameLine();
        ImGui.Text("Z");
        ImGui.SameLine();
        ImGui.DragFloat($"##Z", ref newVector3.Z);
        vector3 = new Vector3(newVector3.X, newVector3.Y, newVector3.Z);
        
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}
