using System.Numerics;
using Box2D.NetStandard.Dynamics.Bodies;
using GameEngine.AutoGenerator;
using GameEngine.Core;
using Vector3 = GameEngine.Numerics.Vector3;

namespace GameEngine.Components; 

[RequireComponent(typeof(Transform))]
public partial class RigidBody : Collider {
    
    protected override void OnAwake() {
        BodyType = BodyType.Dynamic;
        base.OnAwake();
    }
    
    protected override void OnPhysicsUpdate() {
        if(Body is null) //todo: shouldn't receive physics updates if it has not been awaken yet
            return;
        Transform.Position = new Vector3(Body.GetPosition().X, Body.GetPosition().Y, Transform.Position.Z);
        Transform.Rotation = Body.GetAngle();
    }
    
//    protected override void OnAwake() {
//        BodyType = BodyType.Dynamic;
//        base.OnAwake();
//    }
//
//    protected override void OnPhysicsUpdate() {
//        if(Body is null) //todo: shouldn't receive physics updates if it has not been awaken yet
//            return;
//        Body.SetGravityScale(0);
//        
//        Transform.Position = new Vector3(Body.GetPosition().X, Body.GetPosition().Y, Transform.Position.Z);
//        Transform.Rotation = Body.GetAngle();
//
//        Vector2 vel = new Vector2(0, -1);
//        Body.SetLinearVelocity(vel);
//    }

}
