using GameEngine.Core;
using GameEngine.Input;
using GameEngine.Rendering.Cameras;
using GameEngine.Rendering.Shaders;
using GameEngine.Rendering.Window;
using ImGuiNET;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace GameEngine.Rendering;

public delegate void OnLoad();
public delegate void OnDraw();

public sealed unsafe class RenderingEngine {
    
    public static event OnLoad OnLoad;
    public static event OnDraw OnDraw;
    public static BaseCamera CurrentCamera { get; private set; }
    
    public static GlfwWindow GlfwWindow;
    public static GL Gl => GlfwWindow.Gl;
    public static Glfw Glfw => GlfwWindow.Glfw;
    
    private FrameBuffer _frameBuffer;
    private uint _fullscreenVao;
    
    private string _screenShader = "ScreenShader";
    

    internal void Initialize() {
        Setup();
        InputHandler inputHandler = new InputHandler();
        Glfw.SetKeyCallback(GlfwWindow.Handle, inputHandler.OnKeyAction);
        
        RenderLoop(GlfwWindow.Handle, inputHandler);
        
        Game.Terminate();
    }
    
    private void RenderLoop(WindowHandle* window, InputHandler inputHandler) {
        
        while(!Glfw.WindowShouldClose(window)) {
            
            // render and draw frame
            if(CurrentCamera != null)
                Render(window);
            
            // handle input
            Glfw.PollEvents();
            inputHandler.HandleMouseInput(window);

        }
        
    }

    private void Setup() {
        GlfwWindow = new GlfwWindow();
        _frameBuffer = new FrameBuffer();
        _fullscreenVao = GetFullScreenRenderQuadVao();
        LoadResources();
    }
    
    private void LoadResources() {
        ShaderRegister.Load();
        TextureRegister.Load();
        OnLoad?.Invoke();
    }

    private void Render(WindowHandle* window) {
        RenderFirstPass();
        RenderSecondPass();
        Glfw.SwapBuffers(window);
    }

    private void RenderFirstPass() {
        // bind custom framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBuffer.ID);
        
        GlfwWindow.ImGuiController.Update(0.1f);
        
        Gl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        Gl.Enable(EnableCap.DepthTest); // reenable depth test
        OnDraw?.Invoke();
        
        ImGui.ShowDemoWindow();
        GlfwWindow.ImGuiController.Render();
    }
    
    private void RenderSecondPass() {
        // bind default framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        
        RenderBackground();
        Gl.Clear(ClearBufferMask.ColorBufferBit);
        // use screen shader
        ShaderRegister.Get(_screenShader).Use();
        ShaderRegister.Get(_screenShader).SetFloat("time", Time.TotalTimeElapsed);
        Gl.BindVertexArray(_fullscreenVao);
        Gl.Disable(EnableCap.DepthTest);
        Gl.BindTexture(TextureTarget.Texture2D, _frameBuffer.TextureColorBuffer);
        Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    private void RenderBackground() {
        Gl.ClearColor(CurrentCamera.BackgroundColor.R, CurrentCamera.BackgroundColor.G, CurrentCamera.BackgroundColor.B, CurrentCamera.BackgroundColor.A);
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
        
        uint vao = Gl.GenVertexArray();
        uint vbo = Gl.GenBuffer();
        
        Gl.BindVertexArray(vao);
        Gl.BindBuffer(GLEnum.ArrayBuffer, vbo);
        

        unsafe {
            fixed(float* v = &vertexData[0]) {
                Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(float) * vertexData.Length), v, BufferUsageARB.StaticDraw);
            }
            
            // xyz
            Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*) (0 * sizeof(float)));
            Gl.EnableVertexAttribArray(0);
            
            // texture coordinates
            Gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*) (3 * sizeof(float)));
            Gl.EnableVertexAttribArray(1);

            Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            Gl.BindVertexArray(0);
        }
        return vao;
    }
    
    public static void SetActiveCamera(BaseCamera baseCamera) {
        CurrentCamera = baseCamera;
    }
    
}
