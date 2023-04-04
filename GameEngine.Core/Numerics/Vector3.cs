//using System;
//using System.Globalization;
//using System.Text;
//using GameEngine.Core.Serialization;
//
//namespace GameEngine.Core.Numerics;
//
//public struct Vector3 : IEquatable<Vector3>, IFormattable {
//    
//    [Serialized] public float X { get; set; }
//    [Serialized] public float Y { get; set; }
//    [Serialized] public float Z { get; set; }
//    
//    public Vector3(float x, float y, float z) {
//        X = x;
//        Y = y;
//        Z = z;
//    }
//    
//    public Vector3(float value) : this(value, value, value) { }
//    
//    public static Vector3 Zero => new Vector3(0f, 0f, 0f);
//    public static Vector3 One => new Vector3(1f, 1f, 1f);
//    public static Vector3 Front => new Vector3(0f, 0f, 1f);
//    public static Vector3 Back => new Vector3(1f, 0f, -1f);
//    public static Vector3 Up => new Vector3(0f, 1f, 0f);
//    public static Vector3 Down => new Vector3(0f, -1f, 0f);
//    public static Vector3 Left => new Vector3(-1f, 0f, 0f);
//    public static Vector3 Right => new Vector3(1f, 0f, 0f);
//    
//    /// <summary>
//    /// Returns the hash code for this instance.
//    /// </summary>
//    /// <returns>The hash code.</returns>
//    public override int GetHashCode() {
//        int hash = X.GetHashCode();
//        hash = HashCodeHelper.CombineHashCodes(hash, Y.GetHashCode());
//        hash = HashCodeHelper.CombineHashCodes(hash, Z.GetHashCode());
//        return hash;
//    }
//
//    /// <summary>
//    /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
//    /// </summary>
//    /// <param name="obj">The Object to compare against.</param>
//    /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
//    public override bool Equals(object obj) {
//        if (!(obj is Vector3))
//            return false;
//        return Equals((Vector3)obj);
//    }
//
//    public override string ToString() {
//        return ToString("G", CultureInfo.CurrentCulture);
//    }
//    
//    public string ToString(string format) {
//        return ToString(format, CultureInfo.CurrentCulture);
//    }
//    
//    public string ToString(string format, IFormatProvider formatProvider) {
//        StringBuilder sb = new StringBuilder();
//        string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
//        sb.Append('<');
//        sb.Append(X.ToString(format, formatProvider));
//        sb.Append(separator);
//        sb.Append(' ');
//        sb.Append(Y.ToString(format, formatProvider));
//        sb.Append(separator);
//        sb.Append(' ');
//        sb.Append(Z.ToString(format, formatProvider));
//        sb.Append('>');
//        return sb.ToString();
//    }
//    
//    /// <summary>
//    /// Returns the length of the vector.
//    /// </summary>
//    /// <returns>The vector's length.</returns>
//    public float Magnitude => (float) Math.Sqrt(Dot(this, this));
//    
//    /// <summary>
//    /// Returns the length of the vector squared. This operation is cheaper than Length().
//    /// </summary>
//    /// <returns>The vector's length squared.</returns>
//    public float MagnitudeSquared => Dot(this, this);
//    
//    /// <summary>
//    /// Returns a vector with the same direction as the given vector, but with a length of 1.
//    /// </summary>
//    /// <returns>The normalized vector.</returns>
//    public Vector3 Normalized {
//        get {
//            float magnitude = Magnitude;
//            return magnitude == 0f ? Zero : this / magnitude;
//        }
//    }
//    
//    /// <summary>
//    /// Returns a vector with the same direction as the given vector, but with a length of 1.
//    /// </summary>
//    /// <param name="vec3">The vector to normalize.</param>
//    /// <returns>The normalized vector.</returns>
//    public static Vector3 Normalize(Vector3 vec3) {
//        return vec3.Normalized;
//    }
//
//    /// <summary>
//    /// Returns the Euclidean distance between the two given points.
//    /// </summary>
//    /// <param name="value1">The first point.</param>
//    /// <param name="value2">The second point.</param>
//    /// <returns>The distance.</returns>
//    public static float Distance(Vector3 value1, Vector3 value2) {
//        Vector3 difference = value1 - value2;
//        return (float) Math.Sqrt(Dot(difference, difference));
//    }
//
//    /// <summary>
//    /// Returns the Euclidean distance squared between the two given points.
//    /// </summary>
//    /// <param name="value1">The first point.</param>
//    /// <param name="value2">The second point.</param>
//    /// <returns>The distance squared.</returns>
//    public static float DistanceSquared(Vector3 value1, Vector3 value2) {
//        Vector3 difference = value1 - value2;
//        return Dot(difference, difference);
//    }
//
//    /// <summary>
//    /// Computes the cross product of two vectors.
//    /// </summary>
//    /// <param name="vector1">The first vector.</param>
//    /// <param name="vector2">The second vector.</param>
//    /// <returns>The cross product.</returns>
//    public static Vector3 Cross(Vector3 vector1, Vector3 vector2) {
//        return new Vector3(
//            vector1.Y * vector2.Z - vector1.Z * vector2.Y,
//            vector1.Z * vector2.X - vector1.X * vector2.Z,
//            vector1.X * vector2.Y - vector1.Y * vector2.X);
//    }
//
//    /// <summary>
//    /// Returns the reflection of a vector off a surface that has the specified normal.
//    /// </summary>
//    /// <param name="vector">The source vector.</param>
//    /// <param name="normal">The normal of the surface being reflected off.</param>
//    /// <returns>The reflected vector.</returns>
//    public static Vector3 Reflect(Vector3 vector, Vector3 normal) {
//        float dot = Vector3.Dot(vector, normal);
//        Vector3 temp = normal * dot * 2f;
//        return vector - temp;
//    }
//
//    /// <summary>
//    /// Restricts a vector between a min and max value.
//    /// </summary>
//    /// <param name="value1">The source vector.</param>
//    /// <param name="min">The minimum value.</param>
//    /// <param name="max">The maximum value.</param>
//    /// <returns>The restricted vector.</returns>
//    public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max) {
//        // This compare order is very important!!!
//        // We must follow HLSL behavior in the case user specified min value is bigger than max value.
//
//        float x = value1.X;
//        x = (x > max.X) ? max.X : x;
//        x = (x < min.X) ? min.X : x;
//
//        float y = value1.Y;
//        y = (y > max.Y) ? max.Y : y;
//        y = (y < min.Y) ? min.Y : y;
//
//        float z = value1.Z;
//        z = (z > max.Z) ? max.Z : z;
//        z = (z < min.Z) ? min.Z : z;
//
//        return new Vector3(x, y, z);
//    }
//
//    /// <summary>
//    /// Linearly interpolates between two vectors based on the given weighting.
//    /// </summary>
//    /// <param name="value1">The first source vector.</param>
//    /// <param name="value2">The second source vector.</param>
//    /// <param name="amount">Value between 0 and 1 indicating the weight of the second source vector.</param>
//    /// <returns>The interpolated vector.</returns>
//    public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount) {
//        Vector3 firstInfluence = value1 * (1f - amount);
//        Vector3 secondInfluence = value2 * amount;
//        return firstInfluence + secondInfluence;
//    }
//
//    /// <summary>
//    /// Transforms a vector by the given matrix.
//    /// </summary>
//    /// <param name="position">The source vector.</param>
//    /// <param name="matrix">The transformation matrix.</param>
//    /// <returns>The transformed vector.</returns>
//    public static Vector3 Transform(Vector3 position, Matrix4x4 matrix) {
//        return new Vector3(
//            position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41,
//            position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42,
//            position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43);
//    }
//
//    /// <summary>
//    /// Transforms a vector normal by the given matrix.
//    /// </summary>
//    /// <param name="normal">The source vector.</param>
//    /// <param name="matrix">The transformation matrix.</param>
//    /// <returns>The transformed vector.</returns>
//    public static Vector3 TransformNormal(Vector3 normal, Matrix4x4 matrix) {
//        return new Vector3(
//            normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31,
//            normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32,
//            normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33);
//    }
//
//    /// <summary>
//    /// Transforms a vector by the given Quaternion rotation value.
//    /// </summary>
//    /// <param name="value">The source vector to be rotated.</param>
//    /// <param name="rotation">The rotation to apply.</param>
//    /// <returns>The transformed vector.</returns>
//    public static Vector3 Transform(Vector3 value, Quaternion rotation) {
//        float x2 = rotation.X + rotation.X;
//        float y2 = rotation.Y + rotation.Y;
//        float z2 = rotation.Z + rotation.Z;
//
//        float wx2 = rotation.W * x2;
//        float wy2 = rotation.W * y2;
//        float wz2 = rotation.W * z2;
//        float xx2 = rotation.X * x2;
//        float xy2 = rotation.X * y2;
//        float xz2 = rotation.X * z2;
//        float yy2 = rotation.Y * y2;
//        float yz2 = rotation.Y * z2;
//        float zz2 = rotation.Z * z2;
//
//        return new Vector3(
//            value.X * (1.0f - yy2 - zz2) + value.Y * (xy2 - wz2) + value.Z * (xz2 + wy2),
//            value.X * (xy2 + wz2) + value.Y * (1.0f - xx2 - zz2) + value.Z * (yz2 - wx2),
//            value.X * (xz2 - wy2) + value.Y * (yz2 + wx2) + value.Z * (1.0f - xx2 - yy2));
//    }
//    
//    public bool Equals(Vector3 other)
//    {
//        return X == other.X &&
//               Y == other.Y &&
//               Z == other.Z;
//    }
//
//    /// <summary>
//    /// Returns the dot product of two vectors.
//    /// </summary>
//    /// <param name="vector1">The first vector.</param>
//    /// <param name="vector2">The second vector.</param>
//    /// <returns>The dot product.</returns>
//    public static float Dot(Vector3 vector1, Vector3 vector2) {
//        return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
//    }
//
//    /// <summary>
//    /// Returns a vector whose elements are the minimum of each of the pairs of elements in the two source vectors.
//    /// </summary>
//    /// <param name="value1">The first source vector.</param>
//    /// <param name="value2">The second source vector.</param>
//    /// <returns>The minimized vector.</returns>
//    public static Vector3 Min(Vector3 value1, Vector3 value2) {
//        return new Vector3(
//            (value1.X < value2.X) ? value1.X : value2.X,
//            (value1.Y < value2.Y) ? value1.Y : value2.Y,
//            (value1.Z < value2.Z) ? value1.Z : value2.Z);
//    }
//
//    /// <summary>
//    /// Returns a vector whose elements are the maximum of each of the pairs of elements in the two source vectors.
//    /// </summary>
//    /// <param name="value1">The first source vector.</param>
//    /// <param name="value2">The second source vector.</param>
//    /// <returns>The maximized vector.</returns>
//    public static Vector3 Max(Vector3 value1, Vector3 value2) {
//        return new Vector3(
//            (value1.X > value2.X) ? value1.X : value2.X,
//            (value1.Y > value2.Y) ? value1.Y : value2.Y,
//            (value1.Z > value2.Z) ? value1.Z : value2.Z);
//    }
//
//    /// <summary>
//    /// Returns a vector whose elements are the absolute values of each of the source vector's elements.
//    /// </summary>
//    /// <param name="value">The source vector.</param>
//    /// <returns>The absolute value vector.</returns>
//    public static Vector3 Abs(Vector3 value) {
//        return new Vector3(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z));
//    }
//
//    /// <summary>
//    /// Returns a vector whose elements are the square root of each of the source vector's elements.
//    /// </summary>
//    /// <param name="value">The source vector.</param>
//    /// <returns>The square root vector.</returns>
//    public static Vector3 SquareRoot(Vector3 value) {
//        return new Vector3((float) Math.Sqrt(value.X), (float) Math.Sqrt(value.Y), (float) Math.Sqrt(value.Z));
//    }
//    
//    public static Vector3 operator +(Vector3 left, Vector3 right) {
//        return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
//    }
//
//    public static Vector3 operator -(Vector3 left, Vector3 right) {
//        return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
//    }
//
//    public static Vector3 operator *(Vector3 left, Vector3 right) {
//        return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
//    }
//
//    public static Vector3 operator *(Vector3 left, float right) {
//        return left * new Vector3(right);
//    }
//
//    public static Vector3 operator *(float left, Vector3 right) {
//        return new Vector3(left) * right;
//    }
//
//    public static Vector3 operator /(Vector3 left, Vector3 right) {
//        return new Vector3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
//    }
//
//    public static Vector3 operator /(Vector3 value1, float value2) {
//        float invDiv = 1.0f / value2;
//
//        return new Vector3(
//            value1.X * invDiv,
//            value1.Y * invDiv,
//            value1.Z * invDiv);
//    }
//
//    public static Vector3 operator -(Vector3 value) {
//        return Zero - value;
//    }
//
//    public static bool operator ==(Vector3 left, Vector3 right) {
//        return (left.X == right.X &&
//                left.Y == right.Y &&
//                left.Z == right.Z);
//    }
//
//    public static bool operator !=(Vector3 left, Vector3 right) {
//        return (left.X != right.X ||
//                left.Y != right.Y ||
//                left.Z != right.Z);
//    }
//    
//}
