using ExampleGame.Entities;
using GameEngine.Core;
using GameEngine.Layers;
using GameEngine.Numerics;
using GameEngine.Rendering;
using GameEngine.SceneManagement;

namespace ExampleGame;

internal class Program {
    
    public static int Main(string[] args) {
        Application application = new Application();

        SetConfig();
        
        application.Initialize();

        InitializeWorld();
        
        application.Start();

        return 0;
    }

    private static void SetConfig() {
        Configuration.TargetFrameRate = -1;
        Configuration.WindowTitle = "Example Game";
        Configuration.DoUseVsync = false;
    }
    
    public static void InitializeWorld() {
        Hierarchy.Instance.Add(new PhysicsCheckerboard());
        Player player = new Player();
        RenderingEngine.SetActiveCamera(player.Camera2D);
        Hierarchy.Instance.Add(player);
    }
    
}
