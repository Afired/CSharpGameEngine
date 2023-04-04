using System.Numerics;
using GameEngine.Core;
using GameEngine.Core.Rendering;
using GameEngine.Core.SceneManagement;
using GameEngine.Numerics;
using ImGuiNET;
using Silk.NET.OpenGL;

namespace GameEngine.Editor.EditorWindows; 

public class ViewportWindow : EditorWindow, IEditorUpdate {
    
    private EditorCamera EditorCamera { get; }
    
    public ViewportWindow(GL gl, Renderer renderer) {
        Title = " Viewport";
        EditorCamera = new EditorCamera(gl, renderer);
    }
    
    public void EditorUpdate(float deltaTime) {
        EditorCamera.EditorUpdate(deltaTime);
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
        
        Renderer renderer = Application.Instance.Renderer;
        GL gl = renderer.MainWindow.Gl;
        
        FrameBuffer activeFrameBuffer = renderer.FinalFrameBuffer;
        
        if((int)desiredSize.X != EditorCamera.FrameBuffer.Width
           || (int)desiredSize.Y != EditorCamera.FrameBuffer.Height) {
            EditorCamera.FrameBuffer.Resize((int)desiredSize.X, (int)desiredSize.Y);
        }
        
        EditorCamera.FrameBuffer.Bind();
        gl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        gl.Enable(EnableCap.DepthTest); // reenable depth
        gl.Viewport(0, 0, EditorCamera.FrameBuffer.Width, EditorCamera.FrameBuffer.Height);
        gl.ClearColor(EditorCamera.BackgroundColor.R, EditorCamera.BackgroundColor.G, EditorCamera.BackgroundColor.B, EditorCamera.BackgroundColor.A);
        Renderer.ViewMatrix = EditorCamera.ViewMatrix;
        Renderer.ProjectionMatrix = EditorCamera.ProjectionMatrix;
        Hierarchy.Draw();
        //todo: post processing stack
//        DoPostProcessing();
        
        activeFrameBuffer.Bind();
        gl.Viewport(0, 0, activeFrameBuffer.Width, activeFrameBuffer.Height);
        
        ImGui.Image((nint) EditorCamera.FrameBuffer.ColorAttachment, desiredSize, new Vector2(0, 1), new Vector2(1, 0));
    }
    
}
