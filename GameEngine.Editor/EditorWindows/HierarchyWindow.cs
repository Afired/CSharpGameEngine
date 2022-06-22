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

    private object? v_selected;
    public object? Selected {
        get => v_selected;
        set {
            v_selected = value;
            if(v_selected is Node selectedNode)
                OnSelect?.Invoke(selectedNode);
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
                DrawEntityRootNode(entity);
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
                            .Single(ctor => ctor.IsPublic && ctor.GetParameters().Length == 1)
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
                            .Single(ctor => ctor.IsPublic && ctor.GetParameters().Length == 1)
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

    private void DrawEntityRootNode(Node node) {
        ImGuiTreeNodeFlags treeNodeFlags = (node.ChildNodes.Count == 0 ? ImGuiTreeNodeFlags.Bullet : ImGuiTreeNodeFlags.OpenOnArrow) |
                                           (Selected == node ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None) |
                                           ImGuiTreeNodeFlags.SpanFullWidth;
        ImGui.PushID(node.GetHashCode());
        bool opened = ImGui.TreeNodeEx(node.GetType().Name, treeNodeFlags);
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
                DrawEntityChildNode(childNode);
            }
            ///
            if(node is SceneNode sceneNode) {
                DrawNodeArr(sceneNode.Nodes, sceneNode);
            }
            ///
            ImGui.TreePop();
        }
    }
    
    private void DrawEntityChildNode(Node node) {
        ImGuiTreeNodeFlags treeNodeFlags = (node.ChildNodes.Count == 0 ? ImGuiTreeNodeFlags.Bullet : ImGuiTreeNodeFlags.OpenOnArrow) |
                                           (Selected == node ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None) |
                                           ImGuiTreeNodeFlags.SpanFullWidth;
        ImGui.PushID(node.GetHashCode());
        bool opened = ImGui.TreeNodeEx(node.GetType().Name, treeNodeFlags);
        ImGui.PopID();
        if(ImGui.IsItemClicked()) {
            Selected = node;
        }
        
        if(opened) {
            foreach(Node childNode in node.ChildNodes) {
                DrawEntityChildNode(childNode);
            }
            ImGui.TreePop();
        }
    }
    
    private void DrawNodeArr(List<Node> nodes, Node container) {
        ImGuiTreeNodeFlags treeNodeFlags = ImGuiTreeNodeFlags.OpenOnArrow |
                                           (Selected == nodes ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None) |
                                           ImGuiTreeNodeFlags.SpanFullWidth |
                                           ImGuiTreeNodeFlags.AllowItemOverlap |
                                           ImGuiTreeNodeFlags.FramePadding;
        ImGui.PushID(nodes.GetHashCode());
        ImGui.AlignTextToFramePadding();
        bool opened = ImGui.TreeNodeEx(nodes.GetType().GenericTypeArguments[0].Name + "s", treeNodeFlags);
        ImGui.PopID();
        if(ImGui.IsItemClicked()) {
            Selected = nodes;
        }
        
        ImGui.SameLine();
        if (ImGui.Button("+", new Vector2(20, 19))) {
            Transform newNode = new();
            nodes.Add(newNode);
            (container.ChildNodes as List<Node>)!.Add(newNode);
        }
        
        // if(ImGui.BeginPopupContextItem()) {
        //     if (ImGui.MenuItem("Add Entry")) {
        //         // Type type = typeof(Transform);
        //         // object[] parameters = type
        //         //     .GetConstructors()
        //         //     .Single(ctor => ctor.IsPublic && ctor.GetParameters().Length == 1)
        //         //     .GetParameters()
        //         //     .Select(p => (object)null!)
        //         //     .ToArray();
        //         // Node newNode = (Node) Activator.CreateInstance(type, parameters)!;
        //         
        //         Transform newNode = new();
        //         nodes.Add(newNode);
        //     }
        //     ImGui.EndPopup();
        // }
        
        if(opened) {
            foreach(Node node in nodes) {
                DrawEntityChildNode(node);
            }
            ImGui.TreePop();
        }
        
    }
    
    // https://github.com/ocornut/imgui/issues/282
    private static unsafe bool MyTreeNode(string label, ImGuiTreeNodeFlags treeNodeFlags) {
        
        ImGuiStylePtr style = ImGui.GetStyle();
        ImGuiStoragePtr storage = ImGui.GetStateStorage();
        
        uint id = ImGui.GetID(label);
        // int opened = storage.GetInt(id, 0);
        bool opened = storage.GetBool(id, false);
        float x = ImGui.GetCursorPosX();
        ImGui.BeginGroup();
        if(ImGui.InvisibleButton(label, new Vector2(-1, ImGui.GetFontSize() + style.FramePadding.Y * 2))) {
            // int* p_opened = storage.GetIntRef(id, 0);
            // opened = *p_opened = !*p_opened;
            bool* p_opened = (bool*) storage.GetBoolRef(id, false);
            opened = *p_opened = !*p_opened;
        }
        bool hovered = ImGui.IsItemHovered();
        bool active = ImGui.IsItemActive();

        Vector2 itemBoxMin = new(5, 5);
        Vector2 itemBoxMax = new(5, 5);
        
        // ImGui.GetStyle().Colors[active ? (int) ImGuiCol.HeaderActive : (int) ImGuiCol.HeaderHovered]
        if(hovered || active)
            ImGui.GetWindowDrawList().AddRectFilled(itemBoxMin, itemBoxMax, active ? (uint) ImGuiCol.HeaderActive : (uint) ImGuiCol.HeaderHovered);

        // Icon, text
        ImGui.SameLine(x);
        ImGui.ColorButton("button_id", opened ? new Vector4(255, 0, 0, 255) : new Vector4(0, 255, 0, 255));
        ImGui.SameLine();
        ImGui.Text(label);
        ImGui.EndGroup();
        if(opened)
            ImGui.TreePush(label);
        return opened;
    }
    
}
