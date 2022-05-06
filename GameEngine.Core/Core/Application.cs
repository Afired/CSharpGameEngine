using System;
using System.Diagnostics;
using System.Threading;
using GameEngine.Core.Guard;
using GameEngine.Core.Physics;
using GameEngine.Core.Rendering;
using GameEngine.Core.SceneManagement;

namespace GameEngine.Core.Core;

public static unsafe class Application {
    
    public static bool IsRunning { get; private set; }
    
    
    public static void Initialize() {
        Debugging.Console.Log("Initializing...");
        Debugging.Console.Log("Initializing engine...");
        Debugging.Console.LogSuccess("Initialized engine (1/3)");
        
        Debugging.Console.Log("Initializing physics engine...");
        PhysicsEngine.Initialize();
        Debugging.Console.LogSuccess("Initialized physics engine (2/3)");
        
        Debugging.Console.Log("Initializing render engine...");
        RenderingEngine.Initialize();
        Debugging.Console.LogSuccess("Initialized render engine (3/3)");
        
        Debugging.Console.LogSuccess("Initialization complete");
    }
    
    public static void Run() {
        if(IsRunning)
            throw new Exception("Application is already running!");
        IsRunning = true;
        // starts loops on all threads
        Throw.If(!RenderingEngine.IsInit, "rendering engine has not yet been initialized or initialization has not been fully completed");
        Throw.If(!PhysicsEngine.IsInit, "physics engine has not yet been initialized or initialization has not been fully completed");
        
        Loop();
    }
    
    private static void Loop() {
        Stopwatch updateTimer = new();
        Stopwatch physicsTimer = new();
        updateTimer.Start();
        physicsTimer.Start();
        
        while(IsRunning) {
            
            float updateTime = (float) updateTimer.Elapsed.TotalSeconds;
            if(Configuration.TargetFrameRate > 0) {
                TimeSpan timeOut = TimeSpan.FromSeconds(1 / Configuration.TargetFrameRate - updateTime);
                if(timeOut.TotalSeconds > 0) {
                    Thread.Sleep(timeOut);
                    updateTime = (float) updateTimer.Elapsed.TotalSeconds;
                }
            }
            Time.TotalTimeElapsed += (float) updateTimer.Elapsed.TotalSeconds;
            updateTimer.Restart();
            
            Hierarchy.Update(updateTime);
            RenderingEngine.InputHandler.ResetMouseDelta();
            
            RenderingEngine.Render();
            
            // handle input
            Glfw.PollEvents();
            RenderingEngine.InputHandler.HandleMouseInput(RenderingEngine.WindowHandle);
            
            float physicsTime = (float) physicsTimer.Elapsed.TotalSeconds;
            if(physicsTime > Configuration.FixedTimeStep) {
                PhysicsEngine.DoStep();
                physicsTimer.Restart();
            }
            
            if(Glfw.WindowShouldClose(RenderingEngine.WindowHandle))
                Terminate();
        }
        
    }
    
    public static void Terminate() {
        IsRunning = false;
        Console.Log("Terminating...");
    }
    
}
