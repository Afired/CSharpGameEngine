using GameEngine.Core.Core;
using GameEngine.Core.Numerics;
using WindowHandle = Silk.NET.GLFW.WindowHandle;

namespace GameEngine.Core.Input; 

public partial class InputHandler {
    
    private Vector2 _windowCenter;

    private static bool _catchCursor = false;
    
    
    public void ResetMouseDelta() {
        if(!_catchCursor)
            return;
        Input.MouseDelta = Vector2.Zero;
    }
    
    public InputHandler() {
        _windowCenter = new Vector2(Configuration.WindowWidth / 2, -Configuration.WindowHeight / 2);
    }
    
    
    public unsafe void HandleMouseInput(WindowHandle* window) {
        if(!_catchCursor)
            return;
        Glfw.GetCursorPos(window, out double x, out double y);
        Vector2 mousePos = new Vector2((int) x, -(int) y);
        Input.MouseDelta += mousePos - _windowCenter;
        Glfw.SetCursorPos(window, (double) Configuration.WindowWidth / 2d, (double) Configuration.WindowHeight / 2d);
    }
    
}
