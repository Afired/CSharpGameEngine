using GameEngine.Core.Core;
using GameEngine.Core.Numerics;

namespace GameEngine.Core.Nodes;

/// <summary>
/// Orthographic Camera looking into -Z direction
/// </summary>
public partial class Camera2D : BaseCamera {
    
    public float Zoom { get; set; } = 50;
    
    
    public override Matrix4x4 GetProjectionMatrix() {
        Matrix4x4 transMatrix = Matrix4x4.CreateTranslation(-Transform.Position.X, -Transform.Position.Y, 0);
        Matrix4x4 orthoMatrix = Matrix4x4.CreateOrthographic(Configuration.WindowWidth, Configuration.WindowHeight, 0.01f, 100f);
        Matrix4x4 zoomMatrix = Matrix4x4.CreateScale(Zoom);
        
        return transMatrix * orthoMatrix * zoomMatrix;
    }
    
}
