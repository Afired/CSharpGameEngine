using GameEngine.Core;
using GameEngine.Numerics;

namespace GameEngine.Rendering.Cameras;

public class Camera2D : BaseCamera {
    
    public float Zoom { get; set; }
    private bool _flipped;


    public Camera2D(float zoom, bool flipped = false) : base() {
        Zoom = zoom;
        _flipped = flipped;
    }
    
    public override Matrix4x4 GetProjectionMatrix() {
        float left = Transform.Position.X - Configuration.WindowWidth / 2.0f;
        float right = Transform.Position.X + Configuration.WindowHeight / 2.0f;
        
        Matrix4x4 transMatrix = Matrix4x4.CreateTranslation(Transform.Position.X, Transform.Position.Y, 0);
        Matrix4x4 orthoMatrix = Matrix4x4.CreateOrthographic(_flipped ? -left : left, right, 0.01f, 100f);
        Matrix4x4 zoomMatrix = Matrix4x4.CreateScale(Zoom);
        
        return transMatrix * zoomMatrix * orthoMatrix;
    }
    
}
