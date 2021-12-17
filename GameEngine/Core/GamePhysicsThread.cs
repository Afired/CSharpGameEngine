using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using Box2D.NetStandard.Dynamics.World;

namespace GameEngine.Core;

public delegate void OnFixedUpdate(float fixedDeltaTime);

public sealed partial class Game {
    
    public static event OnFixedUpdate OnFixedUpdate;
    
    private void FixedUpdateLoop() {
        
        //world
        Vector2 gravity = new Vector2(0, -9.81f);
        World world = new World(gravity);
        
        //ground
        PolygonShape groundBox = new PolygonShape();
        groundBox.SetAsBox(50f, 10f);
        
        BodyDef groundBodyDef = new BodyDef();
        groundBodyDef.type = BodyType.Static;
        groundBodyDef.position = new Vector2(0, -5f);

        FixtureDef groundFixtureDef = new FixtureDef();
        groundFixtureDef.shape = groundBox;

        Body ground = world.CreateBody(groundBodyDef);
        ground.CreateFixture(groundFixtureDef);
        
        //dynamic object
        BodyDef dynamicBodyDef = new BodyDef();
        dynamicBodyDef.type = BodyType.Dynamic;
        dynamicBodyDef.position = new Vector2(0, 40f);

        PolygonShape dynamicBox = new PolygonShape();
        dynamicBox.SetAsBox(1f, 1f);

        FixtureDef dynamicFixtureDef = new FixtureDef();
        dynamicFixtureDef.shape = dynamicBox;
        dynamicFixtureDef.density = 1.0f;
        dynamicFixtureDef.friction = 0.3f;

        Body dynamicBody = world.CreateBody(dynamicBodyDef);

        dynamicBody.CreateFixture(dynamicFixtureDef);
        
        int velocityIterations = 6;
        int positionIterations = 2;
        
        
        Stopwatch stopwatch = new();
        while(_isRunning) {
            float elapsedTime = (float) stopwatch.Elapsed.TotalSeconds;
            TimeSpan timeOut = TimeSpan.FromSeconds(Configuration.FixedTimeStep - elapsedTime);
            Thread.Sleep(timeOut);
            stopwatch.Restart();
            OnFixedUpdate?.Invoke(Configuration.FixedTimeStep);
            
            world.Step(Configuration.FixedTimeStep, velocityIterations, positionIterations);
            Console.WriteLine(dynamicBody.GetPosition());
            
            
        }
    }
    
}
