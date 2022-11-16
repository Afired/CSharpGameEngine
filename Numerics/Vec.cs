using System.Numerics;

namespace Numerics; 

public struct Vec3<T> where T : INumber<T> {
    
    public T X { get; }
    public T Y { get; }
    public T Z { get; }
    
    public Vec3(T x, T y, T z) {
        X = x;
        Y = y;
        Z = z;
    }
    
    public static Vec3<T> Zero => new Vec3<T>(T.Zero, T.Zero, T.Zero);
    public static Vec3<T> One => new Vec3<T>(T.One, T.One, T.One);
    public static Vec3<T> Front => new Vec3<T>(T.Zero, T.Zero, T.One);
    public static Vec3<T> Back => new Vec3<T>(T.One, T.Zero, -T.One);
    public static Vec3<T> Up => new Vec3<T>(T.Zero, T.One, T.Zero);
    public static Vec3<T> Down => new Vec3<T>(T.Zero, -T.One, T.Zero);
    public static Vec3<T> Left => new Vec3<T>(-T.One, T.Zero, T.Zero);
    public static Vec3<T> Right => new Vec3<T>(T.One, T.Zero, T.Zero);
    
}
