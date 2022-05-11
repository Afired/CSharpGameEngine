using GameEngine.Core.Numerics;

namespace GameEngine.Core.Nodes;

public partial class Transform : Node {
    
    internal Vector3 _localPosition = Vector3.Zero;
    
    public Vector3 Position = Vector3.Zero;
    // public Vector3 Position {
    //     get {
    //         Vector3 value = _localPosition;
    //         
    //         for(Node? current = ParentNode; current is not null; current = current.ParentNode) {
    //             if(current is not Transform transform)
    //                 continue;
    //             value += transform._localPosition;
    //         }
    //
    //         return value;
    //     }
    //     set {
    //         _localPosition = Position - value;
    //     }
    // }
    
    public Vector3 Scale { get; set; } = Vector3.One;
    public float Rotation { get; set; } = 0f;
    
}
