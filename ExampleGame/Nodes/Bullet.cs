using GameEngine.Core.Debugging;
using GameEngine.Core.Nodes;

namespace ExampleGame.Nodes; 

public partial class Bullet : RigidBody, Has<Renderer>, Has<Trigger> {
    
    protected override void OnAwake() {
        base.OnAwake();
        Trigger.OnBeginTrigger += OnEnemyHit;
    }
    
    private void OnEnemyHit(Trigger other) {
        Console.Log("Bullet hit something!");
        var test = ChildNodes;
    }
    
}
