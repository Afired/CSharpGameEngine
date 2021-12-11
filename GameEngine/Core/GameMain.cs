using System.Threading;
using GameEngine.Rendering.Camera2D;

namespace GameEngine.Core;

public sealed partial class Game {

    public static BaseCamera CurrentCamera { get; private set; }
    private bool _isRunning;
    private Thread _updateLoopThread;
    private Thread _physicsThread;
    private Thread _renderThread;
    
    
    public void Initialize() {
        _isRunning = true;
        _updateLoopThread = new Thread(UpdateLoop);
        _physicsThread = new Thread(FixedUpdateLoop);
        _renderThread = new Thread(StartRenderThread);
    }
    
    public void Start() {
        _updateLoopThread.Start();
        _physicsThread.Start();
        _renderThread.Start();
    }

    public static void SetActiveCamera(Camera2D camera2D) {
        CurrentCamera = camera2D;
    }
    
    private void Terminate() {
        _isRunning = false;
    }
    
}
