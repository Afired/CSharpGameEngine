using System.Diagnostics;
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
        
        Debug.Assert(window != Window.None, "Window failed to load");
        
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
        Glfw.SetCursorPosition(window, (double) width / 2d, (double) height / 2d);
        Glfw.SetInputMode(window, InputMode.Cursor, (int) CursorMode.Hidden);
        
        // MSAA
        //Glfw.WindowHint(Hint.Samples, 4);
        //GL.glEnable(GL.GL_MULTISAMPLE);
        
        // more open gl setup
        Glfw.MakeContextCurrent(window);
        GL.Import(Glfw.GetProcAddress);
        GL.glViewport(0, 0, width, height);
        GL.glEnable(GL.GL_DEPTH);
        GL.glEnable(GL.GL_DEPTH_TEST);
        GL.glDepthFunc(GL.GL_LEQUAL);
        
        return window;
    }
    
}
