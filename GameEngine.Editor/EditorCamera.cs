using GameEngine.Core;
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
        
        float x = 0;
        x += Core.Input.Input.IsKeyDown(KeyCode.A) ? -1 : 0;
        x += Core.Input.Input.IsKeyDown(KeyCode.D) ? 1 : 0;
        
        float y = 0;
        y += Core.Input.Input.IsKeyDown(KeyCode.E) ? 1 : 0;
        y += Core.Input.Input.IsKeyDown(KeyCode.Q) ? -1 : 0;
        
        float z = 0;
        z += Core.Input.Input.IsKeyDown(KeyCode.W) ? -1 : 0;
        z += Core.Input.Input.IsKeyDown(KeyCode.S) ? 1 : 0;
        
        vec3 moveRelative = new vec3(x, y, z);
        vec3 moveWorld = RotateVectorByQuaternion(moveRelative, orientation);
        WorldPosition += new Vector3(moveWorld.x, moveWorld.y, moveWorld.z) * FlyingSpeed * deltaTime;
        
        float yaw = -Core.Input.Input.MouseDelta.X * 0.005f;
        float pitch = Core.Input.Input.MouseDelta.Y * 0.005f;
        
        orientation = quat.FromAxisAngle(yaw, new vec3(0, 1, 0)) * orientation;
        orientation = orientation * quat.FromAxisAngle(pitch, new vec3(1, 0, 0));
        
        LocalRotation = new Quaternion(orientation.x, orientation.y, orientation.z, orientation.w);
    }
    
    private vec3 RotateVectorByQuaternion(vec3 v3, quat q4) {
        // Extract the vector part of the quaternion
        vec3 q3 = new vec3(q4.x, q4.y, q4.z);
        
        // Extract the scalar part of the quaternion
        float s = q4.w;
        
        // Do the math
        return 2.0f * vec3.Dot(q3, v3) * q3
                 + (s*s - vec3.Dot(q3, q3)) * v3
                 + 2.0f * s * vec3.Cross(q3, v3);
    }
    
    public override mat4 GLM_GetProjectionMatrix() {
        quat orientation = new quat(LocalRotation.X, LocalRotation.Y, LocalRotation.Z, LocalRotation.W);
        vec3 relativeForward = RotateVectorByQuaternion(new vec3(0, 0, -1), orientation);
        relativeForward = relativeForward.Normalized;
        vec3 relativeUp = RotateVectorByQuaternion(new vec3(0, 1, 0), orientation);
        relativeUp = relativeUp.Normalized;
        
        vec3 position = new vec3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z);
        mat4 viewMatrix = mat4.LookAt(position, position + relativeForward, relativeUp);
        
        float aspectRatio = (float) Application.Instance!.Renderer.MainFrameBuffer2.Width / (float) Application.Instance!.Renderer.MainFrameBuffer2.Height;
        mat4 projectionMatrix = mat4.Perspective(GlmNet.glm.radians(FOV), aspectRatio, ClippingDistance.X, ClippingDistance.Y);
        
        mat4 viewProjectionMat = projectionMatrix * viewMatrix;
        return viewProjectionMat;
    }
    
}
