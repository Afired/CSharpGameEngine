using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Numerics;
using GameEngine.Rendering.Cameras;

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
        Pyramid myCamera = new Pyramid();
        myCamera.Transform.Position = new Vector3(0, 0, -5f);
        
        Player player = new Player();
        Game.SetActiveCamera(player.Camera3D);
    }
    
}
