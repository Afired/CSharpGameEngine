using System;
using System.Threading;
using GameEngine.Core;
using NUnit.Framework;

namespace UnitTesting;

internal static class Program {

    public static int Main(string[] args) {
        return 0;
    }
    
    [Test]
    public static void TestUpdateLoop() {
        float testForSeconds = 5f;
        
        Game game = new();
        game.Initialize();

        int updateLoopCalled = 0;
        
        Game.OnUpdate += deltaTime => {
            updateLoopCalled++;
        };
        
        Thread.Sleep(TimeSpan.FromSeconds(testForSeconds));
        Console.WriteLine($"UpdateLoop called {updateLoopCalled} in {testForSeconds} seconds");
        Assert.Greater(updateLoopCalled, 10);
    }

}
