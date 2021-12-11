using System.Numerics;
using GameEngine.Core;
using GameEngine.Rendering;
using GameEngine.Rendering.Camera2D;

namespace ExampleGame;

internal class Program {
    
    public static int Main(string[] args) {
        Game game = new Game();
        
        Game.OnUpdate += deltaTime => {
            // do something in update
        };
        
        Game.OnFixedUpdate += fixedDeltaTime => {
            // do something in fixed update
        };
        
        game.Initialize();
        
        Game.SetActiveCamera(new Camera2D(new Vector2(Configuration.WindowWidth, Configuration.WindowHeight) / 2.0f, 1f));
        
        game.Start();
        
        return 0;
    }
    
}
