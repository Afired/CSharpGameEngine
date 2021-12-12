using System.Numerics;
using GameEngine.Rendering;

namespace GameEngine.Core; 

//TODO: create and deserialize configuration file when loading
public static class Configuration {
    
    public static float FixedTimeStep = 0.2f;
    public static float TargetFrameRate = 30f;
    
    public static int WindowHeight = 800;
    public static int WindowWidth = 1000;
    
    public static Color DefaultBackgroundColor = new Color(0.1f, 0.1f, 0.1f, 1f);
    
    public static bool DoDebugLogs = true;
    public static bool DoDebugWarnings = true;
    public static bool DoDebugErrors = true;
    public static bool DoDebugSuccess = true;

}
