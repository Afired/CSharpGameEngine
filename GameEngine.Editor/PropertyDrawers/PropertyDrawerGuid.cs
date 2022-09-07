using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerGuid : PropertyDrawer<Guid> {
    
    protected override void DrawProperty(ref Guid value, Property property) {
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();
        
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);
        ImGui.PushID(property.Name);
        
        string guidString = value.ToString();
        ImGui.InputText("", ref guidString, 100);
        if(Guid.TryParse(guidString, out Guid newGuid))
            value = newGuid;
        
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}
