using System;
using System.Globalization;
using System.Text;

namespace GameEngine.Numerics;

public struct Vector4 : IEquatable<Vector4>, IFormattable {
    
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }
    
    
    public Vector4(float x, float y, float z, float w) {
        W = w;
        X = x;
        Y = y;
        Z = z;
    }
    
    public Vector4(float value) : this(value, value, value, value) { }
    
    public static Vector4 Zero => new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
    public static Vector4 One => new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
    public static Vector4 UnitX => new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
    public static Vector4 UnitY => new Vector4(0.0f, 1.0f, 0.0f, 0.0f);
    public static Vector4 UnitZ => new Vector4(0.0f, 0.0f, 1.0f, 0.0f);
    public static Vector4 UnitW => new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
    
    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode() {
        int hash = X.GetHashCode();
        hash = HashCodeHelper.CombineHashCodes(hash, Y.GetHashCode());
        hash = HashCodeHelper.CombineHashCodes(hash, Z.GetHashCode());
        hash = HashCodeHelper.CombineHashCodes(hash, W.GetHashCode());
        return hash;
    }

    /// <summary>
    /// Returns a boolean indicating whether the given Object is equal to this Vector4 instance.
    /// </summary>
    /// <param name="obj">The Object to compare against.</param>
    /// <returns>True if the Object is equal to this Vector4; False otherwise.</returns>
    public override bool Equals(object obj) {
        if (!(obj is Vector4))
            return false;
        return Equals((Vector4)obj);
    }

    /// <summary>
    /// Returns a String representing this Vector4 instance.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() {
        return ToString("G", CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns a String representing this Vector4 instance, using the specified format to format individual elements.
    /// </summary>
    /// <param name="format">The format of individual elements.</param>
    /// <returns>The string representation.</returns>
    public string ToString(string format) {
        return ToString(format, CultureInfo.CurrentCulture);
    }
    
    public string ToString(string format, IFormatProvider formatProvider) {
        StringBuilder sb = new StringBuilder();
        string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
        sb.Append('<');
        sb.Append(X.ToString(format, formatProvider));
        sb.Append(separator);
        sb.Append(' ');
        sb.Append(Y.ToString(format, formatProvider));
        sb.Append(separator);
        sb.Append(' ');
        sb.Append(Z.ToString(format, formatProvider));
        sb.Append(separator);
        sb.Append(' ');
        sb.Append(W.ToString(format, formatProvider));
        sb.Append('>');
        return sb.ToString();
    }

    /// <summary>
    /// Returns the length of the vector. This operation is cheaper than Length().
    /// </summary>
    /// <returns>The vector's length.</returns>
    public float Magnitude => (float) Math.Sqrt(Dot(this, this));

    /// <summary>
    /// Returns the length of the vector squared.
    /// </summary>
    /// <returns>The vector's length squared.</returns>
    public float MagnitudeSquared => Dot(this, this);
    
    /// <summary>
    /// Returns a vector with the same direction as the given vector, but with a length of 1.
    /// </summary>
    /// <returns>The normalized vector.</returns>
    public Vector4 Normalized {
        get {
            float magnitude = Magnitude;
            return magnitude == 0f ? Zero : this / magnitude;
        }
    }
    
    /// <summary>
    /// Returns a vector with the same direction as the given vector, but with a length of 1.
    /// </summary>
    /// <param name="vec4">The vector to normalize.</param>
    /// <returns>The normalized vector.</returns>
    public static Vector4 Normalize(Vector4 vec4) {
        return vec4.Normalized;
    }

    /// <summary>
    /// Returns the Euclidean distance between the two given points.
    /// </summary>
    /// <param name="value1">The first point.</param>
    /// <param name="value2">The second point.</param>
    /// <returns>The distance.</returns>
    public static float Distance(Vector4 value1, Vector4 value2) {
        Vector4 difference = value1 - value2;
        return (float) Math.Sqrt(Dot(difference, difference));
    }

    /// <summary>
    /// Returns the Euclidean distance squared between the two given points.
    /// </summary>
    /// <param name="value1">The first point.</param>
    /// <param name="value2">The second point.</param>
    /// <returns>The distance squared.</returns>
    public static float DistanceSquared(Vector4 value1, Vector4 value2) {
        Vector4 difference = value1 - value2;
        return Dot(difference, difference);
    }

    /// <summary>
    /// Restricts a vector between a min and max value.
    /// </summary>
    /// <param name="value1">The source vector.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The restricted vector.</returns>
    public static Vector4 Clamp(Vector4 value1, Vector4 min, Vector4 max) {
        // This compare order is very important!!!
        // We must follow HLSL behavior in the case user specified min value is bigger than max value.

        float x = value1.X;
        x = (x > max.X) ? max.X : x;
        x = (x < min.X) ? min.X : x;

        float y = value1.Y;
        y = (y > max.Y) ? max.Y : y;
        y = (y < min.Y) ? min.Y : y;

        float z = value1.Z;
        z = (z > max.Z) ? max.Z : z;
        z = (z < min.Z) ? min.Z : z;

        float w = value1.W;
        w = (w > max.W) ? max.W : w;
        w = (w < min.W) ? min.W : w;

        return new Vector4(x, y, z, w);
    }

    /// <summary>
    /// Linearly interpolates between two vectors based on the given weighting.
    /// </summary>
    /// <param name="value1">The first source vector.</param>
    /// <param name="value2">The second source vector.</param>
    /// <param name="amount">Value between 0 and 1 indicating the weight of the second source vector.</param>
    /// <returns>The interpolated vector.</returns>
    public static Vector4 Lerp(Vector4 value1, Vector4 value2, float amount) {
        return new Vector4(
            value1.X + (value2.X - value1.X) * amount,
            value1.Y + (value2.Y - value1.Y) * amount,
            value1.Z + (value2.Z - value1.Z) * amount,
            value1.W + (value2.W - value1.W) * amount);
    }

    /// <summary>
    /// Transforms a vector by the given matrix.
    /// </summary>
    /// <param name="position">The source vector.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns>The transformed vector.</returns>
    public static Vector4 Transform(Vector2 position, Matrix4x4 matrix) {
        return new Vector4(
            position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41,
            position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42,
            position.X * matrix.M13 + position.Y * matrix.M23 + matrix.M43,
            position.X * matrix.M14 + position.Y * matrix.M24 + matrix.M44);
    }

    /// <summary>
    /// Transforms a vector by the given matrix.
    /// </summary>
    /// <param name="position">The source vector.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns>The transformed vector.</returns>
    public static Vector4 Transform(Vector3 position, Matrix4x4 matrix) {
        return new Vector4(
            position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41,
            position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42,
            position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43,
            position.X * matrix.M14 + position.Y * matrix.M24 + position.Z * matrix.M34 + matrix.M44);
    }

    /// <summary>
    /// Transforms a vector by the given matrix.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns>The transformed vector.</returns>
    public static Vector4 Transform(Vector4 vector, Matrix4x4 matrix) {
        return new Vector4(
            vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + vector.W * matrix.M41,
            vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + vector.W * matrix.M42,
            vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + vector.W * matrix.M43,
            vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + vector.W * matrix.M44);
    }

    /// <summary>
    /// Transforms a vector by the given Quaternion rotation value.
    /// </summary>
    /// <param name="value">The source vector to be rotated.</param>
    /// <param name="rotation">The rotation to apply.</param>
    /// <returns>The transformed vector.</returns>
    public static Vector4 Transform(Vector2 value, Quaternion rotation) {
        float x2 = rotation.X + rotation.X;
        float y2 = rotation.Y + rotation.Y;
        float z2 = rotation.Z + rotation.Z;

        float wx2 = rotation.W * x2;
        float wy2 = rotation.W * y2;
        float wz2 = rotation.W * z2;
        float xx2 = rotation.X * x2;
        float xy2 = rotation.X * y2;
        float xz2 = rotation.X * z2;
        float yy2 = rotation.Y * y2;
        float yz2 = rotation.Y * z2;
        float zz2 = rotation.Z * z2;

        return new Vector4(
            value.X * (1.0f - yy2 - zz2) + value.Y * (xy2 - wz2),
            value.X * (xy2 + wz2) + value.Y * (1.0f - xx2 - zz2),
            value.X * (xz2 - wy2) + value.Y * (yz2 + wx2),
            1.0f);
    }

    /// <summary>
    /// Transforms a vector by the given Quaternion rotation value.
    /// </summary>
    /// <param name="value">The source vector to be rotated.</param>
    /// <param name="rotation">The rotation to apply.</param>
    /// <returns>The transformed vector.</returns>
    public static Vector4 Transform(Vector3 value, Quaternion rotation) {
        float x2 = rotation.X + rotation.X;
        float y2 = rotation.Y + rotation.Y;
        float z2 = rotation.Z + rotation.Z;

        float wx2 = rotation.W * x2;
        float wy2 = rotation.W * y2;
        float wz2 = rotation.W * z2;
        float xx2 = rotation.X * x2;
        float xy2 = rotation.X * y2;
        float xz2 = rotation.X * z2;
        float yy2 = rotation.Y * y2;
        float yz2 = rotation.Y * z2;
        float zz2 = rotation.Z * z2;

        return new Vector4(
            value.X * (1.0f - yy2 - zz2) + value.Y * (xy2 - wz2) + value.Z * (xz2 + wy2),
            value.X * (xy2 + wz2) + value.Y * (1.0f - xx2 - zz2) + value.Z * (yz2 - wx2),
            value.X * (xz2 - wy2) + value.Y * (yz2 + wx2) + value.Z * (1.0f - xx2 - yy2),
            1.0f);
    }

    /// <summary>
    /// Transforms a vector by the given Quaternion rotation value.
    /// </summary>
    /// <param name="value">The source vector to be rotated.</param>
    /// <param name="rotation">The rotation to apply.</param>
    /// <returns>The transformed vector.</returns>
    
    public static Vector4 Transform(Vector4 value, Quaternion rotation) {
        float x2 = rotation.X + rotation.X;
        float y2 = rotation.Y + rotation.Y;
        float z2 = rotation.Z + rotation.Z;

        float wx2 = rotation.W * x2;
        float wy2 = rotation.W * y2;
        float wz2 = rotation.W * z2;
        float xx2 = rotation.X * x2;
        float xy2 = rotation.X * y2;
        float xz2 = rotation.X * z2;
        float yy2 = rotation.Y * y2;
        float yz2 = rotation.Y * z2;
        float zz2 = rotation.Z * z2;

        return new Vector4(
            value.X * (1.0f - yy2 - zz2) + value.Y * (xy2 - wz2) + value.Z * (xz2 + wy2),
            value.X * (xy2 + wz2) + value.Y * (1.0f - xx2 - zz2) + value.Z * (yz2 - wx2),
            value.X * (xz2 - wy2) + value.Y * (yz2 + wx2) + value.Z * (1.0f - xx2 - yy2),
            value.W);
    }
    
    /// <summary>
    /// Returns a boolean indicating whether the given Vector4 is equal to this Vector4 instance.
    /// </summary>
    /// <param name="other">The Vector4 to compare this instance to.</param>
    /// <returns>True if the other Vector4 is equal to this instance; False otherwise.</returns>
    public bool Equals(Vector4 other) {
        return X == other.X
            && Y == other.Y
            && Z == other.Z
            && W == other.W;
    }

    /// <summary>
    /// Returns the dot product of two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The dot product.</returns>
    public static float Dot(Vector4 vector1, Vector4 vector2) {
        return vector1.X * vector2.X +
               vector1.Y * vector2.Y +
               vector1.Z * vector2.Z +
               vector1.W * vector2.W;
    }

    /// <summary>
    /// Returns a vector whose elements are the minimum of each of the pairs of elements in the two source vectors.
    /// </summary>
    /// <param name="value1">The first source vector.</param>
    /// <param name="value2">The second source vector.</param>
    /// <returns>The minimized vector.</returns>
    public static Vector4 Min(Vector4 value1, Vector4 value2) {
        return new Vector4(
            (value1.X < value2.X) ? value1.X : value2.X,
            (value1.Y < value2.Y) ? value1.Y : value2.Y,
            (value1.Z < value2.Z) ? value1.Z : value2.Z,
            (value1.W < value2.W) ? value1.W : value2.W);
    }

    /// <summary>
    /// Returns a vector whose elements are the maximum of each of the pairs of elements in the two source vectors.
    /// </summary>
    /// <param name="value1">The first source vector.</param>
    /// <param name="value2">The second source vector.</param>
    /// <returns>The maximized vector.</returns>
    public static Vector4 Max(Vector4 value1, Vector4 value2) {
        return new Vector4(
            (value1.X > value2.X) ? value1.X : value2.X,
            (value1.Y > value2.Y) ? value1.Y : value2.Y,
            (value1.Z > value2.Z) ? value1.Z : value2.Z,
            (value1.W > value2.W) ? value1.W : value2.W);
    }

    /// <summary>
    /// Returns a vector whose elements are the absolute values of each of the source vector's elements.
    /// </summary>
    /// <param name="value">The source vector.</param>
    /// <returns>The absolute value vector.</returns>
    public static Vector4 Abs(Vector4 value) {
        return new Vector4(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z), Math.Abs(value.W));
    }

    /// <summary>
    /// Returns a vector whose elements are the square root of each of the source vector's elements.
    /// </summary>
    /// <param name="value">The source vector.</param>
    /// <returns>The square root vector.</returns>
    public static Vector4 SquareRoot(Vector4 value) {
        return new Vector4((float) Math.Sqrt(value.X), (float) Math.Sqrt(value.Y), (float) Math.Sqrt(value.Z), (float) Math.Sqrt(value.W));
    }
    
    public static Vector4 operator +(Vector4 left, Vector4 right) {
        return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    }
    
    public static Vector4 operator -(Vector4 left, Vector4 right) {
        return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
    }
    
    public static Vector4 operator *(Vector4 left, Vector4 right) {
        return new Vector4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
    }
    
    public static Vector4 operator *(Vector4 left, float right) {
        return left * new Vector4(right);
    }
    
    public static Vector4 operator *(float left, Vector4 right) {
        return new Vector4(left) * right;
    }
    
    public static Vector4 operator /(Vector4 left, Vector4 right) {
        return new Vector4(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
    }
    
    public static Vector4 operator /(Vector4 value1, float value2) {
        float invDiv = 1.0f / value2;

        return new Vector4(
            value1.X * invDiv,
            value1.Y * invDiv,
            value1.Z * invDiv,
            value1.W * invDiv);
    }
    
    public static Vector4 operator -(Vector4 value) {
        return Zero - value;
    }
    
    public static bool operator ==(Vector4 left, Vector4 right) {
        return left.Equals(right);
    }
    
    public static bool operator !=(Vector4 left, Vector4 right) {
        return !(left == right);
    }

}
