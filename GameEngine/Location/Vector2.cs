using System;

namespace GameEngine; 

public struct Vector2 {
    
    public float X { get; set; }
    public float Y { get; set; }
    
    
    public Vector2(float x, float y) {
        X = x;
        Y = y;
    }
    
    public float Magnitude => (float) Math.Sqrt(X * X + Y * Y);
    public Vector2 Normalized => this / Magnitude;
    
    public Vector3 XY_ => new Vector3(X, Y, 0f);
    public Vector3 X_Y => new Vector3(X, 0f, Y);
    public Vector3 _XY => new Vector3(0f, X, Y);
    
    public override string ToString() {
        return $"Vector2({X}, {Y})";
    }
    
    public static Vector2 operator +(Vector2 v1, Vector2 v2) {
        return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
    }
    
    public static Vector2 operator -(Vector2 v1, Vector2 v2) {
        return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
    }
    
    public static Vector2 operator *(Vector2 v, float f) {
        return new Vector2(v.X * f, v.Y * f);
    }
    
    public static Vector2 operator /(Vector2 v, float f) {
        return new Vector2(v.X / f, v.Y / f);
    }
    
    public static implicit operator System.Numerics.Vector2(Vector2 v) => new System.Numerics.Vector2(v.X, v.Y);
    
    public static Vector2 Zero => new Vector2(0f, 0f);
    
    public static Vector2 One => new Vector2(1f, 1f);
    
    public static Vector2 Up => new Vector2(0f, 1f);
    
    public static Vector2 Down => new Vector2(0f, -1f);
    
    public static Vector2 Left => new Vector2(-1f, 0f);
    
    public static Vector2 Right => new Vector2(1f, 0f);
    
}
