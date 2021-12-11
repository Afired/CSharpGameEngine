using System;
using System.Numerics;

namespace GameEngine.Rendering.Camera2D; 

public class Camera3D : BaseCamera {
    
    public Camera3D(Vector2 focusPosition, float zoom) : base(focusPosition, zoom) { }
    
    public override Matrix4x4 GetProjectionMatrix() {
        throw new NotImplementedException();
    }
    
}
