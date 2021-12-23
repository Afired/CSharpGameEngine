﻿using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Entities;
using GameEngine.Numerics;

namespace GameEngine.Rendering.Cameras;

/// <summary>
/// Orthographic Camera looking into -Z direction
/// </summary>
public class Camera2D : BaseCamera {
    
    public float Zoom { get; set; }


    public Camera2D(Entity entity, float zoom) : base(entity) {
        Zoom = zoom;
    }
    
    public override Matrix4x4 GetProjectionMatrix() {
        Matrix4x4 transMatrix = Matrix4x4.CreateTranslation(-(Entity as ITransform).Transform.Position.X, -(Entity as ITransform).Transform.Position.Y, 0);
        Matrix4x4 orthoMatrix = Matrix4x4.CreateOrthographic(Configuration.WindowWidth, Configuration.WindowHeight, 0.01f, 100f);
        Matrix4x4 zoomMatrix = Matrix4x4.CreateScale(Zoom);
        
        return transMatrix * orthoMatrix * zoomMatrix;
    }
    
}

public interface ICamera2D {
    Camera2D Camera2D { get; set; }
}
