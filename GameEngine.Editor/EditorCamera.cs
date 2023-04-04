using GameEngine.Core;
using GameEngine.Core.Input;
using GameEngine.Core.Nodes;
using GameEngine.Core.Rendering;
using GameEngine.Numerics;
using Silk.NET.OpenGL;

namespace GameEngine.Editor; 

public sealed class EditorCamera : Transform3D, ICamera {
    
    public Color BackgroundColor { get; set; } = Application.Instance.Config.DefaultBackgroundColor;
    public FrameBuffer FrameBuffer { get; }
    
    public Vec2<float> ClippingDistance { get; set; } = new Vec2<float>(0.01f, 10000f);
    public float Fov = 90f;
    public float FlyingSpeed = 10f;
    
    public EditorCamera(GL gl, Renderer renderer) {
        FrameBuffer = new FrameBuffer(
            gl,
            renderer,
            1,
            1,
            false
        );
        LocalPosition = new Vec3<float>(0, 0, -5);
    }
    
    public Matrix<float> ViewMatrix =>
        Matrix<float>.Invert(
            Matrix<float>.CreateFromQuaternion(LocalRotation.Normalized()) *
            Matrix<float>.CreateTranslation(-LocalPosition));
    
    public Matrix<float> ProjectionMatrix {
        get {
            float aspectRatioGameFrameBuffer = (float) FrameBuffer.Width / (float) FrameBuffer.Height;
            
            return Matrix<float>.CreatePerspectiveFieldOfView(MathHelper.ToRadians(Fov), aspectRatioGameFrameBuffer, ClippingDistance.X, ClippingDistance.Y);
        }
    }
    
    internal void EditorUpdate(float deltaTime) {
        Application.Instance.Renderer.SetActiveCamera(this);
        base.OnUpdate();
        
        if(!Input.IsKeyDown(KeyCode.LeftControl))
            return;
        
        float x = 0;
        x += Input.IsKeyDown(KeyCode.A) ? +1 : 0;
        x += Input.IsKeyDown(KeyCode.D) ? -1 : 0;
        
        float y = 0;
        y += Input.IsKeyDown(KeyCode.E) ? -1 : 0;
        y += Input.IsKeyDown(KeyCode.Q) ? +1 : 0;
        
        float z = 0;
        z += Input.IsKeyDown(KeyCode.W) ? +1 : 0;
        z += Input.IsKeyDown(KeyCode.S) ? -1 : 0;
        
        LocalPosition += (LocalRotation * new Vec3<float>(x, y, z)) * FlyingSpeed * deltaTime;
        
        float yaw = -Input.MouseDelta.X * 0.005f;
        float pitch = Input.MouseDelta.Y * 0.005f;
        
        LocalRotation = Quaternion<float>.CreateFromAxisAngle(Vec3<float>.Up, yaw) * LocalRotation * Quaternion<float>.CreateFromAxisAngle(Vec3<float>.Right, pitch);
    }
    
}
