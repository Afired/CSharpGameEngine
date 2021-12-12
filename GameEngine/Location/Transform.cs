﻿using GameEngine.Numerics;

namespace GameEngine; 

public class Transform {
    
    public Vector3 Position { get; set; }
    public Vector3 Scale { get; set; }
    public Vector3 Rotation { get; set; }
    
    
    public Transform() {
        Position = new Vector3(0f, 0f, 0f);
        Scale = new Vector3(1f, 1f, 1f);
        Rotation = new Vector3(0f, 0f, 0f);
    }
    
}
