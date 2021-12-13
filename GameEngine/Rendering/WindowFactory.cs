using System.Drawing;
using GameEngine.Core;
using GameEngine.Debugging;
using GLFW;
using OpenGL;

namespace GameEngine.Rendering; 

internal static class WindowFactory {

    public static Window CreateWindow() {
        return CreateWindow(Configuration.WindowWidth, Configuration.WindowHeight, Configuration.WindowTitle, Configuration.WindowIsResizable, Configuration.DoUseVsync);
    }

    private static Window CreateWindow(int width, int height, string title, bool isResizable, bool vsync) {

        Glfw.Init();
        
        Glfw.WindowHint(Hint.ContextVersionMajor, 3);
        Glfw.WindowHint(Hint.ContextVersionMinor, 3);
        Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
        
        Glfw.WindowHint(Hint.Focused, true);
        Glfw.WindowHint(Hint.Resizable, Configuration.WindowIsResizable);

        Window window = Glfw.CreateWindow(width, height, title, Monitor.None, Window.None);

        if(window == Window.None) {
            throw new WindowFailedToLoadException();
        }

        Rectangle screen = Glfw.PrimaryMonitor.WorkArea;
        int x = (screen.Width - width) / 2;
        int y = (screen.Height - height) / 2;
        Glfw.SetWindowPosition(window, x, y);
        
        Glfw.MakeContextCurrent(window);
        GL.Import(Glfw.GetProcAddress);
        
        GL.glViewport(0, 0, width, height);
        Glfw.SwapInterval(vsync ? 1 : 0);
        
        Cursor cursor = Glfw.CreateStandardCursor(CursorType.Crosshair);
        Glfw.SetCursor(window, cursor);
        //Glfw.SetInputMode(window, InputMode.Cursor, 1 << 3);
        /*
         GLFW_CURSOR_NORMAL makes the cursor visible and behaving normally.
        GLFW_CURSOR_HIDDEN makes the cursor invisible when it is over the content area of the window but does not restrict the cursor from leaving.
        GLFW_CURSOR_DISABLED hides and grabs the cursor, providing virtual and unlimited cursor movement. This is useful for implementing for example 3D camera controls.
         */
        
        return window;
    }
    
}
