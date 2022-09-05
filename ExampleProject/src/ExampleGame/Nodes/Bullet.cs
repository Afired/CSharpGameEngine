using GameEngine.Core.Debugging;
using GameEngine.Core.Nodes;

namespace ExampleGame.Nodes; 

public partial class Bullet : RigidBody, Has<SpriteRenderer> {
    
    protected override void OnBeginCollision(Collider other) {
        base.OnBeginCollision(other);
        Console.Log("Bullet hit something!");
    }
    
}
