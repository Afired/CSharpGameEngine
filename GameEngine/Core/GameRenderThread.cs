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
        
        ShaderRegister.Load();
        TextureRegister.Load();
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
        uint framebuffer = SetupFrameBuffers();
        GL.glClear(GL.GL_DEPTH_BUFFER_BIT | GL.GL_COLOR_BUFFER_BIT);
        RenderBackground();
        
        OnDraw?.Invoke();
        
        Glfw.SwapBuffers(window);
    }

    private void RenderBackground() {
        GL.glClearColor(CurrentCamera.BackgroundColor.R, CurrentCamera.BackgroundColor.G, CurrentCamera.BackgroundColor.B, CurrentCamera.BackgroundColor.A);
    }

    private uint SetupFrameBuffers() {
        // create and bind frame buffer object
        uint framebuffer = GL.glGenFramebuffer();
        GL.glBindFramebuffer(GL.GL_FRAMEBUFFER, framebuffer);

        // delete framebuffer when all framebuffer operations are done
        //GL.glDeleteBuffer(framebuffer);
        
        // attach textures buffer object
        uint texture = GL.glGenTexture();
        GL.glBindTexture(GL.GL_TEXTURE_2D, texture);
        unsafe {
            GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_RGB, Configuration.WindowWidth, Configuration.WindowHeight, 0, GL.GL_RGB, GL.GL_UNSIGNED_BYTE, null);
        }
        GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_LINEAR);
        GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_LINEAR);
        GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_COLOR_ATTACHMENT0, GL.GL_TEXTURE_2D, texture, 0);
        
        // attach depth stencil buffer
        //unsafe {
        //    GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_DEPTH24_STENCIL8, 800, 600, 0, GL.GL_DEPTH_STENCIL, GL.GL_UNSIGNED_INT_24_8, null);
        //}
        //GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_STENCIL_ATTACHMENT, GL.GL_TEXTURE_2D, texture, 0); 
        
        // attach render buffer object
        uint rbo = GL.glGenRenderbuffer();
        GL.glBindRenderbuffer(rbo);
        GL.glRenderbufferStorage(GL.GL_RENDERBUFFER, GL.GL_DEPTH24_STENCIL8, Configuration.WindowWidth, Configuration.WindowHeight);
        
        GL.glBindRenderbuffer(0);

        GL.glFramebufferRenderbuffer(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_STENCIL_ATTACHMENT, GL.GL_RENDERBUFFER, rbo);
        
        //check for framebuffer status
        if(GL.glCheckFramebufferStatus(GL.GL_FRAMEBUFFER) == GL.GL_FRAMEBUFFER_COMPLETE)
            Console.LogSuccess("FRAMEBUFFER SUCCESS");
        else
            Console.LogError("Framebuffer is not complete!");
        
        // bind default frame buffer so we dont accidentally rendering to the wrong framebuffer
        GL.glBindFramebuffer(GL.GL_FRAMEBUFFER, 0);

        return framebuffer;
    }
    
}
