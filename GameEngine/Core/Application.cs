using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GameEngine.Guard;
using GameEngine.Input;
using GameEngine.Physics;
using GameEngine.Rendering;
using GameEngine.SceneManagement;

namespace GameEngine.Core;

public sealed unsafe class Application {
    
    public static bool IsRunning { get; private set; }
    private PhysicsEngine _physicsEngine;
    private RenderingEngine _renderingEngine;
    
    
    public void Initialize() {
        Console.Log("Initializing...");
        Console.Log("Initializing engine...");
        
        Console.LogSuccess("Initialized engine (1/3)");
        Console.Log("Initializing physics engine...");
        
        _physicsEngine = new PhysicsEngine();
        _physicsEngine.Initialize();
        
        Console.LogSuccess("Initialized physics engine (2/3)");
        Console.Log("Initializing render engine...");
        
        _renderingEngine = new RenderingEngine();
        _renderingEngine.Initialize();
        
        Console.LogSuccess("Initialized render engine (3/3)");
        Console.LogSuccess("Initialization complete");
    }
    
    public void Run() {
        IsRunning = true;
        // starts loops on all threads
        Throw.If(!RenderingEngine.IsInit, "rendering engine has not yet been initialized or initialization has not been awaited");
        Throw.If(!PhysicsEngine.IsInit, "physics engine has not yet been initialized or initialization has not been awaited");
        
        UpdateLoop();
    }
    
    private void UpdateLoop() {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        while(IsRunning) {
            float elapsedTime = (float) stopwatch.Elapsed.TotalSeconds;
            if(Configuration.TargetFrameRate > 0) {
                TimeSpan timeOut = TimeSpan.FromSeconds(1 / Configuration.TargetFrameRate - elapsedTime);
                if(timeOut.TotalSeconds > 0) {
                    Thread.Sleep(timeOut);
                    elapsedTime = (float) stopwatch.Elapsed.TotalSeconds;
                }
            }
            Time.TotalTimeElapsed += (float) stopwatch.Elapsed.TotalSeconds;
            stopwatch.Restart();
            
            Hierarchy.Update(elapsedTime);
            InputHandler.ResetMouseDelta();
            
            _renderingEngine.Render(_renderingEngine.WindowHandle);
            
            // handle input
            Glfw.PollEvents();
            _renderingEngine.InputHandler.HandleMouseInput(_renderingEngine.WindowHandle);

            if(Glfw.WindowShouldClose(_renderingEngine.WindowHandle))
                Application.Terminate();
        }
    }
    
    public static void Terminate() {
        IsRunning = false;
        Console.Log("Terminating...");
    }
    
}
