using ExampleGame.Scenes;
using GameEngine.Core;
using GameEngine.Rendering;
using GameEngine.SceneManagement;

namespace Runtime;

internal class Program {
    
    public static int Main(string[] args) {
        
        SetConfig();
        
        Application.Initialize();
        //RenderingEngine.OnLoad += () => Hierarchy.LoadScene(TestScene.Get());
        Hierarchy.LoadScene(RigidBodyScene.Get());
        Application.Run();
        
        return 0;
    }

    private static void SetConfig() {
        Configuration.TargetFrameRate = -1;
        Configuration.WindowTitle = "Example Game";
        Configuration.DoUseVsync = false;
    }
    
}
