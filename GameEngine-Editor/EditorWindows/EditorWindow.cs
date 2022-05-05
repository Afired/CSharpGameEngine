using GameEngine.Core.Rendering;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class EditorWindow {

    protected string Title = "Title";
    private readonly int _id;
    
    public EditorWindow() {
        Program.EditorLayer.OnDraw += DrawWindow;
        _id = GetHashCode();
    }
    
    private void DrawWindow() {
        bool opened = true;
        // push id doesnt work with windows since it cant be handled with the id stack, c++ uses ## or ### to set an identifier
        ImGui.Begin(Title + " - id:" + _id, ref opened, ImGuiWindowFlags.NoCollapse);
        Draw();
        ImGui.End();
        if(!opened)
            Program.EditorLayer.OnDraw -= DrawWindow;
    }
    
    protected virtual void Draw() { }
    
}
