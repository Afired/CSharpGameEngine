using System.Numerics;
using GameEngine.Core;

namespace GameEngine.Rendering.Camera2D; 

public abstract class BaseCamera {
    
    public Color BackgroundColor { get; set; }
    public Vector2 FocusPosition { get; set; }
    public float Zoom { get; set; }
    
    
    public BaseCamera(Vector2 focusPosition, float zoom) {
        FocusPosition = focusPosition;
        Zoom = zoom;
        BackgroundColor = Configuration.DefaultBackgroundColor;
    }
    
    public abstract Matrix4x4 GetProjectionMatrix();
    
}
