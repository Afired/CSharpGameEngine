using GameEngine.Core.Numerics;
using WindowHandle = Silk.NET.GLFW.WindowHandle;

namespace GameEngine.Core.Input; 

public partial class InputHandler {

    private static bool _catchCursorOld = false;
    private static bool _catchCursor = false;
    
    public unsafe void ResetMouseDelta(WindowHandle* window) {
        _catchCursor = Input.IsKeyDown(KeyCode.LeftControl);
        
        Input.MouseDelta = Vector2.Zero;
        if(_catchCursor)
            Application.Instance.Renderer.MainWindow.Glfw.SetCursorPos(window, (double) Application.Instance!.Config.WindowWidth / 2d, (double) Application.Instance!.Config.WindowHeight / 2d); //todo: not config but current glfw window size
    }
    
    public unsafe void HandleMouseInput(WindowHandle* window) {
        if(_catchCursorOld) {
            Application.Instance.Renderer.MainWindow.Glfw.GetCursorPos(window, out double x, out double y);
            Vector2 mousePos = new Vector2((int) x, -(int) y);
        
            Vector2 windowCenter = new Vector2((float) Application.Instance!.Config.WindowWidth / 2, (float) -Application.Instance!.Config.WindowHeight / 2); //todo: not config but current glfw window size
            Input.MouseDelta += mousePos - windowCenter;
        }  else
            Input.MouseDelta = Vector2.Zero;
        
        _catchCursorOld = _catchCursor;
    }
    
}
