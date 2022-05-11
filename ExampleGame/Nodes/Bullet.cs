using GameEngine.Core.Debugging;
using GameEngine.Core.Nodes;

namespace ExampleGame.Nodes; 

public partial class Bullet : Transform, IRenderer, ITrigger, IRigidBody {
    
    protected override void OnAwake() {
        base.OnAwake();
        Trigger.OnBeginTrigger += OnEnemyHit;
    }
    
    private void OnEnemyHit(Trigger other) {
        Console.Log("Bullet hit something!");
    }
    
}
