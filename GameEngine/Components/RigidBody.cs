using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using GameEngine.AutoGenerator;
using GameEngine.Entities;
using GameEngine.Numerics;
using GameEngine.Physics;
using Vector2 = System.Numerics.Vector2;

namespace GameEngine.Components; 

[RequireComponent(typeof(ITransform))]
public class RigidBody : Component {
    
    private Body _body;
    
    
    public RigidBody(Entity entity) : base(entity) {
        PhysicsEngine.OnRegisterRigidBody += CreateBody;
        PhysicsEngine.OnFixedUpdate += OnFixedUpdate;
    }
    
    private void CreateBody() {
        //dynamic object
        BodyDef dynamicBodyDef = new BodyDef();
        dynamicBodyDef.type = BodyType.Dynamic;
        dynamicBodyDef.position = new Vector2((Entity as ITransform).Transform.Position.X, (Entity as ITransform).Transform.Position.Y);
        dynamicBodyDef.angle = (Entity as ITransform).Transform.Rotation;

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
        (Entity as ITransform).Transform.Position = new Vector3(_body.GetPosition().X, _body.GetPosition().Y, (Entity as ITransform).Transform.Position.Z);
        (Entity as ITransform).Transform.Rotation = _body.GetAngle();
    }
    
}
