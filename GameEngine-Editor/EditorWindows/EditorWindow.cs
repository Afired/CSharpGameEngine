using GameEngine.Rendering;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class EditorWindow {

    protected string Title = "Title";
//    private readonly int _id;
    
    public EditorWindow() {
        Program.EditorLayer.OnDraw += DrawWindow;
//        _id = GetHashCode();
    }

    private void DrawWindow() {
        bool opened = true;
//        ImGui.PushID(_id);
        ImGui.Begin(Title, ref opened, ImGuiWindowFlags.NoCollapse);
        Draw();
        ImGui.End();
//        ImGui.PopID();
        if(!opened)
            Program.EditorLayer.OnDraw -= DrawWindow;
    }

    protected virtual void Draw() { }
    
}
