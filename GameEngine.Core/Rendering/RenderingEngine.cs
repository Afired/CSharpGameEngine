using GameEngine.Core.Core;
using GameEngine.Core.Input;
using GameEngine.Core.Layers;
using GameEngine.Core.Rendering.Cameras;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Rendering.Window;
using GameEngine.Core.SceneManagement;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace GameEngine.Core.Rendering;

public delegate void OnLoad();

public static unsafe class RenderingEngine {

    internal static WindowHandle* WindowHandle;
    internal static InputHandler InputHandler;
    
    public static event OnLoad OnLoad;
    public static bool IsInit { get; private set; }
    public static BaseCamera CurrentCamera { get; private set; }
    
    public static GlfwWindow GlfwWindow;
    public static GL Gl => GlfwWindow.Gl;
    public static Glfw Glfw => GlfwWindow.Glfw;
    
    // this frame buffer is the main frame buffer to render to, it is also used for any drawing of post processing when ping ponging (ping pong frame buffer 1)
    public static FrameBuffer MainFrameBuffer1 { get; private set; }
    // this frame buffer is used for post processing (ping pong frame buffer 2)
    public static FrameBuffer MainFrameBuffer2 { get; private set; }

    private static FrameBuffer _activeFrameBuffer;
    private static FrameBuffer _inactiveFrameBuffer;

    internal static void SwapActiveFrameBuffer() {
        // Swap via deconstruction
        (_activeFrameBuffer, _inactiveFrameBuffer) = (_inactiveFrameBuffer, _activeFrameBuffer);
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, _activeFrameBuffer.ID);
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }
    
    private static uint _fullscreenVao;
    
    public static string ScreenShader = "ScreenShader";
    
    public static LayerStack LayerStack { get; private set; }
    
    
    internal static void Initialize() {
        Setup();
        InputHandler = new InputHandler();
        Glfw.SetKeyCallback(GlfwWindow.Handle, InputHandler.OnKeyAction);
        IsInit = true;
        OnLoad?.Invoke();
        WindowHandle = GlfwWindow.Handle;
    }
    
    private static void Setup() {
        GlfwWindow = new GlfwWindow();
        MainFrameBuffer1 = new FrameBuffer(new FrameBufferConfig() { Width = Configuration.WindowWidth, Height = Configuration.WindowHeight, AutomaticResize = true });
        MainFrameBuffer2 = new FrameBuffer(new FrameBufferConfig() { Width = Configuration.WindowWidth, Height = Configuration.WindowHeight, AutomaticResize = true });
        _activeFrameBuffer = MainFrameBuffer1;
        _inactiveFrameBuffer = MainFrameBuffer2;
        _fullscreenVao = GetFullScreenRenderQuadVao();
        LayerStack = new LayerStack();
        LoadResources();
    }
    
    private static void LoadResources() {
        ShaderRegister.Load();
        TextureRegister.Load();
        GeometryRegister.Load();
    }

    internal static void Render() {
        // bind default framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, MainFrameBuffer1.ID);
        Gl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        Gl.Enable(EnableCap.DepthTest); // reenable depth
        
        // render and draw frame
        if(CurrentCamera != null) {
            DrawBackground();
//            foreach(Layer layer in LayerStack.GetNormalLayers()) {
//                layer.Draw();
//            }
            Hierarchy.Draw();
        }
        
        //todo: post processing stack
        DoPostProcessing();
        
        //todo: implement in game GUI and Editor GUI as two separate things, so that they dont interfere
        foreach(Layer layer in LayerStack.GetOverlayLayers()) {
            layer.Attach();
            layer.Draw();
            layer.Detach();
        }
        
        DrawToBackBuffer();
        Glfw.SwapBuffers(WindowHandle);
    }

    private static void DrawToBackBuffer() {
        // bind default framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        // use default screen shader
        ShaderRegister.Get("ScreenShader").Use();
        Gl.BindVertexArray(_fullscreenVao);
        Gl.Disable(EnableCap.DepthTest);
        Gl.BindTexture(TextureTarget.Texture2D, _activeFrameBuffer.ColorAttachment);
        Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }
    
    private static void DoPostProcessing() {
        SwapActiveFrameBuffer();
        // use screen shader
        ShaderRegister.Get(ScreenShader).Use();
        ShaderRegister.Get(ScreenShader).SetFloat("time", Time.TotalTimeElapsed);
        Gl.BindVertexArray(_fullscreenVao);
        Gl.Disable(EnableCap.DepthTest);
        Gl.BindTexture(TextureTarget.Texture2D, _inactiveFrameBuffer.ColorAttachment);
        Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }
    
    private static void DrawBackground() {
        Gl.ClearColor(CurrentCamera.BackgroundColor.R, CurrentCamera.BackgroundColor.G, CurrentCamera.BackgroundColor.B, CurrentCamera.BackgroundColor.A);
    }

    private static uint GetFullScreenRenderQuadVao() {
        
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
