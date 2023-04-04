using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using GameEngine.Core.Serialization;

namespace GameEngine.Numerics
{
    /// <summary>
    /// Describes a 2D-vector.
    /// </summary>
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct Vec2<T> : IEquatable<Vec2<T>> where T : struct, IFloatingPointIeee754<T>
    {
        
        [Serialized] public T X;
        [Serialized] public T Y;

        public static Vec2<T> Zero => new Vec2<T>(T.Zero, T.Zero);
        public static Vec2<T> One => new Vec2<T>(T.One, T.One);
        public static Vec2<T> UnitX => new Vec2<T>(T.One, T.Zero);
        public static Vec2<T> UnitY => new Vec2<T>(T.Zero, T.One);

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    this.X.ToString(), "  ",
                    this.Y.ToString()
                );
            }
        }

        /// <summary>
        /// Constructs a 2d vector with X and Y from two values.
        /// </summary>
        /// <param name="x">The x coordinate in 2d-space.</param>
        /// <param name="y">The y coordinate in 2d-space.</param>
        public Vec2(T x, T y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Constructs a 2d vector with X and Y set to the same value.
        /// </summary>
        /// <param name="value">The x and y coordinates in 2d-space.</param>
        public Vec2(T value)
        {
            this.X = value;
            this.Y = value;
        }

        /// <summary>
        /// Converts a <see cref="System.Numerics.Vector2"/> to a <see cref="Vec2{T}"/>.
        /// </summary>
        /// <param name="value">The converted value.</param>
        public static implicit operator Vec2<T>(Vector2 value)
        {
            return new Vec2<T>(T.CreateChecked(value.X), T.CreateChecked(value.Y));
        }

        /// <summary>
        /// Inverts values in the specified <see cref="Vec2{T}"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/> on the right of the sub sign.</param>
        /// <returns>Result of the inversion.</returns>
        public static Vec2<T> operator -(Vec2<T> value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/> on the left of the add sign.</param>
        /// <param name="value2">Source <see cref="Vec2{T}"/> on the right of the add sign.</param>
        /// <returns>Sum of the vectors.</returns>
        public static Vec2<T> operator +(Vec2<T> value1, Vec2<T> value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        /// <summary>
        /// Subtracts a <see cref="Vec2{T}"/> from a <see cref="Vec2{T}"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/> on the left of the sub sign.</param>
        /// <param name="value2">Source <see cref="Vec2{T}"/> on the right of the sub sign.</param>
        /// <returns>Result of the vector subtraction.</returns>
        public static Vec2<T> operator -(Vec2<T> value1, Vec2<T> value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/> on the left of the mul sign.</param>
        /// <param name="value2">Source <see cref="Vec2{T}"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication.</returns>
        public static Vec2<T> operator *(Vec2<T> value1, Vec2<T> value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/> on the left of the mul sign.</param>
        /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vec2<T> operator *(Vec2<T> value, T scaleFactor)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="scaleFactor">Scalar value on the left of the mul sign.</param>
        /// <param name="value">Source <see cref="Vec2{T}"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vec2<T> operator *(T scaleFactor, Vec2<T> value)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vec2{T}"/> by the components of another <see cref="Vec2{T}"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/> on the left of the div sign.</param>
        /// <param name="value2">Divisor <see cref="Vec2{T}"/> on the right of the div sign.</param>
        /// <returns>The result of dividing the vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2<T> operator /(Vec2<T> value1, Vec2<T> value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vec2{T}"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/> on the left of the div sign.</param>
        /// <param name="divider">Divisor scalar on the right of the div sign.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2<T> operator /(Vec2<T> value1, T divider)
        {
            T factor = T.One / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        /// <summary>
        /// Compares whether two <see cref="Vec2{T}"/> instances are equal.
        /// </summary>
        /// <param name="value1"><see cref="Vec2{T}"/> instance on the left of the equal sign.</param>
        /// <param name="value2"><see cref="Vec2{T}"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Vec2<T> value1, Vec2<T> value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }

        /// <summary>
        /// Compares whether two <see cref="Vec2{T}"/> instances are not equal.
        /// </summary>
        /// <param name="value1"><see cref="Vec2{T}"/> instance on the left of the not equal sign.</param>
        /// <param name="value2"><see cref="Vec2{T}"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>	
        public static bool operator !=(Vec2<T> value1, Vec2<T> value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }

        /// <summary>
        /// Performs vector addition on <paramref name="value1"/> and <paramref name="value2"/>.
        /// </summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <returns>The result of the vector addition.</returns>
        public static Vec2<T> Add(Vec2<T> value1, Vec2<T> value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
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
        public static void Add(ref Vec2<T> value1, ref Vec2<T> value2, out Vec2<T> result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 2d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 2d-triangle.</param>
        /// <param name="value2">The second vector of 2d-triangle.</param>
        /// <param name="value3">The third vector of 2d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 2d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 2d-triangle.</param>
        /// <returns>The cartesian translation of barycentric coordinates.</returns>
        public static Vec2<T> Barycentric(Vec2<T> value1, Vec2<T> value2, Vec2<T> value3, T amount1, T amount2)
        {
            return new Vec2<T>(
                MathHelper.Barycentric<T>(value1.X, value2.X, value3.X, amount1, amount2),
                MathHelper.Barycentric<T>(value1.Y, value2.Y, value3.Y, amount1, amount2));
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 2d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 2d-triangle.</param>
        /// <param name="value2">The second vector of 2d-triangle.</param>
        /// <param name="value3">The third vector of 2d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 2d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 2d-triangle.</param>
        /// <param name="result">The cartesian translation of barycentric coordinates as an output parameter.</param>
        public static void Barycentric(ref Vec2<T> value1, ref Vec2<T> value2, ref Vec2<T> value3, T amount1, T amount2, out Vec2<T> result)
        {
            result.X = MathHelper.Barycentric<T>(value1.X, value2.X, value3.X, amount1, amount2);
            result.Y = MathHelper.Barycentric<T>(value1.Y, value2.Y, value3.Y, amount1, amount2);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The result of CatmullRom interpolation.</returns>
        public static Vec2<T> CatmullRom(Vec2<T> value1, Vec2<T> value2, Vec2<T> value3, Vec2<T> value4, T amount)
        {
            return new Vec2<T>(
                MathHelper.CatmullRom<T>(value1.X, value2.X, value3.X, value4.X, amount),
                MathHelper.CatmullRom<T>(value1.Y, value2.Y, value3.Y, value4.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The result of CatmullRom interpolation as an output parameter.</param>
        public static void CatmullRom(ref Vec2<T> value1, ref Vec2<T> value2, ref Vec2<T> value3, ref Vec2<T> value4, T amount, out Vec2<T> result)
        {
            result.X = MathHelper.CatmullRom<T>(value1.X, value2.X, value3.X, value4.X, amount);
            result.Y = MathHelper.CatmullRom<T>(value1.Y, value2.Y, value3.Y, value4.Y, amount);
        }

        /// <summary>
        /// Round the members of this <see cref="Vec2{T}"/> towards positive infinity.
        /// </summary>
        public void Ceiling()
        {
            X = T.Ceiling(X);
            Y = T.Ceiling(Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains members from another vector rounded towards positive infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
        /// <returns>The rounded <see cref="Vec2{T}"/>.</returns>
        public static Vec2<T> Ceiling(Vec2<T> value)
        {
            value.X = T.Ceiling(value.X);
            value.Y = T.Ceiling(value.Y);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains members from another vector rounded towards positive infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="result">The rounded <see cref="Vec2{T}"/>.</param>
        public static void Ceiling(ref Vec2<T> value, out Vec2<T> result)
        {
            result.X = T.Ceiling(value.X);
            result.Y = T.Ceiling(value.Y);
        }

        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>The clamped value.</returns>
        public static Vec2<T> Clamp(Vec2<T> value1, Vec2<T> min, Vec2<T> max)
        {
            return new Vec2<T>(
                T.Clamp(value1.X, min.X, max.X),
                T.Clamp(value1.Y, min.Y, max.Y));
        }

        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <param name="result">The clamped value as an output parameter.</param>
        public static void Clamp(ref Vec2<T> value1, ref Vec2<T> min, ref Vec2<T> max, out Vec2<T> result)
        {
            result.X = T.Clamp(value1.X, min.X, max.X);
            result.Y = T.Clamp(value1.Y, min.Y, max.Y);
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The distance between two vectors.</returns>
        public static T Distance(Vec2<T> value1, Vec2<T> value2)
        {
            T v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return T.Sqrt((v1 * v1) + (v2 * v2));
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The distance between two vectors as an output parameter.</param>
        public static void Distance(ref Vec2<T> value1, ref Vec2<T> value2, out T result)
        {
            T v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = T.Sqrt((v1 * v1) + (v2 * v2));
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The squared distance between two vectors.</returns>
        public static T DistanceSquared(Vec2<T> value1, Vec2<T> value2)
        {
            T v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return (v1 * v1) + (v2 * v2);
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The squared distance between two vectors as an output parameter.</param>
        public static void DistanceSquared(ref Vec2<T> value1, ref Vec2<T> value2, out T result)
        {
            T v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = (v1 * v1) + (v2 * v2);
        }

        /// <summary>
        /// Divides the components of a <see cref="Vec2{T}"/> by the components of another <see cref="Vec2{T}"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="value2">Divisor <see cref="Vec2{T}"/>.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Vec2<T> Divide(Vec2<T> value1, Vec2<T> value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vec2{T}"/> by the components of another <see cref="Vec2{T}"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="value2">Divisor <see cref="Vec2{T}"/>.</param>
        /// <param name="result">The result of dividing the vectors as an output parameter.</param>
        public static void Divide(ref Vec2<T> value1, ref Vec2<T> value2, out Vec2<T> result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vec2{T}"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Vec2<T> Divide(Vec2<T> value1, T divider)
        {
            T factor = T.One / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vec2{T}"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <param name="result">The result of dividing a vector by a scalar as an output parameter.</param>
        public static void Divide(ref Vec2<T> value1, T divider, out Vec2<T> result)
        {
            T factor = T.One / divider;
            result.X = value1.X * factor;
            result.Y = value1.Y * factor;
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        public static T Dot(Vec2<T> value1, Vec2<T> value2)
        {
            return (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The dot product of two vectors as an output parameter.</param>
        public static void Dot(ref Vec2<T> value1, ref Vec2<T> value2, out T result)
        {
            result = (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object? obj) {
            return obj is Vec2<T> vec2 && Equals(vec2);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Vec2{T}"/>.
        /// </summary>
        /// <param name="other">The <see cref="Vec2{T}"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Vec2<T> other)
        {
            return (X == other.X) && (Y == other.Y);
        }

        /// <summary>
        /// Round the members of this <see cref="Vec2{T}"/> towards negative infinity.
        /// </summary>
        public void Floor()
        {
            X = T.Floor(X);
            Y = T.Floor(Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains members from another vector rounded towards negative infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
        /// <returns>The rounded <see cref="Vec2{T}"/>.</returns>
        public static Vec2<T> Floor(Vec2<T> value)
        {
            value.X = T.Floor(value.X);
            value.Y = T.Floor(value.Y);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains members from another vector rounded towards negative infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="result">The rounded <see cref="Vec2{T}"/>.</param>
        public static void Floor(ref Vec2<T> value, out Vec2<T> result)
        {
            result.X = T.Floor(value.X);
            result.Y = T.Floor(value.Y);
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Vec2{T}"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Vec2{T}"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The hermite spline interpolation vector.</returns>
        public static Vec2<T> Hermite(Vec2<T> value1, Vec2<T> tangent1, Vec2<T> value2, Vec2<T> tangent2, T amount)
        {
            return new Vec2<T>(MathHelper.Hermite<T>(value1.X, tangent1.X, value2.X, tangent2.X, amount), MathHelper.Hermite<T>(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The hermite spline interpolation vector as an output parameter.</param>
        public static void Hermite(ref Vec2<T> value1, ref Vec2<T> tangent1, ref Vec2<T> value2, ref Vec2<T> tangent2, T amount, out Vec2<T> result)
        {
            result.X = MathHelper.Hermite<T>(value1.X, tangent1.X, value2.X, tangent2.X, amount);
            result.Y = MathHelper.Hermite<T>(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
        }

        /// <summary>
        /// Returns the length of this <see cref="Vec2{T}"/>.
        /// </summary>
        /// <returns>The length of this <see cref="Vec2{T}"/>.</returns>
        public T Length()
        {
            return T.Sqrt((X * X) + (Y * Y));
        }

        /// <summary>
        /// Returns the squared length of this <see cref="Vec2{T}"/>.
        /// </summary>
        /// <returns>The squared length of this <see cref="Vec2{T}"/>.</returns>
        public T LengthSquared()
        {
            return (X * X) + (Y * Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>The result of linear interpolation of the specified vectors.</returns>
        public static Vec2<T> Lerp(Vec2<T> value1, Vec2<T> value2, T amount)
        {
            return new Vec2<T>(
                MathHelper.Lerp<T>(value1.X, value2.X, amount),
                MathHelper.Lerp<T>(value1.Y, value2.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
        public static void Lerp(ref Vec2<T> value1, ref Vec2<T> value2, T amount, out Vec2<T> result)
        {
            result.X = MathHelper.Lerp<T>(value1.X, value2.X, amount);
            result.Y = MathHelper.Lerp<T>(value1.Y, value2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains linear interpolation of the specified vectors.
        /// Uses <see cref="MathHelper.LerpPrecise{T}"/> on MathHelper for the interpolation.
        /// Less efficient but more precise compared to <see cref="Vec2{T}.Lerp(Vec2{T}, Vec2{T}, T)"/>.
        /// See remarks section of <see cref="MathHelper.LerpPrecise{T}"/> on MathHelper for more info.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>The result of linear interpolation of the specified vectors.</returns>
        public static Vec2<T> LerpPrecise(Vec2<T> value1, Vec2<T> value2, T amount)
        {
            return new Vec2<T>(
                MathHelper.LerpPrecise<T>(value1.X, value2.X, amount),
                MathHelper.LerpPrecise<T>(value1.Y, value2.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains linear interpolation of the specified vectors.
        /// Uses <see cref="MathHelper.LerpPrecise{T}"/> on MathHelper for the interpolation.
        /// Less efficient but more precise compared to <see cref="Vec2{T}.Lerp(ref Vec2{T}, ref Vec2{T}, T, out Vec2{T})"/>.
        /// See remarks section of <see cref="MathHelper.LerpPrecise{T}"/> on MathHelper for more info.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
        public static void LerpPrecise(ref Vec2<T> value1, ref Vec2<T> value2, T amount, out Vec2<T> result)
        { 
            result.X = MathHelper.LerpPrecise<T>(value1.X, value2.X, amount);
            result.Y = MathHelper.LerpPrecise<T>(value1.Y, value2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Vec2{T}"/> with maximal values from the two vectors.</returns>
        public static Vec2<T> Max(Vec2<T> value1, Vec2<T> value2)
        {
            return new Vec2<T>(value1.X > value2.X ? value1.X : value2.X,
                               value1.Y > value2.Y ? value1.Y : value2.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Vec2{T}"/> with maximal values from the two vectors as an output parameter.</param>
        public static void Max(ref Vec2<T> value1, ref Vec2<T> value2, out Vec2<T> result)
        {
            result.X = value1.X > value2.X ? value1.X : value2.X;
            result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Vec2{T}"/> with minimal values from the two vectors.</returns>
        public static Vec2<T> Min(Vec2<T> value1, Vec2<T> value2)
        {
            return new Vec2<T>(value1.X < value2.X ? value1.X : value2.X,
                               value1.Y < value2.Y ? value1.Y : value2.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Vec2{T}"/> with minimal values from the two vectors as an output parameter.</param>
        public static void Min(ref Vec2<T> value1, ref Vec2<T> value2, out Vec2<T> result)
        {
            result.X = value1.X < value2.X ? value1.X : value2.X;
            result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="value2">Source <see cref="Vec2{T}"/>.</param>
        /// <returns>The result of the vector multiplication.</returns>
        public static Vec2<T> Multiply(Vec2<T> value1, Vec2<T> value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="value2">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="result">The result of the vector multiplication as an output parameter.</param>
        public static void Multiply(ref Vec2<T> value1, ref Vec2<T> value2, out Vec2<T> result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a multiplication of <see cref="Vec2{T}"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <returns>The result of the vector multiplication with a scalar.</returns>
        public static Vec2<T> Multiply(Vec2<T> value1, T scaleFactor)
        {
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a multiplication of <see cref="Vec2{T}"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">The result of the multiplication with a scalar as an output parameter.</param>
        public static void Multiply(ref Vec2<T> value1, T scaleFactor, out Vec2<T> result)
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains the specified vector inversion.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
        /// <returns>The result of the vector inversion.</returns>
        public static Vec2<T> Negate(Vec2<T> value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains the specified vector inversion.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="result">The result of the vector inversion as an output parameter.</param>
        public static void Negate(ref Vec2<T> value, out Vec2<T> result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
        }

        /// <summary>
        /// Turns this <see cref="Vec2{T}"/> to a unit vector with the same direction.
        /// </summary>
        public void Normalize()
        {
            T val = T.One / T.Sqrt((X * X) + (Y * Y));
            X *= val;
            Y *= val;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
        /// <returns>Unit vector.</returns>
        public static Vec2<T> Normalize(Vec2<T> value)
        {
            T val = T.One / T.Sqrt((value.X * value.X) + (value.Y * value.Y));
            value.X *= val;
            value.Y *= val;
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="result">Unit vector as an output parameter.</param>
        public static void Normalize(ref Vec2<T> value, out Vec2<T> result)
        {
            T val = T.One / T.Sqrt((value.X * value.X) + (value.Y * value.Y));
            result.X = value.X * val;
            result.Y = value.Y * val;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <returns>Reflected vector.</returns>
        public static Vec2<T> Reflect(Vec2<T> vector, Vec2<T> normal)
        {
            Vec2<T> result;
            T val = T.CreateChecked(2) * ((vector.X * normal.X) + (vector.Y * normal.Y));
            result.X = vector.X - (normal.X * val);
            result.Y = vector.Y - (normal.Y * val);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <param name="result">Reflected vector as an output parameter.</param>
        public static void Reflect(ref Vec2<T> vector, ref Vec2<T> normal, out Vec2<T> result)
        {
            T val = T.CreateChecked(2) * ((vector.X * normal.X) + (vector.Y * normal.Y));
            result.X = vector.X - (normal.X * val);
            result.Y = vector.Y - (normal.Y * val);
        }

        /// <summary>
        /// Round the members of this <see cref="Vec2{T}"/> to the nearest integer value.
        /// </summary>
        public void Round()
        {
            X = T.Round(X);
            Y = T.Round(Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains members from another vector rounded to the nearest integer value.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
        /// <returns>The rounded <see cref="Vec2{T}"/>.</returns>
        public static Vec2<T> Round(Vec2<T> value)
        {
            value.X = T.Round(value.X);
            value.Y = T.Round(value.Y);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains members from another vector rounded to the nearest integer value.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="result">The rounded <see cref="Vec2{T}"/>.</param>
        public static void Round(ref Vec2<T> value, out Vec2<T> result)
        {
            result.X = T.Round(value.X);
            result.Y = T.Round(value.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="value2">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <returns>Cubic interpolation of the specified vectors.</returns>
        public static Vec2<T> SmoothStep(Vec2<T> value1, Vec2<T> value2, T amount)
        {
            return new Vec2<T>(
                MathHelper.SmoothStep<T>(value1.X, value2.X, amount),
                MathHelper.SmoothStep<T>(value1.Y, value2.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="value2">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <param name="result">Cubic interpolation of the specified vectors as an output parameter.</param>
        public static void SmoothStep(ref Vec2<T> value1, ref Vec2<T> value2, T amount, out Vec2<T> result)
        {
            result.X = MathHelper.SmoothStep<T>(value1.X, value2.X, amount);
            result.Y = MathHelper.SmoothStep<T>(value1.Y, value2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains subtraction of on <see cref="Vec2{T}"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="value2">Source <see cref="Vec2{T}"/>.</param>
        /// <returns>The result of the vector subtraction.</returns>
        public static Vec2<T> Subtract(Vec2<T> value1, Vec2<T> value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains subtraction of on <see cref="Vec2{T}"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="value2">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="result">The result of the vector subtraction as an output parameter.</param>
        public static void Subtract(ref Vec2<T> value1, ref Vec2<T> value2, out Vec2<T> result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="Vec2{T}"/> in the format:
        /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>]}
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="Vec2{T}"/>.</returns>
        public override string ToString()
        {
            return "{X:" + X + " Y:" + Y + "}";
        }

        /// <summary>
        /// Gets a <see cref="Point"/> representation for this object.
        /// </summary>
        /// <returns>A <see cref="Point"/> representation for this object.</returns>
        public Point ToPoint()
        {
            return new Point(int.CreateChecked(X), int.CreateChecked(Y));
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
        /// <returns>Transformed <see cref="Vec2{T}"/>.</returns>
        public static Vec2<T> Transform(Vec2<T> position, Matrix<T> matrix)
        {
            return new Vec2<T>((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41, (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
        /// <param name="result">Transformed <see cref="Vec2{T}"/> as an output parameter.</param>
        public static void Transform(ref Vec2<T> position, ref Matrix<T> matrix, out Vec2<T> result)
        {
            T x = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41;
            T y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42;
            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a transformation of 2d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <returns>Transformed <see cref="Vec2{T}"/>.</returns>
        public static Vec2<T> Transform(Vec2<T> value, Quaternion<T> rotation)
        {
            Transform(ref value, ref rotation, out value);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a transformation of 2d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2{T}"/>.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="result">Transformed <see cref="Vec2{T}"/> as an output parameter.</param>
        public static void Transform(ref Vec2<T> value, ref Quaternion<T> rotation, out Vec2<T> result)
        {
            Vec3<T> rot1 = new Vec3<T>(rotation.X + rotation.X, rotation.Y + rotation.Y, rotation.Z + rotation.Z);
            Vec3<T> rot2 = new Vec3<T>(rotation.X, rotation.X, rotation.W);
            Vec3<T> rot3 = new Vec3<T>(T.One, rotation.Y, rotation.Z);
            Vec3<T> rot4 = rot1*rot2;
            Vec3<T> rot5 = rot1*rot3;

            Vec2<T> v = new Vec2<T>();
            v.X = T.CreateChecked(
                double.CreateChecked(value.X) * (1.0d - double.CreateChecked(rot5.Y) - double.CreateChecked(rot5.Z)) + double.CreateChecked(value.Y) * (double.CreateChecked(rot4.Y) - double.CreateChecked(rot4.Z)));
            v.Y = T.CreateChecked(double.CreateChecked(value.X) * (double.CreateChecked(rot4.Y) + double.CreateChecked(rot4.Z)) + double.CreateChecked(value.Y) * (1.0d - double.CreateChecked(rot4.X) - double.CreateChecked(rot5.Z)));
            result.X = v.X;
            result.Y = v.Y;
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Vec2{T}"/> by the specified <see cref="Matrix{T}"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vec2{T}"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(
            Vec2<T>[] sourceArray,
            int sourceIndex,
            ref Matrix<T> matrix,
            Vec2<T>[] destinationArray,
            int destinationIndex,
            int length)
        {
            if (sourceArray == null)
                throw new ArgumentNullException(nameof(sourceArray));
            if (destinationArray == null)
                throw new ArgumentNullException(nameof(destinationArray));
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            for (int x = 0; x < length; x++)
            {
                Vec2<T> position = sourceArray[sourceIndex + x];
                Vec2<T> destination = destinationArray[destinationIndex + x];
                destination.X = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41;
                destination.Y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42;
                destinationArray[destinationIndex + x] = destination;
            }
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Vec2{T}"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vec2{T}"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform
        (
            Vec2<T>[] sourceArray,
            int sourceIndex,
            ref Quaternion<T> rotation,
            Vec2<T>[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            if (sourceArray == null)
                throw new ArgumentNullException(nameof(sourceArray));
            if (destinationArray == null)
                throw new ArgumentNullException(nameof(destinationArray));
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            for (int x = 0; x < length; x++)
            {
                var position = sourceArray[sourceIndex + x];
                var destination = destinationArray[destinationIndex + x];

                Vec2<T> v;
                Transform(ref position,ref rotation,out v); 

                destination.X = v.X;
                destination.Y = v.Y;

                destinationArray[destinationIndex + x] = destination;
            }
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Vec2{T}"/> by the specified <see cref="Matrix{T}"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(
            Vec2<T>[] sourceArray,
            ref Matrix<T> matrix,
            Vec2<T>[] destinationArray)
        {
            Transform(sourceArray, 0, ref matrix, destinationArray, 0, sourceArray.Length);
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Vec2{T}"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform
        (
            Vec2<T>[] sourceArray,
            ref Quaternion<T> rotation,
            Vec2<T>[] destinationArray
        )
        {
            Transform(sourceArray, 0, ref rotation, destinationArray, 0, sourceArray.Length);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a transformation of the specified normal by the specified <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Vec2{T}"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
        /// <returns>Transformed normal.</returns>
        public static Vec2<T> TransformNormal(Vec2<T> normal, Matrix<T> matrix)
        {
            return new Vec2<T>((normal.X * matrix.M11) + (normal.Y * matrix.M21),(normal.X * matrix.M12) + (normal.Y * matrix.M22));
        }

        /// <summary>
        /// Creates a new <see cref="Vec2{T}"/> that contains a transformation of the specified normal by the specified <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Vec2{T}"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
        /// <param name="result">Transformed normal as an output parameter.</param>
        public static void TransformNormal(ref Vec2<T> normal, ref Matrix<T> matrix, out Vec2<T> result)
        {
            var x = (normal.X * matrix.M11) + (normal.Y * matrix.M21);
            var y = (normal.X * matrix.M12) + (normal.Y * matrix.M22);
            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Apply transformation on normals within array of <see cref="Vec2{T}"/> by the specified <see cref="Matrix{T}"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vec2{T}"/> should be written.</param>
        /// <param name="length">The number of normals to be transformed.</param>
        public static void TransformNormal
        (
            Vec2<T>[] sourceArray,
            int sourceIndex,
            ref Matrix<T> matrix,
            Vec2<T>[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            if (sourceArray == null)
                throw new ArgumentNullException(nameof(sourceArray));
            if (destinationArray == null)
                throw new ArgumentNullException(nameof(destinationArray));
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            for (int i = 0; i < length; i++)
            {
                var normal = sourceArray[sourceIndex + i];

                destinationArray[destinationIndex + i] = new Vec2<T>((normal.X * matrix.M11) + (normal.Y * matrix.M21),
                                                                     (normal.X * matrix.M12) + (normal.Y * matrix.M22));
            }
        }

        /// <summary>
        /// Apply transformation on all normals within array of <see cref="Vec2{T}"/> by the specified <see cref="Matrix{T}"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void TransformNormal
            (
            Vec2<T>[] sourceArray,
            ref Matrix<T> matrix,
            Vec2<T>[] destinationArray
            )
        {
            if (sourceArray == null)
                throw new ArgumentNullException(nameof(sourceArray));
            if (destinationArray == null)
                throw new ArgumentNullException(nameof(destinationArray));
            if (destinationArray.Length < sourceArray.Length)
                throw new ArgumentException("Destination array length is lesser than source array length");

            for (int i = 0; i < sourceArray.Length; i++)
            {
                var normal = sourceArray[i];

                destinationArray[i] = new Vec2<T>((normal.X * matrix.M11) + (normal.Y * matrix.M21),
                                                  (normal.X * matrix.M12) + (normal.Y * matrix.M22));
            }
        }

        /// <summary>
        /// Deconstruction method for <see cref="Vec2{T}"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Deconstruct(out T x, out T y)
        {
            x = X;
            y = Y;
        }

        /// <summary>
        /// Returns a <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        public Vector2 ToNumerics()
        {
            return new Vector2(float.CreateChecked(this.X), float.CreateChecked(this.Y));
        }

    }
}
