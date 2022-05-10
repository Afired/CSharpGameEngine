using GameEngine.Core;
using GameEngine.Core.Entities;
using GameEngine.Core.SceneManagement;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows;

public delegate void OnSelect(Node node);

public class HierarchyWindow : EditorWindow {
    
    public static event OnSelect OnSelect;

    private Node v_selected;
    public Node Selected {
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
        ImGui.PushID(scene.GetHashCode());
        bool opened = ImGui.TreeNodeEx("Scene: " + scene.Name, treeNodeFlags);
        ImGui.PopID();
        
        if(opened) {
            foreach(Node entity in scene.Entities) {
                DrawEntityNode(entity);
            }
            ImGui.TreePop();
        }
        
    }

    private void DrawEntityNode(Node node) {
        ImGuiTreeNodeFlags treeNodeFlags = ImGuiTreeNodeFlags.OpenOnArrow | (Selected == node ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None);
        ImGui.PushID(node.GetHashCode());
        bool opened = ImGui.TreeNodeEx(node.GetType().ToString(), treeNodeFlags);
        ImGui.PopID();
        if(ImGui.IsItemClicked()) {
            Selected = node;
        }

        if(opened) {
            ImGui.Text("Test");
            ImGui.TreePop();
        }
    }
    
}
