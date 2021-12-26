using System.Diagnostics;
using GameEngine.Core;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace GameEngine.Rendering; 

internal static class WindowFactory {
    
    public static unsafe WindowHandle* CreateWindow(out GL gl, out Glfw glfw) {
        return CreateWindow(Configuration.WindowWidth, Configuration.WindowHeight, Configuration.WindowTitle, Configuration.WindowIsResizable, Configuration.DoUseVsync, out gl, out glfw);
    }
    
    private static unsafe WindowHandle* CreateWindow(uint width, uint height, string title, bool isResizable, bool vsync, out GL gl, out Glfw glfw) {
        
        glfw = Glfw.GetApi();
        glfw.Init();
        gl = GL.GetApi(glfw.GetProcAddress);
        
        glfw.WindowHint(WindowHintInt.ContextVersionMajor, 3);
        glfw.WindowHint(WindowHintInt.ContextVersionMinor, 3);
        glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
        
        glfw.WindowHint(WindowHintBool.Focused, true);
        glfw.WindowHint(WindowHintBool.Resizable, Configuration.WindowIsResizable);
        
        //Window window = Glfw.CreateWindow((int) width, (int) height, title, Monitor.None, Window.None);
        
        WindowHandle* window = glfw.CreateWindow((int)width, (int)height, title, null, null);

        Debug.Assert(window != null, "Window failed to load");
        
        //Rectangle screen = Glfw.PrimaryMonitor.WorkArea;
        //Monitor* monitor = Glfw.GetPrimaryMonitor();
        //int x = (int) (screen.Width - width) / 2;
        //int y = (int) (screen.Height - height) / 2;
        //Glfw.SetWindowPos(window, x, y);
        
        glfw.MakeContextCurrent(window);
        //gl.Import(Glfw.GetProcAddress);

        gl.Viewport(0, 0, width, height);
        glfw.SwapInterval(vsync ? 1 : 0);
        
        Cursor* cursor = glfw.CreateStandardCursor(CursorShape.Crosshair);
        glfw.SetCursor(window, cursor);
        glfw.SetCursorPos(window, (double) width / 2d, (double) height / 2d);
        glfw.SetInputMode(window, CursorStateAttribute.Cursor, CursorModeValue.CursorHidden);
        
        // MSAA
        //Glfw.WindowHint(Hint.Samples, 4);
        //GL.glEnable(GL.GL_MULTISAMPLE);
        
        // more open gl setup
        glfw.MakeContextCurrent(window);
        //gl.Import(Glfw.GetProcAddress);
        gl.Viewport(0, 0, width, height);
        gl.Enable(EnableCap.DepthTest);
        gl.DepthFunc(DepthFunction.Lequal);
        
        return window;
    }
    
}
