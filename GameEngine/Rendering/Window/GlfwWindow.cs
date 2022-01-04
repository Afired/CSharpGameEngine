//ref https://gist.github.com/tgfrerer/0d2b92763111a4e5da17
//ref https://stackoverflow.com/questions/1554674/subclassing-a-external-window-in-c-sharp-net
using System;
using GameEngine.Core;
using ImGuiNET;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using VideoMode = Silk.NET.Windowing.VideoMode;

namespace GameEngine.Rendering.Window;

public delegate void OnResize(uint width, uint height);

public sealed unsafe class GlfwWindow : IDisposable {

    public event OnResize OnResize;
    
    public WindowHandle* Handle => GlfwWindowing.GetHandle(_window);
    private readonly IWindow _window;
    public readonly Glfw Glfw;
    public readonly ImGuiController ImGuiController;
    public readonly GL Gl;
    private readonly IInputContext _inputContext;
    
    
    WindowOptions windowOptions = new WindowOptions() {
        Position = new Vector2D<int>(-1, -1), // ? doesnt work
        Samples = 1, // multisample anti aliasing?
        Size = new Vector2D<int>((int) Configuration.WindowWidth, (int) Configuration.WindowHeight), // size of the window in pixel
        Title = Configuration.WindowTitle, // title of the window
        IsVisible = true, // ?
        TransparentFramebuffer = false, // makes window transparent as long as no color is drawn
        VideoMode = VideoMode.Default,
        VSync = true, // vertical synchronisation
        WindowBorder = WindowBorder.Hidden, // window border type
        WindowClass = "idk", // ?
        WindowState = WindowState.Normal, // window state
        API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Debug, new APIVersion(3, 3)), // graphics api
        FramesPerSecond = -1, // fps
        IsEventDriven = true, // ?
        PreferredBitDepth = null, // ?
        ShouldSwapAutomatically = false, // if true swaps frame buffers at the end of rendering automatically
        UpdatesPerSecond = 1, // ? polling?
        IsContextControlDisabled = true, // ?
        PreferredDepthBufferBits = null, // ?
        PreferredStencilBufferBits = null, // ?
    };
    
    public GlfwWindow() {
        
        // use glfw for window api
        GlfwWindowing.Use();
        
        // create and initialize window
        _window = Silk.NET.Windowing.Window.Create(windowOptions);
        _window.Initialize();
        
        // once window has been initialized, create ImGUI controller, get gl context, get input context and initialize glfw api
        Gl = _window.CreateOpenGL();
        _inputContext = _window.CreateInput();
        ImGuiController = new ImGuiController(Gl, _window, _inputContext);
        //ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.ViewportsEnable; // enable ImGui multi window viewports || currently not supported with OpenGl as rendering context?
        ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable; // enable ImGui docking
        Glfw = Glfw.GetApi();
        Glfw.Init();
        
        // set to middle of the screen
        Silk.NET.GLFW.Monitor* monitor = Glfw.GetPrimaryMonitor();
        Glfw.GetMonitorWorkarea(monitor, out int width, out int height, out int x, out int y);
        int middleMonitorX = (x + width) / 2;
        int middleMonitorY = (y + height) / 2;
        int windowOffsetX = ((int)Configuration.WindowWidth / 2);
        int windowOffsetY = ((int)Configuration.WindowHeight / 2);
        Glfw.SetWindowPos(Handle, (middleMonitorX - windowOffsetX), (middleMonitorY - windowOffsetY));

        // Handle resizes
        _window.FramebufferResize += s => {
            // Adjust the viewport to the new window size
            Gl.Viewport(s);
            OnResize?.Invoke((uint) s.X, (uint) s.Y);
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
