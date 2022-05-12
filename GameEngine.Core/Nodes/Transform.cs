using GameEngine.Core.Numerics;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes;

public partial class Transform : Node {
    
    [Serialized] public Vector3 LocalPosition { get; set; } = Vector3.Zero;
    
//    public Vector3 Position = Vector3.Zero;
    [Serialized] public Vector3 Position {
        get {
            Vector3 value = LocalPosition;
            
            for(Node? current = ParentNode; current is not null; current = current.ParentNode) {
                if(current is not Transform transform)
                    continue;
                value += transform.LocalPosition;
            }
    
            return value;
        }
//        set {
//            _localPosition = Position - value;
//        }
    }
    
    [Serialized] public Vector3 Scale { get; set; } = Vector3.One;
    [Serialized] public float Rotation { get; set; } = 0f;
    
}
