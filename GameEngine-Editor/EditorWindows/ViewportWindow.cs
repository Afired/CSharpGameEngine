using System.Numerics;
using GameEngine.Core;
using GameEngine.Rendering;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class ViewportWindow : EditorWindow {

    public ViewportWindow() {
        Title = "Viewport";
    }
    
    protected override void Draw() {
        Vector2 desiredSize = ImGui.GetContentRegionAvail();
        Vector2 size = new Vector2(Configuration.WindowWidth, Configuration.WindowHeight);
        size = size / Configuration.WindowHeight * desiredSize.Y;
        ImGui.Image((IntPtr) RenderingEngine.FinalFrameBuffer.ColorAttachment, size, new Vector2(0, 1) , new Vector2(1, 0));
    }
    
}
