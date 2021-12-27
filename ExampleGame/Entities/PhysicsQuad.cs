using GameEngine.Components;

namespace ExampleGame.Entities; 

public class PhysicsQuad : Quad, IRigidBody {
    
    public RigidBody RigidBody { get; set; }

    public PhysicsQuad(string texture, string shader) : base(texture, shader) {
        RigidBody = new RigidBody(this);
    }
    
}