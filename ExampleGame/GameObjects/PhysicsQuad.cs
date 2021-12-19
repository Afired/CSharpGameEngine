using GameEngine.Components;

namespace ExampleGame.GameObjects; 

public class PhysicsQuad : Quad, IRigidBody {
    
    public RigidBody RigidBody { get; set; }

    public PhysicsQuad(string texture) : base(texture) {
        RigidBody = new RigidBody(this);
    }
    
}
