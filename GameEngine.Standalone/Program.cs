using GameEngine.Core;

namespace GameEngine.Standalone;

public static class Program {
    
    public static int Main(string[] args) {
        
        while(true) {
            Configuration configuration = new Configuration() {
                TargetFrameRate = -1,
                WindowTitle = "GameEngine-Standalone",
                DoUseVsync = true
            };
            
            using(StandaloneApplication standaloneApplication = new(configuration)) {
                standaloneApplication.InvokeFinishedInit();
                standaloneApplication.Run();
            }
        }
        
        return 0;
    }
    
}
