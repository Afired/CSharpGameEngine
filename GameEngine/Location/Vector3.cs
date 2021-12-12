using System;

namespace GameEngine; 

public struct Vector3 {
    
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    
    
    public Vector3(float x, float y, float z) {
        X = x;
        Y = y;
        Z = z;
    }
    
    public float Magnitude => (float) Math.Sqrt(X * X + Y * Y + Z * Z);
    public Vector3 Normalized => this / Magnitude;
    
    public Vector2 XY => new Vector2(X, Y);
    public Vector2 YZ => new Vector2(Y, Z);
    
    public override string ToString() {
        return $"Vector3({X}, {Y}, {Z})";
    }
    
    public static Vector3 operator +(Vector3 v1, Vector3 v2) {
        return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
    }
    
    public static Vector3 operator -(Vector3 v1, Vector3 v2) {
        return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
    }
    
    public static Vector3 operator *(Vector3 v, float f) {
        return new Vector3(v.X * f, v.Y * f, v.Z * f);
    }
    
    public static Vector3 operator /(Vector3 v, float f) {
        return new Vector3(v.X / f, v.Y / f, v.Z / f);
    }
    
    public static implicit operator System.Numerics.Vector3(Vector3 v) => new System.Numerics.Vector3(v.X, v.Y, v.Z);

    public static Vector3 Zero => new Vector3(0f, 0f, 0f);

    public static Vector3 One => new Vector3(1f, 1f, 1f);

    public static Vector3 Front => new Vector3(0f, 0f, 1f);

    public static Vector3 Back => new Vector3(1f, 0f, -1f);

    public static Vector3 Up => new Vector3(0f, 1f, 0f);

    public static Vector3 Down => new Vector3(0f, -1f, 0f);

    public static Vector3 Left => new Vector3(-1f, 0f, 0f);

    public static Vector3 Right => new Vector3(1f, 0f, 0f);

}
