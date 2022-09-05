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
        Body.SetTransform(new Vector2(LocalPosition.X, LocalPosition.Y), LocalRotation);
        Body.SetLinearVelocity(new Vector2(Velocity.X, Velocity.Y));
        Body.SetGravityScale(Gravity);
    }
    
    protected override void OnPhysicsUpdate() {
        LocalPosition = new Numerics.Vector3(Body.GetPosition().X, Body.GetPosition().Y, Position.Z); // swap out for world pos
        LocalRotation = Body.GetAngle();
        
        Vector2 vec2 = Body.GetLinearVelocity();
        Velocity = new Numerics.Vector2(vec2.X, vec2.Y);
        Gravity = Body.GetGravityScale();
    }
    
}
