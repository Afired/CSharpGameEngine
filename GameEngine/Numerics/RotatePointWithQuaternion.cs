using System;

namespace GameEngine.Numerics; 

public partial struct Quaternion : IEquatable<Quaternion> {
    
    public static Quaternion operator *(Quaternion q, Vector3 v) {
        return q * v.XYZ_ * Conjugate(q);
    }
    
}
