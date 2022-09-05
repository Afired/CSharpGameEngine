using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerEnum : PropertyDrawer<Enum> {
    
    protected override void DrawProperty(ref Enum value, Property property) {
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();

        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);
        ImGui.PushID(property.Name);
        
        if(ImGui.BeginMenu(value.ToString())) {
            
            foreach(object o in Enum.GetValues(value.GetType())) {
                if(ImGui.MenuItem(o.ToString()))
                    value = (Enum) o;
            }
            
            ImGui.EndMenu();
        }
        
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}
