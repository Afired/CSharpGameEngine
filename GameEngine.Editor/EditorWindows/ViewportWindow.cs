using System.Numerics;
using GameEngine.Core;
using GameEngine.Core.Rendering;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class ViewportWindow : EditorWindow {
    
    public ViewportWindow() {
        Title = "Viewport";
    }
    
    protected override void PreDraw() => 
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0, 0));
    
    protected override void PostDraw() =>
        ImGui.PopStyleVar(1);
    
    protected override void Draw() {
        DrawViewport();
    }
    
    private void DrawViewport() {
        // ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0, 0));
        
        Vector2 desiredSize = ImGui.GetContentRegionAvail();
        
        if(desiredSize != new Vector2(Renderer.MainFrameBuffer1.Width, Renderer.MainFrameBuffer1.Height)) {
            Renderer.MainFrameBuffer1.Resize((int) desiredSize.X, (int) desiredSize.Y);
            Renderer.MainFrameBuffer2.Resize((int) desiredSize.X, (int) desiredSize.Y);
        }
        
        Vector2 size = new Vector2(Application.Instance!.Config.WindowWidth, Application.Instance!.Config.WindowHeight);
        // size = size / Configuration.WindowHeight * desiredSize.Y;
        
        // ImGui.Image((IntPtr) Renderer.MainFrameBuffer1.ColorAttachment, desiredSize, new Vector2(0, 1) , new Vector2(1, 0));
        ImGui.Image((IntPtr) Renderer.MainFrameBuffer1.ColorAttachment, desiredSize, new Vector2(0, 1) , new Vector2(1, 0));
        
        // ImGui.PopStyleVar(1);
    }
    
}
