using System;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using GameEngine.Core.Physics;
using Vector2 = System.Numerics.Vector2;

namespace GameEngine.Core.Nodes; 

public partial class Collider : Transform {
    
    protected Body Body { get; private set; }
    protected BodyType BodyType = BodyType.Dynamic;
    protected float Density = 1.0f;
    protected float Friction = 0.3f;
    
    
    protected override void OnAwake() {
        base.OnAwake();
        CreateBody();
    }
    
    private void CreateBody() {
        //dynamic object
        BodyDef dynamicBodyDef = new BodyDef() {
            type = BodyType,
            position = new Vector2(Position.X, Position.Y),
            angle = Rotation
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
    
    protected virtual void OnBeginCollision(Collider other) { }
    
}
