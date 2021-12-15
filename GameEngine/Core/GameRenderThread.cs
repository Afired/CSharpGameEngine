using System;
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
        
        Glfw.MakeContextCurrent(_window);
        
        GL.glEnable(GL.GL_DEPTH);
        GL.glEnable(GL.GL_DEPTH_TEST);
        GL.glDepthFunc(GL.GL_LEQUAL);
        
        SetUpInputCallback(_window);
        InputHandler inputHandler = new InputHandler();

        DefaultShader.Initialize();
        OnLoad?.Invoke();
        
        while(!Glfw.WindowShouldClose(_window)) {
            if(CurrentCamera != null)
                Render(_window);
            Glfw.PollEvents();
            inputHandler.HandleInput(_window);
        }
        Terminate();
    }
    
    private void SetUpInputCallback(Window window) {
        Glfw.SetKeyCallback(window, KeyCallback);
    }

    private void KeyCallback(IntPtr window, Keys key, int scancode, InputState state, ModifierKeys mods) {
        Console.WriteLine(key.ToString());
    }

    private void Render(Window window) {
        GL.glClear(GL.GL_DEPTH_BUFFER_BIT | GL.GL_COLOR_BUFFER_BIT);
        RenderBackground();
        
        OnDraw?.Invoke();
        
        Glfw.SwapBuffers(window);
    }

    private void RenderBackground() {
        GL.glClearColor(CurrentCamera.BackgroundColor.R, CurrentCamera.BackgroundColor.G, CurrentCamera.BackgroundColor.B, CurrentCamera.BackgroundColor.A);
    }
    
}
