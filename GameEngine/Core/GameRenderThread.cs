using GameEngine.Debugging;
using GameEngine.Input;
using GameEngine.Rendering;
using GameEngine.Rendering.Shaders;
using GameEngine.Rendering.Textures;
using GLFW;
using Silk.NET.OpenGL;
using GL = OpenGL.GL;

namespace GameEngine.Core;

public delegate void OnDraw();
public delegate void OnLoad();

public sealed partial class Game {
    
    public static event OnDraw OnDraw;
    public static event OnLoad OnLoad;
    
    
    private void StartRenderThread() {
        Window window = WindowFactory.CreateWindow();
        
        Glfw.MakeContextCurrent(window);

        GL.Import(Glfw.GetProcAddress);
        
        GL.glViewport(0, 0, Configuration.WindowWidth, Configuration.WindowHeight);
        GL.glEnable(GL.GL_DEPTH);
        GL.glEnable(GL.GL_DEPTH_TEST);
        GL.glDepthFunc(GL.GL_LEQUAL);
        
        InputHandler inputHandler = new InputHandler();
        Glfw.SetKeyCallback(window, inputHandler.OnKeyAction);

        DefaultShader.Initialize();
        TextureRegister.Register("Checkerboard", new Texture2D("Checkerboard.png"));
        TextureRegister.Register("Box", new Texture2D("Box.png"));
        OnLoad?.Invoke();
        
        while(!Glfw.WindowShouldClose(window)) {
            if(CurrentCamera != null)
                Render(window);
            Glfw.PollEvents();
            inputHandler.HandleMouseInput(window);
        }
        Terminate();
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
