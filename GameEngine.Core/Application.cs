using System;
using System.Diagnostics;
using System.Threading;
using GameEngine.Core.Guard;
using GameEngine.Core.Physics;
using GameEngine.Core.Rendering;
using GameEngine.Core.SceneManagement;

namespace GameEngine.Core;

public abstract unsafe class Application<T> where T : Application<T> {
    
    public static T Instance { get; private set; } = null!;
    public bool IsRunning { get; protected set; }


    public Application() {
        Instance = (this as T)!;
    }
    
    public virtual void Initialize() {
        Debugging.Console.Log("Initializing...");
        Debugging.Console.Log("Initializing engine...");
        Debugging.Console.LogSuccess("Initialized engine (1/3)");
        
        Debugging.Console.Log("Initializing physics engine...");
        PhysicsEngine.Initialize();
        Debugging.Console.LogSuccess("Initialized physics engine (2/3)");
        
        Debugging.Console.Log("Initializing render engine...");
        Renderer.Initialize();
        Debugging.Console.LogSuccess("Initialized render engine (3/3)");
        
        Debugging.Console.LogSuccess("Initialization complete");
    }
    
    public virtual void Run() {
        if(IsRunning)
            throw new Exception("Application is already running!");
        IsRunning = true;
        // starts loops on all threads
        Throw.If(!Renderer.IsInit, "rendering engine has not yet been initialized or initialization has not been fully completed");
        Throw.If(!PhysicsEngine.IsInit, "physics engine has not yet been initialized or initialization has not been fully completed");
        
        Loop();
    }
    
    protected virtual void Loop() {
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
            
            Hierarchy.Awake();
            Hierarchy.Update(updateTime);
            Renderer.InputHandler.ResetMouseDelta();
            
            float physicsTime = (float) physicsTimer.Elapsed.TotalSeconds;
            if(physicsTime > Configuration.FixedTimeStep) {
                Hierarchy.PrePhysicsUpdate();
                PhysicsEngine.DoStep();
                Hierarchy.PhysicsUpdate(Configuration.FixedTimeStep);
                physicsTimer.Restart();
            }
            
            Renderer.Render();
            
            // handle input
            Glfw.PollEvents();
            Renderer.InputHandler.HandleMouseInput(Renderer.WindowHandle);
            
            if(Glfw.WindowShouldClose(Renderer.WindowHandle))
                Terminate();
        }
        
    }
    
    public virtual void Terminate() {
        IsRunning = false;
        Console.Log("Is terminating...");
    }
    
}
