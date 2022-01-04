using GameEngine.Entities;
using GameEngine.SceneManagement;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows;

public delegate void OnSelect(Entity entity);

public class HierarchyWindow : EditorWindow {
    
    public static event OnSelect OnSelect;

    private Entity v_selected;
    public Entity Selected {
        get => v_selected;
        set {
            v_selected = value;
            OnSelect?.Invoke(v_selected);
        }
    }
    

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
        ImGuiTreeNodeFlags treeNodeFlags = ImGuiTreeNodeFlags.OpenOnArrow | (Selected == entity ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None);
        bool opened = ImGui.TreeNodeEx(entity.GetType().ToString(), treeNodeFlags);
        if(ImGui.IsItemClicked()) {
            Selected = entity;
        }

        if(opened) {
            ImGui.Text("Test");
            ImGui.TreePop();
        }
    }
    
}
