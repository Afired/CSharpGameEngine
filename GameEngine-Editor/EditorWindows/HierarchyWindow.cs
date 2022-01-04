using GameEngine.Entities;
using GameEngine.SceneManagement;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class HierarchyWindow : EditorWindow {

    public HierarchyWindow() {
        Title = "Hierarchy";
    }
    
    protected override void Draw() {
        if(Hierarchy.Instance.Count == 0) {
            ImGui.Text("no objects");
            return;
        }

        foreach(Entity entity in Hierarchy.Instance) {
            ImGui.Text(entity.GetType().ToString());
        }
        
    }
    
}
