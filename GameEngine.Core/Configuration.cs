using GameEngine.Core.Rendering;

namespace GameEngine.Core; 

//TODO: create and deserialize configuration file when loading
public class Configuration {
    
    public float FixedTimeStep { get; init; } = 0.02f;
    public float TargetFrameRate { get; init; } = 144f;
    
    public string WindowTitle { get; init; } = "Window Title";
    public uint WindowHeight { get; init; } = 900;
    public uint WindowWidth { get; init; } = 1800;
    public bool WindowIsResizable { get; init; } = false;
    public bool DoUseVsync { get; init; } = false;
    
    public Color DefaultBackgroundColor { get; init; } = new Color(0.1f, 0.1f, 0.1f, 1f);
    
    public bool DoDebugLogs { get; init; } = true;
    public bool DoDebugWarnings { get; init; } = true;
    public bool DoDebugErrors { get; init; } = true;
    public bool DoDebugSuccess { get; init; } = true;
    
    public bool UseHDR { get; init; } = true;
    
}
