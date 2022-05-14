using System.Numerics;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using GameEngine.Core.Physics;

namespace GameEngine.Core.Nodes;

public delegate void OnBeginTrigger(Trigger other);

public partial class Trigger : Transform {
    
    public event OnBeginTrigger OnBeginTrigger;
    protected Body Body { get; private set; }
    protected BodyType BodyType = BodyType.Dynamic;
    
    
    protected override void OnAwake() {
        CreateBody();
    }
    
    protected override void OnPhysicsUpdate() {
        Vector2 position = new Vector2(Position.X, Position.Y);
        Body.SetTransform(position, Rotation);
    }
    
    private void CreateBody() {
        //dynamic object
        BodyDef dynamicBodyDef = new BodyDef() {
            type = BodyType,
            position = new Vector2(Position.X, Position.Y),
            angle = Rotation,
            awake = true,
            allowSleep = false,
            gravityScale = 0
        };
        
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

    internal void BeginTrigger(Trigger other) => OnBeginTrigger?.Invoke(other);
    
    // protected virtual void OnBeginTrigger(Trigger other) { }
    
}
