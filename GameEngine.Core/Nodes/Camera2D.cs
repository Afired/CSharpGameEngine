using GameEngine.Core.Numerics;
using GameEngine.Core.Serialization;
using GlmSharp;

namespace GameEngine.Core.Nodes;

/// <summary>
/// Orthographic Camera looking into -Z direction
/// </summary>
public partial class Camera2D : BaseCamera {
    
    [Serialized] public float Zoom { get; set; } = 10;
    [Serialized] public Vector2 ClippingDistance { get; set; } = new Vector2(0.01f, 100f);
    
    public override mat4 GLM_GetProjectionMatrix() {
        float aspectRatioGameFrameBuffer = (float) Application.Instance!.Renderer.MainFrameBuffer2.Width / (float) Application.Instance!.Renderer.MainFrameBuffer2.Height;
        mat4 projectionMatrix = mat4.Ortho(-aspectRatioGameFrameBuffer * Zoom, aspectRatioGameFrameBuffer * Zoom, -Zoom, Zoom, -ClippingDistance.X, -ClippingDistance.Y);
        
        mat4 viewProjectionMat = projectionMatrix * GetViewMat();
        return viewProjectionMat;
    }
    
    private mat4 GetViewMat() {
        mat4 t = mat4.Translate(WorldPosition.X, WorldPosition.Y, WorldPosition.Z) *
                        new quat(WorldRotation.X, WorldRotation.Y, WorldRotation.Z, WorldRotation.W).Normalized.ToMat4;
        return t.Inverse;
    }
    
}
