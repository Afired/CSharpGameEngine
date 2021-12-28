using GameEngine.Core;

namespace GameEngine.Editor;

public static class Program {

    public static int Main(string[] args) {

        Application application = new Application();

        SetConfig();
        
        application.Initialize();
        
        EditorGui editorGui = new EditorGui();
        
        application.Start();
        
        return 0;
    }
    
    private static void SetConfig() {
        Configuration.TargetFrameRate = -1;
        Configuration.WindowTitle = "GameEngine-Editor";
        Configuration.DoUseVsync = false;
    }
    
}
