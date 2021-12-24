using System.Diagnostics;
using System.Drawing;
using GameEngine.Core;
using GLFW;
using Silk.NET.OpenGL;

namespace GameEngine.Rendering; 

internal static class WindowFactory {
    
    public static Window CreateWindow(out GL gl) {
        return CreateWindow(Configuration.WindowWidth, Configuration.WindowHeight, Configuration.WindowTitle, Configuration.WindowIsResizable, Configuration.DoUseVsync, out gl);
    }
    
    private static Window CreateWindow(uint width, uint height, string title, bool isResizable, bool vsync, out GL gl) {
        
        Glfw.Init();
        gl = GL.GetApi(Glfw.GetProcAddress);
        
        Glfw.WindowHint(Hint.ContextVersionMajor, 3);
        Glfw.WindowHint(Hint.ContextVersionMinor, 3);
        Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
        
        Glfw.WindowHint(Hint.Focused, true);
        Glfw.WindowHint(Hint.Resizable, Configuration.WindowIsResizable);
        
        Window window = Glfw.CreateWindow((int) width, (int) height, title, Monitor.None, Window.None);
        
        Debug.Assert(window != Window.None, "Window failed to load");
        
        Rectangle screen = Glfw.PrimaryMonitor.WorkArea;
        int x = (int) (screen.Width - width) / 2;
        int y = (int) (screen.Height - height) / 2;
        Glfw.SetWindowPosition(window, x, y);
        
        Glfw.MakeContextCurrent(window);
        //gl.Import(Glfw.GetProcAddress);

        gl.Viewport(0, 0, width, height);
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
        //gl.Import(Glfw.GetProcAddress);
        gl.Viewport(0, 0, width, height);
        gl.Enable(EnableCap.DepthTest);
        gl.DepthFunc(DepthFunction.Lequal);
        
        return window;
    }
    
}
