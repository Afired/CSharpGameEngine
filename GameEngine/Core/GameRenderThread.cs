using GameEngine.Input;
using GameEngine.Rendering;
using GameEngine.Rendering.Shaders;
using GLFW;
using OpenGL;

namespace GameEngine.Core;

public delegate void OnDraw();
public delegate void OnLoad();

public sealed partial class Game {
    
    public static event OnDraw OnDraw;
    public static event OnLoad OnLoad;
    
    private void StartRenderThread() {
        Window window = WindowFactory.CreateWindow();
        
        GL.glEnable(GL.GL_DEPTH);
        GL.glEnable(GL.GL_DEPTH_TEST);
        GL.glDepthFunc(GL.GL_LEQUAL);
        
        //SetUpInputCallback(window);
        InputHandler inputHandler = new InputHandler();

        DefaultShader.Initialize();
        OnLoad?.Invoke();
        
        while(!Glfw.WindowShouldClose(window)) {
            if(CurrentCamera != null)
                Render(window);
            Glfw.PollEvents();
            inputHandler.HandleInput(window);
        }
        Terminate();
    }
    
    private void SetUpInputCallback(Window window) {
        Glfw.SetKeyCallback(window, KeyCallback);
    }

    private void KeyCallback(Window window, Keys key, int scancode, InputState state, ModifierKeys mods) {
        
    }

    private void Render(Window window) {
        RenderBackground();
        GL.glClear(GL.GL_COLOR_BUFFER_BIT);
        GL.glClear(GL.GL_DEPTH_BUFFER_BIT);
        OnDraw?.Invoke();
        
        Glfw.SwapBuffers(window);
    }

    private void RenderBackground() {
        GL.glClearColor(CurrentCamera.BackgroundColor.R, CurrentCamera.BackgroundColor.G, CurrentCamera.BackgroundColor.B, CurrentCamera.BackgroundColor.A);
    }
    
}
