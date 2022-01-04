using System.Numerics;
using GameEngine.Layers;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class EditorWindow {

    protected string Title = "Title";
    
    public EditorWindow() {
        DefaultOverlayLayer.OnDraw += DrawWindow;
    }

    private void DrawWindow() {
        ImGui.Begin(Title);
        Draw();
        ImGui.End();
    }

    protected virtual void Draw() { }
    
}
