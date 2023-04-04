using System.Diagnostics;
using System.Numerics;
using GameEngine.Core.Serialization;

namespace GameEngine.Numerics;

/// <summary>
/// Describes a 4D-vector.
/// </summary>
[DebuggerDisplay("<{X}, {Y}, {Z}, {W}>")]
public struct Vec4<T> : IEquatable<Vec4<T>> where T : struct, IFloatingPointIeee754<T> {
    
    [Serialized] public T X;
    [Serialized] public T Y;
    [Serialized] public T Z;
    [Serialized] public T W;
    
    public static Vec4<T> Zero => new Vec4<T>(T.Zero, T.Zero, T.Zero, T.Zero);
    public static Vec4<T> One => new Vec4<T>(T.One, T.One, T.One, T.One);
    public static Vec4<T> UnitX => new Vec4<T>(T.One, T.Zero, T.Zero, T.Zero);
    public static Vec4<T> UnitY => new Vec4<T>(T.Zero, T.One, T.Zero, T.Zero);
    public static Vec4<T> UnitZ => new Vec4<T>(T.Zero, T.Zero, T.One, T.Zero);
    public static Vec4<T> UnitW => new Vec4<T>(T.Zero, T.Zero, T.Zero, T.One);
    
    /// <summary>
    /// Constructs a 3d vector with X, Y, Z and W from four values.
    /// </summary>
    /// <param name="x">The x coordinate in 4d-space.</param>
    /// <param name="y">The y coordinate in 4d-space.</param>
    /// <param name="z">The z coordinate in 4d-space.</param>
    /// <param name="w">The w coordinate in 4d-space.</param>
    public Vec4(T x, T y, T z, T w) {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// Constructs a 3d vector with X and Z from <see cref="Vec2{T}"/> and Z and W from the scalars.
    /// </summary>
    /// <param name="value">The x and y coordinates in 4d-space.</param>
    /// <param name="z">The z coordinate in 4d-space.</param>
    /// <param name="w">The w coordinate in 4d-space.</param>
    public Vec4(Vec2<T> value, T z, T w) {
        X = value.X;
        Y = value.Y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// Constructs a 3d vector with X, Y, Z from <see cref="Vec3{T}"/> and W from a scalar.
    /// </summary>
    /// <param name="value">The x, y and z coordinates in 4d-space.</param>
    /// <param name="w">The w coordinate in 4d-space.</param>
    public Vec4(Vec3<T> value, T w) {
        X = value.X;
        Y = value.Y;
        Z = value.Z;
        W = w;
    }

    /// <summary>
    /// Constructs a 4d vector with X, Y, Z and W set to the same value.
    /// </summary>
    /// <param name="value">The x, y, z and w coordinates in 4d-space.</param>
    public Vec4(T value) {
        X = value;
        Y = value;
        Z = value;
        W = value;
    }
    
    /// <summary>
    /// Performs vector addition on <paramref name="value1"/> and <paramref name="value2"/>.
    /// </summary>
    /// <param name="value1">The first vector to add.</param>
    /// <param name="value2">The second vector to add.</param>
    /// <returns>The result of the vector addition.</returns>
    public static Vec4<T> Add(Vec4<T> value1, Vec4<T> value2) {
        value1.X += value2.X;
        value1.Y += value2.Y;
        value1.Z += value2.Z;
        value1.W += value2.W;
        return value1;
    }

    /// <summary>
    /// Performs vector addition on <paramref name="value1"/> and
    /// <paramref name="value2"/>, storing the result of the
    /// addition in <paramref name="result"/>.
    /// </summary>
    /// <param name="value1">The first vector to add.</param>
    /// <param name="value2">The second vector to add.</param>
    /// <param name="result">The result of the vector addition.</param>
    public static void Add(ref Vec4<T> value1, ref Vec4<T> value2, out Vec4<T> result) {
        result.X = value1.X + value2.X;
        result.Y = value1.Y + value2.Y;
        result.Z = value1.Z + value2.Z;
        result.W = value1.W + value2.W;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 4d-triangle.
    /// </summary>
    /// <param name="value1">The first vector of 4d-triangle.</param>
    /// <param name="value2">The second vector of 4d-triangle.</param>
    /// <param name="value3">The third vector of 4d-triangle.</param>
    /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 4d-triangle.</param>
    /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 4d-triangle.</param>
    /// <returns>The cartesian translation of barycentric coordinates.</returns>
    public static Vec4<T> Barycentric(Vec4<T> value1, Vec4<T> value2, Vec4<T> value3, T amount1, T amount2) {
        return new Vec4<T>(
            MathHelper.Barycentric<T>(value1.X, value2.X, value3.X, amount1, amount2),
            MathHelper.Barycentric<T>(value1.Y, value2.Y, value3.Y, amount1, amount2),
            MathHelper.Barycentric<T>(value1.Z, value2.Z, value3.Z, amount1, amount2),
            MathHelper.Barycentric<T>(value1.W, value2.W, value3.W, amount1, amount2)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 4d-triangle.
    /// </summary>
    /// <param name="value1">The first vector of 4d-triangle.</param>
    /// <param name="value2">The second vector of 4d-triangle.</param>
    /// <param name="value3">The third vector of 4d-triangle.</param>
    /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 4d-triangle.</param>
    /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 4d-triangle.</param>
    /// <param name="result">The cartesian translation of barycentric coordinates as an output parameter.</param>
    public static void Barycentric(ref Vec4<T> value1, ref Vec4<T> value2, ref Vec4<T> value3, T amount1,
        T amount2, out Vec4<T> result
    ) {
        result.X = MathHelper.Barycentric<T>(value1.X, value2.X, value3.X, amount1, amount2);
        result.Y = MathHelper.Barycentric<T>(value1.Y, value2.Y, value3.Y, amount1, amount2);
        result.Z = MathHelper.Barycentric<T>(value1.Z, value2.Z, value3.Z, amount1, amount2);
        result.W = MathHelper.Barycentric<T>(value1.W, value2.W, value3.W, amount1, amount2);
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains CatmullRom interpolation of the specified vectors.
    /// </summary>
    /// <param name="value1">The first vector in interpolation.</param>
    /// <param name="value2">The second vector in interpolation.</param>
    /// <param name="value3">The third vector in interpolation.</param>
    /// <param name="value4">The fourth vector in interpolation.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <returns>The result of CatmullRom interpolation.</returns>
    public static Vec4<T> CatmullRom(Vec4<T> value1, Vec4<T> value2, Vec4<T> value3, Vec4<T> value4, T amount) {
        return new Vec4<T>(
            MathHelper.CatmullRom<T>(value1.X, value2.X, value3.X, value4.X, amount),
            MathHelper.CatmullRom<T>(value1.Y, value2.Y, value3.Y, value4.Y, amount),
            MathHelper.CatmullRom<T>(value1.Z, value2.Z, value3.Z, value4.Z, amount),
            MathHelper.CatmullRom<T>(value1.W, value2.W, value3.W, value4.W, amount)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains CatmullRom interpolation of the specified vectors.
    /// </summary>
    /// <param name="value1">The first vector in interpolation.</param>
    /// <param name="value2">The second vector in interpolation.</param>
    /// <param name="value3">The third vector in interpolation.</param>
    /// <param name="value4">The fourth vector in interpolation.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <param name="result">The result of CatmullRom interpolation as an output parameter.</param>
    public static void CatmullRom(ref Vec4<T> value1, ref Vec4<T> value2, ref Vec4<T> value3, ref Vec4<T> value4,
        T amount, out Vec4<T> result
    ) {
        result.X = MathHelper.CatmullRom<T>(value1.X, value2.X, value3.X, value4.X, amount);
        result.Y = MathHelper.CatmullRom<T>(value1.Y, value2.Y, value3.Y, value4.Y, amount);
        result.Z = MathHelper.CatmullRom<T>(value1.Z, value2.Z, value3.Z, value4.Z, amount);
        result.W = MathHelper.CatmullRom<T>(value1.W, value2.W, value3.W, value4.W, amount);
    }

    /// <summary>
    /// Round the members of this <see cref="Vec4{T}"/> towards positive infinity.
    /// </summary>
    public void Ceiling() {
        X = T.Ceiling(X);
        Y = T.Ceiling(Y);
        Z = T.Ceiling(Z);
        W = T.Ceiling(W);
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains members from another vector rounded towards positive infinity.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <returns>The rounded <see cref="Vec4{T}"/>.</returns>
    public static Vec4<T> Ceiling(Vec4<T> value) {
        value.X = T.Ceiling(value.X);
        value.Y = T.Ceiling(value.Y);
        value.Z = T.Ceiling(value.Z);
        value.W = T.Ceiling(value.W);
        return value;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains members from another vector rounded towards positive infinity.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="result">The rounded <see cref="Vec4{T}"/>.</param>
    public static void Ceiling(ref Vec4<T> value, out Vec4<T> result) {
        result.X = T.Ceiling(value.X);
        result.Y = T.Ceiling(value.Y);
        result.Z = T.Ceiling(value.Z);
        result.W = T.Ceiling(value.W);
    }

    /// <summary>
    /// Clamps the specified value within a range.
    /// </summary>
    /// <param name="value1">The value to clamp.</param>
    /// <param name="min">The min value.</param>
    /// <param name="max">The max value.</param>
    /// <returns>The clamped value.</returns>
    public static Vec4<T> Clamp(Vec4<T> value1, Vec4<T> min, Vec4<T> max) {
        return new Vec4<T>(
            MathHelper.Clamp(value1.X, min.X, max.X),
            MathHelper.Clamp(value1.Y, min.Y, max.Y),
            MathHelper.Clamp(value1.Z, min.Z, max.Z),
            MathHelper.Clamp(value1.W, min.W, max.W)
        );
    }

    /// <summary>
    /// Clamps the specified value within a range.
    /// </summary>
    /// <param name="value1">The value to clamp.</param>
    /// <param name="min">The min value.</param>
    /// <param name="max">The max value.</param>
    /// <param name="result">The clamped value as an output parameter.</param>
    public static void Clamp(ref Vec4<T> value1, ref Vec4<T> min, ref Vec4<T> max, out Vec4<T> result) {
        result.X = T.Clamp(value1.X, min.X, max.X);
        result.Y = T.Clamp(value1.Y, min.Y, max.Y);
        result.Z = T.Clamp(value1.Z, min.Z, max.Z);
        result.W = T.Clamp(value1.W, min.W, max.W);
    }

    /// <summary>
    /// Returns the distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The distance between two vectors.</returns>
    public static T Distance(Vec4<T> value1, Vec4<T> value2) {
        return T.Sqrt(DistanceSquared(value1, value2));
    }

    /// <summary>
    /// Returns the distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The distance between two vectors as an output parameter.</param>
    public static void Distance(ref Vec4<T> value1, ref Vec4<T> value2, out T result) {
        result = T.Sqrt(DistanceSquared(value1, value2));
    }

    /// <summary>
    /// Returns the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The squared distance between two vectors.</returns>
    public static T DistanceSquared(Vec4<T> value1, Vec4<T> value2) {
        return (value1.W - value2.W) * (value1.W - value2.W) +
               (value1.X - value2.X) * (value1.X - value2.X) +
               (value1.Y - value2.Y) * (value1.Y - value2.Y) +
               (value1.Z - value2.Z) * (value1.Z - value2.Z);
    }

    /// <summary>
    /// Returns the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The squared distance between two vectors as an output parameter.</param>
    public static void DistanceSquared(ref Vec4<T> value1, ref Vec4<T> value2, out T result) {
        result = (value1.W - value2.W) * (value1.W - value2.W) +
                 (value1.X - value2.X) * (value1.X - value2.X) +
                 (value1.Y - value2.Y) * (value1.Y - value2.Y) +
                 (value1.Z - value2.Z) * (value1.Z - value2.Z);
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec4{T}"/> by the components of another <see cref="Vec4{T}"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="value2">Divisor <see cref="Vec4{T}"/>.</param>
    /// <returns>The result of dividing the vectors.</returns>
    public static Vec4<T> Divide(Vec4<T> value1, Vec4<T> value2) {
        value1.W /= value2.W;
        value1.X /= value2.X;
        value1.Y /= value2.Y;
        value1.Z /= value2.Z;
        return value1;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec4{T}"/> by a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="divider">Divisor scalar.</param>
    /// <returns>The result of dividing a vector by a scalar.</returns>
    public static Vec4<T> Divide(Vec4<T> value1, T divider) {
        T factor = T.One / divider;
        value1.W *= factor;
        value1.X *= factor;
        value1.Y *= factor;
        value1.Z *= factor;
        return value1;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec4{T}"/> by a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="divider">Divisor scalar.</param>
    /// <param name="result">The result of dividing a vector by a scalar as an output parameter.</param>
    public static void Divide(ref Vec4<T> value1, T divider, out Vec4<T> result) {
        T factor = T.One / divider;
        result.W = value1.W * factor;
        result.X = value1.X * factor;
        result.Y = value1.Y * factor;
        result.Z = value1.Z * factor;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec4{T}"/> by the components of another <see cref="Vec4{T}"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="value2">Divisor <see cref="Vec4{T}"/>.</param>
    /// <param name="result">The result of dividing the vectors as an output parameter.</param>
    public static void Divide(ref Vec4<T> value1, ref Vec4<T> value2, out Vec4<T> result) {
        result.W = value1.W / value2.W;
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
    public static T Dot(Vec4<T> value1, Vec4<T> value2) {
        return value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z + value1.W * value2.W;
    }

    /// <summary>
    /// Returns a dot product of two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The dot product of two vectors as an output parameter.</param>
    public static void Dot(ref Vec4<T> value1, ref Vec4<T> value2, out T result) {
        result = value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z + value1.W * value2.W;
    }

    /// <summary>
    /// Compares whether current instance is equal to specified <see cref="Object"/>.
    /// </summary>
    /// <param name="obj">The <see cref="Object"/> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public override bool Equals(object? obj) {
        return obj is Vec4<T> vec4 && this == vec4;
    }

    /// <summary>
    /// Compares whether current instance is equal to specified <see cref="Vec4{T}"/>.
    /// </summary>
    /// <param name="other">The <see cref="Vec4{T}"/> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public bool Equals(Vec4<T> other) {
        return W == other.W
               && X == other.X
               && Y == other.Y
               && Z == other.Z;
    }

    /// <summary>
    /// Round the members of this <see cref="Vec4{T}"/> towards negative infinity.
    /// </summary>
    public void Floor() {
        X = T.Floor(X);
        Y = T.Floor(Y);
        Z = T.Floor(Z);
        W = T.Floor(W);
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains members from another vector rounded towards negative infinity.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <returns>The rounded <see cref="Vec4{T}"/>.</returns>
    public static Vec4<T> Floor(Vec4<T> value) {
        value.X = T.Floor(value.X);
        value.Y = T.Floor(value.Y);
        value.Z = T.Floor(value.Z);
        value.W = T.Floor(value.W);
        return value;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains members from another vector rounded towards negative infinity.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="result">The rounded <see cref="Vec4{T}"/>.</param>
    public static void Floor(ref Vec4<T> value, out Vec4<T> result) {
        result.X = T.Floor(value.X);
        result.Y = T.Floor(value.Y);
        result.Z = T.Floor(value.Z);
        result.W = T.Floor(value.W);
    }

    /// <summary>
    /// Gets the hash code of this <see cref="Vec4{T}"/>.
    /// </summary>
    /// <returns>Hash code of this <see cref="Vec4{T}"/>.</returns>
    public override int GetHashCode() {
        unchecked {
            var hashCode = W.GetHashCode();
            hashCode = (hashCode * 397) ^ X.GetHashCode();
            hashCode = (hashCode * 397) ^ Y.GetHashCode();
            hashCode = (hashCode * 397) ^ Z.GetHashCode();
            return hashCode;
        }
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains hermite spline interpolation.
    /// </summary>
    /// <param name="value1">The first position vector.</param>
    /// <param name="tangent1">The first tangent vector.</param>
    /// <param name="value2">The second position vector.</param>
    /// <param name="tangent2">The second tangent vector.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <returns>The hermite spline interpolation vector.</returns>
    public static Vec4<T> Hermite(Vec4<T> value1, Vec4<T> tangent1, Vec4<T> value2, Vec4<T> tangent2, T amount) {
        return new Vec4<T>(MathHelper.Hermite<T>(value1.X, tangent1.X, value2.X, tangent2.X, amount),
            MathHelper.Hermite<T>(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount),
            MathHelper.Hermite<T>(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount),
            MathHelper.Hermite<T>(value1.W, tangent1.W, value2.W, tangent2.W, amount)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains hermite spline interpolation.
    /// </summary>
    /// <param name="value1">The first position vector.</param>
    /// <param name="tangent1">The first tangent vector.</param>
    /// <param name="value2">The second position vector.</param>
    /// <param name="tangent2">The second tangent vector.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <param name="result">The hermite spline interpolation vector as an output parameter.</param>
    public static void Hermite(ref Vec4<T> value1, ref Vec4<T> tangent1, ref Vec4<T> value2, ref Vec4<T> tangent2, T amount, out Vec4<T> result) {
        result.W = MathHelper.Hermite<T>(value1.W, tangent1.W, value2.W, tangent2.W, amount);
        result.X = MathHelper.Hermite<T>(value1.X, tangent1.X, value2.X, tangent2.X, amount);
        result.Y = MathHelper.Hermite<T>(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
        result.Z = MathHelper.Hermite<T>(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount);
    }

    /// <summary>
    /// Returns the length of this <see cref="Vec4{T}"/>.
    /// </summary>
    /// <returns>The length of this <see cref="Vec4{T}"/>.</returns>
    public T Length() {
        return T.Sqrt(X * X + Y * Y + Z * Z + W * W);
    }

    /// <summary>
    /// Returns the squared length of this <see cref="Vec4{T}"/>.
    /// </summary>
    /// <returns>The squared length of this <see cref="Vec4{T}"/>.</returns>
    public T LengthSquared() {
        return X * X + Y * Y + Z * Z + W * W;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains linear interpolation of the specified vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
    /// <returns>The result of linear interpolation of the specified vectors.</returns>
    public static Vec4<T> Lerp(Vec4<T> value1, Vec4<T> value2, T amount) {
        return new Vec4<T>(
            MathHelper.Lerp<T>(value1.X, value2.X, amount),
            MathHelper.Lerp<T>(value1.Y, value2.Y, amount),
            MathHelper.Lerp<T>(value1.Z, value2.Z, amount),
            MathHelper.Lerp<T>(value1.W, value2.W, amount)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains linear interpolation of the specified vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
    /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
    public static void Lerp(ref Vec4<T> value1, ref Vec4<T> value2, T amount, out Vec4<T> result) {
        result.X = MathHelper.Lerp<T>(value1.X, value2.X, amount);
        result.Y = MathHelper.Lerp<T>(value1.Y, value2.Y, amount);
        result.Z = MathHelper.Lerp<T>(value1.Z, value2.Z, amount);
        result.W = MathHelper.Lerp<T>(value1.W, value2.W, amount);
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains linear interpolation of the specified vectors.
    /// Uses <see cref="MathHelper.LerpPrecise{T}"/> on MathHelper for the interpolation.
    /// Less efficient but more precise compared to <see cref="Vec4{T}.Lerp(Vec4{T}, Vec4{T}, T)"/>.
    /// See remarks section of <see cref="MathHelper.LerpPrecise{T}"/> on MathHelper for more info.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
    /// <returns>The result of linear interpolation of the specified vectors.</returns>
    public static Vec4<T> LerpPrecise(Vec4<T> value1, Vec4<T> value2, T amount) {
        return new Vec4<T>(
            MathHelper.LerpPrecise<T>(value1.X, value2.X, amount),
            MathHelper.LerpPrecise<T>(value1.Y, value2.Y, amount),
            MathHelper.LerpPrecise<T>(value1.Z, value2.Z, amount),
            MathHelper.LerpPrecise<T>(value1.W, value2.W, amount)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains linear interpolation of the specified vectors.
    /// Uses <see cref="MathHelper.LerpPrecise{T}"/> on MathHelper for the interpolation.
    /// Less efficient but more precise compared to <see cref="Vec4{T}.Lerp(ref Vec4{T}, ref Vec4{T}, T, out Vec4{T})"/>.
    /// See remarks section of <see cref="MathHelper.LerpPrecise{T}"/> on MathHelper for more info.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
    /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
    public static void LerpPrecise(ref Vec4<T> value1, ref Vec4<T> value2, T amount, out Vec4<T> result) {
        result.X = MathHelper.LerpPrecise<T>(value1.X, value2.X, amount);
        result.Y = MathHelper.LerpPrecise<T>(value1.Y, value2.Y, amount);
        result.Z = MathHelper.LerpPrecise<T>(value1.Z, value2.Z, amount);
        result.W = MathHelper.LerpPrecise<T>(value1.W, value2.W, amount);
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a maximal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The <see cref="Vec4{T}"/> with maximal values from the two vectors.</returns>
    public static Vec4<T> Max(Vec4<T> value1, Vec4<T> value2) {
        return new Vec4<T>(
            T.Max(value1.X, value2.X),
            T.Max(value1.Y, value2.Y),
            T.Max(value1.Z, value2.Z),
            T.Max(value1.W, value2.W)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a maximal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The <see cref="Vec4{T}"/> with maximal values from the two vectors as an output parameter.</param>
    public static void Max(ref Vec4<T> value1, ref Vec4<T> value2, out Vec4<T> result) {
        result.X = T.Max(value1.X, value2.X);
        result.Y = T.Max(value1.Y, value2.Y);
        result.Z = T.Max(value1.Z, value2.Z);
        result.W = T.Max(value1.W, value2.W);
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a minimal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The <see cref="Vec4{T}"/> with minimal values from the two vectors.</returns>
    public static Vec4<T> Min(Vec4<T> value1, Vec4<T> value2) {
        return new Vec4<T>(
            T.Min(value1.X, value2.X),
            T.Min(value1.Y, value2.Y),
            T.Min(value1.Z, value2.Z),
            T.Min(value1.W, value2.W)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a minimal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The <see cref="Vec4{T}"/> with minimal values from the two vectors as an output parameter.</param>
    public static void Min(ref Vec4<T> value1, ref Vec4<T> value2, out Vec4<T> result) {
        result.X = T.Min(value1.X, value2.X);
        result.Y = T.Min(value1.Y, value2.Y);
        result.Z = T.Min(value1.Z, value2.Z);
        result.W = T.Min(value1.W, value2.W);
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a multiplication of two vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="value2">Source <see cref="Vec4{T}"/>.</param>
    /// <returns>The result of the vector multiplication.</returns>
    public static Vec4<T> Multiply(Vec4<T> value1, Vec4<T> value2) {
        value1.W *= value2.W;
        value1.X *= value2.X;
        value1.Y *= value2.Y;
        value1.Z *= value2.Z;
        return value1;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a multiplication of <see cref="Vec4{T}"/> and a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="scaleFactor">Scalar value.</param>
    /// <returns>The result of the vector multiplication with a scalar.</returns>
    public static Vec4<T> Multiply(Vec4<T> value1, T scaleFactor) {
        value1.W *= scaleFactor;
        value1.X *= scaleFactor;
        value1.Y *= scaleFactor;
        value1.Z *= scaleFactor;
        return value1;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a multiplication of <see cref="Vec4{T}"/> and a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="scaleFactor">Scalar value.</param>
    /// <param name="result">The result of the multiplication with a scalar as an output parameter.</param>
    public static void Multiply(ref Vec4<T> value1, T scaleFactor, out Vec4<T> result) {
        result.W = value1.W * scaleFactor;
        result.X = value1.X * scaleFactor;
        result.Y = value1.Y * scaleFactor;
        result.Z = value1.Z * scaleFactor;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a multiplication of two vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="value2">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="result">The result of the vector multiplication as an output parameter.</param>
    public static void Multiply(ref Vec4<T> value1, ref Vec4<T> value2, out Vec4<T> result) {
        result.W = value1.W * value2.W;
        result.X = value1.X * value2.X;
        result.Y = value1.Y * value2.Y;
        result.Z = value1.Z * value2.Z;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains the specified vector inversion.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <returns>The result of the vector inversion.</returns>
    public static Vec4<T> Negate(Vec4<T> value) {
        value = new Vec4<T>(-value.X, -value.Y, -value.Z, -value.W);
        return value;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains the specified vector inversion.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="result">The result of the vector inversion as an output parameter.</param>
    public static void Negate(ref Vec4<T> value, out Vec4<T> result) {
        result.X = -value.X;
        result.Y = -value.Y;
        result.Z = -value.Z;
        result.W = -value.W;
    }

    /// <summary>
    /// Turns this <see cref="Vec4{T}"/> to a unit vector with the same direction.
    /// </summary>
    public void Normalize() {
        T factor = T.Sqrt(X * X + Y * Y + Z * Z + W * W);
        factor = T.One / factor;
        X *= factor;
        Y *= factor;
        Z *= factor;
        W *= factor;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a normalized values from another vector.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <returns>Unit vector.</returns>
    public static Vec4<T> Normalize(Vec4<T> value) {
        T factor = T.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W);
        factor = T.One / factor;
        return new Vec4<T>(value.X * factor, value.Y * factor, value.Z * factor, value.W * factor);
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a normalized values from another vector.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="result">Unit vector as an output parameter.</param>
    public static void Normalize(ref Vec4<T> value, out Vec4<T> result) {
        T factor = T.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W);
        factor = T.One / factor;
        result.W = value.W * factor;
        result.X = value.X * factor;
        result.Y = value.Y * factor;
        result.Z = value.Z * factor;
    }

    /// <summary>
    /// Round the members of this <see cref="Vec4{T}"/> to the nearest integer value.
    /// </summary>
    public void Round() {
        X = T.Round(X);
        Y = T.Round(Y);
        Z = T.Round(Z);
        W = T.Round(W);
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains members from another vector rounded to the nearest integer value.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <returns>The rounded <see cref="Vec4{T}"/>.</returns>
    public static Vec4<T> Round(Vec4<T> value) {
        value.X = T.Round(value.X);
        value.Y = T.Round(value.Y);
        value.Z = T.Round(value.Z);
        value.W = T.Round(value.W);
        return value;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains members from another vector rounded to the nearest integer value.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="result">The rounded <see cref="Vec4{T}"/>.</param>
    public static void Round(ref Vec4<T> value, out Vec4<T> result) {
        result.X = T.Round(value.X);
        result.Y = T.Round(value.Y);
        result.Z = T.Round(value.Z);
        result.W = T.Round(value.W);
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains cubic interpolation of the specified vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="value2">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="amount">Weighting value.</param>
    /// <returns>Cubic interpolation of the specified vectors.</returns>
    public static Vec4<T> SmoothStep(Vec4<T> value1, Vec4<T> value2, T amount) {
        return new Vec4<T>(
            MathHelper.SmoothStep<T>(value1.X, value2.X, amount),
            MathHelper.SmoothStep<T>(value1.Y, value2.Y, amount),
            MathHelper.SmoothStep<T>(value1.Z, value2.Z, amount),
            MathHelper.SmoothStep<T>(value1.W, value2.W, amount)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains cubic interpolation of the specified vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="value2">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="amount">Weighting value.</param>
    /// <param name="result">Cubic interpolation of the specified vectors as an output parameter.</param>
    public static void SmoothStep(ref Vec4<T> value1, ref Vec4<T> value2, T amount, out Vec4<T> result) {
        result.X = MathHelper.SmoothStep<T>(value1.X, value2.X, amount);
        result.Y = MathHelper.SmoothStep<T>(value1.Y, value2.Y, amount);
        result.Z = MathHelper.SmoothStep<T>(value1.Z, value2.Z, amount);
        result.W = MathHelper.SmoothStep<T>(value1.W, value2.W, amount);
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains subtraction of on <see cref="Vec4{T}"/> from a another.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="value2">Source <see cref="Vec4{T}"/>.</param>
    /// <returns>The result of the vector subtraction.</returns>
    public static Vec4<T> Subtract(Vec4<T> value1, Vec4<T> value2) {
        value1.W -= value2.W;
        value1.X -= value2.X;
        value1.Y -= value2.Y;
        value1.Z -= value2.Z;
        return value1;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains subtraction of on <see cref="Vec4{T}"/> from a another.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="value2">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="result">The result of the vector subtraction as an output parameter.</param>
    public static void Subtract(ref Vec4<T> value1, ref Vec4<T> value2, out Vec4<T> result) {
        result.W = value1.W - value2.W;
        result.X = value1.X - value2.X;
        result.Y = value1.Y - value2.Y;
        result.Z = value1.Z - value2.Z;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <returns>Transformed <see cref="Vec4{T}"/>.</returns>
    public static Vec4<T> Transform(Vec2<T> value, Matrix<T> matrix) {
        Vec4<T> result;
        Transform(ref value, ref matrix, out result);
        return result;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a transformation of 2d-vector by the specified <see cref="Quaternion"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
    /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
    /// <returns>Transformed <see cref="Vec4{T}"/>.</returns>
    public static Vec4<T> Transform(Vec2<T> value, Quaternion<T> rotation) {
        Vec4<T> result;
        Transform(ref value, ref rotation, out result);
        return result;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a transformation of 3d-vector by the specified <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <returns>Transformed <see cref="Vec4{T}"/>.</returns>
    public static Vec4<T> Transform(Vec3<T> value, Matrix<T> matrix) {
        Vec4<T> result;
        Transform(ref value, ref matrix, out result);
        return result;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a transformation of 3d-vector by the specified <see cref="Quaternion"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
    /// <returns>Transformed <see cref="Vec4{T}"/>.</returns>
    public static Vec4<T> Transform(Vec3<T> value, Quaternion<T> rotation) {
        Vec4<T> result;
        Transform(ref value, ref rotation, out result);
        return result;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a transformation of 4d-vector by the specified <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <returns>Transformed <see cref="Vec4{T}"/>.</returns>
    public static Vec4<T> Transform(Vec4<T> value, Matrix<T> matrix) {
        Transform(ref value, ref matrix, out value);
        return value;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a transformation of 4d-vector by the specified <see cref="Quaternion"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
    /// <returns>Transformed <see cref="Vec4{T}"/>.</returns>
    public static Vec4<T> Transform(Vec4<T> value, Quaternion<T> rotation) {
        Vec4<T> result;
        Transform(ref value, ref rotation, out result);
        return result;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <param name="result">Transformed <see cref="Vec4{T}"/> as an output parameter.</param>
    public static void Transform(ref Vec2<T> value, ref Matrix<T> matrix, out Vec4<T> result) {
        result.X = value.X * matrix.M11 + value.Y * matrix.M21 + matrix.M41;
        result.Y = value.X * matrix.M12 + value.Y * matrix.M22 + matrix.M42;
        result.Z = value.X * matrix.M13 + value.Y * matrix.M23 + matrix.M43;
        result.W = value.X * matrix.M14 + value.Y * matrix.M24 + matrix.M44;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a transformation of 2d-vector by the specified <see cref="Quaternion"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
    /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
    /// <param name="result">Transformed <see cref="Vec4{T}"/> as an output parameter.</param>
    public static void Transform(ref Vec2<T> value, ref Quaternion<T> rotation, out Vec4<T> result) {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a transformation of 3d-vector by the specified <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <param name="result">Transformed <see cref="Vec4{T}"/> as an output parameter.</param>
    public static void Transform(ref Vec3<T> value, ref Matrix<T> matrix, out Vec4<T> result) {
        result.X = value.X * matrix.M11 + value.Y * matrix.M21 + value.Z * matrix.M31 + matrix.M41;
        result.Y = value.X * matrix.M12 + value.Y * matrix.M22 + value.Z * matrix.M32 + matrix.M42;
        result.Z = value.X * matrix.M13 + value.Y * matrix.M23 + value.Z * matrix.M33 + matrix.M43;
        result.W = value.X * matrix.M14 + value.Y * matrix.M24 + value.Z * matrix.M34 + matrix.M44;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a transformation of 3d-vector by the specified <see cref="Quaternion"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3{T}"/>.</param>
    /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
    /// <param name="result">Transformed <see cref="Vec4{T}"/> as an output parameter.</param>
    public static void Transform(ref Vec3<T> value, ref Quaternion<T> rotation, out Vec4<T> result) {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a transformation of 4d-vector by the specified <see cref="Matrix{T}"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <param name="result">Transformed <see cref="Vec4{T}"/> as an output parameter.</param>
    public static void Transform(ref Vec4<T> value, ref Matrix<T> matrix, out Vec4<T> result) {
        var x = value.X * matrix.M11 + value.Y * matrix.M21 + value.Z * matrix.M31 + value.W * matrix.M41;
        var y = value.X * matrix.M12 + value.Y * matrix.M22 + value.Z * matrix.M32 + value.W * matrix.M42;
        var z = value.X * matrix.M13 + value.Y * matrix.M23 + value.Z * matrix.M33 + value.W * matrix.M43;
        var w = value.X * matrix.M14 + value.Y * matrix.M24 + value.Z * matrix.M34 + value.W * matrix.M44;
        result.X = x;
        result.Y = y;
        result.Z = z;
        result.W = w;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4{T}"/> that contains a transformation of 4d-vector by the specified <see cref="Quaternion"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/>.</param>
    /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
    /// <param name="result">Transformed <see cref="Vec4{T}"/> as an output parameter.</param>
    public static void Transform(ref Vec4<T> value, ref Quaternion<T> rotation, out Vec4<T> result) {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Apply transformation on vectors within array of <see cref="Vec4{T}"/> by the specified <see cref="Matrix{T}"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <param name="destinationArray">Destination array.</param>
    /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vec4{T}"/> should be written.</param>
    /// <param name="length">The number of vectors to be transformed.</param>
    public static void Transform(Vec4<T>[] sourceArray, int sourceIndex, ref Matrix<T> matrix, Vec4<T>[] destinationArray, int destinationIndex, int length) {
        if(sourceArray == null) throw new ArgumentNullException(nameof(sourceArray));
        if(destinationArray == null) throw new ArgumentNullException(nameof(destinationArray));
        if(sourceArray.Length < sourceIndex + length)
            throw new ArgumentException("Source array length is lesser than sourceIndex + length");
        if(destinationArray.Length < destinationIndex + length)
            throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

        for(var i = 0; i < length; i++) {
            Vec4<T> value = sourceArray[sourceIndex + i];
            destinationArray[destinationIndex + i] = Transform(value, matrix);
        }
    }

    /// <summary>
    /// Apply transformation on vectors within array of <see cref="Vec4{T}"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
    /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
    /// <param name="destinationArray">Destination array.</param>
    /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vec4{T}"/> should be written.</param>
    /// <param name="length">The number of vectors to be transformed.</param>
    public static void Transform(Vec4<T>[] sourceArray, int sourceIndex, ref Quaternion<T> rotation, Vec4<T>[] destinationArray, int destinationIndex, int length) {
        if(sourceArray == null) throw new ArgumentNullException(nameof(sourceArray));
        if(destinationArray == null) throw new ArgumentNullException(nameof(destinationArray));
        if(sourceArray.Length < sourceIndex + length)
            throw new ArgumentException("Source array length is lesser than sourceIndex + length");
        if(destinationArray.Length < destinationIndex + length)
            throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

        for(var i = 0; i < length; i++) {
            var value = sourceArray[sourceIndex + i];
            destinationArray[destinationIndex + i] = Transform(value, rotation);
        }
    }

    /// <summary>
    /// Apply transformation on all vectors within array of <see cref="Vec4{T}"/> by the specified <see cref="Matrix{T}"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
    /// <param name="destinationArray">Destination array.</param>
    public static void Transform(Vec4<T>[] sourceArray, ref Matrix<T> matrix, Vec4<T>[] destinationArray) {
        if(sourceArray == null) throw new ArgumentNullException(nameof(sourceArray));
        if(destinationArray == null) throw new ArgumentNullException(nameof(destinationArray));
        if(destinationArray.Length < sourceArray.Length)
            throw new ArgumentException("Destination array length is lesser than source array length");

        for(var i = 0; i < sourceArray.Length; i++) {
            var value = sourceArray[i];
            destinationArray[i] = Transform(value, matrix);
        }
    }

    /// <summary>
    /// Apply transformation on all vectors within array of <see cref="Vec4{T}"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
    /// <param name="destinationArray">Destination array.</param>
    public static void Transform(Vec4<T>[] sourceArray, ref Quaternion<T> rotation, Vec4<T>[] destinationArray) {
        if(sourceArray == null) throw new ArgumentNullException(nameof(sourceArray));
        if(destinationArray == null) throw new ArgumentNullException(nameof(destinationArray));
        if(destinationArray.Length < sourceArray.Length)
            throw new ArgumentException("Destination array length is lesser than source array length");

        for(var i = 0; i < sourceArray.Length; i++) {
            var value = sourceArray[i];
            destinationArray[i] = Transform(value, rotation);
        }
    }

    /// <summary>
    /// Returns a <see cref="String"/> representation of this <see cref="Vec4{T}"/> in the format:
    /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>] Z:[<see cref="Z"/>] W:[<see cref="W"/>]}
    /// </summary>
    /// <returns>A <see cref="String"/> representation of this <see cref="Vec4{T}"/>.</returns>
    public override string ToString() {
        return "{X:" + X + " Y:" + Y + " Z:" + Z + " W:" + W + "}";
    }

    /// <summary>
    /// Deconstruction method for <see cref="Vec4{T}"/>.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="w"></param>
    public void Deconstruct(out T x, out T y, out T z, out T w) {
        x = X;
        y = Y;
        z = Z;
        w = W;
    }

    /// <summary>
    /// Returns a <see cref="System.Numerics.Vector4"/>.
    /// </summary>
    public Vector4 ToNumerics() {
        return new Vector4(float.CreateChecked(X), float.CreateChecked(Y), float.CreateChecked(Z), float.CreateChecked(W));
    }

    /// <summary>
    /// Converts a <see cref="System.Numerics.Vector4"/> to a <see cref="Vec4{T}"/>.
    /// </summary>
    /// <param name="value">The converted value.</param>
    public static implicit operator Vec4<T>(Vector4 value) {
        return new Vec4<T>(T.CreateChecked(value.X), T.CreateChecked(value.Y), T.CreateChecked(value.Z), T.CreateChecked(value.W));
    }

    /// <summary>
    /// Inverts values in the specified <see cref="Vec4{T}"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/> on the right of the sub sign.</param>
    /// <returns>Result of the inversion.</returns>
    public static Vec4<T> operator -(Vec4<T> value) {
        return new Vec4<T>(-value.X, -value.Y, -value.Z, -value.W);
    }

    /// <summary>
    /// Compares whether two <see cref="Vec4{T}"/> instances are equal.
    /// </summary>
    /// <param name="value1"><see cref="Vec4{T}"/> instance on the left of the equal sign.</param>
    /// <param name="value2"><see cref="Vec4{T}"/> instance on the right of the equal sign.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public static bool operator ==(Vec4<T> value1, Vec4<T> value2) {
        return value1.W == value2.W
               && value1.X == value2.X
               && value1.Y == value2.Y
               && value1.Z == value2.Z;
    }

    /// <summary>
    /// Compares whether two <see cref="Vec4{T}"/> instances are not equal.
    /// </summary>
    /// <param name="value1"><see cref="Vec4{T}"/> instance on the left of the not equal sign.</param>
    /// <param name="value2"><see cref="Vec4{T}"/> instance on the right of the not equal sign.</param>
    /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>	
    public static bool operator !=(Vec4<T> value1, Vec4<T> value2) {
        return !(value1 == value2);
    }

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/> on the left of the add sign.</param>
    /// <param name="value2">Source <see cref="Vec4{T}"/> on the right of the add sign.</param>
    /// <returns>Sum of the vectors.</returns>
    public static Vec4<T> operator +(Vec4<T> value1, Vec4<T> value2) {
        value1.W += value2.W;
        value1.X += value2.X;
        value1.Y += value2.Y;
        value1.Z += value2.Z;
        return value1;
    }

    /// <summary>
    /// Subtracts a <see cref="Vec4{T}"/> from a <see cref="Vec4{T}"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/> on the left of the sub sign.</param>
    /// <param name="value2">Source <see cref="Vec4{T}"/> on the right of the sub sign.</param>
    /// <returns>Result of the vector subtraction.</returns>
    public static Vec4<T> operator -(Vec4<T> value1, Vec4<T> value2) {
        value1.W -= value2.W;
        value1.X -= value2.X;
        value1.Y -= value2.Y;
        value1.Z -= value2.Z;
        return value1;
    }

    /// <summary>
    /// Multiplies the components of two vectors by each other.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/> on the left of the mul sign.</param>
    /// <param name="value2">Source <see cref="Vec4{T}"/> on the right of the mul sign.</param>
    /// <returns>Result of the vector multiplication.</returns>
    public static Vec4<T> operator *(Vec4<T> value1, Vec4<T> value2) {
        value1.W *= value2.W;
        value1.X *= value2.X;
        value1.Y *= value2.Y;
        value1.Z *= value2.Z;
        return value1;
    }

    /// <summary>
    /// Multiplies the components of vector by a scalar.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4{T}"/> on the left of the mul sign.</param>
    /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
    /// <returns>Result of the vector multiplication with a scalar.</returns>
    public static Vec4<T> operator *(Vec4<T> value, T scaleFactor) {
        value.W *= scaleFactor;
        value.X *= scaleFactor;
        value.Y *= scaleFactor;
        value.Z *= scaleFactor;
        return value;
    }

    /// <summary>
    /// Multiplies the components of vector by a scalar.
    /// </summary>
    /// <param name="scaleFactor">Scalar value on the left of the mul sign.</param>
    /// <param name="value">Source <see cref="Vec4{T}"/> on the right of the mul sign.</param>
    /// <returns>Result of the vector multiplication with a scalar.</returns>
    public static Vec4<T> operator *(T scaleFactor, Vec4<T> value) {
        value.W *= scaleFactor;
        value.X *= scaleFactor;
        value.Y *= scaleFactor;
        value.Z *= scaleFactor;
        return value;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec4{T}"/> by the components of another <see cref="Vec4{T}"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/> on the left of the div sign.</param>
    /// <param name="value2">Divisor <see cref="Vec4{T}"/> on the right of the div sign.</param>
    /// <returns>The result of dividing the vectors.</returns>
    public static Vec4<T> operator /(Vec4<T> value1, Vec4<T> value2) {
        value1.W /= value2.W;
        value1.X /= value2.X;
        value1.Y /= value2.Y;
        value1.Z /= value2.Z;
        return value1;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec4{T}"/> by a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4{T}"/> on the left of the div sign.</param>
    /// <param name="divider">Divisor scalar on the right of the div sign.</param>
    /// <returns>The result of dividing a vector by a scalar.</returns>
    public static Vec4<T> operator /(Vec4<T> value1, T divider) {
        T factor = T.One / divider;
        value1.W *= factor;
        value1.X *= factor;
        value1.Y *= factor;
        value1.Z *= factor;
        return value1;
    }

}
