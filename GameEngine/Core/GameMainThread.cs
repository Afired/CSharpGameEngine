using System.Threading;
using GameEngine.Debugging;
using GameEngine.Rendering.Cameras;

namespace GameEngine.Core;

public sealed partial class Game {
    
    public static bool _isRunning { get; private set; }
    private Thread _updateLoopThread;
    private Thread _physicsThread;
    private Thread _renderThread;
    
    
    public void Initialize() {
        Console.Log("Initializing...");
        Console.Log("Initializing engine...");
        _updateLoopThread = new Thread(UpdateLoop);
        Console.LogSuccess("Initialized engine (1/3)");
        Console.Log("Initializing physics engine...");
        PhysicsEngine physicsEngine = new PhysicsEngine();
        _physicsThread = new Thread(physicsEngine.Initialize);
        Console.LogSuccess("Initialized physics engine (2/3)");
        Console.Log("Initializing render engine...");
        RenderingEngine renderingEngine = new RenderingEngine();
        _renderThread = new Thread(renderingEngine.Initialize);
        Console.LogSuccess("Initialized render engine (3/3)");
        Console.LogSuccess("Initialization complete");
    }
    
    public void Start() {
        _isRunning = true;
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
        _isRunning = false;
        Console.Log("Terminating...");
    }
    
}
