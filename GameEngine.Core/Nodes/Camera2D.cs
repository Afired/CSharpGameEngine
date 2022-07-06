using GameEngine.Core.Numerics;
using GameEngine.Core.Serialization;
using GlmNet;

namespace GameEngine.Core.Nodes;

/// <summary>
/// Orthographic Camera looking into -Z direction
/// </summary>
public partial class Camera2D : BaseCamera {
    
    [Serialized] public float Zoom { get; set; } = 10;
    [Serialized] public Vector2 ClippingDistance { get; set; } = new Vector2(0.01f, 100f);
    
    public override Matrix4x4 GetProjectionMatrix() {
        Matrix4x4 transMatrix = Matrix4x4.CreateTranslation(-Position.X, -Position.Y, 0);
        Matrix4x4 orthoMatrix = Matrix4x4.CreateOrthographic(Configuration.WindowWidth, Configuration.WindowHeight, 0.01f, 100f);
        Matrix4x4 zoomMatrix = Matrix4x4.CreateScale(Zoom);
        
        return transMatrix * orthoMatrix * zoomMatrix;
    }
    
    public override mat4 GLM_GetProjectionMatrix() {
        float aspectRatio = (float) Configuration.WindowWidth / (float) Configuration.WindowHeight;
        mat4 projectionMatrix = glm.ortho(-aspectRatio * Zoom, aspectRatio * Zoom, -Zoom, Zoom, -ClippingDistance.X, -ClippingDistance.Y);
        mat4 viewProjectionMat = projectionMatrix * GetViewMat();
        return viewProjectionMat;
    }
    
    private mat4 GetViewMat() {
        mat4 translationMatrix = glm.translate(new mat4(1), new vec3(Position.X, Position.Y, Position.Z));
        mat4 translationAndRotationMatrix = glm.rotate(translationMatrix, glm.radians(LocalRotation), new vec3(0, 0, 1));
        return glm.inverse(translationAndRotationMatrix);
    }
    
}
