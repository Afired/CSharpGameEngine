using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using GameEngine.Core.Serialization;
using GameEngine.Numerics;

namespace GameEngine.Core.Nodes; 

public partial class Collider : Transform3D {
    
    [Serialized] private Vec2<float> Size { get; init; } = Vec2<float>.One;
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
            position = new System.Numerics.Vector2(LocalPosition.X, LocalPosition.Y),
            angle = LocalPosition.Z,
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
        
        Body = Application.Instance.PhysicsEngine.World.CreateBody(dynamicBodyDef);
        
        Body.SetUserData(this);
        
        Body.CreateFixture(dynamicFixtureDef);
    }
    
    protected override void OnPrePhysicsUpdate() {
        Body.SetTransform(new System.Numerics.Vector2(LocalPosition.X, LocalPosition.Y), LocalPosition.Z);
    }
    
    // currently disabled because switched to quaternion representation of rotation
//    protected override void OnPhysicsUpdate() {
//        WorldPosition = new Numerics.Vector3(Body.GetPosition().X, Body.GetPosition().Y, WorldPosition.Z); // swap out for world pos
//        WorldRotation = new Vector3(WorldRotation.X, WorldRotation.Y, Body.GetAngle());
//    }
    
    internal void BeginCollision(Collider other) => OnBeginCollision(other);
    
    protected virtual void OnBeginCollision(Collider other) { }
    
}
