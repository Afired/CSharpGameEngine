﻿using GameEngine.AutoGenerator;
using GameEngine.Entities;
using GameEngine.Numerics;

namespace GameEngine.Components;

public partial class Transform : Component {
    
    public Vector3 Position { get; set; }
    public Vector3 Scale { get; set; }
    public float Rotation { get; set; }
    
    
    protected override void OnAwake() {
        Position = new Vector3(0f, 0f, 0f);
        Scale = new Vector3(1f, 1f, 1f);
        Rotation = new float();
    }
    
}
