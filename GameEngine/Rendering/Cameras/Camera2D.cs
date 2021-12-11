using System.Numerics;
using GameEngine.Core;

namespace GameEngine.Rendering.Camera2D; 

public class Camera2D : BaseCamera {
    
    public Camera2D(Vector2 focusPosition, float zoom) : base(focusPosition, zoom) { }
    
    public override Matrix4x4 GetProjectionMatrix() {
        float left = FocusPosition.X - Configuration.WindowWidth / 2.0f;
        float right = FocusPosition.X + Configuration.WindowHeight / 2.0f;
        float top = FocusPosition.Y - Configuration.WindowHeight / 2.0f;
        float bottom = FocusPosition.Y + Configuration.WindowWidth / 2.0f;

        Matrix4x4 orthoMatrix = Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, 0.01f, 100f);
        Matrix4x4 zoomMatrix = Matrix4x4.CreateScale(Zoom);

        return orthoMatrix * zoomMatrix;
    }
    
}
