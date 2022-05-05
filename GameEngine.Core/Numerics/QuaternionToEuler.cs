using System;

namespace GameEngine.Core.Numerics; 

public partial struct Quaternion : IEquatable<Quaternion> {
    
    public float ComputeXAngle() {
        float sinr_cosp = 2 * (W * X + Y * Z);
        float cosr_cosp = 1 - 2 * (X * X + Y * Y);
        return (float) Math.Atan2(sinr_cosp, cosr_cosp);
    }

    public float ComputeYAngle()
    {
        float sinp = 2 * (W * Y - Z * X);
        if (Math.Abs(sinp) >= 1)
            return (float) (Math.PI / 2 * Math.Sign(sinp)); // use 90 degrees if out of range
        else
            return (float) Math.Asin(sinp);
    }

    public float ComputeZAngle()
    {
        float siny_cosp = 2 * (W * Z + X * Y);
        float cosy_cosp = 1 - 2 * (Y * Y + Z * Z);
        return (float) Math.Atan2(siny_cosp, cosy_cosp);
    }

    public Vector3 ToEulerAngles()
    {
        return new Vector3(ComputeXAngle(), ComputeYAngle(), ComputeZAngle());
    }
    
}