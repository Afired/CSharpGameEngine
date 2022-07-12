using System;
using System.Reflection;
using GameEngine.Core.Input;
using GameEngine.Core.Numerics;
using GameEngine.Core.Serialization;
using GlmSharp;

namespace GameEngine.Core.Nodes;

public partial class Camera3D : BaseCamera {
    
    [Serialized] public Vector2 ClippingDistance { get; set; } = new Vector2(0.01f, 100f);
    [Serialized] public float FOV = 90f;
    // [Serialized] public Vector3 Rotation3D;
    [Serialized] public float FlyingSpeed = 10f;
    
    [Serialized] public float RotationX = 0;
    [Serialized] public float RotationY = 0;
    [Serialized] public float RotationZ = 0;
    [Serialized] public float RotationW = 1;
    public override Matrix4x4 GetProjectionMatrix() {
        // Matrix4x4 transMatrix = Matrix4x4.CreateTranslation(Position);
        // Matrix4x4 rotMatrix = Matrix4x4.CreateRotationZ(LocalRotation);
        // Matrix4x4 perMatrix = Matrix4x4.CreatePerspectiveFieldOfView(_fieldOfView, (float) Configuration.WindowWidth / (float) Configuration.WindowHeight, NearPlaneDistance, FarPlaneDistance);
        //
        // return transMatrix * rotMatrix * perMatrix;
        return default;
    }
    
    protected override void OnUpdate() {
        base.OnUpdate();
        
        quat orientation = new quat(RotationX, RotationY, RotationZ, RotationW);
        quat orientationConj = orientation.Conjugate;
        vec3 up = new vec3(orientation * vec4.UnitY * orientationConj);
        vec3 forwards = new vec3(orientation * -vec4.UnitZ * orientationConj);
        vec3 right = new vec3(orientation * vec4.UnitX * orientationConj);
        
        float x = 0;
        x += Input.Input.IsKeyDown(KeyCode.A) ? -1 : 0;
        x += Input.Input.IsKeyDown(KeyCode.D) ? 1 : 0;
        
        float y = 0;
        y += Input.Input.IsKeyDown(KeyCode.E) ? 1 : 0;
        y += Input.Input.IsKeyDown(KeyCode.Q) ? -1 : 0;
        
        float z = 0;
        z += Input.Input.IsKeyDown(KeyCode.W) ? -1 : 0;
        z += Input.Input.IsKeyDown(KeyCode.S) ? 1 : 0;

        vec3 moveRelative = new vec3(x, y, z);
        vec3 moveWorld = RotateVectorByQuaternion(moveRelative, orientation);
        Position += new Vector3(moveWorld.x, moveWorld.y, moveWorld.z) * FlyingSpeed * Time.DeltaTime;
        
        float yaw = -Input.Input.MouseDelta.X * 0.005f;
        float pitch = Input.Input.MouseDelta.Y * 0.005f;
        
        orientation = quat.FromAxisAngle(yaw, new vec3(0, 1, 0)) * orientation;
        orientation = orientation * quat.FromAxisAngle(pitch, new vec3(1, 0, 0));
        
        RotationX = orientation.x;
        RotationY = orientation.y;
        RotationZ = orientation.z;
        RotationW = orientation.w;
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
    
    public override GlmNet.mat4 GLM_GetProjectionMatrix() {
        
        if(Input.Input.IsKeyDown(KeyCode.L))
            typeof(InputHandler).GetField("_catchCursor", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, true);
        else if(Input.Input.IsKeyDown(KeyCode.U))
            typeof(InputHandler).GetField("_catchCursor", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, false);
        
        float aspectRatio = (float) Configuration.WindowWidth / (float) Configuration.WindowHeight;
        mat4 projectionMatrix = mat4.Perspective(GlmNet.glm.radians(FOV), aspectRatio, ClippingDistance.X, ClippingDistance.Y);
        mat4 viewProjectionMat = projectionMatrix * GetViewMat();
        
        return new GlmNet.mat4(
            new GlmNet.vec4(viewProjectionMat.m00, viewProjectionMat.m01, viewProjectionMat.m02, viewProjectionMat.m03),
            new GlmNet.vec4(viewProjectionMat.m10, viewProjectionMat.m11, viewProjectionMat.m12, viewProjectionMat.m13),
            new GlmNet.vec4(viewProjectionMat.m20, viewProjectionMat.m21, viewProjectionMat.m22, viewProjectionMat.m23),
            new GlmNet.vec4(viewProjectionMat.m30, viewProjectionMat.m31, viewProjectionMat.m32, viewProjectionMat.m33)
        );
        
        // float aspectRatio = (float) Configuration.WindowWidth / (float) Configuration.WindowHeight;
        // GlmNet.mat4 projectionMatrix = GlmNet.glm.perspective(GlmNet.glm.radians(FOV), aspectRatio, ClippingDistance.X, ClippingDistance.Y);
        // GlmNet.mat4 viewProjectionMat = projectionMatrix * GetViewMat();
        // return viewProjectionMat;
    }
    
    // private GlmNet.mat4 GetViewMat() {
    //     quat quat = new quat(RotationX, RotationY, RotationZ, RotationW);
    //     vec3 quatAxis = quat.Axis;
    //     GlmNet.mat4 translationAndRotationMatrix = GlmNet.glm.translate(new GlmNet.mat4(1), new GlmNet.vec3(Position.X, Position.Y, Position.Z)) *
    //                                                GlmNet.glm.rotate(quatAxis.x, new GlmNet.vec3(1, 0, 0)) *
    //                                                GlmNet.glm.rotate(quatAxis.y, new GlmNet.vec3(0, 1, 0)) *
    //                                                GlmNet.glm.rotate(quatAxis.z, new GlmNet.vec3(0, 0, 1));
    //     return GlmNet.glm.inverse(translationAndRotationMatrix);
    // }
    
    private mat4 GetViewMat() {
        mat4 t = mat4.Translate(Position.X, Position.Y, Position.Z) * new quat(RotationX, RotationY, RotationZ, RotationW).Normalized.ToMat4;
        return t.Inverse;
    }
    
}
