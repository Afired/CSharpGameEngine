using GameEngine.Core;
using GameEngine.Input;
using GameEngine.Numerics;
using GameEngine.Rendering.Cameras;
using GameEngine.Rendering.Shaders;
using GameEngine.Rendering.Window;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace GameEngine.Rendering;

public delegate void OnLoad();
public delegate void OnDraw();
public delegate void OnImGui();

public sealed unsafe class RenderingEngine {
    
    public static event OnLoad OnLoad;
    public static event OnDraw OnDraw;
    public static event OnImGui OnImGui;
    public static BaseCamera CurrentCamera { get; private set; }
    
    public static GlfwWindow GlfwWindow;
    public static GL Gl => GlfwWindow.Gl;
    public static Glfw Glfw => GlfwWindow.Glfw;
    
    public static SomeFrameBuffer SomeFrameBuffer { get; private set; }
    public static FrameBuffer FinalFrameBuffer { get; private set; }
    private uint _fullscreenVao;
    
    public static string ScreenShader = "ScreenShader";
    

    internal void Initialize() {
        Setup();
        InputHandler inputHandler = new InputHandler();
        Glfw.SetKeyCallback(GlfwWindow.Handle, inputHandler.OnKeyAction);
        
        RenderLoop(GlfwWindow.Handle, inputHandler);
    }
    
    private void RenderLoop(WindowHandle* window, InputHandler inputHandler) {
        
        while(Application.IsRunning) {
            
            Render(window);
            
            // handle input
            Glfw.PollEvents();
            inputHandler.HandleMouseInput(window);

            if(Glfw.WindowShouldClose(window))
                Application.Terminate();
        }
        
    }

    private void Setup() {
        GlfwWindow = new GlfwWindow();
        SomeFrameBuffer = new SomeFrameBuffer();
        FinalFrameBuffer = new FrameBuffer(new FrameBufferConfig() { Width = Configuration.WindowWidth, Height = Configuration.WindowHeight });
        _fullscreenVao = GetFullScreenRenderQuadVao();
        LoadResources();
    }
    
    private void LoadResources() {
        ShaderRegister.Load();
        TextureRegister.Load();
        OnLoad?.Invoke();
    }

    private void Render(WindowHandle* window) {
        Gl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        // render and draw frame
        if(CurrentCamera != null) {
            RenderFirstPass();
            RenderSecondPass();
        }
        RenderOverlay();
        Glfw.SwapBuffers(window);
    }

    private void RenderFirstPass() {
        // bind custom framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, SomeFrameBuffer.ID);
        
        Gl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        Gl.Enable(EnableCap.DepthTest); // reenable depth test
        Gl.ClearColor(System.Drawing.Color.Aqua);
        OnDraw?.Invoke();
    }
    
    private void RenderSecondPass() {
        // bind default framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, FinalFrameBuffer.ID);
        
        RenderBackground();
        Gl.Clear(ClearBufferMask.ColorBufferBit);
        // use screen shader
        ShaderRegister.Get(ScreenShader).Use();
        ShaderRegister.Get(ScreenShader).SetFloat("time", Time.TotalTimeElapsed);
        Gl.BindVertexArray(_fullscreenVao);
        Gl.Disable(EnableCap.DepthTest);
        Gl.BindTexture(TextureTarget.Texture2D, SomeFrameBuffer.TextureColorBuffer);
        Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    private void RenderOverlay() {
        GlfwWindow.ImGuiController.Update(0.1f);
        // bind default framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        
        OnImGui?.Invoke();
        
        GlfwWindow.ImGuiController.Render();
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
