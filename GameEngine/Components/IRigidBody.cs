using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using GameEngine.Core;
using GameEngine.Numerics;
using Vector2 = System.Numerics.Vector2;

namespace GameEngine.Components; 

public class RigidBody : Component {
    
    private Body _body;
    
    
    public RigidBody(GameObject gameObject) : base(gameObject) {
        Game.OnRegisterRigidBody += CreateBody;
        Game.OnFixedUpdate += OnFixedUpdate;
    }
    
    private void CreateBody() {
        //dynamic object
        BodyDef dynamicBodyDef = new BodyDef();
        dynamicBodyDef.type = BodyType.Dynamic;
        dynamicBodyDef.position = new Vector2((GameObject as ITransform).Transform.Position.X, (GameObject as ITransform).Transform.Position.Y);
        dynamicBodyDef.angle = (GameObject as ITransform).Transform.Rotation;

        PolygonShape dynamicBox = new PolygonShape();
        dynamicBox.SetAsBox(0.5f, 0.5f);

        FixtureDef dynamicFixtureDef = new FixtureDef();
        dynamicFixtureDef.shape = dynamicBox;
        dynamicFixtureDef.density = 1.0f;
        dynamicFixtureDef.friction = 0.3f;

        _body = Game.World.CreateBody(dynamicBodyDef);
        
        _body.CreateFixture(dynamicFixtureDef);
    }
    
    private void OnFixedUpdate(float fixedDeltaTime) {
        (GameObject as ITransform).Transform.Position = new Vector3(_body.GetPosition().X, _body.GetPosition().Y, (GameObject as ITransform).Transform.Position.Z);
        (GameObject as ITransform).Transform.Rotation = _body.GetAngle();
    }
    
}

public interface IRigidBody : ITransform {
    RigidBody RigidBody { get; set; }
}