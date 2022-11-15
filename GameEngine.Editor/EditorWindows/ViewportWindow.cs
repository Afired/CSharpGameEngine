using System.Numerics;
using GameEngine.Core;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class ViewportWindow : EditorWindow {
    
    public ViewportWindow() {
        Title = "Viewport";
    }
    
    protected override void PreDraw() {
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0, 0));
    }
    
    protected override void PostDraw() {
        ImGui.PopStyleVar(1);
    }
    
    protected override void Draw() {
        DrawViewport();
        Vector2 windowPos = ImGui.GetWindowPos();
        ImGui.SetCursorScreenPos(windowPos + new Vector2(25, 50));
        ImGui.Text((1f / Application.Instance.Renderer.FrameTime).ToString("F1") + " FPS");
        ImGui.SetCursorScreenPos( windowPos + new Vector2(25, 65));
        ImGui.Text(Application.Instance.Renderer.FrameTime.ToString("F3") + " s");
    }
    
    private void DrawViewport() {
        Vector2 desiredSize = ImGui.GetContentRegionAvail();
        
        if(desiredSize != new Vector2(Application.Instance.Renderer.MainFrameBuffer1.Width, Application.Instance.Renderer.MainFrameBuffer1.Height)) {
            Application.Instance.Renderer.MainFrameBuffer1.Resize((int) desiredSize.X, (int) desiredSize.Y);
            Application.Instance.Renderer.MainFrameBuffer2.Resize((int) desiredSize.X, (int) desiredSize.Y);
        }
        
        ImGui.Image((IntPtr) Application.Instance.Renderer.ActiveFrameBuffer.ColorAttachment, desiredSize, new Vector2(0, 1) , new Vector2(1, 0));
    }
    
}
