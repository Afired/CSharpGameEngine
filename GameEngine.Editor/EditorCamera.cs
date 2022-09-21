using GameEngine.Core.Input;
using GameEngine.Core.Nodes;
using GameEngine.Core.Numerics;
using GlmSharp;

namespace GameEngine.Editor; 

public class EditorCamera : BaseCamera {
    
    public static EditorCamera Instance => _instance ??= NewDefault();
    private static EditorCamera? _instance;
    
    private static EditorCamera NewDefault() {
        return new EditorCamera {
            WorldPosition = new Vector3(0, 0, -5),
            IsMainCamera = true
        };
    }
    
    public Vector2 ClippingDistance { get; set; } = new Vector2(0.01f, 100f);
    public float FOV = 90f;
    public float FlyingSpeed = 10f;
    
    internal void EditorUpdate(float deltaTime) {
        base.OnUpdate();
        
        if(!Input.IsKeyDown(KeyCode.LeftControl))
            return;
        
        quat orientation = new quat(LocalRotation.X, LocalRotation.Y, LocalRotation.Z, LocalRotation.W);
        quat orientationConj = orientation.Conjugate;
        // vec3 up = new vec3(orientation * vec4.UnitY * orientationConj);
        // vec3 forwards = new vec3(orientation * -vec4.UnitZ * orientationConj);
        // vec3 right = new vec3(orientation * vec4.UnitX * orientationConj);
        
        float x = 0;
        x += Core.Input.Input.IsKeyDown(KeyCode.A) ? -1 : 0;
        x += Core.Input.Input.IsKeyDown(KeyCode.D) ? 1 : 0;
        
        float y = 0;
        y += Core.Input.Input.IsKeyDown(KeyCode.E) ? 1 : 0;
        y += Core.Input.Input.IsKeyDown(KeyCode.Q) ? -1 : 0;
        
        float z = 0;
        z += Core.Input.Input.IsKeyDown(KeyCode.W) ? -1 : 0;
        z += Core.Input.Input.IsKeyDown(KeyCode.S) ? 1 : 0;
        
        GlmSharp.vec3 moveRelative = new GlmSharp.vec3(x, y, z);
        GlmSharp.vec3 moveWorld = RotateVectorByQuaternion(moveRelative, orientation);
        WorldPosition += new Vector3(moveWorld.x, moveWorld.y, moveWorld.z) * FlyingSpeed * deltaTime;
        
        float yaw = -Core.Input.Input.MouseDelta.X * 0.005f;
        float pitch = Core.Input.Input.MouseDelta.Y * 0.005f;
        
        orientation = quat.FromAxisAngle(yaw, new GlmSharp.vec3(0, 1, 0)) * orientation;
        orientation = orientation * quat.FromAxisAngle(pitch, new GlmSharp.vec3(1, 0, 0));
        
        LocalRotation = new Quaternion(orientation.x, orientation.y, orientation.z, orientation.w);
    }
    
    private GlmSharp.vec3 RotateVectorByQuaternion(GlmSharp.vec3 v3, quat q4) {
        // Extract the vector part of the quaternion
        GlmSharp.vec3 q3 = new GlmSharp.vec3(q4.x, q4.y, q4.z);
        
        // Extract the scalar part of the quaternion
        float s = q4.w;
        
        // Do the math
        return 2.0f * GlmSharp.vec3.Dot(q3, v3) * q3
                 + (s*s - GlmSharp.vec3.Dot(q3, q3)) * v3
                 + 2.0f * s * GlmSharp.vec3.Cross(q3, v3);
    }
    
    public override GlmNet.mat4 GLM_GetProjectionMatrix() {
        
        float aspectRatioGameFrameBuffer = (float) Core.Rendering.Renderer.MainFrameBuffer2.Width / (float) Core.Rendering.Renderer.MainFrameBuffer2.Height;
        // float aspectRatioWindow = (float) Configuration.WindowWidth / (float) Configuration.WindowHeight;
        GlmSharp.mat4 projectionMatrix = GlmSharp.mat4.Perspective(GlmNet.glm.radians(FOV), aspectRatioGameFrameBuffer, ClippingDistance.X, ClippingDistance.Y);
        GlmSharp.mat4 viewProjectionMat = projectionMatrix * GetViewMat();
        
        return new GlmNet.mat4(
            new GlmNet.vec4(viewProjectionMat.m00, viewProjectionMat.m01, viewProjectionMat.m02, viewProjectionMat.m03),
            new GlmNet.vec4(viewProjectionMat.m10, viewProjectionMat.m11, viewProjectionMat.m12, viewProjectionMat.m13),
            new GlmNet.vec4(viewProjectionMat.m20, viewProjectionMat.m21, viewProjectionMat.m22, viewProjectionMat.m23),
            new GlmNet.vec4(viewProjectionMat.m30, viewProjectionMat.m31, viewProjectionMat.m32, viewProjectionMat.m33)
        );
    }
    
    private GlmSharp.mat4 GetViewMat() {
        GlmSharp.mat4 t = GlmSharp.mat4.Translate(WorldPosition.X, WorldPosition.Y, WorldPosition.Z) *
                          new GlmSharp.quat(WorldRotation.X, WorldRotation.Y, WorldRotation.Z, WorldRotation.W).Normalized.ToMat4;
        return t.Inverse;
    }
    
}
