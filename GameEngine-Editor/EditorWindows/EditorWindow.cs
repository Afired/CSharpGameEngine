using GameEngine.Rendering;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class EditorWindow {

    protected string Title = "Title";
    
    public EditorWindow() {
        Program.EditorLayer.OnDraw += DrawWindow;
    }

    private void DrawWindow() {
        ImGui.Begin(Title);
        Draw();
        ImGui.End();
    }

    protected virtual void Draw() { }
    
}
