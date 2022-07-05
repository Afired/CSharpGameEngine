using System.Numerics;
using GameEngine.Core.Core;
using GameEngine.Core.Rendering;
using GameEngine.Core.SceneManagement;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class ViewportWindow : EditorWindow {

    public ViewportWindow() {
        Title = "Viewport";
    }
    
    protected override void Draw() {
        DrawViewport();
    }

    private void DrawViewport() {
        Vector2 desiredSize = ImGui.GetContentRegionAvail();
        Vector2 size = new Vector2(Configuration.WindowWidth, Configuration.WindowHeight);
        size = size / Configuration.WindowHeight * desiredSize.Y;
        ImGui.Image((IntPtr) Renderer.MainFrameBuffer2.ColorAttachment, size, new Vector2(0, 1) , new Vector2(1, 0));
    }
    
}
