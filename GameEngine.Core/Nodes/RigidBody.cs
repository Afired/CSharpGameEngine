using System.Numerics;
using Box2D.NetStandard.Dynamics.Bodies;
using GameEngine.Core.Serialization;
using GameEngine.Numerics;

namespace GameEngine.Core.Nodes; 

public partial class RigidBody : Collider {
    
    [Serialized] public Vec2<float> Velocity { get; set; }
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
        Velocity = Body.GetLinearVelocity();
        Gravity = Body.GetGravityScale();
        LocalPosition = new Vec3<float>(Body.Position.X, Body.Position.Y, LocalPosition.Z);
    }
    
}
