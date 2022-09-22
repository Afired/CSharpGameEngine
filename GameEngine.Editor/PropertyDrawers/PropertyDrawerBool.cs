using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerBool : PropertyDrawer<bool> {
    
    protected override void DrawProperty(ref bool value, Property property) {
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();

        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);
        ImGui.PushID(property.Name);
        ImGui.Checkbox("", ref value);
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}
