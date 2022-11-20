using GameEngine.Core.Nodes;

namespace ExampleGame.Nodes; 

[Has<SpriteRenderer>]
public partial class Bullet : RigidBody {
    
    protected override void OnBeginCollision(Collider other) {
        base.OnBeginCollision(other);
        Console.Log("Bullet hit something!");
    }
    
}
