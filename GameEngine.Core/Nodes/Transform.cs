using GameEngine.Core.Numerics;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes;

public partial class Transform : Node {

    protected virtual bool TransformIsIndependent => false;
    [Serialized] public Vector3 LocalPosition { get; set; } = Vector3.Zero;
    
    [Serialized] public Vector3 Position {
        get {
            if(TransformIsIndependent)
                return LocalPosition;
            
            Vector3 value = LocalPosition;
            
            for(Node? current = ParentNode; current is not null; current = current.ParentNode) {
                if(current is not Transform transform)
                    continue;
                value += transform.LocalPosition;
                if(transform.TransformIsIndependent)
                    break;
            }
            
            return value;
        }
        set {
            Vector3 origin = Vector3.Zero;
            
            for(Node? current = ParentNode; current is not null; current = current.ParentNode) {
                if(current is not Transform transform)
                    continue;
                origin += transform.LocalPosition;
                if(transform.TransformIsIndependent)
                    break;
            }
            
            LocalPosition = -origin + value;
        }
    }
    
    [Serialized] public Vector3 Scale { get; set; } = Vector3.One;
    [Serialized] public float LocalRotation { get; set; } = 0f;
    
    [Serialized] public float Rotation {
        get {
            if(TransformIsIndependent)
                return LocalRotation;
            
            float value = LocalRotation;
            
            for(Node? current = ParentNode; current is not null; current = current.ParentNode) {
                if(current is not Transform transform)
                    continue;
                value += transform.LocalRotation;
                if(transform.TransformIsIndependent)
                    break;
            }
            
            return value;
        }
        set {
            float origin = 0;
            
            for(Node? current = ParentNode; current is not null; current = current.ParentNode) {
                if(current is not Transform transform)
                    continue;
                origin += transform.LocalRotation;
                if(transform.TransformIsIndependent)
                    break;
            }
            
            LocalRotation = -origin + value;
        }
    }
    
}
