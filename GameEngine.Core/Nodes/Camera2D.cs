using System;
using GameEngine.Core.Serialization;
using GameEngine.Numerics;

namespace GameEngine.Core.Nodes;

/// <summary>
/// Orthographic Camera looking into -Z direction
/// </summary>
public partial class Camera2D : BaseCamera {
    
    [Serialized] public float Zoom { get; set; } = 10;
    [Serialized] public Vec2<float> ClippingDistance { get; set; } = new Vec2<float>(0.01f, 100f);
    
    public override Matrix<float> ViewMatrix =>
        Matrix<float>.Invert(
            Matrix<float>.CreateTranslation(-LocalPosition) *
            Matrix<float>.CreateFromQuaternion(LocalRotation.Normalized())
        );
    
    public override Matrix<float> ProjectionMatrix =>
        Matrix<float>.CreateOrthographic(
//            Application.Instance.Renderer.MainFrameBuffer2.Width,
//            Application.Instance.Renderer.MainFrameBuffer2.Height,
1,
1,
            ClippingDistance.X,
            ClippingDistance.Y
        );
    
}
