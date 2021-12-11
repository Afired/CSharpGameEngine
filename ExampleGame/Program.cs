using GameEngine.Core;

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
        game.Start();
        
        return 0;
    }
    
}
