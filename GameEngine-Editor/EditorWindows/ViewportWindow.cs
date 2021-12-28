using System.Numerics;
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
        ImGui.Image((IntPtr) texture.ID, new Vector2(texture.Width, texture.Height) * 100);
    }
    
}
