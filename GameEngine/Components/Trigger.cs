using System.Numerics;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using GameEngine.AutoGenerator;
using GameEngine.Physics;

namespace GameEngine.Components; 

[RequireComponent(typeof(Transform))]
public partial class Trigger : Component {
    
    protected Body Body { get; private set; }
    protected BodyType BodyType = BodyType.Dynamic;
    
    
    protected override void OnAwake() {
        CreateBody();
    }
    
    protected override void OnPhysicsUpdate() {
        if(Body is null) //todo: shouldn't receive physics updates if it has not been awaken yet
            return;
        
        Vector2 position = new Vector2(Transform.Position.X, Transform.Position.Y);
        Body.SetTransform(position, Transform.Rotation);
    }
    
    private void CreateBody() {
        //dynamic object
        BodyDef dynamicBodyDef = new BodyDef() {
            type = BodyType,
            position = new Vector2(Transform.Position.X, Transform.Position.Y),
            angle = Transform.Rotation,
            awake = true,
            allowSleep = false,
            gravityScale = 0
        };
        
        Console.LogWarning(GetType().Name + " " + Transform.Position.X + ", " + Transform.Position.Y);
        
        PolygonShape dynamicBox = new PolygonShape();
        dynamicBox.SetAsBox(0.5f, 0.5f);

        FixtureDef dynamicFixtureDef = new FixtureDef() {
            shape = dynamicBox,
            density = 0f,
            friction = 0f,
            isSensor = true,
        };
        
        Body = PhysicsEngine.World.CreateBody(dynamicBodyDef);
        
        Body.SetUserData(this);
        
        Body.CreateFixture(dynamicFixtureDef);
    }

    internal void BeginTrigger(Trigger other) => OnBeginTrigger(other);
    
    protected virtual void OnBeginTrigger(Trigger other) { }
    
}
