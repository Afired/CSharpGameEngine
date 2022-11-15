using GameEngine.Core;

namespace GameEngine.Editor;

public static class Program {
    
    public static int Main(string[] args) {
        
        while(true) {
            
            Configuration configuration = new Configuration() {
                TargetFrameRate = -1,
                WindowTitle = "GameEngine-Editor",
                DoUseVsync = true
            };
            
            using(EditorApplication editorApplication = new(configuration)) {
                editorApplication.InvokeFinishedInit();
                editorApplication.Run();
            }
        }
        
        return 0;
    }
    
}
