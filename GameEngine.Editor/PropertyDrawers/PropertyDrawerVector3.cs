using System.Numerics;
using GameEngine.Numerics;
using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerVector3<T> : PropertyDrawer<Vec3<T>> where T : struct, IFloatingPointIeee754<T> {
    
    protected override void DrawProperty(ref Vec3<T> vec3, Property property) {
        Vector3 vector3 = vec3.ToNumerics();
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();
        
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X / 3 - 20);
        ImGui.PushID(property.Name);
        ImGui.Text("X");
        ImGui.SameLine();
        ImGui.DragFloat($"##X", ref vector3.X);
        ImGui.SameLine();
        ImGui.Text("Y");
        ImGui.SameLine();
        ImGui.DragFloat($"##Y", ref vector3.Y);
        ImGui.SameLine();
        ImGui.Text("Z");
        ImGui.SameLine();
        ImGui.DragFloat($"##Z", ref vector3.Z);
        vec3 = vector3;
        
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}
