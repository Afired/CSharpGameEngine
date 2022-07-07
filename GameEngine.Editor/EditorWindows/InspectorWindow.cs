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
        
        if(ImGui.BeginMenuBar()) {
            ImGui.Text(Selected is not null ? Selected.GetType().ToString() : "select a node to inspect");
            ImGui.EndMenuBar();
        }
        if(Selected is null) {
            return;
        }
        
        DrawNode(Selected);
    }

    private void DrawNode(Node node) {
        NodeDrawer<Node>.Draw(node);
    }
    
}
