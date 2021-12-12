using GameEngine.Rendering;
using GLFW;
using OpenGL;

namespace GameEngine.Core;

public delegate void OnDraw();
public delegate void OnLoad();

public sealed partial class Game {
    
    public static event OnDraw OnDraw;
    public static event OnLoad OnLoad;
    
    private void StartRenderThread() {
        Window window = WindowFactory.CreateWindow("Window Title", false);

        InitializeDefaultShader();
        OnLoad?.Invoke();
        
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
