using GameEngine.Core.Nodes;
using GameEngine.Editor.NodeDrawers;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows;

public class InspectorWindow : EditorWindow {
    
    public InspectorWindow() {
        Title = "Inspector";
    }
    
    protected override void Draw() {
        
        if(ImGui.BeginMenuBar()) {
            ImGui.Text(Selection.Current is not null ? Selection.Current.GetType().ToString() : "select an object to inspect");
            ImGui.EndMenuBar();
        }
        if(Selection.Current is null) {
            return;
        }
        
        if(Selection.Current is Node node)
            NodeDrawer.Draw(node);
    }
    
}
