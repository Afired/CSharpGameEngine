using GameEngine.Rendering;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class EditorWindow {

    protected string Title = "Title";
    private bool _opened = true;
    
    public EditorWindow() {
        Program.EditorLayer.OnDraw += DrawWindow;
    }

    private void DrawWindow() {
        if(!_opened)
            return;
        ImGui.Begin(Title, ref _opened, ImGuiWindowFlags.NoCollapse);
        Draw();
        ImGui.End();
    }

    protected virtual void Draw() { }
    
}
