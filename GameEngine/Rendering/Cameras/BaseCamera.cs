﻿using System.Numerics;
using GameEngine.Core;

namespace GameEngine.Rendering.Camera2D; 

public abstract class BaseCamera : ITransform {

    public Transform Transform { get; set; }
    public Color BackgroundColor { get; set; }
    public float Zoom { get; set; }
    
    
    public BaseCamera(float zoom) {
        Zoom = zoom;
        BackgroundColor = Configuration.DefaultBackgroundColor;
        Transform = new Transform();
    }
    
    public abstract Matrix4x4 GetProjectionMatrix();
}