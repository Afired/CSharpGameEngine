using GameEngine.Rendering;

namespace GameEngine.Core; 

//TODO: create and deserialize configuration file when loading
public static class Configuration {
    
    public static float FixedTimeStep = 0.02f;
    public static float TargetFrameRate = 144f;
    
    public static string WindowTitle = "Window Title";
    public static int WindowHeight = 800;
    public static int WindowWidth = 1000;
    public static bool WindowIsResizable = false;
    public static bool DoUseVsync = false;
    
    public static Color DefaultBackgroundColor = new Color(0.1f, 0.1f, 0.1f, 1f);
    
    public static bool DoDebugLogs = true;
    public static bool DoDebugWarnings = true;
    public static bool DoDebugErrors = true;
    public static bool DoDebugSuccess = true;
    
}
