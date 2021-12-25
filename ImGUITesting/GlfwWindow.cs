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
    private Glfw _glfw;
    private ImGuiController _imGuiController;
    private GL _gl;
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
        GlfwWindowing.Use();
        
        // Create Silk.NET window with window options
        _window = Window.Create(windowOptions);
        
        // Our loading function
        _window.Load += () => {
            _imGuiController = new ImGuiController(_gl = _window.CreateOpenGL(), _window, _inputContext = _window.CreateInput());
            _glfw = Glfw.GetApi();
            _glfw.Init();
        };
        
        // Handle resizes
        _window.FramebufferResize += s =>
        {
            // Adjust the viewport to the new window size
            _gl.Viewport(s);
        };
        
        // The render function
        _window.Render += delta =>
        {
            // Make sure ImGui is up-to-date
            _imGuiController.Update((float) delta);
        
            // This is where you'll do any rendering beneath the ImGui context
            // Here, we just have a blank screen.
            // gl.ClearColor(Color.FromArgb(255, (int) (.45f * 255), (int) (.55f * 255), (int) (.60f * 255)));
            _gl.Clear((uint) ClearBufferMask.ColorBufferBit);
        
            // This is where you'll do all of your ImGUi rendering
            // Here, we're just showing the ImGui built-in demo window.
            ImGuiNET.ImGui.ShowDemoWindow();
        
            // Make sure ImGui renders too!
            _imGuiController.Render();
            _window.SwapBuffers();
        };
        
        // The closing function
        _window.Closing += () =>
        {
            // Dispose our controller first
            _imGuiController?.Dispose();
        
            // Dispose the input context
            _inputContext?.Dispose();
        
            // Unload OpenGL
            _gl?.Dispose();
        };
        
        // Now that everything's defined, let's run this bad boy!
        _window.Run();
    }

    public void Dispose() {
        _window.Dispose();
    }
    
}
