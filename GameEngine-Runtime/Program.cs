using ExampleGame.Pathfinding;
using GameEngine.Core;
using GameEngine.SceneManagement;

namespace Runtime;

internal class Program {
    
    public static int Main(string[] args) {
        
        SetConfig();
        
        Application.Initialize();
        Hierarchy.LoadScene(PathfindingScene.Get());
        Application.Run();
        
        return 0;
    }

    private static void SetConfig() {
        Configuration.TargetFrameRate = -1;
        Configuration.WindowTitle = "Example Game";
        Configuration.DoUseVsync = false;
    }
    
}
