using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Numerics;

namespace GameEngine.Rendering.Cameras;

public class Camera2D : BaseCamera {
    
    public float Zoom { get; set; }
    private bool _flipped;


    public Camera2D(GameObject gameObject, float zoom, bool flipped = false) : base(gameObject) {
        Zoom = zoom;
        _flipped = flipped;
    }
    
    public override Matrix4x4 GetProjectionMatrix() {
        float left = (GameObject as ITransform).Transform.Position.X - Configuration.WindowWidth / 2.0f;
        float right = (GameObject as ITransform).Transform.Position.X + Configuration.WindowHeight / 2.0f;
        
        Matrix4x4 transMatrix = Matrix4x4.CreateTranslation((GameObject as ITransform).Transform.Position.X, (GameObject as ITransform).Transform.Position.Y, 0);
        Matrix4x4 orthoMatrix = Matrix4x4.CreateOrthographic(_flipped ? -left : left, right, 0.01f, 100f);
        Matrix4x4 zoomMatrix = Matrix4x4.CreateScale(Zoom);
        
        return transMatrix * zoomMatrix * orthoMatrix;
    }
    
}

public interface ICamera2D {
    Camera2D Camera2D { get; set; }
}