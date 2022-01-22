using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using GameEngine.AutoGenerator;
using GameEngine.Entities;
using GameEngine.Numerics;
using GameEngine.Physics;
using Vector2 = System.Numerics.Vector2;

namespace GameEngine.Components; 

[RequireComponent(typeof(Transform))]
public partial class RigidBody : Component {
    
    private Body _body;
    
    
    protected override void Init() {
        PhysicsEngine.OnRegisterRigidBody += CreateBody;
        PhysicsEngine.OnFixedUpdate += OnFixedUpdate;
    }

    private void CreateBody() {
        //dynamic object
        BodyDef dynamicBodyDef = new BodyDef();
        dynamicBodyDef.type = BodyType.Dynamic;
        dynamicBodyDef.position = new Vector2(Transform.Position.X, Transform.Position.Y);
        dynamicBodyDef.angle = Transform.Rotation;

        PolygonShape dynamicBox = new PolygonShape();
        dynamicBox.SetAsBox(0.5f, 0.5f);

        FixtureDef dynamicFixtureDef = new FixtureDef();
        dynamicFixtureDef.shape = dynamicBox;
        dynamicFixtureDef.density = 1.0f;
        dynamicFixtureDef.friction = 0.3f;

        _body = PhysicsEngine.World.CreateBody(dynamicBodyDef);
        
        _body.CreateFixture(dynamicFixtureDef);
    }
    
    private void OnFixedUpdate(float fixedDeltaTime) {
        Transform.Position = new Vector3(_body.GetPosition().X, _body.GetPosition().Y, Transform.Position.Z);
        Transform.Rotation = _body.GetAngle();
    }
    
}
