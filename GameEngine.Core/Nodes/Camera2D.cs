using System;
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
    
    public override mat4 GLM_GetProjectionMatrix() {
        // float aspectRatio = (float) Configuration.WindowWidth / (float) Configuration.WindowHeight;
        float aspectRatioGameWindow = (float) MainFrameBuffer1.Width / (float) MainFrameBuffer1.Height;
        mat4 projectionMatrix = glm.ortho(-aspectRatioGameWindow * Zoom, aspectRatioGameWindow * Zoom, -Zoom, Zoom, -ClippingDistance.X, -ClippingDistance.Y);
         mat4 viewProjectionMat = projectionMatrix * GetViewMat();
        return viewProjectionMat;
    }
    
    private mat4 GetViewMat() {
        mat4 translationMatrix = glm.translate(new mat4(1f), new vec3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z));
        mat4 translationAndRotationMatrix = glm.rotate(translationMatrix, glm.radians(WorldRotation.Z), new vec3(0, 0, 1));
        return glm.inverse(translationAndRotationMatrix);
    }
    
}
