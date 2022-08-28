using GameEngine.Editor.PropertyDrawers;
using ImGuiNET;

public class PropertyDrawerList<T> : PropertyDrawer<List<T?>> {
    
    protected override void DrawProperty(ref List<T?> list, Property property) {
        
//        if(list is null)
//            if(ImGui.Button("Initialize"))
//                list = new List<T?>();
        
//        if(list is null)
//            return;
        
        ImGui.Text(property.Name);
        
        for(int i = 0; i < list.Count; i++) {
            list[i] = (T?) PropertyDrawer.DrawDirect(typeof(T), list[i], new Property() {
                Name = $"[{i}]",
                IsReadonly = false,
            });
        }
        
        if(ImGui.Button("Add")) {
            list.Add(default(T));
        }
        
        if(ImGui.Button("Remove Last")) {
            if(list.Count > 0)
                list.RemoveAt(list.Count - 1);
        }
        
    }
    
}
