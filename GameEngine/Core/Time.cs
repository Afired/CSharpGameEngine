namespace GameEngine.Core; 

public static class Time {
    
    public static float TotalTimeElapsed { get; internal set; }
    public static float DeltaTime { get; internal set; }
    public static float FixedTimeStep { get; internal set; }

}
