using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using GameEngine.AutoGenerator;
using GameEngine.Physics;
using Vector2 = System.Numerics.Vector2;

namespace GameEngine.Components; 

[RequireComponent(typeof(Transform))]
public partial class Collider : Component {
    
    protected Body Body { get; private set; }
    protected BodyType BodyType = BodyType.Dynamic;
    protected float Density = 1.0f;
    protected float Friction = 0.3f;
    
    
    protected override void OnAwake() {
        CreateBody();
    }
    
    private void CreateBody() {
        //dynamic object
        BodyDef dynamicBodyDef = new BodyDef() {
            type = BodyType,
            position = new Vector2(Transform.Position.X, Transform.Position.Y),
            angle = Transform.Rotation
        };
        
        PolygonShape dynamicBox = new PolygonShape();
        dynamicBox.SetAsBox(0.5f, 0.5f);

        FixtureDef dynamicFixtureDef = new FixtureDef() {
            shape = dynamicBox,
            density = Density,
            friction = Friction,
            isSensor = false,
        };
        
        Body = PhysicsEngine.World.CreateBody(dynamicBodyDef);
        
        Body.SetUserData(this);
        
        Body.CreateFixture(dynamicFixtureDef);
    }

    internal void BeginCollision(Collider other) => OnBeginCollision(other);
    
    protected void OnBeginCollision(Collider other) {
        Console.LogSuccess($"{Entity.GetType().Name} collided with {other.Entity.GetType().Name}");
    }
    
}
