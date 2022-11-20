using GameEngine.Core.Debugging;
using GameEngine.Core.Nodes;

namespace ExampleGame.Nodes; 

[Has<SpriteRenderer>]
//[Has<Trigger>]
public partial class Enemy : RigidBody {
    
    // protected override void OnAwake() {
    //     base.OnAwake();
    //     Trigger.OnBeginTrigger += OnBeginTrigger;
    // }
    //
    // private void OnBeginTrigger(Trigger other) {
    //     Console.Log("Enemy hit something!");
    // }
    
    protected override void OnBeginCollision(Collider other) {
        base.OnBeginCollision(other);
        Console.LogWarning("Enemy hit something!");
    }
    
}
