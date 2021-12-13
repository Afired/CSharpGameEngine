using GameEngine.Core;
using GameEngine.Numerics;

namespace GameEngine.Rendering.Cameras;

public class Camera3D : BaseCamera {
    
    public Camera3D() : base() { }
    
    public override Matrix4x4 GetProjectionMatrix() {
        Matrix4x4 transMatrix = Matrix4x4.CreateTranslation(Transform.Position);
        Matrix4x4 rot = Matrix4x4.CreateFromYawPitchRoll(Transform.Rotation.X, Transform.Rotation.Y, Transform.Rotation.Z);
        Matrix4x4 perMatrix = Matrix4x4.CreatePerspectiveFieldOfView(2f, (float) Configuration.WindowWidth / (float) Configuration.WindowHeight, 0.01f, 100f);
        
        return transMatrix * rot * perMatrix;
    }
    
}
