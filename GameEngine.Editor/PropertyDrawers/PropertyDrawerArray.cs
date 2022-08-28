using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerArray : PropertyDrawer<Array> {
    
    protected override void DrawProperty(ref Array? array, Property property) {
        
        if(array is null) {
            ImGui.Text("Array is null");
        } else {
            
            ImGui.Text("Start Array");
            for(int i = 0; i < array.Length; i++) {

                object? element = array.GetValue(i);

                if(element is null) {
                    ImGui.Text("Element is null");
                    continue;
                }
                
                array.SetValue(PropertyDrawer.DrawDirect(element.GetType(), element, property), i);
            }
            ImGui.Text("End Array");
            
        }
    }
    
}
