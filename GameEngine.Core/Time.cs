﻿namespace GameEngine.Core; 

public static class Time {
    
    public static float TotalTimeElapsed { get; set; } // refactor to internal set
    public static float DeltaTime { get; internal set; }
    public static float PhysicsTimeStep { get; internal set; }

}
