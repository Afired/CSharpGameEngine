// This file is part of Silk.NET.
//
// You may modify and distribute Silk.NET under the terms
// of the MIT license. See the LICENSE file for details.

using Silk.NET.GLFW;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing.Glfw;
using VideoMode = Silk.NET.Windowing.VideoMode;

namespace ImGui
{
    
    class Program
    {
        static void Main(string[] args)
        {
            
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
            
            
            GlfwWindowing.Use();

            // Create Silk.NET window with window options
            /*using*/ var window = Window.Create(windowOptions);

            // Declare some variables
            ImGuiController controller = null;
            GL gl = null;
            IInputContext inputContext = null;

            // Our loading function
            window.Load += () => {
                controller = new ImGuiController(gl = window.CreateOpenGL(), window, inputContext = window.CreateInput());
                Console.WriteLine(GlfwWindowing.IsViewGlfw(window));
                Glfw glfw = Glfw.GetApi();
                glfw.Init();
                unsafe {
                    //glfw.HideWindow(GlfwWindowing.GetHandle(window));
                    Cursor* cursor = glfw.CreateStandardCursor(CursorShape.Crosshair);
                    glfw.SetCursor(GlfwWindowing.GetHandle(window), cursor);
                }
            };

            // Handle resizes
            window.FramebufferResize += s =>
            {
                // Adjust the viewport to the new window size
                gl.Viewport(s);
            };

            // The render function
            window.Render += delta =>
            {
                // Make sure ImGui is up-to-date
                controller.Update((float) delta);

                // This is where you'll do any rendering beneath the ImGui context
                // Here, we just have a blank screen.
               // gl.ClearColor(Color.FromArgb(255, (int) (.45f * 255), (int) (.55f * 255), (int) (.60f * 255)));
                gl.Clear((uint) ClearBufferMask.ColorBufferBit);

                // This is where you'll do all of your ImGUi rendering
                // Here, we're just showing the ImGui built-in demo window.
                ImGuiNET.ImGui.ShowDemoWindow();

                // Make sure ImGui renders too!
                controller.Render();
                window.SwapBuffers();
            };

            // The closing function
            window.Closing += () =>
            {
                // Dispose our controller first
                controller?.Dispose();

                // Dispose the input context
                inputContext?.Dispose();

                // Unload OpenGL
                gl?.Dispose();
            };

            // Now that everything's defined, let's run this bad boy!
            window.Run();
        }
    }
}