﻿using System.Numerics;

namespace GameEngine.Core; 

//TODO: create and deserialize configuration file when loading
public static class Configuration {

    public static float FixedTimeStep = 0.2f;
    public static float TargetFrameRate = 30f;

    public static int WindowHeight = 800;
    public static int WindowWidth = 1000;

}
