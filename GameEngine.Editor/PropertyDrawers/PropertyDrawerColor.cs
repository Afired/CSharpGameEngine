using GameEngine.Core.Rendering;
using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerColor : PropertyDrawer<Color> {
    
    protected override void DrawProperty(ref Color value, Property property) {
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();
        
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);
        ImGui.PushID(property.Name);
        
        System.Numerics.Vector4 v4 = new(value.R, value.G, value.B, value.A);
        ImGui.ColorEdit4("", ref v4);
        value = new Color(v4.X, v4.Y, v4.Z, v4.W);
        
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}
