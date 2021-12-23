using ExampleGame.Entities;
using GameEngine.Core;
using GameEngine.Numerics;
using GameEngine.Rendering;

namespace ExampleGame;

internal class Program {
    
    public static int Main(string[] args) {
        Game game = new Game();

        SetConfig();
        
        game.Initialize();

        InitializeWorld();
        
        game.Start();

        return 0;
    }

    private static void SetConfig() {
        Configuration.TargetFrameRate = -1;
        Configuration.WindowHeight = 1000;
        Configuration.WindowWidth = 1000;
        Configuration.WindowTitle = "Example Game";
        Configuration.DoUseVsync = false;
    }
    
    private static void InitializeWorld() {
        new PhysicsCheckerboard().Transform.Position = new Vector3(0, 10, 0);
        new PhysicsCheckerboard().Transform.Position = new Vector3(0.5f, 15, 0);
        new PhysicsCheckerboard().Transform.Position = new Vector3(0, 10, 0);
        new PhysicsCheckerboard().Transform.Position = new Vector3(0.5f, 15, 0);
        new PhysicsCheckerboard().Transform.Position = new Vector3(0, 10, 0);
        new PhysicsCheckerboard().Transform.Position = new Vector3(0.5f, 15, 0);
        new PhysicsCheckerboard().Transform.Position = new Vector3(0, 10, 0);
        new PhysicsCheckerboard().Transform.Position = new Vector3(0.5f, 15, 0);
        new PhysicsBox().Transform.Position = new Vector3(0, 10, 0);
        new PhysicsBox().Transform.Position = new Vector3(0.5f, 15, 0);
        new PhysicsBox().Transform.Position = new Vector3(0, 10, 0);
        new PhysicsBox().Transform.Position = new Vector3(0.5f, 15, 0);
        new PhysicsBox().Transform.Position = new Vector3(0, 10, 0);
        new PhysicsBox().Transform.Position = new Vector3(0.5f, 15, 0);
        new PhysicsBox().Transform.Position = new Vector3(0, 10, 0);
        new PhysicsBox().Transform.Position = new Vector3(0.5f, 15, 0);
        new PhysicsBox().Transform.Position = new Vector3(0, 10, 0);
        new PhysicsBox().Transform.Position = new Vector3(0.5f, 15, 0);
        new PhysicsBox().Transform.Position = new Vector3(0, 10, 0);
        new PhysicsBox().Transform.Position = new Vector3(0.5f, 15, 0);
        new PhysicsBox().Transform.Position = new Vector3(0, 10, 0);
        new PhysicsBox().Transform.Position = new Vector3(0.5f, 15, 0);
        new PhysicsBox().Transform.Position = new Vector3(0, 10, 0);
        new PhysicsBox().Transform.Position = new Vector3(0.5f, 15, 0);
        
        Player player = new Player();
        player.Transform.Position = new Vector3(0, 0, -10);
        RenderingEngine.SetActiveCamera(player.Camera2D);
        player.Camera2D.BackgroundColor = new Color(0.05f, 0.05f, 0.05f, 1.0f);
    }
    
}
