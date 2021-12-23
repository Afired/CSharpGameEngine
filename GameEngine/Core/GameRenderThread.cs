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
        Setup(out Window window, out FrameBuffer frameBuffer, out uint vao);
        InputHandler inputHandler = new InputHandler();
        Glfw.SetKeyCallback(window, inputHandler.OnKeyAction);
        
        // render loop
        while(!Glfw.WindowShouldClose(window)) {
            
            // render and draw frame
            if(CurrentCamera != null)
                Render(window, frameBuffer, vao);
            
            // handle input
            Glfw.PollEvents();
            inputHandler.HandleMouseInput(window);
            
        }
        
        Terminate();
    }

    private void Setup(out Window window, out FrameBuffer frameBuffer, out uint vao) {
        window = WindowFactory.CreateWindow();
        
        frameBuffer = new FrameBuffer();
        
        vao = GetFullScreenRenderQuadVao();
        
        LoadResources();
    }

    private void LoadResources() {
        ShaderRegister.Load();
        TextureRegister.Load();
        OnLoad?.Invoke();
    }

    private void Render(Window window, FrameBuffer frameBuffer, uint vao) {
        RenderFirstPass(frameBuffer.ID);
        RenderSecondPass(frameBuffer.TextureColorBuffer, vao);
        Glfw.SwapBuffers(window);
    }

    private void RenderFirstPass(uint frameBuffer) {
        // bind custom framebuffer to render to
        GL.glBindFramebuffer(frameBuffer);
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
        string screenShader = "ScreenShader";
        ShaderRegister.Get(screenShader).Use();
        ShaderRegister.Get(screenShader).SetFloat("time", Time.TotalTimeElapsed);
        GL.glBindVertexArray(vao);
        GL.glDisable(GL.GL_DEPTH_TEST);
        GL.glBindTexture(GL.GL_TEXTURE_2D, textureColorBuffer);
        GL.glDrawArrays(GL.GL_TRIANGLES, 0, 6);
    }

    private void RenderBackground() {
        GL.glClearColor(CurrentCamera.BackgroundColor.R, CurrentCamera.BackgroundColor.G, CurrentCamera.BackgroundColor.B, CurrentCamera.BackgroundColor.A);
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
