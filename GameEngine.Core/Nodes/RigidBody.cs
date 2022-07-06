using Box2D.NetStandard.Dynamics.Bodies;

namespace GameEngine.Core.Nodes; 

public partial class RigidBody : Collider {
    
    protected override void OnAwake() {
        BodyType = BodyType.Dynamic;
        base.OnAwake();
    }
    
    protected override void OnPhysicsUpdate() {
        LocalPosition = new Numerics.Vector3(Body.GetPosition().X, Body.GetPosition().Y, Position.Z); // swap out for world pos
        LocalRotation = Body.GetAngle();
    }
    
}
