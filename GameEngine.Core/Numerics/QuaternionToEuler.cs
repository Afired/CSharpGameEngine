//using System;
//
//namespace GameEngine.Core.Numerics; 
//
//public partial struct Quaternion : IEquatable<Quaternion> {
//    
//    public float ComputeXAngle() {
//        float sinr_cosp = 2 * (W * X + Y * Z);
//        float cosr_cosp = 1 - 2 * (X * X + Y * Y);
//        return (float) Math.Atan2(sinr_cosp, cosr_cosp);
//    }
//
//    public float ComputeYAngle()
//    {
//        float sinp = 2 * (W * Y - Z * X);
//        if (Math.Abs(sinp) >= 1)
//            return (float) (Math.PI / 2 * Math.Sign(sinp)); // use 90 degrees if out of range
//        else
//            return (float) Math.Asin(sinp);
//    }
//
//    public float ComputeZAngle()
//    {
//        float siny_cosp = 2 * (W * Z + X * Y);
//        float cosy_cosp = 1 - 2 * (Y * Y + Z * Z);
//        return (float) Math.Atan2(siny_cosp, cosy_cosp);
//    }
//
////    public Vector3 ToEulerAngles()
////    {
////        return new Vector3(ComputeXAngle(), ComputeYAngle(), ComputeZAngle());
////    }
//    
////    public Vector3 ToEulerAngles() {
////        double roll = Math.Atan2(2 * (X * Y + Z * W), (1 - 2 * (Y * Y + Z * Z)));
////        double pitch = Math.Asin(2 * (X * Z - W * Y));
////        double yaw = Math.Atan2(2 * (X * W + Y * Z), (1 - 2 * (Z * Z + W * W)));
////        
////        return new Vector3((float) pitch, (float) yaw, (float) roll);
////    }
//
//    public Vector3 ToEulerAngles() {
//
//        double t0 = +2.0 * (W * X + Y * Z);
//        double t1 = +1.0 - 2.0 * (X * X + Y * Y);
//        double roll = Math.Atan2(t0, t1);
//        
//        double t2 = 2 * (W * Y - Z * X);
//        double pitch = 0;
//        if (Math.Abs(t2) >= 1)
//            pitch = (Math.PI / 2 * Math.Sign(t2)); // use 90 degrees if out of range
//        else
//            pitch = Math.Asin(t2);
//        
//        double t3 = +2.0 * (W * Z + X * Y);
//        double t4 = +1.0 - 2.0 * (Y * Y + Z * Z);
//        double yaw = Math.Atan2(t3, t4);
//
//        return new Vector3((float) yaw, (float) pitch, (float) roll);
//    }
//    
////    def quaternion_to_euler(q):
////    (x, y, z, w) = (q[0], q[1], q[2], q[3])
////    t0 = +2.0 * (w * x + y * z)
////    t1 = +1.0 - 2.0 * (x * x + y * y)
////    roll = math.atan2(t0, t1)
////        t2 = +2.0 * (w * y - z * x)
////    t2 = +1.0 if t2 > +1.0 else t2
////        t2 = -1.0 if t2 < -1.0 else t2
////        pitch = math.asin(t2)
////    t3 = +2.0 * (w * z + x * y)
////    t4 = +1.0 - 2.0 * (y * y + z * z)
////    yaw = math.atan2(t3, t4)
////    return [yaw, pitch, roll]
//    
//    public static Quaternion FromEulerAngles(Vector3 vector3) {
//        float halfPitch = vector3.X * 0.5f;
//        double sinPitch = Math.Sin(halfPitch);
//        double cosPitch = Math.Cos(halfPitch);
//        
//        float halfYaw = vector3.Y * 0.5f;
//        double sinYaw = Math.Sin(halfYaw);
//        double cosYaw = Math.Cos(halfYaw);
//        
//        float halfRoll = vector3.Z * 0.5f;
//        double sinRoll = Math.Sin(halfRoll);
//        double cosRoll = Math.Cos(halfRoll);
//        
//        return new Quaternion() {
//            X = (float) ((cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll)),
//            Y = (float) ((sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll)),
//            Z = (float) ((cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll)),
//            W = (float) ((cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll)),
//        };
//    }
//    
//}