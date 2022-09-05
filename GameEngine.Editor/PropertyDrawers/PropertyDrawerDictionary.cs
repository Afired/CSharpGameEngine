using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerDictionary<TKey, TValue> : PropertyDrawer<Dictionary<TKey, TValue?>> where TKey : notnull {
    
    protected override void DrawProperty(ref Dictionary<TKey, TValue?> dic, Property property) {
        
        ImGui.PushID(property.Name);
        ImGui.Text($"{property.Name}({dic.Count})");
        
        ImGui.Indent();
        
        Stack<(TKey oldKey, TKey? newKey, TValue? newValue)> changes = new();
        int i = 0;
        foreach((TKey? key, TValue? value) in dic) {
            
            TKey? newKey = (TKey?) PropertyDrawer.DrawDirect(typeof(TKey), key, new Property() {
                Name = $"[{i}] Key",
                IsReadonly = false,
            });
            
//            ImGui.SameLine();
            
            TValue? newValue = (TValue?) PropertyDrawer.DrawDirect(typeof(TValue), value, new Property() {
                Name = $"[{i}] Value",
                IsReadonly = false,
            });
            
            changes.Push((key, newKey, newValue));
            
            i++;
        }
        
        while(changes.TryPop(out (TKey oldKey, TKey? newKey, TValue? newValue) change)) {
            TKey oldKey = change.oldKey;
            TKey? newKey = change.newKey;
            TValue? newValue = change.newValue;
            
            if(!oldKey.Equals(newKey))
                dic.Remove(oldKey);
            if(newKey is not null)
                dic[newKey] = newValue;
        }
        
        if(ImGui.Button("Add")) {
            Console.LogWarning("Unimplemented");
        }
        
        ImGui.SameLine();
        
        if(ImGui.Button("Clear"))
            dic.Clear();
        
        ImGui.Unindent();
        
        ImGui.PopID();
    }
    
}
