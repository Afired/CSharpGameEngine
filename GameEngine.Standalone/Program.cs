using GameEngine.Core.Core;
using GameEngine.Core.Nodes;
using GameEngine.Core.SceneManagement;
using GameEngine.Core.Serialization;

namespace GameEngine.Standalone;

internal class Program {
    
    public static int Main(string[] args) {
        
        SetConfig();
        StandaloneApplication standaloneApplication = new();
        standaloneApplication.Initialize();
        Hierarchy.LoadScene(SceneSerializer.LoadJson("somePath"));
        standaloneApplication.Run();
        
        return 0;
    }
    
    private static void SetConfig() {
        Configuration.TargetFrameRate = -1;
        Configuration.WindowTitle = "Example Game";
        Configuration.DoUseVsync = false;
    }
    
}
