using GameEngine.Rendering;
using GLFW;
using OpenGL;

namespace GameEngine.Core;

public delegate void OnDraw();
public delegate void OnLoadShader();

public sealed partial class Game {
    
    public static event OnDraw OnDraw;
    public static event OnLoadShader OnLoadShader;
    
    private void StartRenderThread() {
        Window window = WindowFactory.CreateWindow("Window Title", false);
        
        OnLoadShader?.Invoke();
        
        while(!Glfw.WindowShouldClose(window)) {
            Glfw.PollEvents();
            if(CurrentCamera != null)
                Render(window);
        }
        Terminate();
    }

    private void Render(Window window) {
        RenderBackground();
        
        OnDraw?.Invoke();
        
        Glfw.SwapBuffers(window);
    }

    private void RenderBackground() {
        GL.glClearColor(CurrentCamera.BackgroundColor.R, CurrentCamera.BackgroundColor.G, CurrentCamera.BackgroundColor.B, CurrentCamera.BackgroundColor.A);
        GL.glClear(GL.GL_COLOR_BUFFER_BIT);
    }
    
}
