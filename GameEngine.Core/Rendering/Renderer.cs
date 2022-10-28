using System;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Input;
using GameEngine.Core.Layers;
using GameEngine.Core.Nodes;
using GameEngine.Core.Rendering.Window;
using GameEngine.Core.SceneManagement;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using Shader = GameEngine.Core.Rendering.Shaders.Shader;

namespace GameEngine.Core.Rendering;

public delegate void OnLoad();

public static unsafe class Renderer {
    
    public static WindowHandle* WindowHandle;
    public static InputHandler InputHandler;
    
    public static event OnLoad OnLoad;
    public static bool IsInit { get; private set; }
    public static BaseCamera? CurrentCamera => _currentCameraRef.Target as BaseCamera;
    private static readonly WeakReference _currentCameraRef = new(null);
    
    public static GlfwWindow GlfwWindow;
    public static GL Gl => GlfwWindow.Gl;
    public static Glfw Glfw => GlfwWindow.Glfw;
    
    // this frame buffer is the main frame buffer to render to, it is also used for any drawing of post processing when ping ponging (ping pong frame buffer 1)
    public static FrameBuffer MainFrameBuffer1 { get; private set; }
    // this frame buffer is used for post processing (ping pong frame buffer 2)
    public static FrameBuffer MainFrameBuffer2 { get; private set; }
    public static FrameBuffer FinalFrameBuffer { get; private set; }

    public static FrameBuffer ActiveFrameBuffer { get; private set; }
    public static FrameBuffer _inactiveFrameBuffer { get; private set; }

    internal static void SwapActiveFrameBuffer() {
        // Swap via deconstruction
        (ActiveFrameBuffer, _inactiveFrameBuffer) = (_inactiveFrameBuffer, ActiveFrameBuffer);
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, ActiveFrameBuffer.ID);
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }
    
    private static uint _fullscreenVao;
    
    public static Guid ScreenShader = new("fb20011e-1126-4439-8a9f-b11d7aa4f447");
    
    public static LayerStack LayerStack { get; private set; }
    
    
    public static void Initialize() {
        Setup();
        InputHandler = new InputHandler();
        Glfw.SetKeyCallback(GlfwWindow.Handle, InputHandler.OnKeyAction);
        IsInit = true;
        OnLoad?.Invoke();
        WindowHandle = GlfwWindow.Handle;
    }
    
    private static void Setup() {
        GlfwWindow = new GlfwWindow();
        MainFrameBuffer1 = new FrameBuffer(Application.Instance!.Config.WindowWidth, Application.Instance!.Config.WindowHeight, false); // game frame buffer1
        MainFrameBuffer2 = new FrameBuffer(Application.Instance!.Config.WindowWidth, Application.Instance!.Config.WindowHeight, false); // game frame buffer2
        FinalFrameBuffer = new FrameBuffer(Application.Instance!.Config.WindowWidth, Application.Instance!.Config.WindowHeight, true); // final framebuffer
        ActiveFrameBuffer = MainFrameBuffer1;
        _inactiveFrameBuffer = MainFrameBuffer2;
        _fullscreenVao = GetFullScreenRenderQuadVao();
        LayerStack = new LayerStack();
        LoadResources();
    }
    
    private static void LoadResources() {
        AssetDatabase.Reload();
    }
    
    public static void Render() {
        // bind default framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, MainFrameBuffer1.ID);
        Gl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        Gl.Enable(EnableCap.DepthTest); // reenable depth
        
        // render and draw frame
        if(CurrentCamera is not null) {
            
            //draw background
            Gl.ClearColor(CurrentCamera.BackgroundColor.R, CurrentCamera.BackgroundColor.G, CurrentCamera.BackgroundColor.B, CurrentCamera.BackgroundColor.A);
            
//            foreach(Layer layer in LayerStack.GetNormalLayers()) {
//                layer.Draw();
//            }
            Hierarchy.Draw();
        }
        
        //todo: post processing stack
        // DoPostProcessing();
        
        //todo: implement in game GUI and Editor GUI as two separate things, so that they dont interfere
        FinalFrameBuffer.Bind();
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
        AssetDatabase.Get<Shader>(ScreenShader).Use();
        Gl.BindVertexArray(_fullscreenVao);
        Gl.Disable(EnableCap.DepthTest);
        Gl.BindTexture(TextureTarget.Texture2D, FinalFrameBuffer.ColorAttachment);
        Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }
    
    private static void DoPostProcessing() {
        SwapActiveFrameBuffer();
        // use screen shader
        AssetDatabase.Get<Shader>(ScreenShader).Use();
        AssetDatabase.Get<Shader>(ScreenShader).SetFloat("time", Time.TotalTimeElapsed);
        Gl.BindVertexArray(_fullscreenVao);
        Gl.Disable(EnableCap.DepthTest);
        Gl.BindTexture(TextureTarget.Texture2D, _inactiveFrameBuffer.ColorAttachment);
        Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
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
        
        return vao;
    }
    
    public static void SetActiveCamera(BaseCamera? baseCamera) {
        _currentCameraRef.Target = baseCamera;
    }
    
}
