using System;

namespace GameEngine.Numerics; 

public partial struct Quaternion : IEquatable<Quaternion> {
    
    public static Quaternion operator *(Quaternion q, Vector3 v) {
        return q * v.XYZ_ * Conjugate(q);
    }
    
}

/*
Actually to rotate a vector / point by a quaternion "q" you have to do

p' = q * p * q^-1

where q^-1 is the complex conjugate of q. It has to be done is this order. Unity does this "sandwich multiplication" internally for you when you do q * p. I recommend to watch this 3blue1brown video on quaternions.
*/