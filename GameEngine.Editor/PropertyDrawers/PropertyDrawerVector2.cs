using System.Numerics;
using GameEngine.Numerics;
using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerVector2<T> : PropertyDrawer<Vec2<T>> where T : struct, IFloatingPointIeee754<T> {
    
    protected override void DrawProperty(ref Vec2<T> vec2, Property property) {
        Vector2 vector2 = vec2.ToNumerics();
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();
        
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X / 2 - 20);
        ImGui.PushID(property.Name);
        ImGui.Text("X");
        ImGui.SameLine();
        ImGui.DragFloat("##X", ref vector2.X);
        ImGui.SameLine();
        ImGui.Text("Y");
        ImGui.SameLine();
        ImGui.DragFloat("##Y", ref vector2.Y);
        vec2 = vector2;
        
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}
