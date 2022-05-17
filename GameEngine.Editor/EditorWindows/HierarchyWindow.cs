using System.Numerics;
using System.Reflection;
using ExampleGame;
using GameEngine.Core.Nodes;
using GameEngine.Core.SceneManagement;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows;

public delegate void OnSelect(Node node);

public class HierarchyWindow : EditorWindow {
    
    public static event OnSelect OnSelect;

    private Node? v_selected;
    public Node? Selected {
        get => v_selected;
        set {
            v_selected = value;
            OnSelect?.Invoke(v_selected);
        }
    }
    

    public HierarchyWindow() {
        Title = "Outliner";
    }
    
    protected override void Draw() {
        if(ImGui.BeginMenuBar()) {
            ImGui.Text(Hierarchy.Scene is not null ? Hierarchy.Scene.Name : "no scene loaded");
            ImGui.EndMenuBar();
        }
        
        if(Hierarchy.Scene is not null)
            DrawSceneNode(Hierarchy.Scene);
        
        if(ImGui.IsMouseDown(ImGuiMouseButton.Left) && ImGui.IsWindowHovered()) {
            Selected = null;
        }
    }

    private void DrawSceneNode(Scene scene) {
        
        ImGuiTreeNodeFlags treeNodeFlags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.CollapsingHeader | ImGuiTreeNodeFlags.DefaultOpen;
        ImGui.PushID(scene.GetHashCode());
        bool opened = ImGui.TreeNodeEx("Scene: " + scene.Name, treeNodeFlags);
        ImGui.PopID();
        
        if(opened) {
            foreach(Node entity in scene.Entities) {
                DrawEntityNode(entity);
            }
            ImGui.TreePop();
            
            ImGui.Spacing();
            ImGui.Button("Create new Node", new Vector2(ImGui.GetContentRegionAvail().X + 10, 20));
            if(ImGui.BeginPopupContextItem("", ImGuiPopupFlags.MouseButtonLeft)) {
                ImGui.Text("Create new Node");
                ImGui.Spacing();
                ImGui.Separator();
                ImGui.Spacing();
                foreach(Type type in typeof(Node).Assembly.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(Node)))) {
                    if(ImGui.MenuItem(type.Name)) {
                        object[] parameters = type
                            .GetConstructors()
                            .Single()
                            .GetParameters()
                            .Select(p => (object)null!)
                            .ToArray();
                        Node newNode = (Node) Activator.CreateInstance(type, parameters)!;
                        Hierarchy.AddEntity(newNode);
                    }
                }
                foreach(Type type in typeof(AssemblyRef).Assembly.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(Node)))) {
                    if(ImGui.MenuItem(type.Name)) {
                        object[] parameters = type
                            .GetConstructors()
                            .Single()
                            .GetParameters()
                            .Select(p => (object)null!)
                            .ToArray();
                        Node newNode = (Node) Activator.CreateInstance(type, parameters)!;
                        Hierarchy.AddEntity(newNode);
                    }
                }
                ImGui.EndPopup();
            }
            
        }
        
    }

    private void DrawEntityNode(Node node) {
        ImGuiTreeNodeFlags treeNodeFlags =
            (node.ChildNodes.Count == 0 ? ImGuiTreeNodeFlags.Bullet : ImGuiTreeNodeFlags.OpenOnArrow) |
            (Selected == node ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None) |
            ImGuiTreeNodeFlags.SpanAvailWidth;
        ImGui.PushID(node.GetHashCode());
        bool opened = ImGui.TreeNodeEx(node.GetType().ToString(), treeNodeFlags);
        ImGui.PopID();
        if(ImGui.IsItemClicked()) {
            Selected = node;
        }

        if(ImGui.BeginPopupContextItem()) {
            if(ImGui.MenuItem("Delete Node"))
                Hierarchy.DeleteEntity(node.GetRootNode());
            ImGui.EndPopup();
        }

        if(opened) {
            foreach(Node childNode in node.ChildNodes) {
                DrawEntityNode(childNode);
            }
            ImGui.TreePop();
        }
    }
    
}
