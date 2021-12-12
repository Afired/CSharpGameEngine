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

        Sprite sprite1 = new Sprite();
        Game.OnUpdate += deltaTime => sprite1.Transform.Position.X += deltaTime;
        Game.OnUpdate += deltaTime => sprite1.Transform.Position.Y += deltaTime;
        Sprite sprite2 = new Sprite();
        sprite2.Transform.Position.Y = -4;
    }
    
}
