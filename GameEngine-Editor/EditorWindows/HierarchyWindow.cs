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
        if(Hierarchy.Scene is not null)
            DrawSceneNode(Hierarchy.Scene);
        else
            ImGui.Text("no scene loaded");
        if(ImGui.IsMouseDown(ImGuiMouseButton.Left) && ImGui.IsWindowHovered()) {
            Selected = null;
        }
    }

    private void DrawSceneNode(Scene scene) {
        
        ImGuiTreeNodeFlags treeNodeFlags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.None;
        bool opened = ImGui.TreeNodeEx("scene " + scene.Name, treeNodeFlags);
        if(opened) {
            
            foreach(Entity entity in scene.Entities) {
                DrawEntityNode(entity);
            }
            
            ImGui.TreePop();
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
