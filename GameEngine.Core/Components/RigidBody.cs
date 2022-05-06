using Box2D.NetStandard.Dynamics.Bodies;
using GameEngine.Core.SourceGenerator;

namespace GameEngine.Core.Components; 

[RequireComponent(typeof(Transform))]
public partial class RigidBody : Collider {
    
    protected override void OnAwake() {
        BodyType = BodyType.Dynamic;
        base.OnAwake();
    }
    
    protected override void OnPhysicsUpdate() {
        Transform.Position = new Numerics.Vector3(Body.GetPosition().X, Body.GetPosition().Y, Transform.Position.Z);
        Transform.Rotation = Body.GetAngle();
    }
    
}
