using System.Numerics;
using GameEngine.Numerics;
using WindowHandle = Silk.NET.GLFW.WindowHandle;

namespace GameEngine.Core.Input; 

public partial class InputHandler {

    private static bool _catchCursorOld = false;
    private static bool _catchCursor = false;
    
    public unsafe void ResetMouseDelta(WindowHandle* window) {
        _catchCursor = Input.IsKeyDown(KeyCode.LeftControl);
        
        Input.MouseDelta = Vec2<float>.Zero;
        if(_catchCursor)
            Application.Instance.Renderer.MainWindow.Glfw.SetCursorPos(window, (double) Application.Instance.Config.WindowWidth / 2d, (double) Application.Instance.Config.WindowHeight / 2d); //todo: not config but current glfw window size
    }
    
    public unsafe void HandleMouseInput(WindowHandle* window) {
        if(_catchCursorOld) {
            Application.Instance.Renderer.MainWindow.Glfw.GetCursorPos(window, out double x, out double y);
            Vec2<float> mousePos = new Vec2<float>((int) x, -(int) y);
            
            Vec2<float> windowCenter = new Vec2<float>(Application.Instance.Config.WindowWidth / 2.0f, -Application.Instance.Config.WindowHeight / 2.0f); //todo: not config but current glfw window size
            Input.MouseDelta += mousePos - windowCenter;
        }  else
            Input.MouseDelta = Vec2<float>.Zero;
        
        _catchCursorOld = _catchCursor;
    }
    
}
