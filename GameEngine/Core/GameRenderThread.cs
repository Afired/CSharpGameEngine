using GameEngine.Debugging;
using GameEngine.Input;
using GameEngine.Rendering;
using GameEngine.Rendering.Shaders;
using GLFW;
using Silk.NET.OpenGL;
using GL = OpenGL.GL;

namespace GameEngine.Core;

public delegate void OnLoad();
public delegate void OnDraw();

public sealed partial class Game {
    
    public static event OnLoad OnLoad;
    public static event OnDraw OnDraw;
    
    
    private void StartRenderThread() {
        // initial setup
        Setup(out Window window, out uint framebuffer, out uint textureColorBuffer, out uint vao);
        InputHandler inputHandler = new InputHandler();
        Glfw.SetKeyCallback(window, inputHandler.OnKeyAction);
        
        // render loop
        while(!Glfw.WindowShouldClose(window)) {
            
            // render and draw frame
            if(CurrentCamera != null)
                Render(window, framebuffer, textureColorBuffer, vao);
            
            // handle input
            Glfw.PollEvents();
            inputHandler.HandleMouseInput(window);
            
        }
        
        Terminate();
    }

    private void Setup(out Window window, out uint framebuffer, out uint textureColorBuffer, out uint vao) {
        window = WindowFactory.CreateWindow();

        SetupFrameBuffers(out framebuffer, out textureColorBuffer);
        
        vao = GetFullScreenRenderQuadVao();
        
        LoadResources();
    }

    private void LoadResources() {
        ShaderRegister.Load();
        TextureRegister.Load();
        OnLoad?.Invoke();
    }

    private void Render(Window window, uint framebuffer, uint textureColorBuffer, uint vao) {
        RenderFirstPass(framebuffer);
        RenderSecondPass(textureColorBuffer, vao);
        Glfw.SwapBuffers(window);
    }

    private void RenderFirstPass(uint framebuffer) {
        // bind custom framebuffer to render to
        GL.glBindFramebuffer(framebuffer);
        GL.glClear(GL.GL_DEPTH_BUFFER_BIT | GL.GL_COLOR_BUFFER_BIT);
        GL.glEnable(GL.GL_DEPTH_TEST); // reenable depth test
        OnDraw?.Invoke();
    }
    
    private void RenderSecondPass(uint textureColorBuffer, uint vao) {
        // bind default framebuffer to render to
        GL.glBindFramebuffer(0);
        
        RenderBackground();
        GL.glClear(GL.GL_COLOR_BUFFER_BIT);
        // use shader
        ShaderRegister.Get("SimpleRadiusBloomScreenShader2").Use();
        ShaderRegister.Get("SimpleRadiusBloomScreenShader2").SetFloat("time", Time.TotalTimeElapsed);
        GL.glBindVertexArray(vao);
        GL.glDisable(GL.GL_DEPTH_TEST);
        GL.glBindTexture(GL.GL_TEXTURE_2D, textureColorBuffer);
        GL.glDrawArrays(GL.GL_TRIANGLES, 0, 6);
    }

    private void RenderBackground() {
        GL.glClearColor(CurrentCamera.BackgroundColor.R, CurrentCamera.BackgroundColor.G, CurrentCamera.BackgroundColor.B, CurrentCamera.BackgroundColor.A);
    }

    private void SetupFrameBuffers(out uint framebuffer, out uint textureColorBuffer) {
        // create and bind frame buffer object
        framebuffer = GL.glGenFramebuffer();
        GL.glBindFramebuffer(GL.GL_FRAMEBUFFER, framebuffer);

        // delete framebuffer when all framebuffer operations are done
        //GL.glDeleteBuffer(framebuffer);
        
        // attach textures buffer object
        textureColorBuffer = GL.glGenTexture();
        GL.glBindTexture(GL.GL_TEXTURE_2D, textureColorBuffer);
        unsafe {
            if(Configuration.UseHDR)
                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int) InternalFormat.Rgb16f, Configuration.WindowWidth, Configuration.WindowHeight, 0, (int) PixelFormat.Rgb, GL.GL_FLOAT, null);
            else
                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int) InternalFormat.Rgb, Configuration.WindowWidth, Configuration.WindowHeight, 0, (int) PixelFormat.Rgb, GL.GL_UNSIGNED_BYTE, null);
        }
        GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_LINEAR);
        GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_LINEAR);
        GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_COLOR_ATTACHMENT0, GL.GL_TEXTURE_2D, textureColorBuffer, 0);
        
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
            Console.LogSuccess("Framebuffer setup succeeded");
        else
            Console.LogError("Framebuffer is not complete");
        
        // bind default frame buffer to render to
        GL.glBindFramebuffer(GL.GL_FRAMEBUFFER, 0);
    }

    private uint GetFullScreenRenderQuadVao() {
        
        float[] vertexData = {
            -1, 1f, 0f, 0f, 1f,   // top left
            1f, 1f, 0f, 1f, 1f,    // top right
            -1f, -1f, 0f, 0f , 0f, // bottom left

            1f, 1f, 0f, 1f, 1f,    // top right
            1f, -1f, 0f, 1f, 0f,   // bottom right
            -1f, -1f, 0f, 0f, 0f,  // bottom left
        };
        
        uint vao = GL.glGenVertexArray();
        uint vbo = GL.glGenBuffer();
        
        GL.glBindVertexArray(vao);
        GL.glBindBuffer(GL.GL_ARRAY_BUFFER, vbo);
        

        unsafe {
            fixed(float* v = &vertexData[0]) {
                GL.glBufferData(GL.GL_ARRAY_BUFFER, sizeof(float) * vertexData.Length, v, GL.GL_STATIC_DRAW);
            }
            
            // xyz
            GL.glVertexAttribPointer(0, 3, GL.GL_FLOAT, false, 5 * sizeof(float), (void*) (0 * sizeof(float)));
            GL.glEnableVertexAttribArray(0);
            
            // texture coordinates
            GL.glVertexAttribPointer(1, 2, GL.GL_FLOAT, false, 5 * sizeof(float), (void*) (3 * sizeof(float)));
            GL.glEnableVertexAttribArray(1);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, 0);
            GL.glBindVertexArray(0);
        }
        return vao;
    }
    
}
