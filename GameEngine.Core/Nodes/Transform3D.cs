using GameEngine.Core.Serialization;
using GameEngine.Numerics;

namespace GameEngine.Core.Nodes;

public partial class Transform3D : Node {
    
    protected virtual bool TransformIsIndependent => false;
    
    [Serialized] public Vec3<float> LocalPosition { get; set; } = Vec3<float>.Zero;
    [Serialized] public Quaternion<float> LocalRotation { get; set; } = Quaternion<float>.Identity;
    [Serialized] public Vec3<float> LocalScale { get; set; } = Vec3<float>.One;
    
    public Matrix<float> LocalToWorldMatrix =>
        Matrix<float>.CreateTranslation(LocalPosition) *
        Matrix<float>.CreateFromQuaternion(LocalRotation) *
        Matrix<float>.CreateScale(LocalScale);
    
}
