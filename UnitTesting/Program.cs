using GameEngine.Core;
using NUnit.Framework;

namespace UnitTesting;

internal static class Program {

    [Test]
    public static void TestUpdateLoop() {
        float testForSeconds = 5f;
        
        Game game = new();
        game.Initialize();

        int updateLoopCalled = 0;
        
        Game.OnUpdate += deltaTime => {
            updateLoopCalled++;
        };
        
        game.Start();
        
        Thread.Sleep(TimeSpan.FromSeconds(testForSeconds));
        Console.WriteLine($"UpdateLoop called {updateLoopCalled} in {testForSeconds} seconds");
        Assert.Greater(updateLoopCalled, 10);
        Console.WriteLine("Test");
        Game.Terminate();
    }
    
    [Test]
    public static void TestFixedUpdateLoop() {
        float testForSeconds = 5f;
        
        Game game = new();
        game.Initialize();

        int fixedUpdateLoopCalled = 0;
        
        PhysicsEngine.OnFixedUpdate += fixedDeltaTime => {
            fixedUpdateLoopCalled++;
        };
        
        game.Start();
        
        Thread.Sleep(TimeSpan.FromSeconds(testForSeconds));
        Console.WriteLine($"FixedUpdateLoop called {fixedUpdateLoopCalled} in {testForSeconds} seconds");
        Assert.Greater(fixedUpdateLoopCalled, 0);
        Game.Terminate();
    }

}
