using System;
using GameEngine.Core;
using GameEngine.Numerics;

namespace GameEngine.Rendering.Cameras;

public class Camera3D : BaseCamera {
    
    public float NearPlaneDistance;
    public float FarPlaneDistance;
    public float FieldOfView {
        get => 180 / (float) Math.PI * _fieldOfView;
        set => _fieldOfView = (float) Math.PI / 180 * value;
    }
    private float _fieldOfView;
    
    
    public Camera3D(float fieldOfView, float nearPlaneDistance, float farPlaneDistance) : base() {
        FieldOfView = fieldOfView;
        NearPlaneDistance = nearPlaneDistance;
        FarPlaneDistance = farPlaneDistance;
    }
    
    public Camera3D(float fieldOfView) : this(fieldOfView, 0.01f, 100f) { }
    
    public override Matrix4x4 GetProjectionMatrix() {
        Matrix4x4 transMatrix = Matrix4x4.CreateTranslation(Transform.Position);
        Matrix4x4 rot = Matrix4x4.CreateFromQuaternion(Transform.Rotation);
        Matrix4x4 perMatrix = Matrix4x4.CreatePerspectiveFieldOfView(_fieldOfView, (float) Configuration.WindowWidth / (float) Configuration.WindowHeight, NearPlaneDistance, FarPlaneDistance);
        
        return transMatrix * rot * perMatrix;
    }
    
}
