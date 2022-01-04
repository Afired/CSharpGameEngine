using GameEngine.Core;
using GameEngine.Input;
using GameEngine.Layers;
using GameEngine.Numerics;
using GameEngine.Rendering.Cameras;
using GameEngine.Rendering.Shaders;
using GameEngine.Rendering.Window;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace GameEngine.Rendering;

public delegate void OnLoad();
public delegate void OnImGui();

public sealed unsafe class RenderingEngine {
    
    public static event OnLoad OnLoad;
    //public static event OnImGui OnImGui;
    public static BaseCamera CurrentCamera { get; private set; }
    
    public static GlfwWindow GlfwWindow;
    public static GL Gl => GlfwWindow.Gl;
    public static Glfw Glfw => GlfwWindow.Glfw;
    
    // this frame buffer is the main frame buffer to render to, it is also used for any drawing of post processing when ping ponging (ping pong frame buffer 1)
    public static SomeFrameBuffer MainFrameBuffer1 { get; private set; }
    // this frame buffer is used for post processing (ping pong frame buffer 2)
    public static FrameBuffer MainFrameBuffer2 { get; private set; }
    
    private uint _fullscreenVao;
    
    public static string ScreenShader = "ScreenShader";
    
    public static LayerStack LayerStack { get; private set; }
    

    internal void Initialize() {
        Setup();
        InputHandler inputHandler = new InputHandler();
        Glfw.SetKeyCallback(GlfwWindow.Handle, inputHandler.OnKeyAction);
        LayerStack = new LayerStack();
        
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
        MainFrameBuffer1 = new SomeFrameBuffer();
        MainFrameBuffer2 = new FrameBuffer(new FrameBufferConfig() { Width = Configuration.WindowWidth, Height = Configuration.WindowHeight });
        _fullscreenVao = GetFullScreenRenderQuadVao();
        LoadResources();
    }
    
    private void LoadResources() {
        ShaderRegister.Load();
        TextureRegister.Load();
        OnLoad?.Invoke();
    }

    private void Render(WindowHandle* window) {
        
        // bind default framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, MainFrameBuffer1.ID);
        Gl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        Gl.Enable(EnableCap.DepthTest); // reenable depth
        
        // render and draw frame
        if(CurrentCamera != null) {
            DrawBackground();
            foreach(Layer layer in LayerStack.GetNormalLayers()) {
                layer.Draw();
            }
            //todo: post processing stack
            DoPostProcessing();
        }
        
        GlfwWindow.ImGuiController.Update(0.1f);
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, MainFrameBuffer1.ID);
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        foreach(Layer layer in LayerStack.GetOverlayLayers()) {
            layer.Draw();
        }
        GlfwWindow.ImGuiController.Render();
        
        DrawToBackBuffer();
        Glfw.SwapBuffers(window);
    }

    private void DrawToBackBuffer() {
        // bind default framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        // use default screen shader
        ShaderRegister.Get("ScreenShader").Use();
        Gl.BindVertexArray(_fullscreenVao);
        Gl.Disable(EnableCap.DepthTest);
        // todo: should always be set to last active frame buffer color attachment
        Gl.BindTexture(TextureTarget.Texture2D, MainFrameBuffer1.TextureColorBuffer);
        Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }
    
    private void DoPostProcessing() {
        // bind default framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, MainFrameBuffer2.ID);
        
        //DrawBackground();
        //Gl.Clear(ClearBufferMask.ColorBufferBit);
        // use screen shader
        ShaderRegister.Get(ScreenShader).Use();
        ShaderRegister.Get(ScreenShader).SetFloat("time", Time.TotalTimeElapsed);
        Gl.BindVertexArray(_fullscreenVao);
        Gl.Disable(EnableCap.DepthTest);
        Gl.BindTexture(TextureTarget.Texture2D, MainFrameBuffer1.TextureColorBuffer);
        Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    private void RenderOverlay() {
        GlfwWindow.ImGuiController.Update(0.1f);
        // bind default framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        
        //OnImGui?.Invoke();
        
        GlfwWindow.ImGuiController.Render();
    }

    private void DrawBackground() {
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
