using System.Diagnostics;
using System.Numerics;
using System.Text;
using GameEngine.Core.Serialization;

namespace GameEngine.Numerics;

/// <summary>
/// Describes a 3D-vector.
/// </summary>
[DebuggerDisplay("<{X}, {Y}, {Z}>")]
public struct Vec3<T> : IEquatable<Vec3<T>> where T : struct, IFloatingPointIeee754<T> {
    
    [Serialized] public T X;
    [Serialized] public T Y;
    [Serialized] public T Z;
    
    public static Vec3<T> Zero => new(T.Zero, T.Zero, T.Zero);
    public static Vec3<T> One => new(T.One, T.One, T.One);
    public static Vec3<T> Up => new(T.Zero, T.One, T.Zero);
    public static Vec3<T> Down => new(T.Zero, -T.One, T.Zero);
    public static Vec3<T> Right => new(T.One, T.Zero, T.Zero);
    public static Vec3<T> Left => new(-T.One, T.Zero, T.Zero);
    public static Vec3<T> Forward => new(T.Zero, T.Zero, -T.One);
    public static Vec3<T> Backward => new(T.Zero, T.Zero, T.One);
    
    public Vec3(T x, T y, T z) {
        X = x;
        Y = y;
        Z = z;
    }
    
    public Vec3(T value) {
        X = value;
        Y = value;
        Z = value;
    }
    
    public static Vec3<T> Add(Vec3<T> value1, Vec3<T> value2) {
        T x = value1.X + value2.X;
        T y = value1.Y + value2.Y;
        T z = value1.Z + value2.Z;
        return new Vec3<T>(x, y, z);
    }
    
    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 3d-triangle.
    /// </summary>
    /// <param name="value1">The first vector of 3d-triangle.</param>
    /// <param name="value2">The second vector of 3d-triangle.</param>
    /// <param name="value3">The third vector of 3d-triangle.</param>
    /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 3d-triangle.</param>
    /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 3d-triangle.</param>
    /// <returns>The cartesian translation of barycentric coordinates.</returns>
    public static Vec3<T> Barycentric(Vec3<T> value1, Vec3<T> value2, Vec3<T> value3, T amount1, T amount2) {
        return new Vec3<T>(
            MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
            MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2),
            MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 3d-triangle.
    /// </summary>
    /// <param name="value1">The first vector of 3d-triangle.</param>
    /// <param name="value2">The second vector of 3d-triangle.</param>
    /// <param name="value3">The third vector of 3d-triangle.</param>
    /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 3d-triangle.</param>
    /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 3d-triangle.</param>
    /// <param name="result">The cartesian translation of barycentric coordinates as an output parameter.</param>
    public static void Barycentric(ref Vec3<T> value1, ref Vec3<T> value2, ref Vec3<T> value3, T amount1,
        T amount2, out Vec3<T> result
    ) {
        result.X = MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
        result.Y = MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
        result.Z = MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2);
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains CatmullRom interpolation of the specified vectors.
    /// </summary>
    /// <param name="value1">The first vector in interpolation.</param>
    /// <param name="value2">The second vector in interpolation.</param>
    /// <param name="value3">The third vector in interpolation.</param>
    /// <param name="value4">The fourth vector in interpolation.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <returns>The result of CatmullRom interpolation.</returns>
    public static Vec3<T> CatmullRom(Vec3<T> value1, Vec3<T> value2, Vec3<T> value3, Vec3<T> value4, T amount) {
        return new Vec3<T>(
            MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
            MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount),
            MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains CatmullRom interpolation of the specified vectors.
    /// </summary>
    /// <param name="value1">The first vector in interpolation.</param>
    /// <param name="value2">The second vector in interpolation.</param>
    /// <param name="value3">The third vector in interpolation.</param>
    /// <param name="value4">The fourth vector in interpolation.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <param name="result">The result of CatmullRom interpolation as an output parameter.</param>
    public static void CatmullRom(ref Vec3<T> value1, ref Vec3<T> value2, ref Vec3<T> value3, ref Vec3<T> value4,
        T amount, out Vec3<T> result
    ) {
        result.X = MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
        result.Y = MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
        result.Z = MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount);
    }

    /// <summary>
    /// Round the members of this <see cref="Vec3{T}"/> towards positive infinity.
    /// </summary>
    public void Ceiling() {
        X = T.Ceiling(X);
        Y = T.Ceiling(Y);
        Z = T.Ceiling(Z);
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains members from another vector rounded towards positive infinity.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <returns>The rounded <see cref="Vec3{T}"/>.</returns>
    public static Vec3<T> Ceiling(Vec3<T> value) {
        value.X = T.Ceiling(value.X);
        value.Y = T.Ceiling(value.Y);
        value.Z = T.Ceiling(value.Z);
        return value;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains members from another vector rounded towards positive infinity.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="result">The rounded <see cref="Vec3{T}"/>.</param>
    public static void Ceiling(ref Vec3<T> value, out Vec3<T> result) {
        result.X = T.Ceiling(value.X);
        result.Y = T.Ceiling(value.Y);
        result.Z = T.Ceiling(value.Z);
    }

    /// <summary>
    /// Clamps the specified value within a range.
    /// </summary>
    /// <param name="value1">The value to clamp.</param>
    /// <param name="min">The min value.</param>
    /// <param name="max">The max value.</param>
    /// <returns>The clamped value.</returns>
    public static Vec3<T> Clamp(Vec3<T> value1, Vec3<T> min, Vec3<T> max) {
        return new Vec3<T>(
            T.Clamp(value1.X, min.X, max.X),
            T.Clamp(value1.Y, min.Y, max.Y),
            T.Clamp(value1.Z, min.Z, max.Z)
        );
    }

    /// <summary>
    /// Clamps the specified value within a range.
    /// </summary>
    /// <param name="value1">The value to clamp.</param>
    /// <param name="min">The min value.</param>
    /// <param name="max">The max value.</param>
    /// <param name="result">The clamped value as an output parameter.</param>
    public static void Clamp(ref Vec3<T> value1, ref Vec3<T> min, ref Vec3<T> max, out Vec3<T> result) {
        result.X = T.Clamp(value1.X, min.X, max.X);
        result.Y = T.Clamp(value1.Y, min.Y, max.Y);
        result.Z = T.Clamp(value1.Z, min.Z, max.Z);
    }

    /// <summary>
    /// Computes the cross product of two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The cross product of two vectors.</returns>
    public static Vec3<T> Cross(Vec3<T> vector1, Vec3<T> vector2) {
        Cross(ref vector1, ref vector2, out vector1);
        return vector1;
    }

    /// <summary>
    /// Computes the cross product of two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <param name="result">The cross product of two vectors as an output parameter.</param>
    public static void Cross(ref Vec3<T> vector1, ref Vec3<T> vector2, out Vec3<T> result) {
        var x = vector1.Y * vector2.Z - vector2.Y * vector1.Z;
        var y = -(vector1.X * vector2.Z - vector2.X * vector1.Z);
        var z = vector1.X * vector2.Y - vector2.X * vector1.Y;
        result.X = x;
        result.Y = y;
        result.Z = z;
    }

    /// <summary>
    /// Returns the distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The distance between two vectors.</returns>
    public static T Distance(Vec3<T> value1, Vec3<T> value2) {
        DistanceSquared(ref value1, ref value2, out T result);
        return T.Sqrt(result);
    }
    
    /// <summary>
    /// Returns the distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The distance between two vectors as an output parameter.</param>
    public static void Distance(ref Vec3<T> value1, ref Vec3<T> value2, out T result) {
        DistanceSquared(ref value1, ref value2, out result);
        result = T.Sqrt(result);
    }
    
    /// <summary>
    /// Returns the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The squared distance between two vectors.</returns>
    public static T DistanceSquared(Vec3<T> value1, Vec3<T> value2) {
        return (value1.X - value2.X) * (value1.X - value2.X) +
               (value1.Y - value2.Y) * (value1.Y - value2.Y) +
               (value1.Z - value2.Z) * (value1.Z - value2.Z);
    }

    /// <summary>
    /// Returns the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The squared distance between two vectors as an output parameter.</param>
    public static void DistanceSquared(ref Vec3<T> value1, ref Vec3<T> value2, out T result) {
        result = (value1.X - value2.X) * (value1.X - value2.X) +
                 (value1.Y - value2.Y) * (value1.Y - value2.Y) +
                 (value1.Z - value2.Z) * (value1.Z - value2.Z);
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec3{T}"/> by the components of another <see cref="Vec3{T}"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="value2">Divisor <see cref="Vec3{T}"/>.</param>
    /// <returns>The result of dividing the vectors.</returns>
    public static Vec3<T> Divide(Vec3<T> value1, Vec3<T> value2) {
        value1.X /= value2.X;
        value1.Y /= value2.Y;
        value1.Z /= value2.Z;
        return value1;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec3{T}"/> by a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="divider">Divisor scalar.</param>
    /// <returns>The result of dividing a vector by a scalar.</returns>
    public static Vec3<T> Divide(Vec3<T> value1, T divider) {
        T factor = T.One / divider;
        value1.X *= factor;
        value1.Y *= factor;
        value1.Z *= factor;
        return value1;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec3{T}"/> by a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="divider">Divisor scalar.</param>
    /// <param name="result">The result of dividing a vector by a scalar as an output parameter.</param>
    public static void Divide(ref Vec3<T> value1, T divider, out Vec3<T> result) {
        var factor = T.One / divider;
        result.X = value1.X * factor;
        result.Y = value1.Y * factor;
        result.Z = value1.Z * factor;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec3{T}"/> by the components of another <see cref="Vec3{T}"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="value2">Divisor <see cref="Vec3{T}"/>.</param>
    /// <param name="result">The result of dividing the vectors as an output parameter.</param>
    public static void Divide(ref Vec3<T> value1, ref Vec3<T> value2, out Vec3<T> result) {
        result.X = value1.X / value2.X;
        result.Y = value1.Y / value2.Y;
        result.Z = value1.Z / value2.Z;
    }

    /// <summary>
    /// Returns a dot product of two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The dot product of two vectors.</returns>
    public static T Dot(Vec3<T> value1, Vec3<T> value2) {
        return value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z;
    }

    /// <summary>
    /// Returns a dot product of two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The dot product of two vectors as an output parameter.</param>
    public static void Dot(ref Vec3<T> value1, ref Vec3<T> value2, out T result) {
        result = value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z;
    }

    /// <summary>
    /// Compares whether current instance is equal to specified <see cref="Object"/>.
    /// </summary>
    /// <param name="obj">The <see cref="Object"/> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public override bool Equals(object? obj) {
        if(obj is not Vec3<T> other) return false;
        
        return X == other.X &&
               Y == other.Y &&
               Z == other.Z;
    }

    /// <summary>
    /// Compares whether current instance is equal to specified <see cref="Vec3{T}"/>.
    /// </summary>
    /// <param name="other">The <see cref="Vec3{T}"/> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public bool Equals(Vec3<T> other) {
        return X == other.X &&
               Y == other.Y &&
               Z == other.Z;
    }

    /// <summary>
    /// Round the members of this <see cref="Vec3{T}"/> towards negative infinity.
    /// </summary>
    public void Floor() {
        X = T.Floor(X);
        Y = T.Floor(Y);
        Z = T.Floor(Z);
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains members from another vector rounded towards negative infinity.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <returns>The rounded <see cref="Vec3{T}"/>.</returns>
    public static Vec3<T> Floor(Vec3<T> value) {
        value.X = T.Floor(value.X);
        value.Y = T.Floor(value.Y);
        value.Z = T.Floor(value.Z);
        return value;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains members from another vector rounded towards negative infinity.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="result">The rounded <see cref="Vec3{T}"/>.</param>
    public static void Floor(ref Vec3<T> value, out Vec3<T> result) {
        result.X = T.Floor(value.X);
        result.Y = T.Floor(value.Y);
        result.Z = T.Floor(value.Z);
    }

    /// <summary>
    /// Gets the hash code of this <see cref="Vec3{T}"/>.
    /// </summary>
    /// <returns>Hash code of this <see cref="Vec3{T}"/>.</returns>
    public override int GetHashCode() {
        unchecked {
            var hashCode = X.GetHashCode();
            hashCode = (hashCode * 397) ^ Y.GetHashCode();
            hashCode = (hashCode * 397) ^ Z.GetHashCode();
            return hashCode;
        }
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains hermite spline interpolation.
    /// </summary>
    /// <param name="value1">The first position vector.</param>
    /// <param name="tangent1">The first tangent vector.</param>
    /// <param name="value2">The second position vector.</param>
    /// <param name="tangent2">The second tangent vector.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <returns>The hermite spline interpolation vector.</returns>
    public static Vec3<T> Hermite(Vec3<T> value1, Vec3<T> tangent1, Vec3<T> value2, Vec3<T> tangent2, T amount) {
        return new Vec3<T>(MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount),
            MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount),
            MathHelper.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains hermite spline interpolation.
    /// </summary>
    /// <param name="value1">The first position vector.</param>
    /// <param name="tangent1">The first tangent vector.</param>
    /// <param name="value2">The second position vector.</param>
    /// <param name="tangent2">The second tangent vector.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <param name="result">The hermite spline interpolation vector as an output parameter.</param>
    public static void Hermite(ref Vec3<T> value1, ref Vec3<T> tangent1, ref Vec3<T> value2, ref Vec3<T> tangent2,
        T amount, out Vec3<T> result
    ) {
        result.X = MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
        result.Y = MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
        result.Z = MathHelper.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount);
    }

    /// <summary>
    /// Returns the length of this <see cref="Vec3{T}"/>.
    /// </summary>
    /// <returns>The length of this <see cref="Vec3{T}"/>.</returns>
    public T Length() {
        return T.Sqrt(X * X + Y * Y + Z * Z);
    }

    /// <summary>
    /// Returns the squared length of this <see cref="Vec3{T}"/>.
    /// </summary>
    /// <returns>The squared length of this <see cref="Vec3{T}"/>.</returns>
    public T LengthSquared() {
        return X * X + Y * Y + Z * Z;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains linear interpolation of the specified vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
    /// <returns>The result of linear interpolation of the specified vectors.</returns>
    public static Vec3<T> Lerp(Vec3<T> value1, Vec3<T> value2, T amount) {
        return new Vec3<T>(
            MathHelper.Lerp<T>(value1.X, value2.X, amount),
            MathHelper.Lerp<T>(value1.Y, value2.Y, amount),
            MathHelper.Lerp<T>(value1.Z, value2.Z, amount)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains linear interpolation of the specified vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
    /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
    public static void Lerp(ref Vec3<T> value1, ref Vec3<T> value2, T amount, out Vec3<T> result) {
        result.X = MathHelper.Lerp<T>(value1.X, value2.X, amount);
        result.Y = MathHelper.Lerp<T>(value1.Y, value2.Y, amount);
        result.Z = MathHelper.Lerp<T>(value1.Z, value2.Z, amount);
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains linear interpolation of the specified vectors.
    /// Uses <see cref="MathHelper.LerpPrecise{T}"/> on MathHelper for the interpolation.
    /// Less efficient but more precise compared to <see cref="Vec3{T}.Lerp(Vec3{T}, Vec3{T}, T)"/>.
    /// See remarks section of <see cref="MathHelper.LerpPrecise{T}"/> on MathHelper for more info.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
    /// <returns>The result of linear interpolation of the specified vectors.</returns>
    public static Vec3<T> LerpPrecise(Vec3<T> value1, Vec3<T> value2, T amount) {
        return new Vec3<T>(
            MathHelper.LerpPrecise<T>(value1.X, value2.X, amount),
            MathHelper.LerpPrecise<T>(value1.Y, value2.Y, amount),
            MathHelper.LerpPrecise<T>(value1.Z, value2.Z, amount)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains linear interpolation of the specified vectors.
    /// Uses <see cref="MathHelper.LerpPrecise{T}"/> on MathHelper for the interpolation.
    /// Less efficient but more precise compared to <see cref="Vec3{T}.Lerp(ref Vec3{T}, ref Vec3{T}, T, out Vec3{T})"/>.
    /// See remarks section of <see cref="MathHelper.LerpPrecise{T}"/> on MathHelper for more info.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
    /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
    public static void LerpPrecise(ref Vec3<T> value1, ref Vec3<T> value2, T amount, out Vec3<T> result) {
        result.X = MathHelper.LerpPrecise<T>(value1.X, value2.X, amount);
        result.Y = MathHelper.LerpPrecise<T>(value1.Y, value2.Y, amount);
        result.Z = MathHelper.LerpPrecise<T>(value1.Z, value2.Z, amount);
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a maximal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The <see cref="Vec3{T}"/> with maximal values from the two vectors.</returns>
    public static Vec3<T> Max(Vec3<T> value1, Vec3<T> value2) {
        return new Vec3<T>(
            MathHelper.Max<T>(value1.X, value2.X),
            MathHelper.Max<T>(value1.Y, value2.Y),
            MathHelper.Max<T>(value1.Z, value2.Z)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a maximal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The <see cref="Vec3{T}"/> with maximal values from the two vectors as an output parameter.</param>
    public static void Max(ref Vec3<T> value1, ref Vec3<T> value2, out Vec3<T> result) {
        result.X = MathHelper.Max<T>(value1.X, value2.X);
        result.Y = MathHelper.Max<T>(value1.Y, value2.Y);
        result.Z = MathHelper.Max<T>(value1.Z, value2.Z);
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a minimal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The <see cref="Vec3{T}"/> with minimal values from the two vectors.</returns>
    public static Vec3<T> Min(Vec3<T> value1, Vec3<T> value2) {
        return new Vec3<T>(
            MathHelper.Min<T>(value1.X, value2.X),
            MathHelper.Min<T>(value1.Y, value2.Y),
            MathHelper.Min<T>(value1.Z, value2.Z)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a minimal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The <see cref="Vec3{T}"/> with minimal values from the two vectors as an output parameter.</param>
    public static void Min(ref Vec3<T> value1, ref Vec3<T> value2, out Vec3<T> result) {
        result.X = MathHelper.Min<T>(value1.X, value2.X);
        result.Y = MathHelper.Min<T>(value1.Y, value2.Y);
        result.Z = MathHelper.Min<T>(value1.Z, value2.Z);
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a multiplication of two vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="value2">Source <see cref="Vec3{T}"/>.</param>
    /// <returns>The result of the vector multiplication.</returns>
    public static Vec3<T> Multiply(Vec3<T> value1, Vec3<T> value2) {
        value1.X *= value2.X;
        value1.Y *= value2.Y;
        value1.Z *= value2.Z;
        return value1;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a multiplication of <see cref="Vec3{T}"/> and a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="scaleFactor">Scalar value.</param>
    /// <returns>The result of the vector multiplication with a scalar.</returns>
    public static Vec3<T> Multiply(Vec3<T> value1, T scaleFactor) {
        value1.X *= scaleFactor;
        value1.Y *= scaleFactor;
        value1.Z *= scaleFactor;
        return value1;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a multiplication of <see cref="Vec3{T}"/> and a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="scaleFactor">Scalar value.</param>
    /// <param name="result">The result of the multiplication with a scalar as an output parameter.</param>
    public static void Multiply(ref Vec3<T> value1, T scaleFactor, out Vec3<T> result) {
        result.X = value1.X * scaleFactor;
        result.Y = value1.Y * scaleFactor;
        result.Z = value1.Z * scaleFactor;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a multiplication of two vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="value2">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="result">The result of the vector multiplication as an output parameter.</param>
    public static void Multiply(ref Vec3<T> value1, ref Vec3<T> value2, out Vec3<T> result) {
        result.X = value1.X * value2.X;
        result.Y = value1.Y * value2.Y;
        result.Z = value1.Z * value2.Z;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains the specified vector inversion.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <returns>The result of the vector inversion.</returns>
    public static Vec3<T> Negate(Vec3<T> value) {
        value = new Vec3<T>(-value.X, -value.Y, -value.Z);
        return value;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains the specified vector inversion.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="result">The result of the vector inversion as an output parameter.</param>
    public static void Negate(ref Vec3<T> value, out Vec3<T> result) {
        result.X = -value.X;
        result.Y = -value.Y;
        result.Z = -value.Z;
    }
    
    public Vec3<T> Normalized() {
        T factor = T.Sqrt(X * X + Y * Y + Z * Z);
        factor = T.One / factor;
        return new Vec3<T>(X * factor, Y * factor, Z * factor);
    }
    
    /// <summary>
    /// Turns this <see cref="Vec3{T}"/> to a unit vector with the same direction.
    /// </summary>
    public void Normalize() {
        T factor = T.Sqrt(X * X + Y * Y + Z * Z);
        factor = T.One / factor;
        X *= factor;
        Y *= factor;
        Z *= factor;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a normalized values from another vector.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <returns>Unit vector.</returns>
    public static Vec3<T> Normalize(Vec3<T> value) {
        var factor = T.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
        factor = T.One / factor;
        return new Vec3<T>(value.X * factor, value.Y * factor, value.Z * factor);
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a normalized values from another vector.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="result">Unit vector as an output parameter.</param>
    public static void Normalize(ref Vec3<T> value, out Vec3<T> result) {
        var factor = T.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
        factor = T.One / factor;
        result.X = value.X * factor;
        result.Y = value.Y * factor;
        result.Z = value.Z * factor;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains reflect vector of the given vector and normal.
    /// </summary>
    /// <param name="vector">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="normal">Reflection normal.</param>
    /// <returns>Reflected vector.</returns>
    public static Vec3<T> Reflect(Vec3<T> vector, Vec3<T> normal) {
        // I is the original array
        // N is the normal of the incident plane
        // R = I - (2 * N * ( DotProduct[ I,N] ))
        Vec3<T> reflectedVector;
        // inline the dotProduct here instead of calling method
        var dotProduct = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
        reflectedVector.X = vector.X - T.CreateChecked(2) * normal.X * dotProduct;
        reflectedVector.Y = vector.Y - T.CreateChecked(2) * normal.Y * dotProduct;
        reflectedVector.Z = vector.Z - T.CreateChecked(2) * normal.Z * dotProduct;

        return reflectedVector;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains reflect vector of the given vector and normal.
    /// </summary>
    /// <param name="vector">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="normal">Reflection normal.</param>
    /// <param name="result">Reflected vector as an output parameter.</param>
    public static void Reflect(ref Vec3<T> vector, ref Vec3<T> normal, out Vec3<T> result) {
        // I is the original array
        // N is the normal of the incident plane
        // R = I - (2 * N * ( DotProduct[ I,N] ))

        // inline the dotProduct here instead of calling method
        var dotProduct = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
        result.X = vector.X - T.CreateChecked(2) * normal.X * dotProduct;
        result.Y = vector.Y - T.CreateChecked(2) * normal.Y * dotProduct;
        result.Z = vector.Z - T.CreateChecked(2) * normal.Z * dotProduct;
    }

    /// <summary>
    /// Round the members of this <see cref="Vec3{T}"/> towards the nearest integer value.
    /// </summary>
    public void Round() {
        X = T.Round(X);
        Y = T.Round(Y);
        Z = T.Round(Z);
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains members from another vector rounded to the nearest integer value.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <returns>The rounded <see cref="Vec3{T}"/>.</returns>
    public static Vec3<T> Round(Vec3<T> value) {
        value.X = T.Round(value.X);
        value.Y = T.Round(value.Y);
        value.Z = T.Round(value.Z);
        return value;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains members from another vector rounded to the nearest integer value.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="result">The rounded <see cref="Vec3{T}"/>.</param>
    public static void Round(ref Vec3<T> value, out Vec3<T> result) {
        result.X = T.Round(value.X);
        result.Y = T.Round(value.Y);
        result.Z = T.Round(value.Z);
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains cubic interpolation of the specified vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="value2">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="amount">Weighting value.</param>
    /// <returns>Cubic interpolation of the specified vectors.</returns>
    public static Vec3<T> SmoothStep(Vec3<T> value1, Vec3<T> value2, T amount) {
        return new Vec3<T>(
            MathHelper.SmoothStep(value1.X, value2.X, amount),
            MathHelper.SmoothStep(value1.Y, value2.Y, amount),
            MathHelper.SmoothStep(value1.Z, value2.Z, amount)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains cubic interpolation of the specified vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="value2">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="amount">Weighting value.</param>
    /// <param name="result">Cubic interpolation of the specified vectors as an output parameter.</param>
    public static void SmoothStep(ref Vec3<T> value1, ref Vec3<T> value2, T amount, out Vec3<T> result) {
        result.X = MathHelper.SmoothStep(value1.X, value2.X, amount);
        result.Y = MathHelper.SmoothStep(value1.Y, value2.Y, amount);
        result.Z = MathHelper.SmoothStep(value1.Z, value2.Z, amount);
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains subtraction of on <see cref="Vec3{T}"/> from a another.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="value2">Source <see cref="Vec3{T}"/>.</param>
    /// <returns>The result of the vector subtraction.</returns>
    public static Vec3<T> Subtract(Vec3<T> value1, Vec3<T> value2) {
        value1.X -= value2.X;
        value1.Y -= value2.Y;
        value1.Z -= value2.Z;
        return value1;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains subtraction of on <see cref="Vec3{T}"/> from a another.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="value2">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="result">The result of the vector subtraction as an output parameter.</param>
    public static void Subtract(ref Vec3<T> value1, ref Vec3<T> value2, out Vec3<T> result) {
        result.X = value1.X - value2.X;
        result.Y = value1.Y - value2.Y;
        result.Z = value1.Z - value2.Z;
    }

    /// <summary>
    /// Returns a <see cref="String"/> representation of this <see cref="Vec3{T}"/> in the format:
    /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>] Z:[<see cref="Z"/>]}
    /// </summary>
    /// <returns>A <see cref="String"/> representation of this <see cref="Vec3{T}"/>.</returns>
    public override string ToString() {
        var sb = new StringBuilder(32);
        sb.Append("{X:");
        sb.Append(X);
        sb.Append(" Y:");
        sb.Append(Y);
        sb.Append(" Z:");
        sb.Append(Z);
        sb.Append("}");
        return sb.ToString();
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a transformation of 3d-vector by the specified <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="position">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <returns>Transformed <see cref="Vec3{T}"/>.</returns>
    public static Vec3<T> Transform(Vec3<T> position, Matrix<T> matrix) {
        Transform(ref position, ref matrix, out position);
        return position;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a transformation of 3d-vector by the specified <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="position">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <param name="result">Transformed <see cref="Vec3{T}"/> as an output parameter.</param>
    public static void Transform(ref Vec3<T> position, ref Matrix<T> matrix, out Vec3<T> result) {
        var x = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
        var y = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
        var z = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;
        result.X = x;
        result.Y = y;
        result.Z = z;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a transformation of 3d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
    /// <returns>Transformed <see cref="Vec3{T}"/>.</returns>
    public static Vec3<T> Transform(Vec3<T> value, Quaternion<T> rotation) {
        Vec3<T> result;
        Transform(ref value, ref rotation, out result);
        return result;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a transformation of 3d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
    /// <param name="result">Transformed <see cref="Vec3{T}"/> as an output parameter.</param>
    public static void Transform(ref Vec3<T> value, ref Quaternion<T> rotation, out Vec3<T> result) {
        T x = T.CreateChecked(2) * (rotation.Y * value.Z - rotation.Z * value.Y);
        T y = T.CreateChecked(2) * (rotation.Z * value.X - rotation.X * value.Z);
        T z = T.CreateChecked(2) * (rotation.X * value.Y - rotation.Y * value.X);

        result.X = value.X + x * rotation.W + (rotation.Y * z - rotation.Z * y);
        result.Y = value.Y + y * rotation.W + (rotation.Z * x - rotation.X * z);
        result.Z = value.Z + z * rotation.W + (rotation.X * y - rotation.Y * x);
    }

    /// <summary>
    /// Apply transformation on vectors within array of <see cref="Vec3{T}"/> by the specified <see cref="Matrix{T}"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <param name="destinationArray">Destination array.</param>
    /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vec3{T}"/> should be written.</param>
    /// <param name="length">The number of vectors to be transformed.</param>
    public static void Transform(Vec3<T>[] sourceArray, int sourceIndex, ref Matrix<T> matrix, Vec3<T>[] destinationArray,
        int destinationIndex, int length
    ) {
        if(sourceArray == null) throw new ArgumentNullException(nameof(sourceArray));
        if(destinationArray == null) throw new ArgumentNullException(nameof(destinationArray));
        if(sourceArray.Length < sourceIndex + length)
            throw new ArgumentException("Source array length is lesser than sourceIndex + length");
        if(destinationArray.Length < destinationIndex + length)
            throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

        // TODO: Are there options on some platforms to implement a vectorized version of this?

        for(var i = 0; i < length; i++) {
            Vec3<T> position = sourceArray[sourceIndex + i];
            destinationArray[destinationIndex + i] =
                new Vec3<T>(
                    position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41,
                    position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42,
                    position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43
                );
        }
    }

    /// <summary>
    /// Apply transformation on vectors within array of <see cref="Vec3{T}"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
    /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
    /// <param name="destinationArray">Destination array.</param>
    /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vec3{T}"/> should be written.</param>
    /// <param name="length">The number of vectors to be transformed.</param>
    public static void Transform(Vec3<T>[] sourceArray, int sourceIndex, ref Quaternion<T> rotation,
        Vec3<T>[] destinationArray, int destinationIndex, int length
    ) {
        if(sourceArray == null) throw new ArgumentNullException(nameof(sourceArray));
        if(destinationArray == null) throw new ArgumentNullException(nameof(destinationArray));
        if(sourceArray.Length < sourceIndex + length)
            throw new ArgumentException("Source array length is lesser than sourceIndex + length");
        if(destinationArray.Length < destinationIndex + length)
            throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

        // TODO: Are there options on some platforms to implement a vectorized version of this?

        for(var i = 0; i < length; i++) {
            var position = sourceArray[sourceIndex + i];

            T x = T.CreateChecked(2) * (rotation.Y * position.Z - rotation.Z * position.Y);
            T y = T.CreateChecked(2) * (rotation.Z * position.X - rotation.X * position.Z);
            T z = T.CreateChecked(2) * (rotation.X * position.Y - rotation.Y * position.X);

            destinationArray[destinationIndex + i] =
                new Vec3<T>(
                    position.X + x * rotation.W + (rotation.Y * z - rotation.Z * y),
                    position.Y + y * rotation.W + (rotation.Z * x - rotation.X * z),
                    position.Z + z * rotation.W + (rotation.X * y - rotation.Y * x)
                );
        }
    }

    /// <summary>
    /// Apply transformation on all vectors within array of <see cref="Vec3{T}"/> by the specified <see cref="Matrix{T}"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <param name="destinationArray">Destination array.</param>
    public static void Transform(Vec3<T>[] sourceArray, ref Matrix<T> matrix, Vec3<T>[] destinationArray) {
        if(sourceArray == null) throw new ArgumentNullException(nameof(sourceArray));
        if(destinationArray == null) throw new ArgumentNullException(nameof(destinationArray));
        if(destinationArray.Length < sourceArray.Length)
            throw new ArgumentException("Destination array length is lesser than source array length");

        // TODO: Are there options on some platforms to implement a vectorized version of this?

        for(var i = 0; i < sourceArray.Length; i++) {
            Vec3<T> position = sourceArray[i];
            destinationArray[i] =
                new Vec3<T>(
                    position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41,
                    position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42,
                    position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43
                );
        }
    }

    /// <summary>
    /// Apply transformation on all vectors within array of <see cref="Vec3{T}"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
    /// <param name="destinationArray">Destination array.</param>
    public static void Transform(Vec3<T>[] sourceArray, ref Quaternion<T> rotation, Vec3<T>[] destinationArray) {
        if(sourceArray == null) throw new ArgumentNullException(nameof(sourceArray));
        if(destinationArray == null) throw new ArgumentNullException(nameof(destinationArray));
        if(destinationArray.Length < sourceArray.Length)
            throw new ArgumentException("Destination array length is lesser than source array length");

        // TODO: Are there options on some platforms to implement a vectorized version of this?

        for(var i = 0; i < sourceArray.Length; i++) {
            Vec3<T> position = sourceArray[i];

            T x = T.CreateChecked(2) * (rotation.Y * position.Z - rotation.Z * position.Y);
            T y = T.CreateChecked(2) * (rotation.Z * position.X - rotation.X * position.Z);
            T z = T.CreateChecked(2) * (rotation.X * position.Y - rotation.Y * position.X);

            destinationArray[i] =
                new Vec3<T>(
                    position.X + x * rotation.W + (rotation.Y * z - rotation.Z * y),
                    position.Y + y * rotation.W + (rotation.Z * x - rotation.X * z),
                    position.Z + z * rotation.W + (rotation.X * y - rotation.Y * x)
                );
        }
    }
    
    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a transformation of the specified normal by the specified <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="normal">Source <see cref="Vec3{T}"/> which represents a normal vector.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <returns>Transformed normal.</returns>
    public static Vec3<T> TransformNormal(Vec3<T> normal, Matrix<T> matrix) {
        TransformNormal(ref normal, ref matrix, out normal);
        return normal;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3{T}"/> that contains a transformation of the specified normal by the specified <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="normal">Source <see cref="Vec3{T}"/> which represents a normal vector.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <param name="result">Transformed normal as an output parameter.</param>
    public static void TransformNormal(ref Vec3<T> normal, ref Matrix<T> matrix, out Vec3<T> result) {
        T x = normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31;
        T y = normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32;
        T z = normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33;
        result.X = x;
        result.Y = y;
        result.Z = z;
    }

    /// <summary>
    /// Apply transformation on normals within array of <see cref="Vec3{T}"/> by the specified <see cref="Matrix{T}"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <param name="destinationArray">Destination array.</param>
    /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vec3{T}"/> should be written.</param>
    /// <param name="length">The number of normals to be transformed.</param>
    public static void TransformNormal(Vec3<T>[] sourceArray, int sourceIndex, ref Matrix<T> matrix, Vec3<T>[] destinationArray, int destinationIndex, int length) {
        if(sourceArray == null) throw new ArgumentNullException(nameof(sourceArray));
        if(destinationArray == null) throw new ArgumentNullException(nameof(destinationArray));
        if(sourceArray.Length < sourceIndex + length)
            throw new ArgumentException("Source array length is lesser than sourceIndex + length");
        if(destinationArray.Length < destinationIndex + length)
            throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

        for(var x = 0; x < length; x++) {
            var normal = sourceArray[sourceIndex + x];

            destinationArray[destinationIndex + x] =
                new Vec3<T>(
                    normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31,
                    normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32,
                    normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33
                );
        }
    }

    /// <summary>
    /// Apply transformation on all normals within array of <see cref="Vec3{T}"/> by the specified <see cref="Matrix{T}"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <param name="destinationArray">Destination array.</param>
    public static void TransformNormal(Vec3<T>[] sourceArray, ref Matrix<T> matrix, Vec3<T>[] destinationArray) {
        if(sourceArray == null) throw new ArgumentNullException(nameof(sourceArray));
        if(destinationArray == null) throw new ArgumentNullException(nameof(destinationArray));
        if(destinationArray.Length < sourceArray.Length)
            throw new ArgumentException("Destination array length is lesser than source array length");

        for(var i = 0; i < sourceArray.Length; i++) {
            Vec3<T> normal = sourceArray[i];

            destinationArray[i] =
                new Vec3<T>(
                    normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31,
                    normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32,
                    normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33
                );
        }
    }
    
    /// <summary>
    /// Deconstruction method for <see cref="Vec3{T}"/>.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void Deconstruct(out T x, out T y, out T z) {
        x = X;
        y = Y;
        z = Z;
    }

    /// <summary>
    /// Returns a <see cref="System.Numerics.Vector3"/>.
    /// </summary>
    public Vector3 ToNumerics() {
        return new Vector3(float.CreateChecked(X), float.CreateChecked(Y), float.CreateChecked(Z));
    }

    /// <summary>
    /// Converts a <see cref="System.Numerics.Vector3"/> to a <see cref="Vec3{T}"/>.
    /// </summary>
    /// <param name="value">The converted value.</param>
    public static implicit operator Vec3<T>(Vector3 value) {
        return new Vec3<T>(T.CreateChecked(value.X), T.CreateChecked(value.Y), T.CreateChecked(value.Z));
    }

    /// <summary>
    /// Compares whether two <see cref="Vec3{T}"/> instances are equal.
    /// </summary>
    /// <param name="value1"><see cref="Vec3{T}"/> instance on the left of the equal sign.</param>
    /// <param name="value2"><see cref="Vec3{T}"/> instance on the right of the equal sign.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public static bool operator ==(Vec3<T> value1, Vec3<T> value2) {
        return value1.X == value2.X
               && value1.Y == value2.Y
               && value1.Z == value2.Z;
    }
    
    /// <summary>
    /// Compares whether two <see cref="Vec3{T}"/> instances are not equal.
    /// </summary>
    /// <param name="value1"><see cref="Vec3{T}"/> instance on the left of the not equal sign.</param>
    /// <param name="value2"><see cref="Vec3{T}"/> instance on the right of the not equal sign.</param>
    /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>	
    public static bool operator !=(Vec3<T> value1, Vec3<T> value2) {
        return !(value1 == value2);
    }

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/> on the left of the add sign.</param>
    /// <param name="value2">Source <see cref="Vec3{T}"/> on the right of the add sign.</param>
    /// <returns>Sum of the vectors.</returns>
    public static Vec3<T> operator +(Vec3<T> value1, Vec3<T> value2) {
        value1.X += value2.X;
        value1.Y += value2.Y;
        value1.Z += value2.Z;
        return value1;
    }

    /// <summary>
    /// Inverts values in the specified <see cref="Vec3{T}"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/> on the right of the sub sign.</param>
    /// <returns>Result of the inversion.</returns>
    public static Vec3<T> operator -(Vec3<T> value) {
        value = new Vec3<T>(-value.X, -value.Y, -value.Z);
        return value;
    }

    /// <summary>
    /// Subtracts a <see cref="Vec3{T}"/> from a <see cref="Vec3{T}"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/> on the left of the sub sign.</param>
    /// <param name="value2">Source <see cref="Vec3{T}"/> on the right of the sub sign.</param>
    /// <returns>Result of the vector subtraction.</returns>
    public static Vec3<T> operator -(Vec3<T> value1, Vec3<T> value2) {
        value1.X -= value2.X;
        value1.Y -= value2.Y;
        value1.Z -= value2.Z;
        return value1;
    }

    /// <summary>
    /// Multiplies the components of two vectors by each other.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/> on the left of the mul sign.</param>
    /// <param name="value2">Source <see cref="Vec3{T}"/> on the right of the mul sign.</param>
    /// <returns>Result of the vector multiplication.</returns>
    public static Vec3<T> operator *(Vec3<T> value1, Vec3<T> value2) {
        value1.X *= value2.X;
        value1.Y *= value2.Y;
        value1.Z *= value2.Z;
        return value1;
    }

    /// <summary>
    /// Multiplies the components of vector by a scalar.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/> on the left of the mul sign.</param>
    /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
    /// <returns>Result of the vector multiplication with a scalar.</returns>
    public static Vec3<T> operator *(Vec3<T> value, T scaleFactor) {
        value.X *= scaleFactor;
        value.Y *= scaleFactor;
        value.Z *= scaleFactor;
        return value;
    }

    /// <summary>
    /// Multiplies the components of vector by a scalar.
    /// </summary>
    /// <param name="scaleFactor">Scalar value on the left of the mul sign.</param>
    /// <param name="value">Source <see cref="Vec3{T}"/> on the right of the mul sign.</param>
    /// <returns>Result of the vector multiplication with a scalar.</returns>
    public static Vec3<T> operator *(T scaleFactor, Vec3<T> value) {
        value.X *= scaleFactor;
        value.Y *= scaleFactor;
        value.Z *= scaleFactor;
        return value;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec3{T}"/> by the components of another <see cref="Vec3{T}"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/> on the left of the div sign.</param>
    /// <param name="value2">Divisor <see cref="Vec3{T}"/> on the right of the div sign.</param>
    /// <returns>The result of dividing the vectors.</returns>
    public static Vec3<T> operator /(Vec3<T> value1, Vec3<T> value2) {
        value1.X /= value2.X;
        value1.Y /= value2.Y;
        value1.Z /= value2.Z;
        return value1;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec3{T}"/> by a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3{T}"/> on the left of the div sign.</param>
    /// <param name="divider">Divisor scalar on the right of the div sign.</param>
    /// <returns>The result of dividing a vector by a scalar.</returns>
    public static Vec3<T> operator /(Vec3<T> value1, T divider) {
        var factor = T.One / divider;
        value1.X *= factor;
        value1.Y *= factor;
        value1.Z *= factor;
        return value1;
    }

}
