using GameEngine.Core;
using GameEngine.Geometry;
using GameEngine.Rendering.Camera2D;

namespace ExampleGame;

internal class Program {
    
    public static int Main(string[] args) {
        Game game = new Game();
        
        game.Initialize();
        
        InitializeWorld();
        
        game.Start();
        
        return 0;
    }
    
    private static void InitializeWorld() {
        Camera2D camera = new Camera2D(10.0f);
        camera.Transform.Position.X = 0;
        Game.OnUpdate += deltaTime => camera.Transform.Position.X += deltaTime;
        Game.SetActiveCamera(camera);

        Plane plane1 = new Plane();
        Game.OnUpdate += deltaTime => plane1.Transform.Position.X += deltaTime;
        Game.OnUpdate += deltaTime => plane1.Transform.Position.Y += deltaTime;
        Plane plane2 = new Plane();
        plane2.Transform.Position.Y = -4;
    }
    
}
