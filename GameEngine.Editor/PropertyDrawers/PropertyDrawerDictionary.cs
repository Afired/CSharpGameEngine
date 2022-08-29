using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerDictionary<TKey, TValue> : PropertyDrawer<Dictionary<TKey, TValue?>> where TKey : notnull {
    
    protected override void DrawProperty(ref Dictionary<TKey, TValue?> dic, Property property) {
        
        ImGui.Text($"{property.Name}[{dic.Count}]");
        
        foreach((TKey? key, TValue? value) in dic) {
            
//            TKey? newKey = (TKey?) PropertyDrawer.DrawDirect(typeof(TKey), key, new Property() {
//                Name = $"-",
//                IsReadonly = false,
//            });
            
//            ImGui.SameLine();
            
//            TValue? newValue = (TValue?) PropertyDrawer.DrawDirect(typeof(TValue), value, new Property() {
//                Name = $"-",
//                IsReadonly = false,
//            });
            
//            if(newKey is not null)
//                dic[newKey] = newValue;
            
            ImGui.Text($"{key} => {value}");
        }
        
        if(ImGui.Button("Add")) {
            Console.LogWarning("Unimplemented");
        }
        
        ImGui.SameLine();
        
        if(ImGui.Button("Remove Last")) {
            Console.LogWarning("Unimplemented");
        }
        
    }
    
}
