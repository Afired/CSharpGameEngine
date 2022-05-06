using GameEngine.Core.Core;
using GameEngine.Core.Physics;
using NUnit.Framework;

namespace GameEngine.UnitTesting;

internal static class Program {

    [Test]
    public static void TestUpdateLoop() {
        float testForSeconds = 5f;
        
        Application application = new();
        application.Initialize();

        int updateLoopCalled = 0;
        
        Application.OnUpdate += deltaTime => {
            updateLoopCalled++;
        };
        
        application.Run();
        
        Thread.Sleep(TimeSpan.FromSeconds(testForSeconds));
        Console.WriteLine($"UpdateLoop called {updateLoopCalled} in {testForSeconds} seconds");
        Assert.Greater(updateLoopCalled, 10);
        Console.WriteLine("Test");
        Application.Terminate();
    }
    
    [Test]
    public static void TestFixedUpdateLoop() {
        float testForSeconds = 5f;
        
        Application application = new();
        application.Initialize();

        int fixedUpdateLoopCalled = 0;
        
        PhysicsEngine.OnFixedUpdate += fixedDeltaTime => {
            fixedUpdateLoopCalled++;
        };
        
        application.Run();
        
        Thread.Sleep(TimeSpan.FromSeconds(testForSeconds));
        Console.WriteLine($"FixedUpdateLoop called {fixedUpdateLoopCalled} in {testForSeconds} seconds");
        Assert.Greater(fixedUpdateLoopCalled, 0);
        Application.Terminate();
    }

}
