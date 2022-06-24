using System.Numerics;
using System.Reflection;
using ExampleGame;
using GameEngine.Core.Nodes;
using GameEngine.Core.SceneManagement;
using GameEngine.Core.Serialization;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows;

public delegate void OnSelect(Node node);

public class HierarchyWindow : EditorWindow {
    
    public static event OnSelect? OnSelect;
    
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
            DrawScene(Hierarchy.Scene);
        
        if(ImGui.IsMouseDown(ImGuiMouseButton.Left) && ImGui.IsWindowHovered()) {
            Selected = null;
        }
    }
    
    private void DrawScene(Scene scene) {
        
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
                if(ImGui.MenuItem(nameof(Node))) {
                    Node newNode = Node.New<Node>();
                    Hierarchy.AddEntity(newNode);
                }
                foreach(Type type in typeof(Node).Assembly.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(Node)))) {
                    if(ImGui.MenuItem(type.Name)) {
                        Node newNode = Node.New(type);
                        Hierarchy.AddEntity(newNode);
                    }
                }
                foreach(Type type in typeof(AssemblyRef).Assembly.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(Node)))) {
                    if(ImGui.MenuItem(type.Name)) {
                        Node newNode = Node.New(type);
                        Hierarchy.AddEntity(newNode);
                    }
                }
                ImGui.EndPopup();
            }
            
        }
        
    }
    
    private void DrawEntityNode(Node node) {
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
            
            // gets all property infos with [Serialized(Editor.Hierarchy)] Attribute
            IEnumerable<PropertyInfo> serializedProperties = GetAllPropertiesWithSerializedAttribute(node);
            
            foreach(PropertyInfo serializedPropertyInfo in serializedProperties) {
                Serialized? serializedAttribute = serializedPropertyInfo.GetCustomAttribute<Serialized>();
                if(serializedAttribute is null)
                    continue;
                
                if(serializedAttribute.Editor != Core.Serialization.Editor.Hierarchy)
                    continue;
                
                object? value = serializedPropertyInfo.GetValue(node);
                
                if(value is Node valueAsNode) {
                    DrawEntityNode(valueAsNode);
                } else if(value is INodeArr valueAsNodeList) {
                    DrawNodeArr(valueAsNodeList);
                } else {
                    Console.LogWarning($"There is a property defined with Serialized(Inspector) which cant be displayed | node: {node.GetType()}, property: {value?.GetType()}");
                }
                
            }
            
            ImGui.TreePop();
        }
    }

    private static IEnumerable<PropertyInfo> GetAllPropertiesWithSerializedAttribute(Node node) {
        return node.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(Serialized)));
    }
    
    private void DrawNodeArr(INodeArr nodes) {
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
        if(ImGui.Button("+", new Vector2(20, 19))) {
            Node newNode = Node.New(nodes.GetNodeType);
            nodes.Add(newNode);
        }
        
        if(ImGui.BeginPopupContextItem("", ImGuiPopupFlags.MouseButtonRight)) {
            ImGui.Text("Create new Node");
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
            if(ImGui.MenuItem(nodes.GetNodeType.Name)) {
                Node newNode = Node.New(nodes.GetNodeType);
                nodes.Add(newNode);
            }
            foreach(Type type in typeof(Node).Assembly.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(nodes.GetNodeType))) {
                if(ImGui.MenuItem(type.Name)) {
                    Node newNode = Node.New(type);
                    nodes.Add(newNode);
                }
            }
            foreach(Type type in typeof(AssemblyRef).Assembly.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(nodes.GetNodeType))) {
                if(ImGui.MenuItem(type.Name)) {
                    Node newNode = Node.New(type);
                    nodes.Add(newNode);
                }
            }
            ImGui.EndPopup();
        }
        
        if(opened) {
            foreach(Node node in nodes) {
                DrawEntityNode(node);
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
