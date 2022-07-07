using GameEngine.Core;

namespace GameEngine.Editor;

public static class Program {
    
    public static int Main(string[] args) {
        
        SetConfig();
        EditorApplication editorApplication = new();
        editorApplication.Initialize();
        editorApplication.Run();
        
        return 0;
    }
    
    private static void SetConfig() {
        Configuration.TargetFrameRate = -1;
        Configuration.WindowTitle = "GameEngine-Editor";
        Configuration.DoUseVsync = false;
    }
    
}
