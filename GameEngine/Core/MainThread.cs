using System.Threading;
using System.Threading.Tasks;
using GameEngine.Debugging;
using GameEngine.Guard;
using GameEngine.Physics;
using GameEngine.Rendering;

namespace GameEngine.Core;

public sealed partial class Application {
    
    public static bool IsRunning { get; private set; }
    private Thread _updateLoopThread;
    private Thread _physicsThread;
    private Thread _renderThread;
    
    
    public async Task Initialize() {
        IsRunning = true;
        Console.Log("Initializing...");
        Console.Log("Initializing engine...");
        (_updateLoopThread = new Thread(UpdateLoop)).Start();
        Console.LogSuccess("Initialized engine (1/3)");
        Console.Log("Initializing physics engine...");
        (_physicsThread = new Thread(new PhysicsEngine().Initialize)).Start();
        Console.LogSuccess("Initialized physics engine (2/3)");
        Console.Log("Initializing render engine...");
        (_renderThread = new Thread(new RenderingEngine().Initialize)).Start();
        Console.LogSuccess("Initialized render engine (3/3)");
        
        // await all threads to be initialized
        while(!RenderingEngine.IsInit || !PhysicsEngine.IsInit) { }
        Console.LogSuccess("Initialization complete");
    }

    public static bool DoStart;
    public void Start() {
        // starts loops on all threads
        Throw.If(!RenderingEngine.IsInit, "rendering engine has not yet been initialized or initialization has not been awaited");
        Throw.If(!PhysicsEngine.IsInit, "physics engine has not yet been initialized or initialization has not been awaited");
        DoStart = true;
    }
    
    public static void Terminate() {
        IsRunning = false;
        Console.Log("Terminating...");
    }
    
}
