using GameEngine.Editor.PropertyDrawers;
using ImGuiNET;

public class PropertyDrawerList<T> : PropertyDrawer<List<T?>> {
    
    protected override void DrawProperty(ref List<T?> list, Property property) {
        
        ImGui.PushID(property.Name);
        ImGui.BeginGroup();
        
        ImGui.Text($"{property.Name}({list.Count})");
        ImGui.SameLine();
        if(ImGui.Button("+"))
            list.Add(default(T));
        ImGui.SameLine();
        if(ImGui.Button("Clear"))
            list.Clear();
        
        ImGui.Indent();

        int indexToRemove = -1;
        
        for(int i = 0; i < list.Count; i++) {
//            ImGui.PushID(i);
            
            list[i] = (T?) PropertyDrawer.DrawDirect(typeof(T), list[i], new Property() {
                Name = $"[{i}]",
                IsReadonly = false,
            });
            
            ImGui.SameLine();
            
            if(ImGui.Button($"-##{i}"))
                indexToRemove = i;
            
//            ImGui.PopID();
        }
        
        if(indexToRemove != -1)
            list.RemoveAt(indexToRemove);
        
        ImGui.Unindent();
        
        ImGui.EndGroup();
        if(ImGui.BeginPopupContextItem("", ImGuiPopupFlags.MouseButtonRight)) {
            ImGui.Text(property.Name);
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
            if(ImGui.MenuItem("Clear"))
                list.Clear();
            ImGui.EndPopup();
        }
        
        ImGui.PopID();
        
    }
    
}
