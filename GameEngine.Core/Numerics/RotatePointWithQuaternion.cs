using System;

namespace GameEngine.Core.Numerics; 

public partial struct Quaternion : IEquatable<Quaternion> {
    
    public static Quaternion operator *(Quaternion q, Vector3 v) {
        return q * new Quaternion(v.X, v.Y, v.Z, 0) * Conjugate(q);
    }
    
}
