using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class EditorWindow {

    public EditorWindow() {
        OnImGui += DrawWindow;
    }

    private void DrawWindow() {
        ImGui.Begin("Window Title");
        Draw();
        ImGui.End();
    }

    protected virtual void Draw() { }
    
}
