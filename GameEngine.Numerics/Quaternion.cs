using System.Diagnostics;
using System.Numerics;
using GameEngine.Core.Serialization;

namespace GameEngine.Numerics
{
    /// <summary>
    /// An efficient mathematical representation for three dimensional rotations.
    /// </summary>
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct Quaternion<T> : IEquatable<Quaternion<T>> where T : struct, IFloatingPointIeee754<T>
    {
        
	    public Vec3<T> ToEulerAngles() {
    	    T roll = T.Atan2(T.CreateChecked(2) * (X * Y + Z * W), (T.One - T.CreateChecked(2) * (Y * Y + Z * Z)));
    	    T pitch = T.Asin(T.CreateChecked(2) * (X * Z - W * Y));
    	    T yaw = T.Atan2(T.CreateChecked(2) * (X * W + Y * Z), (T.One - T.CreateChecked(2) * (Z * Z + W * W)));
    	    
    	    return new Vec3<T>(pitch, yaw, roll);
    	}
	    
	    // rotates point by quaternion
	    public static Vec3<T> operator *(Quaternion<T> rotation, Vec3<T> point)
	    {
		    T num1 = rotation.X * T.CreateChecked(2f);
		    T num2 = rotation.Y * T.CreateChecked(2f);
		    T num3 = rotation.Z * T.CreateChecked(2f);
		    T num4 = rotation.X * num1;
		    T num5 = rotation.Y * num2;
		    T num6 = rotation.Z * num3;
		    T num7 = rotation.X * num2;
		    T num8 = rotation.X * num3;
		    T num9 = rotation.Y * num3;
		    T num10 = rotation.W * num1;
		    T num11 = rotation.W * num2;
		    T num12 = rotation.W * num3;
		    Vec3<T> vector3;
		    
		    vector3.X = ((T.One - (num5 + num6)) * point.X + (num7 - num12) * point.Y + (num8 + num11) * point.Z);
		    vector3.Y = ((num7 + num12) * point.X + (T.One - (num4 + num6)) * point.Y + (num9 - num10) * point.Z);
		    vector3.Z = ((num8 - num11) * point.X + (num9 + num10) * point.Y + (T.One - (num4 + num5)) * point.Z);
		    
//		    vector3.X = (float) ((T.One - ((double) num5 + (double) num6)) * (double) point.X + ((double) num7 - (double) num12) * (double) point.Y + ((double) num8 + (double) num11) * (double) point.Z);
//		    vector3.Y = (float) (((double) num7 + (double) num12) * (double) point.X + (T.One - ((double) num4 + (double) num6)) * (double) point.Y + ((double) num9 - (double) num10) * (double) point.Z);
//		    vector3.Z = (float) (((double) num8 - (double) num11) * (double) point.X + ((double) num9 + (double) num10) * (double) point.Y + (T.One - ((double) num4 + (double) num5)) * (double) point.Z);
		    return vector3;
	    }
	    
        [Serialized] public T X;
        [Serialized] public T Y;
        [Serialized] public T Z;
        [Serialized] public T W;
		
        /// <summary>
        /// Constructs a quaternion with X, Y, Z and W from four values.
        /// </summary>
        /// <param name="x">The x coordinate in 3d-space.</param>
        /// <param name="y">The y coordinate in 3d-space.</param>
        /// <param name="z">The z coordinate in 3d-space.</param>
        /// <param name="w">The rotation component.</param>
        public Quaternion(T x, T y, T z, T w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Constructs a quaternion with X, Y, Z from <see cref="Vec3{T}"/> and rotation component from a scalar.
        /// </summary>
        /// <param name="value">The x, y, z coordinates in 3d-space.</param>
        /// <param name="w">The rotation component.</param>
        public Quaternion(Vec3<T> value, T w)
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = value.Z;
            this.W = w;
        }

        /// <summary>
        /// Constructs a quaternion from <see cref="Vec4{T}"/>.
        /// </summary>
        /// <param name="value">The x, y, z coordinates in 3d-space and the rotation component.</param>
        public Quaternion(Vec4<T> value)
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = value.Z;
            this.W = value.W;
        }
		
        /// <summary>
        /// Returns a quaternion representing no rotation.
        /// </summary>
        public static Quaternion<T> Identity => new Quaternion<T>(T.Zero, T.Zero, T.Zero, T.One);
        
        internal string DebugDisplayString
        {
            get
            {
                if (this == Quaternion<T>.Identity)
                {
                    return "Identity";
                }

                return string.Concat(
                    this.X.ToString(), " ",
                    this.Y.ToString(), " ",
                    this.Z.ToString(), " ",
                    this.W.ToString()
                );
            }
        }
		
        /// <summary>
        /// Creates a new <see cref="Quaternion"/> that contains the sum of two quaternions.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="quaternion2">Source <see cref="Quaternion"/>.</param>
        /// <returns>The result of the quaternion addition.</returns>
        public static Quaternion<T> Add(Quaternion<T> quaternion1, Quaternion<T> quaternion2)
        {
			Quaternion<T> quaternion;
			quaternion.X = quaternion1.X + quaternion2.X;
			quaternion.Y = quaternion1.Y + quaternion2.Y;
			quaternion.Z = quaternion1.Z + quaternion2.Z;
			quaternion.W = quaternion1.W + quaternion2.W;
			return quaternion;
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> that contains the sum of two quaternions.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="quaternion2">Source <see cref="Quaternion"/>.</param>
        /// <param name="result">The result of the quaternion addition as an output parameter.</param>
        public static void Add(ref Quaternion<T> quaternion1, ref Quaternion<T> quaternion2, out Quaternion<T> result)
        {
			result.X = quaternion1.X + quaternion2.X;
			result.Y = quaternion1.Y + quaternion2.Y;
			result.Z = quaternion1.Z + quaternion2.Z;
			result.W = quaternion1.W + quaternion2.W;
        }
		
        /// <summary>
        /// Creates a new <see cref="Quaternion"/> that contains concatenation between two quaternion.
        /// </summary>
        /// <param name="value1">The first <see cref="Quaternion"/> to concatenate.</param>
        /// <param name="value2">The second <see cref="Quaternion"/> to concatenate.</param>
        /// <returns>The result of rotation of <paramref name="value1"/> followed by <paramref name="value2"/> rotation.</returns>
        public static Quaternion<T> Concatenate(Quaternion<T> value1, Quaternion<T> value2)
		{
			Quaternion<T> quaternion;

            T x1 = value1.X;
            T y1 = value1.Y;
            T z1 = value1.Z;
            T w1 = value1.W;

            T x2 = value2.X;
		    T y2 = value2.Y;
		    T z2 = value2.Z;
		    T w2 = value2.W;

		    quaternion.X = ((x2 * w1) + (x1 * w2)) + ((y2 * z1) - (z2 * y1));
		    quaternion.Y = ((y2 * w1) + (y1 * w2)) + ((z2 * x1) - (x2 * z1));
		    quaternion.Z = ((z2 * w1) + (z1 * w2)) + ((x2 * y1) - (y2 * x1));
		    quaternion.W = (w2 * w1) - (((x2 * x1) + (y2 * y1)) + (z2 * z1));

		    return quaternion;
		}

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> that contains concatenation between two quaternion.
        /// </summary>
        /// <param name="value1">The first <see cref="Quaternion"/> to concatenate.</param>
        /// <param name="value2">The second <see cref="Quaternion"/> to concatenate.</param>
        /// <param name="result">The result of rotation of <paramref name="value1"/> followed by <paramref name="value2"/> rotation as an output parameter.</param>
        public static void Concatenate(ref Quaternion<T> value1, ref Quaternion<T> value2, out Quaternion<T> result)
		{
            T x1 = value1.X;
            T y1 = value1.Y;
            T z1 = value1.Z;
            T w1 = value1.W;

            T x2 = value2.X;
            T y2 = value2.Y;
            T z2 = value2.Z;
            T w2 = value2.W;

            result.X = ((x2 * w1) + (x1 * w2)) + ((y2 * z1) - (z2 * y1));
            result.Y = ((y2 * w1) + (y1 * w2)) + ((z2 * x1) - (x2 * z1));
            result.Z = ((z2 * w1) + (z1 * w2)) + ((x2 * y1) - (y2 * x1));
            result.W = (w2 * w1) - (((x2 * x1) + (y2 * y1)) + (z2 * z1));
        }

        /// <summary>
        /// Transforms this quaternion into its conjugated version.
        /// </summary>
        public void Conjugate()
		{
			X = -X;
			Y = -Y;
			Z = -Z;
		}

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> that contains conjugated version of the specified quaternion.
        /// </summary>
        /// <param name="value">The quaternion which values will be used to create the conjugated version.</param>
        /// <returns>The conjugate version of the specified quaternion.</returns>
        public static Quaternion<T> Conjugate(Quaternion<T> value)
		{
			return new Quaternion<T>(-value.X, -value.Y, -value.Z, value.W);
		}

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> that contains conjugated version of the specified quaternion.
        /// </summary>
        /// <param name="value">The quaternion which values will be used to create the conjugated version.</param>
        /// <param name="result">The conjugated version of the specified quaternion as an output parameter.</param>
        public static void Conjugate(ref Quaternion<T> value, out Quaternion<T> result)
		{
			result.X = -value.X;
			result.Y = -value.Y;
			result.Z = -value.Z;
			result.W = value.W;
		}

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> from the specified axis and angle.
        /// </summary>
        /// <param name="axis">The axis of rotation.</param>
        /// <param name="angle">The angle in radians.</param>
        /// <returns>The new quaternion builded from axis and angle.</returns>
        public static Quaternion<T> CreateFromAxisAngle(Vec3<T> axis, T angle)
        {
		    T half = angle * T.CreateChecked(0.5);
		    T sin = T.Sin(half);
		    T cos = T.Cos(half);
		    return new Quaternion<T>(axis.X * sin, axis.Y * sin, axis.Z * sin, cos);
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> from the specified axis and angle.
        /// </summary>
        /// <param name="axis">The axis of rotation.</param>
        /// <param name="angle">The angle in radians.</param>
        /// <param name="result">The new quaternion builded from axis and angle as an output parameter.</param>
        public static void CreateFromAxisAngle(ref Vec3<T> axis, T angle, out Quaternion<T> result)
        {
            T half = angle * T.CreateChecked(0.5);
		    T sin = T.Sin(half);
		    T cos = T.Cos(half);
		    result.X = axis.X * sin;
		    result.Y = axis.Y * sin;
		    result.Z = axis.Z * sin;
		    result.W = cos;
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> from the specified <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <returns>A quaternion composed from the rotation part of the matrix.</returns>
        public static Quaternion<T> CreateFromRotationMatrix(Matrix<T> matrix)
        {
            Quaternion<T> quaternion;
            T sqrt;
            T half;
            T scale = matrix.M11 + matrix.M22 + matrix.M33;

		    if (scale > T.Zero)
		    {
                sqrt = T.Sqrt(scale + T.One);
		        quaternion.W = sqrt * T.CreateChecked(0.5);
                sqrt = T.CreateChecked(0.5) / sqrt;

		        quaternion.X = (matrix.M23 - matrix.M32) * sqrt;
		        quaternion.Y = (matrix.M31 - matrix.M13) * sqrt;
		        quaternion.Z = (matrix.M12 - matrix.M21) * sqrt;

		        return quaternion;
		    }
		    if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
		    {
                sqrt = T.Sqrt(T.One + matrix.M11 - matrix.M22 - matrix.M33);
                half = T.CreateChecked(0.5) / sqrt;

		        quaternion.X = T.CreateChecked(0.5) * sqrt;
		        quaternion.Y = (matrix.M12 + matrix.M21) * half;
		        quaternion.Z = (matrix.M13 + matrix.M31) * half;
		        quaternion.W = (matrix.M23 - matrix.M32) * half;

		        return quaternion;
		    }
		    if (matrix.M22 > matrix.M33)
		    {
                sqrt = T.Sqrt(T.One + matrix.M22 - matrix.M11 - matrix.M33);
                half = T.CreateChecked(0.5) / sqrt;

		        quaternion.X = (matrix.M21 + matrix.M12) * half;
		        quaternion.Y = T.CreateChecked(0.5) * sqrt;
		        quaternion.Z = (matrix.M32 + matrix.M23) * half;
		        quaternion.W = (matrix.M31 - matrix.M13) * half;

		        return quaternion;
		    }
            sqrt = T.Sqrt(T.One + matrix.M33 - matrix.M11 - matrix.M22);
		    half = T.CreateChecked(0.5) / sqrt;

		    quaternion.X = (matrix.M31 + matrix.M13) * half;
		    quaternion.Y = (matrix.M32 + matrix.M23) * half;
		    quaternion.Z = T.CreateChecked(0.5) * sqrt;
		    quaternion.W = (matrix.M12 - matrix.M21) * half;

		    return quaternion;
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> from the specified <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <param name="result">A quaternion composed from the rotation part of the matrix as an output parameter.</param>
        public static void CreateFromRotationMatrix(ref Matrix<T> matrix, out Quaternion<T> result)
        {
            T sqrt;
            T half;
            T scale = matrix.M11 + matrix.M22 + matrix.M33;

            if (scale > T.Zero)
            {
                sqrt = T.Sqrt(scale + T.One);
                result.W = sqrt * T.CreateChecked(0.5);
                sqrt = T.CreateChecked(0.5) / sqrt;

                result.X = (matrix.M23 - matrix.M32) * sqrt;
                result.Y = (matrix.M31 - matrix.M13) * sqrt;
                result.Z = (matrix.M12 - matrix.M21) * sqrt;
            }
            else
            if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
            {
                sqrt = T.Sqrt(T.One + matrix.M11 - matrix.M22 - matrix.M33);
                half = T.CreateChecked(0.5) / sqrt;

                result.X = T.CreateChecked(0.5) * sqrt;
                result.Y = (matrix.M12 + matrix.M21) * half;
                result.Z = (matrix.M13 + matrix.M31) * half;
                result.W = (matrix.M23 - matrix.M32) * half;
            }
            else if (matrix.M22 > matrix.M33)
            {
                sqrt = T.Sqrt(T.One + matrix.M22 - matrix.M11 - matrix.M33);
                half = T.CreateChecked(0.5) / sqrt;

                result.X = (matrix.M21 + matrix.M12)*half;
                result.Y = T.CreateChecked(0.5) * sqrt;
                result.Z = (matrix.M32 + matrix.M23)*half;
                result.W = (matrix.M31 - matrix.M13)*half;
            }
            else
            {
                sqrt = T.Sqrt(T.One + matrix.M33 - matrix.M11 - matrix.M22);
                half = T.CreateChecked(0.5) / sqrt;

                result.X = (matrix.M31 + matrix.M13) * half;
                result.Y = (matrix.M32 + matrix.M23) * half;
                result.Z = T.CreateChecked(0.5) * sqrt;
                result.W = (matrix.M12 - matrix.M21) * half;
            }
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> from the specified yaw, pitch and roll angles.
        /// </summary>
        /// <param name="yaw">Yaw around the y axis in radians.</param>
        /// <param name="pitch">Pitch around the x axis in radians.</param>
        /// <param name="roll">Roll around the z axis in radians.</param>
        /// <returns>A new quaternion from the concatenated yaw, pitch, and roll angles.</returns>
        public static Quaternion<T> CreateFromYawPitchRoll(T yaw, T pitch, T roll)
		{
            T halfRoll = roll * T.CreateChecked(0.5);
            T halfPitch = pitch * T.CreateChecked(0.5);
            T halfYaw = yaw * T.CreateChecked(0.5);

            T sinRoll = T.Sin(halfRoll);
            T cosRoll = T.Cos(halfRoll);
            T sinPitch = T.Sin(halfPitch);
            T cosPitch = T.Cos(halfPitch);
            T sinYaw = T.Sin(halfYaw);
            T cosYaw = T.Cos(halfYaw);

            return new Quaternion<T>((cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll),
                                  (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll),
                                  (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll),
                                  (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll));
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> from the specified yaw, pitch and roll angles.
        /// </summary>
        /// <param name="yaw">Yaw around the y axis in radians.</param>
        /// <param name="pitch">Pitch around the x axis in radians.</param>
        /// <param name="roll">Roll around the z axis in radians.</param>
        /// <param name="result">A new quaternion from the concatenated yaw, pitch, and roll angles as an output parameter.</param>
 		public static void CreateFromYawPitchRoll(T yaw, T pitch, T roll, out Quaternion<T> result)
		{
            T halfRoll = roll * T.CreateChecked(0.5);
            T halfPitch = pitch * T.CreateChecked(0.5);
            T halfYaw = yaw * T.CreateChecked(0.5);

            T sinRoll = T.Sin(halfRoll);
            T cosRoll = T.Cos(halfRoll);
            T sinPitch = T.Sin(halfPitch);
            T cosPitch = T.Cos(halfPitch);
            T sinYaw = T.Sin(halfYaw);
            T cosYaw = T.Cos(halfYaw);

            result.X = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);
            result.Y = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
            result.Z = (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll);
            result.W = (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll);
        }

        /// <summary>
        /// Divides a <see cref="Quaternion"/> by the other <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="quaternion2">Divisor <see cref="Quaternion"/>.</param>
        /// <returns>The result of dividing the quaternions.</returns>
        public static Quaternion<T> Divide(Quaternion<T> quaternion1, Quaternion<T> quaternion2)
        {
            Quaternion<T> quaternion;
		    T x = quaternion1.X;
		    T y = quaternion1.Y;
		    T z = quaternion1.Z;
		    T w = quaternion1.W;
		    T num14 = (((quaternion2.X * quaternion2.X) + (quaternion2.Y * quaternion2.Y)) + (quaternion2.Z * quaternion2.Z)) + (quaternion2.W * quaternion2.W);
		    T num5 = T.One / num14;
		    T num4 = -quaternion2.X * num5;
		    T num3 = -quaternion2.Y * num5;
		    T num2 = -quaternion2.Z * num5;
		    T num = quaternion2.W * num5;
		    T num13 = (y * num2) - (z * num3);
		    T num12 = (z * num4) - (x * num2);
		    T num11 = (x * num3) - (y * num4);
		    T num10 = ((x * num4) + (y * num3)) + (z * num2);
		    quaternion.X = ((x * num) + (num4 * w)) + num13;
		    quaternion.Y = ((y * num) + (num3 * w)) + num12;
		    quaternion.Z = ((z * num) + (num2 * w)) + num11;
		    quaternion.W = (w * num) - num10;
		    return quaternion;
        }

        /// <summary>
        /// Divides a <see cref="Quaternion"/> by the other <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="quaternion2">Divisor <see cref="Quaternion"/>.</param>
        /// <param name="result">The result of dividing the quaternions as an output parameter.</param>
        public static void Divide(ref Quaternion<T> quaternion1, ref Quaternion<T> quaternion2, out Quaternion<T> result)
        {
            T x = quaternion1.X;
		    T y = quaternion1.Y;
		    T z = quaternion1.Z;
		    T w = quaternion1.W;
		    T num14 = (((quaternion2.X * quaternion2.X) + (quaternion2.Y * quaternion2.Y)) + (quaternion2.Z * quaternion2.Z)) + (quaternion2.W * quaternion2.W);
		    T num5 = T.One / num14;
		    T num4 = -quaternion2.X * num5;
		    T num3 = -quaternion2.Y * num5;
		    T num2 = -quaternion2.Z * num5;
		    T num = quaternion2.W * num5;
		    T num13 = (y * num2) - (z * num3);
		    T num12 = (z * num4) - (x * num2);
		    T num11 = (x * num3) - (y * num4);
		    T num10 = ((x * num4) + (y * num3)) + (z * num2);
		    result.X = ((x * num) + (num4 * w)) + num13;
		    result.Y = ((y * num) + (num3 * w)) + num12;
		    result.Z = ((z * num) + (num2 * w)) + num11;
		    result.W = (w * num) - num10;
        }

        /// <summary>
        /// Returns a dot product of two quaternions.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <returns>The dot product of two quaternions.</returns>
        public static T Dot(Quaternion<T> quaternion1, Quaternion<T> quaternion2)
        {
            return ((((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W));
        }

        /// <summary>
        /// Returns a dot product of two quaternions.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <param name="result">The dot product of two quaternions as an output parameter.</param>
        public static void Dot(ref Quaternion<T> quaternion1, ref Quaternion<T> quaternion2, out T result)
        {
            result = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is Quaternion<T> quaternion)
                return Equals(quaternion);
            return false;
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="other">The <see cref="Quaternion"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Quaternion<T> other)
        {
			return X == other.X &&
                   Y == other.Y &&
                   Z == other.Z &&
                   W == other.W;
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Quaternion"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Quaternion"/>.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
        }

        /// <summary>
        /// Returns the inverse quaternion which represents the opposite rotation.
        /// </summary>
        /// <param name="quaternion">Source <see cref="Quaternion"/>.</param>
        /// <returns>The inverse quaternion.</returns>
        public static Quaternion<T> Inverse(Quaternion<T> quaternion)
        {
            Quaternion<T> quaternion2;
		    T num2 = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
		    T num = T.One / num2;
		    quaternion2.X = -quaternion.X * num;
		    quaternion2.Y = -quaternion.Y * num;
		    quaternion2.Z = -quaternion.Z * num;
		    quaternion2.W = quaternion.W * num;
		    return quaternion2;
        }

        /// <summary>
        /// Returns the inverse quaternion which represents the opposite rotation.
        /// </summary>
        /// <param name="quaternion">Source <see cref="Quaternion"/>.</param>
        /// <param name="result">The inverse quaternion as an output parameter.</param>
        public static void Inverse(ref Quaternion<T> quaternion, out Quaternion<T> result)
        {
            T num2 = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
		    T num = T.One / num2;
		    result.X = -quaternion.X * num;
		    result.Y = -quaternion.Y * num;
		    result.Z = -quaternion.Z * num;
		    result.W = quaternion.W * num;
        }

        /// <summary>
        /// Returns the magnitude of the quaternion components.
        /// </summary>
        /// <returns>The magnitude of the quaternion components.</returns>
        public T Length()
        {
    		return T.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
        }

        /// <summary>
        /// Returns the squared magnitude of the quaternion components.
        /// </summary>
        /// <returns>The squared magnitude of the quaternion components.</returns>
        public T LengthSquared()
        {
            return (X * X) + (Y * Y) + (Z * Z) + (W * W);
        }

        /// <summary>
        /// Performs a linear blend between two quaternions.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="quaternion2">Source <see cref="Quaternion"/>.</param>
        /// <param name="amount">The blend amount where 0 returns <paramref name="quaternion1"/> and 1 <paramref name="quaternion2"/>.</param>
        /// <returns>The result of linear blending between two quaternions.</returns>
        public static Quaternion<T> Lerp(Quaternion<T> quaternion1, Quaternion<T> quaternion2, T amount)
        {
            T num = amount;
		    T num2 = T.One - num;
		    Quaternion<T> quaternion = new Quaternion<T>();
		    T num5 = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
		    if (num5 >= T.Zero)
		    {
		        quaternion.X = (num2 * quaternion1.X) + (num * quaternion2.X);
		        quaternion.Y = (num2 * quaternion1.Y) + (num * quaternion2.Y);
		        quaternion.Z = (num2 * quaternion1.Z) + (num * quaternion2.Z);
		        quaternion.W = (num2 * quaternion1.W) + (num * quaternion2.W);
		    }
		    else
		    {
		        quaternion.X = (num2 * quaternion1.X) - (num * quaternion2.X);
		        quaternion.Y = (num2 * quaternion1.Y) - (num * quaternion2.Y);
		        quaternion.Z = (num2 * quaternion1.Z) - (num * quaternion2.Z);
		        quaternion.W = (num2 * quaternion1.W) - (num * quaternion2.W);
		    }
		    T num4 = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
		    T num3 = T.One / T.Sqrt(num4);
		    quaternion.X *= num3;
		    quaternion.Y *= num3;
		    quaternion.Z *= num3;
		    quaternion.W *= num3;
		    return quaternion;
        }

        /// <summary>
        /// Performs a linear blend between two quaternions.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="quaternion2">Source <see cref="Quaternion"/>.</param>
        /// <param name="amount">The blend amount where 0 returns <paramref name="quaternion1"/> and 1 <paramref name="quaternion2"/>.</param>
        /// <param name="result">The result of linear blending between two quaternions as an output parameter.</param>
        public static void Lerp(ref Quaternion<T> quaternion1, ref Quaternion<T> quaternion2, T amount, out Quaternion<T> result)
        {
            T num = amount;
		    T num2 = T.One - num;
		    T num5 = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
		    if (num5 >= T.Zero)
		    {
		        result.X = (num2 * quaternion1.X) + (num * quaternion2.X);
		        result.Y = (num2 * quaternion1.Y) + (num * quaternion2.Y);
		        result.Z = (num2 * quaternion1.Z) + (num * quaternion2.Z);
		        result.W = (num2 * quaternion1.W) + (num * quaternion2.W);
		    }
		    else
		    {
		        result.X = (num2 * quaternion1.X) - (num * quaternion2.X);
		        result.Y = (num2 * quaternion1.Y) - (num * quaternion2.Y);
		        result.Z = (num2 * quaternion1.Z) - (num * quaternion2.Z);
		        result.W = (num2 * quaternion1.W) - (num * quaternion2.W);
		    }
		    T num4 = (((result.X * result.X) + (result.Y * result.Y)) + (result.Z * result.Z)) + (result.W * result.W);
		    T num3 = T.One / T.Sqrt(num4);
		    result.X *= num3;
		    result.Y *= num3;
		    result.Z *= num3;
		    result.W *= num3;

        }

        /// <summary>
        /// Performs a spherical linear blend between two quaternions.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="quaternion2">Source <see cref="Quaternion"/>.</param>
        /// <param name="amount">The blend amount where 0 returns <paramref name="quaternion1"/> and 1 <paramref name="quaternion2"/>.</param>
        /// <returns>The result of spherical linear blending between two quaternions.</returns>
        public static Quaternion<T> Slerp(Quaternion<T> quaternion1, Quaternion<T> quaternion2, T amount)
        {
            T num2;
		    T num3;
		    Quaternion<T> quaternion;
		    T num = amount;
		    T num4 = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
		    bool flag = false;
		    if (num4 < T.Zero)
		    {
		        flag = true;
		        num4 = -num4;
		    }
		    if (num4 > T.CreateChecked(0.999999f))
		    {
		        num3 = T.One - num;
		        num2 = flag ? -num : num;
		    }
		    else
		    {
		        T num5 = T.Acos(num4);
		        T num6 = T.One / T.Sin(num5);
		        num3 = T.Sin((T.One - num) * num5) * num6;
		        num2 = flag ? (-T.Sin(num * num5) * num6) : (T.Sin(num * num5) * num6);
		    }
		    quaternion.X = (num3 * quaternion1.X) + (num2 * quaternion2.X);
		    quaternion.Y = (num3 * quaternion1.Y) + (num2 * quaternion2.Y);
		    quaternion.Z = (num3 * quaternion1.Z) + (num2 * quaternion2.Z);
		    quaternion.W = (num3 * quaternion1.W) + (num2 * quaternion2.W);
		    return quaternion;
        }

        /// <summary>
        /// Performs a spherical linear blend between two quaternions.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="quaternion2">Source <see cref="Quaternion"/>.</param>
        /// <param name="amount">The blend amount where 0 returns <paramref name="quaternion1"/> and 1 <paramref name="quaternion2"/>.</param>
        /// <param name="result">The result of spherical linear blending between two quaternions as an output parameter.</param>
        public static void Slerp(ref Quaternion<T> quaternion1, ref Quaternion<T> quaternion2, T amount, out Quaternion<T> result)
        {
            T num2;
		    T num3;
		    T num = amount;
		    T num4 = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
		    bool flag = false;
		    if (num4 < T.Zero)
		    {
		        flag = true;
		        num4 = -num4;
		    }
		    if (num4 > T.CreateChecked(0.999999f))
		    {
		        num3 = T.One - num;
		        num2 = flag ? -num : num;
		    }
		    else
		    {
		        T num5 = T.Acos(num4);
		        T num6 = T.One / T.Sin(num5);
		        num3 = T.Sin((T.One - num) * num5) * num6;
		        num2 = flag ? (-T.Sin(num * num5) * num6) : (T.Sin(num * num5) * num6);
		    }
		    result.X = (num3 * quaternion1.X) + (num2 * quaternion2.X);
		    result.Y = (num3 * quaternion1.Y) + (num2 * quaternion2.Y);
		    result.Z = (num3 * quaternion1.Z) + (num2 * quaternion2.Z);
		    result.W = (num3 * quaternion1.W) + (num2 * quaternion2.W);
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> that contains subtraction of one <see cref="Quaternion"/> from another.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="quaternion2">Source <see cref="Quaternion"/>.</param>
        /// <returns>The result of the quaternion subtraction.</returns>
        public static Quaternion<T> Subtract(Quaternion<T> quaternion1, Quaternion<T> quaternion2)
        {
            Quaternion<T> quaternion;
		    quaternion.X = quaternion1.X - quaternion2.X;
		    quaternion.Y = quaternion1.Y - quaternion2.Y;
		    quaternion.Z = quaternion1.Z - quaternion2.Z;
		    quaternion.W = quaternion1.W - quaternion2.W;
		    return quaternion;
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> that contains subtraction of one <see cref="Quaternion"/> from another.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="quaternion2">Source <see cref="Quaternion"/>.</param>
        /// <param name="result">The result of the quaternion subtraction as an output parameter.</param>
        public static void Subtract(ref Quaternion<T> quaternion1, ref Quaternion<T> quaternion2, out Quaternion<T> result)
        {
            result.X = quaternion1.X - quaternion2.X;
		    result.Y = quaternion1.Y - quaternion2.Y;
		    result.Z = quaternion1.Z - quaternion2.Z;
		    result.W = quaternion1.W - quaternion2.W;
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> that contains a multiplication of two quaternions.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="quaternion2">Source <see cref="Quaternion"/>.</param>
        /// <returns>The result of the quaternion multiplication.</returns>
        public static Quaternion<T> Multiply(Quaternion<T> quaternion1, Quaternion<T> quaternion2)
        {
            Quaternion<T> quaternion;
		    T x = quaternion1.X;
		    T y = quaternion1.Y;
		    T z = quaternion1.Z;
		    T w = quaternion1.W;
		    T num4 = quaternion2.X;
		    T num3 = quaternion2.Y;
		    T num2 = quaternion2.Z;
		    T num = quaternion2.W;
		    T num12 = (y * num2) - (z * num3);
		    T num11 = (z * num4) - (x * num2);
		    T num10 = (x * num3) - (y * num4);
		    T num9 = ((x * num4) + (y * num3)) + (z * num2);
		    quaternion.X = ((x * num) + (num4 * w)) + num12;
		    quaternion.Y = ((y * num) + (num3 * w)) + num11;
		    quaternion.Z = ((z * num) + (num2 * w)) + num10;
		    quaternion.W = (w * num) - num9;
		    return quaternion;
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> that contains a multiplication of <see cref="Quaternion"/> and a scalar.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <returns>The result of the quaternion multiplication with a scalar.</returns>
        public static Quaternion<T> Multiply(Quaternion<T> quaternion1, T scaleFactor)
        {
            Quaternion<T> quaternion;
		    quaternion.X = quaternion1.X * scaleFactor;
		    quaternion.Y = quaternion1.Y * scaleFactor;
		    quaternion.Z = quaternion1.Z * scaleFactor;
		    quaternion.W = quaternion1.W * scaleFactor;
		    return quaternion;
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> that contains a multiplication of <see cref="Quaternion"/> and a scalar.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">The result of the quaternion multiplication with a scalar as an output parameter.</param>
        public static void Multiply(ref Quaternion<T> quaternion1, T scaleFactor, out Quaternion<T> result)
        {
            result.X = quaternion1.X * scaleFactor;
		    result.Y = quaternion1.Y * scaleFactor;
		    result.Z = quaternion1.Z * scaleFactor;
		    result.W = quaternion1.W * scaleFactor;
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> that contains a multiplication of two quaternions.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/>.</param>
        /// <param name="quaternion2">Source <see cref="Quaternion"/>.</param>
        /// <param name="result">The result of the quaternion multiplication as an output parameter.</param>
        public static void Multiply(ref Quaternion<T> quaternion1, ref Quaternion<T> quaternion2, out Quaternion<T> result)
        {
            T x = quaternion1.X;
		    T y = quaternion1.Y;
		    T z = quaternion1.Z;
		    T w = quaternion1.W;
		    T num4 = quaternion2.X;
		    T num3 = quaternion2.Y;
		    T num2 = quaternion2.Z;
		    T num = quaternion2.W;
		    T num12 = (y * num2) - (z * num3);
		    T num11 = (z * num4) - (x * num2);
		    T num10 = (x * num3) - (y * num4);
		    T num9 = ((x * num4) + (y * num3)) + (z * num2);
		    result.X = ((x * num) + (num4 * w)) + num12;
		    result.Y = ((y * num) + (num3 * w)) + num11;
		    result.Z = ((z * num) + (num2 * w)) + num10;
		    result.W = (w * num) - num9;
        }

        /// <summary>
        /// Flips the sign of the all the quaternion components.
        /// </summary>
        /// <param name="quaternion">Source <see cref="Quaternion"/>.</param>
        /// <returns>The result of the quaternion negation.</returns>
        public static Quaternion<T> Negate(Quaternion<T> quaternion)
        {
		    return new Quaternion<T>(-quaternion.X, -quaternion.Y, -quaternion.Z, -quaternion.W);
        }

        /// <summary>
        /// Flips the sign of the all the quaternion components.
        /// </summary>
        /// <param name="quaternion">Source <see cref="Quaternion"/>.</param>
        /// <param name="result">The result of the quaternion negation as an output parameter.</param>
        public static void Negate(ref Quaternion<T> quaternion, out Quaternion<T> result)
        {
            result.X = -quaternion.X;
		    result.Y = -quaternion.Y;
		    result.Z = -quaternion.Z;
		    result.W = -quaternion.W;
        }
		
        public Quaternion<T> Normalized()
        {
	        T num = T.One / T.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
	        X *= num;
	        Y *= num;
	        Z *= num;
	        W *= num;
	        return new Quaternion<T>(X * num, Y * num, Z * num, W * num);
        }
        
        /// <summary>
        /// Scales the quaternion magnitude to unit length.
        /// </summary>
        public void Normalize()
        {
		    T num = T.One / T.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
		    X *= num;
		    Y *= num;
		    Z *= num;
		    W *= num;
        }

        /// <summary>
        /// Scales the quaternion magnitude to unit length.
        /// </summary>
        /// <param name="quaternion">Source <see cref="Quaternion"/>.</param>
        /// <returns>The unit length quaternion.</returns>
        public static Quaternion<T> Normalize(Quaternion<T> quaternion)
        {
            Quaternion<T> result;
		    T num = T.One / T.Sqrt((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y) + (quaternion.Z * quaternion.Z) + (quaternion.W * quaternion.W));
            result.X = quaternion.X * num;
            result.Y = quaternion.Y * num;
            result.Z = quaternion.Z * num;
            result.W = quaternion.W * num;
		    return result;
        }

        /// <summary>
        /// Scales the quaternion magnitude to unit length.
        /// </summary>
        /// <param name="quaternion">Source <see cref="Quaternion"/>.</param>
        /// <param name="result">The unit length quaternion an output parameter.</param>
        public static void Normalize(ref Quaternion<T> quaternion, out Quaternion<T> result)
        {
		    T num = T.One / T.Sqrt((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y) + (quaternion.Z * quaternion.Z) + (quaternion.W * quaternion.W));
		    result.X = quaternion.X * num;
		    result.Y = quaternion.Y * num;
		    result.Z = quaternion.Z * num;
		    result.W = quaternion.W * num;
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="Quaternion"/> in the format:
        /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>] Z:[<see cref="Z"/>] W:[<see cref="W"/>]}
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="Quaternion"/>.</returns>
        public override string ToString()
        {
            return "{X:" + X + " Y:" + Y + " Z:" + Z + " W:" + W + "}";
        }

        /// <summary>
        /// Gets a <see cref="Vec4{T}"/> representation for this object.
        /// </summary>
        /// <returns>A <see cref="Vec4{T}"/> representation for this object.</returns>
        public Vec4<T> ToVector4()
        {
            return new Vec4<T>(X,Y,Z,W);
        }

        public void Deconstruct(out T x, out T y, out T z, out T w)
        {
            x = X;
            y = Y;
            z = Z;
            w = W;
        }

        /// <summary>
        /// Returns a <see cref="System.Numerics.Quaternion"/>.
        /// </summary>
        public Quaternion ToNumerics()
        {
            return new Quaternion(float.CreateChecked(X), float.CreateChecked(Y), float.CreateChecked(Z), float.CreateChecked(W));
        }
        
        /// <summary>
        /// Converts a <see cref="System.Numerics.Quaternion"/> to a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="value">The converted value.</param>
        public static implicit operator Quaternion<T>(Quaternion value)
        {
            return new Quaternion<T>(T.CreateChecked(value.X), T.CreateChecked(value.Y), T.CreateChecked(value.Z), T.CreateChecked(value.W));
        }

        /// <summary>
        /// Adds two quaternions.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/> on the left of the add sign.</param>
        /// <param name="quaternion2">Source <see cref="Quaternion"/> on the right of the add sign.</param>
        /// <returns>Sum of the vectors.</returns>
        public static Quaternion<T> operator +(Quaternion<T> quaternion1, Quaternion<T> quaternion2)
        {
            Quaternion<T> quaternion;
		    quaternion.X = quaternion1.X + quaternion2.X;
		    quaternion.Y = quaternion1.Y + quaternion2.Y;
		    quaternion.Z = quaternion1.Z + quaternion2.Z;
		    quaternion.W = quaternion1.W + quaternion2.W;
		    return quaternion;
        }

        /// <summary>
        /// Divides a <see cref="Quaternion"/> by the other <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/> on the left of the div sign.</param>
        /// <param name="quaternion2">Divisor <see cref="Quaternion"/> on the right of the div sign.</param>
        /// <returns>The result of dividing the quaternions.</returns>
        public static Quaternion<T> operator /(Quaternion<T> quaternion1, Quaternion<T> quaternion2)
        {
            Quaternion<T> quaternion;
		    T x = quaternion1.X;
		    T y = quaternion1.Y;
		    T z = quaternion1.Z;
		    T w = quaternion1.W;
		    T num14 = (((quaternion2.X * quaternion2.X) + (quaternion2.Y * quaternion2.Y)) + (quaternion2.Z * quaternion2.Z)) + (quaternion2.W * quaternion2.W);
		    T num5 = T.One / num14;
		    T num4 = -quaternion2.X * num5;
		    T num3 = -quaternion2.Y * num5;
		    T num2 = -quaternion2.Z * num5;
		    T num = quaternion2.W * num5;
		    T num13 = (y * num2) - (z * num3);
		    T num12 = (z * num4) - (x * num2);
		    T num11 = (x * num3) - (y * num4);
		    T num10 = ((x * num4) + (y * num3)) + (z * num2);
		    quaternion.X = ((x * num) + (num4 * w)) + num13;
		    quaternion.Y = ((y * num) + (num3 * w)) + num12;
		    quaternion.Z = ((z * num) + (num2 * w)) + num11;
		    quaternion.W = (w * num) - num10;
		    return quaternion;
        }

        /// <summary>
        /// Compares whether two <see cref="Quaternion"/> instances are equal.
        /// </summary>
        /// <param name="quaternion1"><see cref="Quaternion"/> instance on the left of the equal sign.</param>
        /// <param name="quaternion2"><see cref="Quaternion"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Quaternion<T> quaternion1, Quaternion<T> quaternion2)
        {
            return ((((quaternion1.X == quaternion2.X) && (quaternion1.Y == quaternion2.Y)) && (quaternion1.Z == quaternion2.Z)) && (quaternion1.W == quaternion2.W));
        }

        /// <summary>
        /// Compares whether two <see cref="Quaternion"/> instances are not equal.
        /// </summary>
        /// <param name="quaternion1"><see cref="Quaternion"/> instance on the left of the not equal sign.</param>
        /// <param name="quaternion2"><see cref="Quaternion"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(Quaternion<T> quaternion1, Quaternion<T> quaternion2)
        {
            if (((quaternion1.X == quaternion2.X) && (quaternion1.Y == quaternion2.Y)) && (quaternion1.Z == quaternion2.Z))
		    {
		        return (quaternion1.W != quaternion2.W);
		    }
		    return true;
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Quaternion"/> on the left of the mul sign.</param>
        /// <param name="quaternion2">Source <see cref="Quaternion"/> on the right of the mul sign.</param>
        /// <returns>Result of the quaternions multiplication.</returns>
        public static Quaternion<T> operator *(Quaternion<T> quaternion1, Quaternion<T> quaternion2)
        {
            Quaternion<T> quaternion;
		    T x = quaternion1.X;
		    T y = quaternion1.Y;
		    T z = quaternion1.Z;
		    T w = quaternion1.W;
		    T num4 = quaternion2.X;
		    T num3 = quaternion2.Y;
		    T num2 = quaternion2.Z;
		    T num = quaternion2.W;
		    T num12 = (y * num2) - (z * num3);
		    T num11 = (z * num4) - (x * num2);
		    T num10 = (x * num3) - (y * num4);
		    T num9 = ((x * num4) + (y * num3)) + (z * num2);
		    quaternion.X = ((x * num) + (num4 * w)) + num12;
		    quaternion.Y = ((y * num) + (num3 * w)) + num11;
		    quaternion.Z = ((z * num) + (num2 * w)) + num10;
		    quaternion.W = (w * num) - num9;
		    return quaternion;
        }

        /// <summary>
        /// Multiplies the components of quaternion by a scalar.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Vec3{T}"/> on the left of the mul sign.</param>
        /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
        /// <returns>Result of the quaternion multiplication with a scalar.</returns>
        public static Quaternion<T> operator *(Quaternion<T> quaternion1, T scaleFactor)
        {
            Quaternion<T> quaternion;
		    quaternion.X = quaternion1.X * scaleFactor;
		    quaternion.Y = quaternion1.Y * scaleFactor;
		    quaternion.Z = quaternion1.Z * scaleFactor;
		    quaternion.W = quaternion1.W * scaleFactor;
		    return quaternion;
        }

        /// <summary>
        /// Subtracts a <see cref="Quaternion"/> from a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="quaternion1">Source <see cref="Vec3{T}"/> on the left of the sub sign.</param>
        /// <param name="quaternion2">Source <see cref="Vec3{T}"/> on the right of the sub sign.</param>
        /// <returns>Result of the quaternion subtraction.</returns>
        public static Quaternion<T> operator -(Quaternion<T> quaternion1, Quaternion<T> quaternion2)
        {
            Quaternion<T> quaternion;
		    quaternion.X = quaternion1.X - quaternion2.X;
		    quaternion.Y = quaternion1.Y - quaternion2.Y;
		    quaternion.Z = quaternion1.Z - quaternion2.Z;
		    quaternion.W = quaternion1.W - quaternion2.W;
		    return quaternion;
        }

        /// <summary>
        /// Flips the sign of the all the quaternion components.
        /// </summary>
        /// <param name="quaternion">Source <see cref="Quaternion"/> on the right of the sub sign.</param>
        /// <returns>The result of the quaternion negation.</returns>
        public static Quaternion<T> operator -(Quaternion<T> quaternion)
        {
            Quaternion<T> quaternion2;
		    quaternion2.X = -quaternion.X;
		    quaternion2.Y = -quaternion.Y;
		    quaternion2.Z = -quaternion.Z;
		    quaternion2.W = -quaternion.W;
		    return quaternion2;
        }
        
    }
}
