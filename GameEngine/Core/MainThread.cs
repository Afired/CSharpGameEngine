﻿using System.Threading;
using GameEngine.Debugging;
using GameEngine.Physics;
using GameEngine.Rendering;

namespace GameEngine.Core;

public sealed partial class Application {
    
    public static bool IsRunning { get; private set; }
    private Thread _updateLoopThread;
    private Thread _physicsThread;
    private Thread _renderThread;
    
    
    public void Initialize() {
        Console.Log("Initializing...");
        Console.Log("Initializing engine...");
        _updateLoopThread = new Thread(UpdateLoop);
        Console.LogSuccess("Initialized engine (1/3)");
        Console.Log("Initializing physics engine...");
        _physicsThread = new Thread(new PhysicsEngine().Initialize);
        Console.LogSuccess("Initialized physics engine (2/3)");
        Console.Log("Initializing render engine...");
        _renderThread = new Thread(new RenderingEngine().Initialize);
        Console.LogSuccess("Initialized render engine (3/3)");
        Console.LogSuccess("Initialization complete");
    }
    
    public void Start() {
        IsRunning = true;
        Console.Log("Starting...");
        Console.Log("Starting engine...");
        _updateLoopThread.Start();
        Console.LogSuccess("Started engine (1/3)");
        Console.Log("Starting physics engine...");
        _physicsThread.Start();
        Console.LogSuccess("Started physics engine (2/3)");
        Console.Log("Started render engine...");
        _renderThread.Start();
        Console.LogSuccess("Started render engine (3/3)");
        Console.LogSuccess("Started");
    }
    
    public static void Terminate() {
        IsRunning = false;
        Console.Log("Terminating...");
    }
    
}