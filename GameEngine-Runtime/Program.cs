using ExampleGame.Scenes;
using GameEngine.Core;
using GameEngine.Rendering;
using GameEngine.SceneManagement;

namespace Runtime;

internal class Program {
    
    public static int Main(string[] args) {

        Application application = new Application();
        SetConfig();
        
        application.Initialize();
        application.Start();
        
        // currently only waits for renderingEngine init, but if other threads didnt yet fully init it'll crash
        RenderingEngine.OnLoad += () => Hierarchy.LoadScene(TestScene.Get());
        
        return 0;
    }

    private static void SetConfig() {
        Configuration.TargetFrameRate = -1;
        Configuration.WindowTitle = "Example Game";
        Configuration.DoUseVsync = false;
    }
    
}
