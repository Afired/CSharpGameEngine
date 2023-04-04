using GameEngine.Core.Input;
using GameEngine.Core.Serialization;
using GameEngine.Numerics;

namespace GameEngine.Core.Nodes;

public partial class Camera3D : BaseCamera {
    
    [Serialized] public Vec2<float> ClippingDistance { get; set; } = new Vec2<float>(0.01f, 10000f);
    [Serialized] public float Fov = 90f;
    [Serialized] public float FlyingSpeed = 10f;
    
    protected override void OnUpdate() {
        base.OnUpdate();
        
        float x = 0;
        x += Input.Input.IsKeyDown(KeyCode.A) ? +1 : 0;
        x += Input.Input.IsKeyDown(KeyCode.D) ? -1 : 0;
        
        float y = 0;
        y += Input.Input.IsKeyDown(KeyCode.E) ? -1 : 0;
        y += Input.Input.IsKeyDown(KeyCode.Q) ? +1 : 0;
        
        float z = 0;
        z += Input.Input.IsKeyDown(KeyCode.W) ? +1 : 0;
        z += Input.Input.IsKeyDown(KeyCode.S) ? -1 : 0;
        
        LocalPosition += (LocalRotation * new Vec3<float>(x, y, z)) * FlyingSpeed * Time.DeltaTime;
        
        float yaw = -Input.Input.MouseDelta.X * 0.005f;
        float pitch = Input.Input.MouseDelta.Y * 0.005f;
        
        LocalRotation = Quaternion<float>.CreateFromAxisAngle(Vec3<float>.Up, yaw) * LocalRotation * Quaternion<float>.CreateFromAxisAngle(Vec3<float>.Right, pitch);
    }
    
    public override Matrix<float> ViewMatrix =>
        Matrix<float>.Invert(
            Matrix<float>.CreateFromQuaternion(LocalRotation.Normalized()) *
            Matrix<float>.CreateTranslation(-LocalPosition));
    
    public override Matrix<float> ProjectionMatrix  {
        get {
//            float aspectRatioGameFrameBuffer = (float) Application.Instance.Renderer.MainFrameBuffer2.Width / (float) Application.Instance.Renderer.MainFrameBuffer2.Height;
            float aspectRatioGameFrameBuffer = (float) 1 / (float) 1;
            
            return Matrix<float>.CreatePerspectiveFieldOfView(MathHelper.ToRadians(Fov), aspectRatioGameFrameBuffer, ClippingDistance.X, ClippingDistance.Y);
        }
    }
    
}
