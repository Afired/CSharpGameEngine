using GameEngine.Core;

namespace GameEngine.Editor;

public static class Program {
    
    public static int Main(string[] args) {
        
        Configuration configuration = new Configuration() {
            TargetFrameRate = -1,
            WindowTitle = "GameEngine-Editor",
            DoUseVsync = false
        };

        while(true) {
            using(EditorApplication editorApplication = new(configuration)) {
                editorApplication.InvokeFinishedInit();
                editorApplication.Run();
            }
        }
        
        return 0;
    }
    
}
