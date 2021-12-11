using System;
using System.Diagnostics;
using System.Threading;
using GameEngine.Rendering;
using GLFW;
using OpenGL;

namespace GameEngine.Core;

public sealed partial class Game {
    
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
    
    private void Terminate() {
        _isRunning = false;
    }
    
}
