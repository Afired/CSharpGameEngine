using ExampleGame.GameObjects;
using GameEngine.Core;
using GameEngine.Numerics;

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
        Configuration.TargetFrameRate = 144;
        Configuration.WindowTitle = "Example Game";
        Configuration.DoUseVsync = true;
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
        Game.SetActiveCamera(player.Camera2D);
    }
    
}
