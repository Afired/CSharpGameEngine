using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerArray : PropertyDrawer<Array> {
    
    protected override void DrawProperty(ref Array array, Property property) {
        
        int dimensions = array.GetType().GetArrayRank();
        
        //todo: nested arrays
        if(dimensions > 1) {
            Console.LogWarning($"Can't display multi-dimensional arrays (size: {dimensions})");
            return;
        }
        
        ImGui.Text($"{property.Name}({array.Length})");
        
        ImGui.Indent();
        
        for(int i = 0; i < array.Length; i++) {
            
            object? element = array.GetValue(i);
            
            Type elementType = array.GetType().GetElementType()!;
            
            array.SetValue(PropertyDrawer.DrawDirect(elementType, element, new Property() {
                Name = $"[{i}]",
                IsReadonly = false,
            }), i);
            
        }
        
        ImGui.Unindent();
        
    }
    
}
