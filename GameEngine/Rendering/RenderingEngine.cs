using Dear_ImGui_Sample;
using GameEngine.Core;
using GameEngine.Input;
using GameEngine.Rendering.Cameras;
using GameEngine.Rendering.Shaders;
using ImGuiNET;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace GameEngine.Rendering;

public delegate void OnLoad();
public delegate void OnDraw();

public sealed unsafe class RenderingEngine {
    
    public static event OnLoad OnLoad;
    public static event OnDraw OnDraw;
    public static BaseCamera CurrentCamera { get; private set; }
    public static GL Gl;
    public static Glfw Glfw;

    public GlfwImGuiController controller;
    
    internal void Initialize() {
        
        Setup(out WindowHandle* window, out FrameBuffer frameBuffer, out uint vao);
        InputHandler inputHandler = new InputHandler();
        Glfw.SetKeyCallback(window, inputHandler.OnKeyAction);

        //GUI.GUI gui = new GUI.GUI();
        //gui.Attach();

        controller = new GlfwImGuiController((int) Configuration.WindowWidth, (int) Configuration.WindowHeight);

        RenderLoop(window, frameBuffer, vao, inputHandler);
    }
    
    private void RenderLoop(WindowHandle* window, FrameBuffer frameBuffer, uint vao, InputHandler inputHandler) {
        
        while(!Glfw.WindowShouldClose(window)) {
            
            // render and draw frame
            if(CurrentCamera != null)
                Render(window, frameBuffer, vao);
            
            // handle input
            Glfw.PollEvents();
            inputHandler.HandleMouseInput(window);

        }
        
        Game.Terminate();
    }

    private void Setup(out WindowHandle* window, out FrameBuffer frameBuffer, out uint vao) {
        window = WindowFactory.CreateWindow(out Gl, out Glfw);
        
        frameBuffer = new FrameBuffer();
        
        vao = GetFullScreenRenderQuadVao();
        
        LoadResources();
    }

    private void LoadResources() {
        ShaderRegister.Load();
        TextureRegister.Load();
        OnLoad?.Invoke();
    }

    private void Render(WindowHandle* window, FrameBuffer frameBuffer, uint vao) {
        RenderFirstPass(frameBuffer.ID, window);
        RenderSecondPass(frameBuffer.TextureColorBuffer, vao);
        Glfw.SwapBuffers(window);
    }

    private void RenderFirstPass(uint frameBuffer, WindowHandle* window) {
        // bind custom framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
        Gl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        Gl.Enable(EnableCap.DepthTest); // reenable depth test
        OnDraw?.Invoke();
        
        controller.Update(window, 0.1f);
        ImGui.ShowDemoWindow();
        controller.Render();
    }
    
    private void RenderSecondPass(uint textureColorBuffer, uint vao) {
        // bind default framebuffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        
        RenderBackground();
        Gl.Clear(ClearBufferMask.ColorBufferBit);
        // use shader
        string screenShader = "ScreenShader";
        ShaderRegister.Get(screenShader).Use();
        ShaderRegister.Get(screenShader).SetFloat("time", Time.TotalTimeElapsed);
        Gl.BindVertexArray(vao);
        Gl.Disable(EnableCap.DepthTest);
        Gl.BindTexture(TextureTarget.Texture2D, textureColorBuffer);
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
