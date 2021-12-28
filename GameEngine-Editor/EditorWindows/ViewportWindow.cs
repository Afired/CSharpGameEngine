using System.Numerics;
using GameEngine.Core;
using GameEngine.Rendering;
using GameEngine.Rendering.Shaders;
using GameEngine.Rendering.Textures;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class ViewportWindow : EditorWindow {

    public ViewportWindow() {
        Title = "Viewport";
    }
    
    protected override void Draw() {
        Texture2D texture = TextureRegister.Get("Checkerboard") as Texture2D;
        ImGui.Image((IntPtr) RenderingEngine.FinalFrameBuffer.ColorAttachment, new Vector2(Configuration.WindowWidth, Configuration.WindowHeight), new Vector2(0, 1) , new Vector2(1, 0));
    }
    
}
