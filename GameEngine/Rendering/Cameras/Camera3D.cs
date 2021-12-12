using System;
using System.Numerics;

namespace GameEngine.Rendering.Camera2D; 

public class Camera3D : BaseCamera {
    
    public Camera3D(float zoom) : base(zoom) { }
    
    public override Matrix4x4 GetProjectionMatrix() {
        throw new NotImplementedException();
    }
    
}
