using System.Numerics;
using Box2D.NetStandard.Dynamics.Bodies;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes; 

public partial class RigidBody : Collider {
    
    [Serialized] public GameEngine.Core.Numerics.Vector2 Velocity { get; set; }
    [Serialized] public float Gravity { get; set; } = 0f;
    protected override bool TransformIsIndependent => true;
    
    protected override void OnAwake() {
        BodyType = BodyType.Dynamic;
        base.OnAwake();
    }
    
    protected override void OnPrePhysicsUpdate() {
        base.OnPrePhysicsUpdate();
        Body.SetLinearVelocity(new Vector2(Velocity.X, Velocity.Y));
        Body.SetGravityScale(Gravity);
    }
    
    protected override void OnPhysicsUpdate() {
        base.OnPhysicsUpdate();
        Vector2 vec2 = Body.GetLinearVelocity();
        Velocity = new Numerics.Vector2(vec2.X, vec2.Y);
        Gravity = Body.GetGravityScale();
    }
    
}
