﻿using GameEngine.Core.Numerics;

namespace GameEngine.Core.Components;

public partial class Transform : Component {
    
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Vector3 Scale { get; set; } = Vector3.One;
    public float Rotation { get; set; } = 0f;

}