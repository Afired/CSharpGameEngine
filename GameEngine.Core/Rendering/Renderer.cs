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

public unsafe class Renderer : IDisposable {
    
    public WindowHandle* WindowHandle;
    public InputHandler InputHandler;
    
    public event OnLoad? OnLoad;
    public bool IsInit { get; private set; }
    public BaseCamera? CurrentCamera => _currentCameraRef.Target as BaseCamera;
    private readonly WeakReference _currentCameraRef = new(null);
    
    public GlfwWindow MainWindow { get; }
    
    // this frame buffer is the main frame buffer to render to, it is also used for any drawing of post processing when ping ponging (ping pong frame buffer 1)
    public FrameBuffer MainFrameBuffer1 { get; private set; }
    // this frame buffer is used for post processing (ping pong frame buffer 2)
    public FrameBuffer MainFrameBuffer2 { get; private set; }
    public FrameBuffer FinalFrameBuffer { get; private set; }

    public FrameBuffer ActiveFrameBuffer { get; private set; }
    public FrameBuffer InactiveFrameBuffer { get; private set; }

    internal void SwapActiveFrameBuffer() {
        // Swap via deconstruction
        (ActiveFrameBuffer, InactiveFrameBuffer) = (InactiveFrameBuffer, ActiveFrameBuffer);
        MainWindow.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, ActiveFrameBuffer.ID);
        MainWindow.Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }
    
    private uint _fullscreenVao;
    
    public Guid ScreenShader = new("fb20011e-1126-4439-8a9f-b11d7aa4f447");
    
    public LayerStack LayerStack { get; private set; }
    
    public Renderer(Application applicationCtx) {
        MainWindow = new GlfwWindow();
        MainFrameBuffer1 = new FrameBuffer(this, applicationCtx.Config.WindowWidth, applicationCtx.Config.WindowHeight, false); // game frame buffer1
        MainFrameBuffer2 = new FrameBuffer(this, applicationCtx.Config.WindowWidth, applicationCtx.Config.WindowHeight, false); // game frame buffer2
        FinalFrameBuffer = new FrameBuffer(this, applicationCtx.Config.WindowWidth, applicationCtx.Config.WindowHeight, true); // final framebuffer
        ActiveFrameBuffer = MainFrameBuffer1;
        InactiveFrameBuffer = MainFrameBuffer2;
        _fullscreenVao = GetFullScreenRenderQuadVao();
        LayerStack = new LayerStack();
        //LoadResources();
        
        applicationCtx.OnFinishedInit += LoadResources;
        
        InputHandler = new InputHandler();
        MainWindow.Glfw.SetKeyCallback(MainWindow.Handle, InputHandler.OnKeyAction);
        IsInit = true;
        OnLoad?.Invoke();
        WindowHandle = MainWindow.Handle;
    }
    
    private void LoadResources(Application application) {
        AssetDatabase.Reload(application);
    }
    
    public void Render() {
        // bind default framebuffer to render to
        MainWindow.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, MainFrameBuffer1.ID);
        MainWindow.Gl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        MainWindow.Gl.Enable(EnableCap.DepthTest); // reenable depth
        
        // render and draw frame
        if(CurrentCamera is not null) {
            
            //draw background
            MainWindow.Gl.ClearColor(CurrentCamera.BackgroundColor.R, CurrentCamera.BackgroundColor.G, CurrentCamera.BackgroundColor.B, CurrentCamera.BackgroundColor.A);
            
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
            layer.Attach(this);
            layer.Draw(this);
            layer.Detach(this);
        }
        
        DrawToBackBuffer();
        MainWindow.Glfw.SwapBuffers(WindowHandle);
    }
    
    private void DrawToBackBuffer() {
        // bind default framebuffer to render to
        MainWindow.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        MainWindow.Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        // use default screen shader
        AssetDatabase.Get<Shader>(ScreenShader).Use();
        MainWindow.Gl.BindVertexArray(_fullscreenVao);
        MainWindow.Gl.Disable(EnableCap.DepthTest);
        MainWindow.Gl.BindTexture(TextureTarget.Texture2D, FinalFrameBuffer.ColorAttachment);
        MainWindow.Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }
    
    private void DoPostProcessing() {
        SwapActiveFrameBuffer();
        // use screen shader
        AssetDatabase.Get<Shader>(ScreenShader).Use();
        AssetDatabase.Get<Shader>(ScreenShader).SetFloat("time", Time.TotalTimeElapsed);
        MainWindow.Gl.BindVertexArray(_fullscreenVao);
        MainWindow.Gl.Disable(EnableCap.DepthTest);
        MainWindow.Gl.BindTexture(TextureTarget.Texture2D, InactiveFrameBuffer.ColorAttachment);
        MainWindow.Gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
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
        
        uint vao = MainWindow.Gl.GenVertexArray();
        uint vbo = MainWindow.Gl.GenBuffer();
        
        MainWindow.Gl.BindVertexArray(vao);
        MainWindow.Gl.BindBuffer(GLEnum.ArrayBuffer, vbo);
        
        fixed(float* v = &vertexData[0]) {
            MainWindow.Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(float) * vertexData.Length), v, BufferUsageARB.StaticDraw);
        }
        
        // xyz
        MainWindow.Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*) (0 * sizeof(float)));
        MainWindow.Gl.EnableVertexAttribArray(0);
        
        // texture coordinates
        MainWindow.Gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*) (3 * sizeof(float)));
        MainWindow.Gl.EnableVertexAttribArray(1);
        
        MainWindow.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        MainWindow.Gl.BindVertexArray(0);
        
        return vao;
    }
    
    public void SetActiveCamera(BaseCamera? baseCamera) {
        _currentCameraRef.Target = baseCamera;
    }
    
    public void Dispose() {
        MainFrameBuffer1.Dispose();
        MainFrameBuffer2.Dispose();
        FinalFrameBuffer.Dispose();
        MainWindow.Dispose();
    }
    
}
