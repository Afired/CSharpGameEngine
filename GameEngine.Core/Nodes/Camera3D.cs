using System;
using GameEngine.Core.Numerics;
using GlmNet;

namespace GameEngine.Core.Nodes;

public partial class Camera3D : BaseCamera {
    
    public float NearPlaneDistance = 0.01f;
    public float FarPlaneDistance = 100f;
    public float FieldOfView {
        get => 180 / (float)Math.PI * _fieldOfView;
        set => _fieldOfView = (float)Math.PI / 180 * value;
    }
    private float _fieldOfView = (float)Math.PI / 180 * 75;
    
    
    public override Matrix4x4 GetProjectionMatrix() {
        Matrix4x4 transMatrix = Matrix4x4.CreateTranslation(Position);
        Matrix4x4 rotMatrix = Matrix4x4.CreateRotationZ(LocalRotation);
        Matrix4x4 perMatrix = Matrix4x4.CreatePerspectiveFieldOfView(_fieldOfView, (float) Configuration.WindowWidth / (float) Configuration.WindowHeight, NearPlaneDistance, FarPlaneDistance);
        
        return transMatrix * rotMatrix * perMatrix;
    }
    
    public override mat4 GLM_GetProjectionMatrix() {
        throw new NotImplementedException();
    }
    
}
