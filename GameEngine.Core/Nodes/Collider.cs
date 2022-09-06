using System;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using GameEngine.Core.Physics;
using GameEngine.Core.Serialization;
using Vector2 = System.Numerics.Vector2;

namespace GameEngine.Core.Nodes; 

public partial class Collider : Transform {
    
    [Serialized] private Numerics.Vector2 Size { get; init; } = Numerics.Vector2.One;
    [Serialized] protected BodyType BodyType = BodyType.Dynamic;
    [Serialized] public Shape? Shape { get; private set; }
    [Serialized] protected float Density = 1.0f;
    [Serialized] protected float Friction = 0.3f;
    protected Body Body { get; private set; }
    
    protected override void OnAwake() {
        base.OnAwake();
        CreateBody();
    }
    
    private void CreateBody() {
        //dynamic object
        BodyDef dynamicBodyDef = new BodyDef() {
            type = BodyType,
            position = new Vector2(Position.X, Position.Y),
            angle = LocalRotation,
        };
        
        if(Shape is null) {
            PolygonShape dynamicBox = new PolygonShape();
            dynamicBox.SetAsBox(Size.X * 0.5f, Size.Y * 0.5f);
            Shape = dynamicBox;
        }
        
        FixtureDef dynamicFixtureDef = new FixtureDef() {
            shape = Shape,
            density = Density,
            friction = Friction,
            isSensor = false,
        };
        
        Body = PhysicsEngine.World.CreateBody(dynamicBodyDef);
        
        Body.SetUserData(this);
        
        Body.CreateFixture(dynamicFixtureDef);
    }
    
    protected override void OnPrePhysicsUpdate() {
        Body.SetTransform(new Vector2(LocalPosition.X, LocalPosition.Y), LocalRotation);
    }
    
    protected override void OnPhysicsUpdate() {
        LocalPosition = new Numerics.Vector3(Body.GetPosition().X, Body.GetPosition().Y, Position.Z); // swap out for world pos
        LocalRotation = Body.GetAngle();
    }
    
    internal void BeginCollision(Collider other) => OnBeginCollision(other);
    
    protected virtual void OnBeginCollision(Collider other) { }
    
}
