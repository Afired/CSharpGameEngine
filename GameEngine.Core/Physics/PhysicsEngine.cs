using System.Numerics;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using Box2D.NetStandard.Dynamics.World;
using GameEngine.Core.SceneManagement;

namespace GameEngine.Core.Physics;

public static class PhysicsEngine {
    
    public static bool IsInit { get; private set; }
    public static World World;
    
    
    public static void Initialize() {
        InitializeWorld();
        IsInit = true;
    }
    
    internal static void InitializeWorld() {
        //world
        Vector2 gravity = new Vector2(0, -9.81f);
        World = new World(gravity);
        World.SetContactListener(new ContactListener());
        
//        //ground
//        PolygonShape groundBox = new PolygonShape();
//        groundBox.SetAsBox(50f, 10f);
//        
//        BodyDef groundBodyDef = new BodyDef();
//        groundBodyDef.type = BodyType.Static;
//        groundBodyDef.position = new Vector2(0, -12f);
//
//        FixtureDef groundFixtureDef = new FixtureDef();
//        groundFixtureDef.shape = groundBox;
//
//        Body ground = World.CreateBody(groundBodyDef);
//        ground.CreateFixture(groundFixtureDef);
//        
//        //dynamic object
//        BodyDef dynamicBodyDef = new BodyDef();
//        dynamicBodyDef.type = BodyType.Dynamic;
//        dynamicBodyDef.position = new Vector2(0, 40f);
//
//        PolygonShape dynamicBox = new PolygonShape();
//        dynamicBox.SetAsBox(1f, 1f);
//
//        FixtureDef dynamicFixtureDef = new FixtureDef();
//        dynamicFixtureDef.shape = dynamicBox;
//        dynamicFixtureDef.density = 1.0f;
//        dynamicFixtureDef.friction = 0.3f;
//
//        Body dynamicBody = World.CreateBody(dynamicBodyDef);
//
//        dynamicBody.CreateFixture(dynamicFixtureDef);
    }
    
    public static void DoStep() {
        int velocityIterations = 6;
        int positionIterations = 2;
        
        World.Step(Application.Instance!.Config.FixedTimeStep, velocityIterations, positionIterations);
    }
    
}
