using Box2D.NetStandard.Dynamics.Bodies;

namespace GameEngine.Core.Nodes; 

public partial class RigidBody : Collider {
    
    protected override void OnAwake() {
        BodyType = BodyType.Dynamic;
        base.OnAwake();
    }
    
    protected override void OnPhysicsUpdate() {
        Position = new Numerics.Vector3(Body.GetPosition().X, Body.GetPosition().Y, Position.Z);
        Rotation = Body.GetAngle();
    }
    
}
