﻿using ExampleGame.Pathfinding;
using GameEngine.Core.Core;
using GameEngine.Core.SceneManagement;

namespace GameEngine.Standalone;

internal class Program {
    
    public static int Main(string[] args) {
        
        SetConfig();
        
        Application.Initialize();
        Hierarchy.LoadScene(new PathfindingScene());
        Application.Run();
        
        return 0;
    }
    
    private static void SetConfig() {
        Configuration.TargetFrameRate = -1;
        Configuration.WindowTitle = "Example Game";
        Configuration.DoUseVsync = false;
    }
    
}