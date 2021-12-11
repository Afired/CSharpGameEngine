using System;
using System.Diagnostics;
using System.Threading;
using GameEngine.Rendering;
using GLFW;
using OpenGL;

namespace GameEngine.Core;

public delegate void OnUpdate(float deltaTime);
public delegate void OnFixedUpdate(float fixedDeltaTime);

public class Game {
    
    public static event OnUpdate OnUpdate;
    public static event OnFixedUpdate OnFixedUpdate;
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
    
    private void StartRenderThread() {
        Window window = WindowFactory.CreateWindow(900, 600, "Window Title", false);

        while(!Glfw.WindowShouldClose(window)) {
            Glfw.PollEvents();
            Render(window);
        }
        Terminate();
    }
    
    private void Render(Window window) {
        GL.glClearColor(1.0f, 0.0f, 0.0f, 1.0f);
        GL.glClear(GL.GL_COLOR_BUFFER_BIT);
        
        Glfw.SwapBuffers(window);
    }

    private void Terminate() {
        _isRunning = false;
    }

    private void UpdateLoop() {
        Stopwatch stopwatch = new();
        while(_isRunning) {
            float elapsedTime = (float) stopwatch.Elapsed.TotalSeconds;
            if(Configuration.TargetFrameRate > 0) {
                TimeSpan timeOut = TimeSpan.FromSeconds(1 / Configuration.TargetFrameRate - elapsedTime);
                if(timeOut.TotalSeconds > 0) {
                    Thread.Sleep(timeOut);
                    elapsedTime = (float) stopwatch.Elapsed.TotalSeconds;
                }
            }
            stopwatch.Restart();
            OnUpdate?.Invoke(elapsedTime);
        }
    }
    
    private void FixedUpdateLoop() {
        Stopwatch stopwatch = new();
        while(_isRunning) {
            float elapsedTime = (float) stopwatch.Elapsed.TotalSeconds;
            TimeSpan timeOut = TimeSpan.FromSeconds(Configuration.FixedTimeStep - elapsedTime);
            Thread.Sleep(timeOut);
            stopwatch.Restart();
            OnFixedUpdate?.Invoke(Configuration.FixedTimeStep);
        }
    }
    
}
