using GameEngine.Core.Numerics;
using GameEngine.Editor.PropertyDrawers;
using ImGuiNET;

namespace ExampleGame.Editor.PropertyDrawers;

public class Vector2Drawer : PropertyDrawer<Vector2> {
    
    protected override void DrawProperty(ref Vector2 value, Property property) {
        System.Numerics.Vector2 v2 = new System.Numerics.Vector2(value.X, value.Y);
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();
        
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X / 2 - 20);
        ImGui.PushID(property.Name);
        ImGui.Text("X");
        ImGui.SameLine();
        ImGui.DragFloat("##X", ref v2.X);
        ImGui.SameLine();
        ImGui.Text("Y");
        ImGui.SameLine();
        ImGui.DragFloat("##Y", ref v2.Y);
        value = new Vector2(v2.X, v2.Y);
        
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}
