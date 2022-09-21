using GameEngine.Core.Numerics;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes;

public partial class Transform3D : Node {
    
    protected virtual bool TransformIsIndependent => false;
    
    [Serialized] public Vector3 LocalPosition { get; set; } = Vector3.Zero;
    public Vector3 WorldPosition {
        get {
            if(TransformIsIndependent)
                return LocalPosition;
            
            Vector3 value = LocalPosition;
            
            for(Node? current = ParentNode; current is not null; current = current.ParentNode) {
                if(current is not Transform3D transform)
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
                if(current is not Transform3D transform)
                    continue;
                origin += transform.LocalPosition;
                if(transform.TransformIsIndependent)
                    break;
            }
            
            LocalPosition = -origin + value;
        }
    }
    
//    [Serialized] public Quaternion LocalRotation { get; set; } = Quaternion.Identity;
//    public Quaternion WorldRotation {
//        get {
//            if(TransformIsIndependent)
//                return LocalRotation;
//            
//            Quaternion value = LocalRotation;
//            
//            for(Node? current = ParentNode; current is not null; current = current.ParentNode) {
//                if(current is not Transform3D transform)
//                    continue;
//                value += transform.LocalRotation;
//                if(transform.TransformIsIndependent)
//                    break;
//            }
//            
//            return value;
//        }
//        set {
//            Quaternion origin = Quaternion.Identity;
//            
//            for(Node? current = ParentNode; current is not null; current = current.ParentNode) {
//                if(current is not Transform3D transform)
//                    continue;
//                origin += transform.LocalRotation;
//                if(transform.TransformIsIndependent)
//                    break;
//            }
//            
//            LocalRotation = -origin + value;
//        }
//    }
    
    [Serialized] public Quaternion LocalRotation { get; set; } = Quaternion.Identity;
    public Quaternion WorldRotation {
        get {
            if(TransformIsIndependent)
                return LocalRotation;
            
            Quaternion value = LocalRotation;
            
            for(Node? current = ParentNode; current is not null; current = current.ParentNode) {
                if(current is not Transform3D transform)
                    continue;
                value *= transform.LocalRotation;
                if(transform.TransformIsIndependent)
                    break;
            }
            
            return value;
        }
        set {
            Quaternion origin = Quaternion.Identity;
            
            for(Node? current = ParentNode; current is not null; current = current.ParentNode) {
                if(current is not Transform3D transform)
                    continue;
                origin *= transform.LocalRotation;
                if(transform.TransformIsIndependent)
                    break;
            }
            
            LocalRotation = -origin + value;
        }
    }
    
    [Serialized] public Vector3 LocalScale { get; set; } = Vector3.One;
    public Vector3 WorldScale {
        get {
            if(TransformIsIndependent)
                return LocalScale;
            
            Vector3 value = LocalScale;
            
            for(Node? current = ParentNode; current is not null; current = current.ParentNode) {
                if(current is not Transform3D transform)
                    continue;
                value *= transform.LocalScale;
                if(transform.TransformIsIndependent)
                    break;
            }
            
            return value;
        }
        set {
            Vector3 origin = Vector3.One;
            
            for(Node? current = ParentNode; current is not null; current = current.ParentNode) {
                if(current is not Transform3D transform)
                    continue;
                origin *= transform.LocalScale;
                if(transform.TransformIsIndependent)
                    break;
            }
            
            LocalScale = -origin + value;
        }
    }
    
    public Vector3 GetLocalEulerAngles() {
        return LocalRotation.ToEulerAngles();
    }
    
    public Vector3 GetWorldEulerAngles() {
        return WorldRotation.ToEulerAngles();
    }
    
    public void SetLocalEulerAngles(Vector3 eulerAngles) {
        LocalRotation = Quaternion.CreateFromAxisAngle(eulerAngles, 1);
    }
    
    public void SetWorldEulerAngles(Vector3 eulerAngles) {
        WorldRotation = Quaternion.CreateFromAxisAngle(eulerAngles, 1);
    }
    
}
