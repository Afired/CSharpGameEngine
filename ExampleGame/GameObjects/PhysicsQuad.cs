using GameEngine.Components;

namespace ExampleGame.GameObjects; 

public class PhysicsQuad : Quad, IRigidBody {
    
    public RigidBody RigidBody { get; set; }

    public PhysicsQuad() : base() {
        RigidBody = new RigidBody(this);
    }
    
}
