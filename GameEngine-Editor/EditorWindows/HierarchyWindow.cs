using GameEngine.Entities;
using GameEngine.SceneManagement;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class HierarchyWindow : EditorWindow {

    private Entity _selected;
    

    public HierarchyWindow() {
        Title = "Hierarchy";
    }
    
    protected override void Draw() {
        if(Hierarchy.Instance.Count == 0) {
            ImGui.Text("no objects");
            return;
        }

        foreach(Entity entity in Hierarchy.Instance) {
            DrawEntityNode(entity);
        }
        
    }

    private void DrawEntityNode(Entity entity) {
        ImGuiTreeNodeFlags treeNodeFlags = ImGuiTreeNodeFlags.OpenOnArrow | (_selected == entity ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None);
        bool opened = ImGui.TreeNodeEx(entity.GetType().ToString(), treeNodeFlags);
        if(ImGui.IsItemClicked()) {
            _selected = entity;
        }

        if(opened) {
            ImGui.Text("Test");
            ImGui.TreePop();
        }
    }
    
}
