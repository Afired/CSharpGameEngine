using System;
using System.Globalization;
using System.Text;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Numerics;

public struct Vector2 : IEquatable<Vector2>, IFormattable {
    
    [Serialized] public float X { get; internal set; }
    [Serialized] public float Y { get; internal set; }
    
    public Vector2(float x, float y) {
        X = x;
        Y = y;
    }
    
    public Vector2(float value) : this(value, value) { }
    
    public static Vector2 Zero => new Vector2(0f, 0f);
    public static Vector2 One => new Vector2(1f, 1f);
    public static Vector2 Up => new Vector2(0f, 1f);
    public static Vector2 Down => new Vector2(0f, -1f);
    public static Vector2 Left => new Vector2(-1f, 0f);
    public static Vector2 Right => new Vector2(1f, 0f);
    
    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode() {
        int hash = X.GetHashCode();
        hash = HashCodeHelper.CombineHashCodes(hash, Y.GetHashCode());
        return hash;
    }

    /// <summary>
    /// Returns a boolean indicating whether the given Object is equal to this Vector2 instance.
    /// </summary>
    /// <param name="obj">The Object to compare against.</param>
    /// <returns>True if the Object is equal to this Vector2; False otherwise.</returns>
    public override bool Equals(object obj) {
        if (!(obj is Vector2))
            return false;
        return Equals((Vector2)obj);
    }

    /// <summary>
    /// Returns a String representing this Vector2 instance.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() {
        return ToString("G", CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns a String representing this Vector2 instance, using the specified format to format individual elements.
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
        sb.Append('>');
        return sb.ToString();
    }

    /// <summary>
    /// Returns the length of the vector.
    /// </summary>
    /// <returns>The vector's length.</returns>
    public float Magnitude => (float) Math.Sqrt(Dot(this, this));

    /// <summary>
    /// Returns the length of the vector squared. This operation is cheaper than Length().
    /// </summary>
    /// <returns>The vector's length squared.</returns>
    public float MagnitudeSquared() {
        return Dot(this, this);
    }
    
    /// <summary>
    /// Returns a vector with the same direction as the given vector, but with a length of 1.
    /// </summary>
    /// <returns>The normalized vector.</returns>
    public Vector2 Normalized {
        get {
            float magnitude = Magnitude;
            return magnitude == 0f ? Zero : this / magnitude;
        }
    }
    
    /// <summary>
    /// Returns a vector with the same direction as the given vector, but with a length of 1.
    /// </summary>
    /// <param name="vec2">The vector to normalize.</param>
    /// <returns>The normalized vector.</returns>
    public static Vector2 Normalize(Vector2 vec2) {
        return vec2.Normalized;
    }
    
    /// <summary>
    /// Returns the Euclidean distance between the two given points.
    /// </summary>
    /// <param name="value1">The first point.</param>
    /// <param name="value2">The second point.</param>
    /// <returns>The distance.</returns>
    public static float Distance(Vector2 value1, Vector2 value2) {
        Vector2 difference = value1 - value2;
        return (float) Math.Sqrt(Dot(difference, difference));
    }

    /// <summary>
    /// Returns the Euclidean distance squared between the two given points.
    /// </summary>
    /// <param name="value1">The first point.</param>
    /// <param name="value2">The second point.</param>
    /// <returns>The distance squared.</returns>
    public static float DistanceSquared(Vector2 value1, Vector2 value2) {
        Vector2 difference = value1 - value2;
        return Dot(difference, difference);
    }

    /// <summary>
    /// Returns the reflection of a vector off a surface that has the specified normal.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="normal">The normal of the surface being reflected off.</param>
    /// <returns>The reflected vector.</returns>
    public static Vector2 Reflect(Vector2 vector, Vector2 normal) {
        return vector - (2 * Dot(vector, normal) * normal);
    }

    /// <summary>
    /// Restricts a vector between a min and max value.
    /// </summary>
    /// <param name="value1">The source vector.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max) {
        // This compare order is very important!!!
        // We must follow HLSL behavior in the case user specified min value is bigger than max value.
        float x = value1.X;
        x = (x > max.X) ? max.X : x;
        x = (x < min.X) ? min.X : x;

        float y = value1.Y;
        y = (y > max.Y) ? max.Y : y;
        y = (y < min.Y) ? min.Y : y;

        return new Vector2(x, y);
    }

    /// <summary>
    /// Linearly interpolates between two vectors based on the given weighting.
    /// </summary>
    /// <param name="value1">The first source vector.</param>
    /// <param name="value2">The second source vector.</param>
    /// <param name="amount">Value between 0 and 1 indicating the weight of the second source vector.</param>
    /// <returns>The interpolated vector.</returns>
    public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount) {
        return new Vector2(
            value1.X + (value2.X - value1.X) * amount,
            value1.Y + (value2.Y - value1.Y) * amount);
    }

    /// <summary>
    /// Transforms a vector by the given matrix.
    /// </summary>
    /// <param name="position">The source vector.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns>The transformed vector.</returns>
    public static Vector2 Transform(Vector2 position, Matrix3x2 matrix) {
        return new Vector2(
            position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M31,
            position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M32);
    }

    /// <summary>
    /// Transforms a vector by the given matrix.
    /// </summary>
    /// <param name="position">The source vector.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns>The transformed vector.</returns>
    public static Vector2 Transform(Vector2 position, Matrix4x4 matrix) {
        return new Vector2(
            position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41,
            position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42);
    }

    /// <summary>
    /// Transforms a vector normal by the given matrix.
    /// </summary>
    /// <param name="normal">The source vector.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns>The transformed vector.</returns>
    public static Vector2 TransformNormal(Vector2 normal, Matrix3x2 matrix) {
        return new Vector2(
            normal.X * matrix.M11 + normal.Y * matrix.M21,
            normal.X * matrix.M12 + normal.Y * matrix.M22);
    }

    /// <summary>
    /// Transforms a vector normal by the given matrix.
    /// </summary>
    /// <param name="normal">The source vector.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns>The transformed vector.</returns>
    public static Vector2 TransformNormal(Vector2 normal, Matrix4x4 matrix) {
        return new Vector2(
            normal.X * matrix.M11 + normal.Y * matrix.M21,
            normal.X * matrix.M12 + normal.Y * matrix.M22);
    }

    /// <summary>
    /// Transforms a vector by the given Quaternion rotation value.
    /// </summary>
    /// <param name="value">The source vector to be rotated.</param>
    /// <param name="rotation">The rotation to apply.</param>
    /// <returns>The transformed vector.</returns>
    public static Vector2 Transform(Vector2 value, Quaternion rotation) {
        float x2 = rotation.X + rotation.X;
        float y2 = rotation.Y + rotation.Y;
        float z2 = rotation.Z + rotation.Z;

        float wz2 = rotation.W * z2;
        float xx2 = rotation.X * x2;
        float xy2 = rotation.X * y2;
        float yy2 = rotation.Y * y2;
        float zz2 = rotation.Z * z2;

        return new Vector2(
            value.X * (1.0f - yy2 - zz2) + value.Y * (xy2 - wz2),
            value.X * (xy2 + wz2) + value.Y * (1.0f - xx2 - zz2));
    }
    
    /// <summary>
    /// Returns a boolean indicating whether the given Vector2 is equal to this Vector2 instance.
    /// </summary>
    /// <param name="other">The Vector2 to compare this instance to.</param>
    /// <returns>True if the other Vector2 is equal to this instance; False otherwise.</returns>
    public bool Equals(Vector2 other) {
        return X == other.X && Y == other.Y;
    }

    /// <summary>
    /// Returns the dot product of two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The dot product.</returns>
    public static float Dot(Vector2 value1, Vector2 value2) {
        return value1.X * value2.X + value1.Y * value2.Y;
    }

    /// <summary>
    /// Returns a vector whose elements are the minimum of each of the pairs of elements in the two source vectors.
    /// </summary>
    /// <param name="value1">The first source vector.</param>
    /// <param name="value2">The second source vector.</param>
    /// <returns>The minimized vector.</returns>
    public static Vector2 Min(Vector2 value1, Vector2 value2) {
        return new Vector2(
            (value1.X < value2.X) ? value1.X : value2.X,
            (value1.Y < value2.Y) ? value1.Y : value2.Y);
    }

    /// <summary>
    /// Returns a vector whose elements are the maximum of each of the pairs of elements in the two source vectors
    /// </summary>
    /// <param name="value1">The first source vector</param>
    /// <param name="value2">The second source vector</param>
    /// <returns>The maximized vector</returns>
    public static Vector2 Max(Vector2 value1, Vector2 value2) {
        return new Vector2(
            (value1.X > value2.X) ? value1.X : value2.X,
            (value1.Y > value2.Y) ? value1.Y : value2.Y);
    }

    /// <summary>
    /// Returns a vector whose elements are the absolute values of each of the source vector's elements.
    /// </summary>
    /// <param name="value">The source vector.</param>
    /// <returns>The absolute value vector.</returns>
    public static Vector2 Abs(Vector2 value) {
        return new Vector2(Math.Abs(value.X), Math.Abs(value.Y));
    }

    /// <summary>
    /// Returns a vector whose elements are the square root of each of the source vector's elements.
    /// </summary>
    /// <param name="value">The source vector.</param>
    /// <returns>The square root vector.</returns>
    public static Vector2 SquareRoot(Vector2 value) {
        return new Vector2((float) Math.Sqrt(value.X), (float) Math.Sqrt(value.Y));
    }

    /// <summary>
    /// Adds two vectors together.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>The summed vector.</returns>
    public static Vector2 operator +(Vector2 left, Vector2 right) {
        return new Vector2(left.X + right.X, left.Y + right.Y);
    }

    /// <summary>
    /// Subtracts the second vector from the first.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>The difference vector.</returns>
    public static Vector2 operator -(Vector2 left, Vector2 right) {
        return new Vector2(left.X - right.X, left.Y - right.Y);
    }

    /// <summary>
    /// Multiplies two vectors together.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>The product vector.</returns>
    public static Vector2 operator *(Vector2 left, Vector2 right) {
        return new Vector2(left.X * right.X, left.Y * right.Y);
    }

    /// <summary>
    /// Multiplies a vector by the given scalar.
    /// </summary>
    /// <param name="left">The scalar value.</param>
    /// <param name="right">The source vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Vector2 operator *(float left, Vector2 right) {
        return new Vector2(left, left) * right;
    }

    /// <summary>
    /// Multiplies a vector by the given scalar.
    /// </summary>
    /// <param name="left">The source vector.</param>
    /// <param name="right">The scalar value.</param>
    /// <returns>The scaled vector.</returns>
    public static Vector2 operator *(Vector2 left, float right) {
        return left * new Vector2(right, right);
    }

    /// <summary>
    /// Divides the first vector by the second.
    /// </summary>
    /// <param name="left">The first source vector.</param>
    /// <param name="right">The second source vector.</param>
    /// <returns>The vector resulting from the division.</returns>
    public static Vector2 operator /(Vector2 left, Vector2 right) {
        return new Vector2(left.X / right.X, left.Y / right.Y);
    }

    /// <summary>
    /// Divides the vector by the given scalar.
    /// </summary>
    /// <param name="value1">The source vector.</param>
    /// <param name="value2">The scalar value.</param>
    /// <returns>The result of the division.</returns>
    public static Vector2 operator /(Vector2 value1, float value2) {
        float invDiv = 1.0f / value2;
        return new Vector2(
            value1.X * invDiv,
            value1.Y * invDiv);
    }

    /// <summary>
    /// Negates a given vector.
    /// </summary>
    /// <param name="value">The source vector.</param>
    /// <returns>The negated vector.</returns>
    public static Vector2 operator -(Vector2 value) {
        return Zero - value;
    }

    /// <summary>
    /// Returns a boolean indicating whether the two given vectors are equal.
    /// </summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns>True if the vectors are equal; False otherwise.</returns>
    public static bool operator ==(Vector2 left, Vector2 right) {
        return left.Equals(right);
    }

    /// <summary>
    /// Returns a boolean indicating whether the two given vectors are not equal.
    /// </summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns>True if the vectors are not equal; False if they are equal.</returns>
    public static bool operator !=(Vector2 left, Vector2 right) {
        return !(left == right);
    }
    
}
