using GameEngine.Core;
using GameEngine.Numerics;
using GLFW;

namespace GameEngine.Input; 

internal partial class InputHandler {
    
    private Vector2 _windowCenter;
    
    internal static void ResetMouseDelta() {
        Input.MouseDelta = Vector2.Zero;
    }
    
    public InputHandler() {
        _windowCenter = new Vector2(Configuration.WindowWidth / 2, -Configuration.WindowHeight / 2);
    }
    
    
    internal void HandleMouseInput(Window window) {
        Glfw.GetCursorPosition(window, out double x, out double y);
        Vector2 mousePos = new Vector2((int) x, -(int) y);
        Input.MouseDelta += mousePos - _windowCenter;
        Glfw.SetCursorPosition(window, (double) Configuration.WindowWidth / 2d, (double) Configuration.WindowHeight / 2d);
    }
    
}
