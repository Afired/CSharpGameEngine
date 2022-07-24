using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerFloat : PropertyDrawer<float> {
    
    protected override void DrawProperty(ref float value, Property property) {
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();

        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);
        ImGui.PushID(property.Name);
        ImGui.DragFloat("", ref value, 0.01f, float.MinValue, float.MaxValue, "%g");
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}
