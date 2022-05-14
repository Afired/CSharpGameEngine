using GameEngine.Core.Nodes;
using GameEngine.Editor.NodeDrawers;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows;

public class InspectorWindow : EditorWindow {
    
    public Node? Selected { get; private set; }
    
    public InspectorWindow() {
        Title = "Inspector";
        HierarchyWindow.OnSelect += entity => Selected = entity;
        
    }
    
    protected override void Draw() {
        if(Selected is null) {
            ImGui.Text("select a node to inspect");
            return;
        }
        
        DrawNode(Selected);
    }

    private void DrawNode(Node node) {
        NodeDrawer<Node>.Draw(node);
    }
    
}
