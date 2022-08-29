using GameEngine.Editor.PropertyDrawers;
using ImGuiNET;

public class PropertyDrawerList<T> : PropertyDrawer<List<T?>> {
    
    protected override void DrawProperty(ref List<T?> list, Property property) {
        
        ImGui.Text($"{property.Name}[{list.Count}]");
        
        for(int i = 0; i < list.Count; i++) {
            list[i] = (T?) PropertyDrawer.DrawDirect(typeof(T), list[i], new Property() {
                Name = $"[{i}]",
                IsReadonly = false,
            });
        }
        
        if(ImGui.Button("Add")) {
            list.Add(default(T));
        }
        
        ImGui.SameLine();
        
        if(ImGui.Button("Remove Last")) {
            if(list.Count > 0)
                list.RemoveAt(list.Count - 1);
        }
        
    }
    
}
