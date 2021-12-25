using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using VideoMode = Silk.NET.Windowing.VideoMode;

namespace ImGui; 

public sealed unsafe class GlfwWindow : IDisposable {
    
    public WindowHandle* Handle => GlfwWindowing.GetHandle(_window);
    private IWindow _window;
    public Glfw Glfw;
    public ImGuiController ImGuiController;
    public GL Gl;
    private IInputContext _inputContext;
    
    
    WindowOptions windowOptions = new WindowOptions() {
        Position = new Vector2D<int>(-1, -1), // ? doesnt work
        Samples = 1, // multisample anti aliasing?
        Size = new Vector2D<int>(1600, 900), // size of the window in pixel
        Title = "Title", // title of the window
        IsVisible = true, // ?
        TransparentFramebuffer = false, // makes window transparent as long as no color is drawn
        VideoMode = VideoMode.Default,
        VSync = true, // vertical synchronisation
        WindowBorder = WindowBorder.Fixed, // window border type
        WindowClass = "idk", // ?
        WindowState = WindowState.Normal, // window state
        API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Debug, new APIVersion(3, 3)), // graphics api
        FramesPerSecond = -1, // fps
        IsEventDriven = true, // ?
        PreferredBitDepth = new Vector4D<int>(255, 255, 255, 255), // ?
        ShouldSwapAutomatically = false, // if true swaps frame buffers at the end of rendering automatically
        UpdatesPerSecond = 1, // ? polling?
        IsContextControlDisabled = true, // ?
        PreferredDepthBufferBits = 255, // ?
        PreferredStencilBufferBits = 255, // ?
    };

    public GlfwWindow() {
        
        // use glfw for window api
        GlfwWindowing.Use();
        
        // create and initialize window
        _window = Window.Create(windowOptions);
        _window.Initialize();
        
        // once window has been initialized, create ImGUI controller, get gl context, get input context and initialize glfw api
        Gl = _window.CreateOpenGL();
        _inputContext = _window.CreateInput();
        ImGuiController = new ImGuiController(Gl, _window, _inputContext);
        Glfw = Glfw.GetApi();
        Glfw.Init();

        // Handle resizes
        _window.FramebufferResize += s => {
            // Adjust the viewport to the new window size
            Gl.Viewport(s);
        };

        // dispose components when window is closing
        //_window.Closing += Dispose;

    }

    public void Dispose() {
        Glfw.Dispose();
        Gl.Dispose();
        _inputContext.Dispose();
        ImGuiController.Dispose();
        _window.Dispose();
    }
    
}
