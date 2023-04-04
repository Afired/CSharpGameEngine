using System.Diagnostics;
using System.Numerics;
using GameEngine.Core.Serialization;

namespace GameEngine.Numerics
{ 
    /// <summary>
    /// Represents the right-handed 4x4 floating point matrix, which can store translation, scale and rotation information.
    /// </summary>
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct Matrix<T> : IEquatable<Matrix<T>> where T : struct, IFloatingPointIeee754<T> {
	    
	    [Serialized] public T M11;
	    [Serialized] public T M12;
	    [Serialized] public T M13;
	    [Serialized] public T M14;
	    [Serialized] public T M21;
	    [Serialized] public T M22;
	    [Serialized] public T M23;
	    [Serialized] public T M24;
	    [Serialized] public T M31;
	    [Serialized] public T M32;
	    [Serialized] public T M33;
	    [Serialized] public T M34;
	    [Serialized] public T M41;
	    [Serialized] public T M42;
	    [Serialized] public T M43;
	    [Serialized] public T M44;
	    
        public Matrix(T m11, T m12, T m13, T m14, T m21, T m22, T m23, T m24, T m31, T m32, T m33, T m34, T m41, T m42, T m43, T m44) {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;
            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;
            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        /// <summary>
        /// Constructs a matrix.
        /// </summary>
        /// <param name="row1">A first row of the created matrix.</param>
        /// <param name="row2">A second row of the created matrix.</param>
        /// <param name="row3">A third row of the created matrix.</param>
        /// <param name="row4">A fourth row of the created matrix.</param>
        public Matrix(Vec4<T> row1, Vec4<T> row2, Vec4<T> row3, Vec4<T> row4)
        {
            M11 = row1.X;
            M12 = row1.Y;
            M13 = row1.Z;
            M14 = row1.W;
            M21 = row2.X;
            M22 = row2.Y;
            M23 = row2.Z;
            M24 = row2.W;
            M31 = row3.X;
            M32 = row3.Y;
            M33 = row3.Z;
            M34 = row3.W;
            M41 = row4.X;
            M42 = row4.Y;
            M43 = row4.Z;
            M44 = row4.W;
        }
		
		
        #region Indexers

        /// <summary>
        /// Get or set the matrix element at the given index, indexed in row major order.
        /// </summary>
        /// <param name="index">The linearized, zero-based index of the matrix element.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If the index is less than <code>0</code> or larger than <code>15</code>.
        /// </exception>
        public T this[int index]
        {
            get {
	            return index switch {
		            0 => M11,
		            1 => M12,
		            2 => M13,
		            3 => M14,
		            4 => M21,
		            5 => M22,
		            6 => M23,
		            7 => M24,
		            8 => M31,
		            9 => M32,
		            10 => M33,
		            11 => M34,
		            12 => M41,
		            13 => M42,
		            14 => M43,
		            15 => M44,
		            _ => throw new ArgumentOutOfRangeException()
	            };
            }

            set
            {
                switch (index)
                {
                    case 0: M11 = value; break;
                    case 1: M12 = value; break;
                    case 2: M13 = value; break;
                    case 3: M14 = value; break;
                    case 4: M21 = value; break;
                    case 5: M22 = value; break;
                    case 6: M23 = value; break;
                    case 7: M24 = value; break;
                    case 8: M31 = value; break;
                    case 9: M32 = value; break;
                    case 10: M33 = value; break;
                    case 11: M34 = value; break;
                    case 12: M41 = value; break;
                    case 13: M42 = value; break;
                    case 14: M43 = value; break;
                    case 15: M44 = value; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Get or set the value at the specified row and column (indices are zero-based).
        /// </summary>
        /// <param name="row">The row of the element.</param>
        /// <param name="column">The column of the element.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If the row or column is less than <code>0</code> or larger than <code>3</code>.
        /// </exception>
        public T this[int row, int column]
        {
            get => this[(row * 4) + column];
            set => this[(row * 4) + column] = value;
        }

        #endregion
		
        #region Public Properties

        public static Matrix<T> Identity = new Matrix<T>(
	        T.One, T.Zero, T.Zero, T.Zero, 
	        T.Zero, T.One, T.Zero, T.Zero, 
	        T.Zero, T.Zero, T.One, T.Zero, 
	        T.Zero, T.Zero, T.Zero, T.One);
        
        /// <summary>
        /// The backward vector formed from the third row M31, M32, M33 elements.
        /// </summary>
        public Vec3<T> Backward => new Vec3<T>(M31, M32, M33);

        /// <summary>
        /// The down vector formed from the second row -M21, -M22, -M23 elements.
        /// </summary>
        public Vec3<T> Down => new Vec3<T>(-M21, -M22, -M23);

        /// <summary>
        /// The forward vector formed from the third row -M31, -M32, -M33 elements.
        /// </summary>
        public Vec3<T> Forward {
	        get => new Vec3<T>(-M31, -M32, -M33);
	        set {
		        M31 = -value.X;
		        M32 = -value.Y;
		        M33 = -value.Z;
	        }
        }

        /// <summary>
        /// The left vector formed from the first row -M11, -M12, -M13 elements.
        /// </summary>
        public Vec3<T> Left => new Vec3<T>(-M11, -M12, -M13);
		
        /// <summary>
        /// The right vector formed from the first row M11, M12, M13 elements.
        /// </summary>
        public Vec3<T> Right {
	        get => new Vec3<T>(M11, M12, M13);
	        set {
		        M11 = value.X;
		        M12 = value.Y;
		        M13 = value.Z;
	        }
		}

        /// <summary>
        /// Position stored in this matrix.
        /// </summary>
        public Vec3<T> Translation {
	        get => new Vec3<T>(M41, M42, M43);
	        set {
		        M41 = value.X;
		        M42 = value.Y;
		        M43 = value.Z;
	        }
        }

        /// <summary>
        /// The upper vector formed from the second row M21, M22, M23 elements.
        /// </summary>
        public Vec3<T> Up {
	        get => new Vec3<T>(M21, M22, M23);
	        set {
		        M21 = value.X;
		        M22 = value.Y;
		        M23 = value.Z;
	        }
        }
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> which contains sum of two matrixes.
        /// </summary>
        /// <param name="matrix1">The first matrix to add.</param>
        /// <param name="matrix2">The second matrix to add.</param>
        /// <returns>The result of the matrix addition.</returns>
        public static Matrix<T> Add(Matrix<T> matrix1, Matrix<T> matrix2)
        {
            matrix1.M11 += matrix2.M11;
            matrix1.M12 += matrix2.M12;
            matrix1.M13 += matrix2.M13;
            matrix1.M14 += matrix2.M14;
            matrix1.M21 += matrix2.M21;
            matrix1.M22 += matrix2.M22;
            matrix1.M23 += matrix2.M23;
            matrix1.M24 += matrix2.M24;
            matrix1.M31 += matrix2.M31;
            matrix1.M32 += matrix2.M32;
            matrix1.M33 += matrix2.M33;
            matrix1.M34 += matrix2.M34;
            matrix1.M41 += matrix2.M41;
            matrix1.M42 += matrix2.M42;
            matrix1.M43 += matrix2.M43;
            matrix1.M44 += matrix2.M44;
            return matrix1;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> which contains sum of two matrixes.
        /// </summary>
        /// <param name="matrix1">The first matrix to add.</param>
        /// <param name="matrix2">The second matrix to add.</param>
        /// <param name="result">The result of the matrix addition as an output parameter.</param>
        public static void Add(ref Matrix<T> matrix1, ref Matrix<T> matrix2, out Matrix<T> result)
        {
            result.M11 = matrix1.M11 + matrix2.M11;
            result.M12 = matrix1.M12 + matrix2.M12;
            result.M13 = matrix1.M13 + matrix2.M13;
            result.M14 = matrix1.M14 + matrix2.M14;
            result.M21 = matrix1.M21 + matrix2.M21;
            result.M22 = matrix1.M22 + matrix2.M22;
            result.M23 = matrix1.M23 + matrix2.M23;
            result.M24 = matrix1.M24 + matrix2.M24;
            result.M31 = matrix1.M31 + matrix2.M31;
            result.M32 = matrix1.M32 + matrix2.M32;
            result.M33 = matrix1.M33 + matrix2.M33;
            result.M34 = matrix1.M34 + matrix2.M34;
            result.M41 = matrix1.M41 + matrix2.M41;
            result.M42 = matrix1.M42 + matrix2.M42;
            result.M43 = matrix1.M43 + matrix2.M43;
            result.M44 = matrix1.M44 + matrix2.M44;

        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> for spherical billboarding that rotates around specified object position.
        /// </summary>
        /// <param name="objectPosition">Position of billboard object. It will rotate around that vector.</param>
        /// <param name="cameraPosition">The camera position.</param>
        /// <param name="cameraUpVector">The camera up vector.</param>
        /// <param name="cameraForwardVector">Optional camera forward vector.</param>
        /// <returns>The <see cref="Matrix{T}"/> for spherical billboarding.</returns>
        public static Matrix<T> CreateBillboard(Vec3<T> objectPosition, Vec3<T> cameraPosition,
            Vec3<T> cameraUpVector, Vec3<T>? cameraForwardVector)
        {
            Matrix<T> result;
			
            // Delegate to the other overload of the function to do the work
            CreateBillboard(ref objectPosition, ref cameraPosition, ref cameraUpVector, cameraForwardVector, out result);

            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> for spherical billboarding that rotates around specified object position.
        /// </summary>
        /// <param name="objectPosition">Position of billboard object. It will rotate around that vector.</param>
        /// <param name="cameraPosition">The camera position.</param>
        /// <param name="cameraUpVector">The camera up vector.</param>
        /// <param name="cameraForwardVector">Optional camera forward vector.</param>
        /// <param name="result">The <see cref="Matrix{T}"/> for spherical billboarding as an output parameter.</param>
        public static void CreateBillboard(ref Vec3<T> objectPosition, ref Vec3<T> cameraPosition,
            ref Vec3<T> cameraUpVector, Vec3<T>? cameraForwardVector, out Matrix<T> result)
        {
            Vec3<T> vector;
            Vec3<T> vector2;
            Vec3<T> vector3;
            vector.X = objectPosition.X - cameraPosition.X;
            vector.Y = objectPosition.Y - cameraPosition.Y;
            vector.Z = objectPosition.Z - cameraPosition.Z;
            T num = vector.LengthSquared();
            if (num < T.CreateChecked(0.0001f))
            {
                vector = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vec3<T>.Forward;
            }
            else
            {
                Vec3<T>.Multiply(ref vector, T.One / T.Sqrt(num), out vector);
            }
            Vec3<T>.Cross(ref cameraUpVector, ref vector, out vector3);
            vector3.Normalize();
            Vec3<T>.Cross(ref vector, ref vector3, out vector2);
            result.M11 = vector3.X;
            result.M12 = vector3.Y;
            result.M13 = vector3.Z;
            result.M14 = T.Zero;
            result.M21 = vector2.X;
            result.M22 = vector2.Y;
            result.M23 = vector2.Z;
            result.M24 = T.Zero;
            result.M31 = vector.X;
            result.M32 = vector.Y;
            result.M33 = vector.Z;
            result.M34 = T.Zero;
            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = T.One;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> for cylindrical billboarding that rotates around specified axis.
        /// </summary>
        /// <param name="objectPosition">Object position the billboard will rotate around.</param>
        /// <param name="cameraPosition">Camera position.</param>
        /// <param name="rotateAxis">Axis of billboard for rotation.</param>
        /// <param name="cameraForwardVector">Optional camera forward vector.</param>
        /// <param name="objectForwardVector">Optional object forward vector.</param>
        /// <returns>The <see cref="Matrix{T}"/> for cylindrical billboarding.</returns>
        public static Matrix<T> CreateConstrainedBillboard(Vec3<T> objectPosition, Vec3<T> cameraPosition,
            Vec3<T> rotateAxis, Nullable<Vec3<T>> cameraForwardVector, Nullable<Vec3<T>> objectForwardVector)
        {
            Matrix<T> result;
            CreateConstrainedBillboard(ref objectPosition, ref cameraPosition, ref rotateAxis,
                cameraForwardVector, objectForwardVector, out result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> for cylindrical billboarding that rotates around specified axis.
        /// </summary>
        /// <param name="objectPosition">Object position the billboard will rotate around.</param>
        /// <param name="cameraPosition">Camera position.</param>
        /// <param name="rotateAxis">Axis of billboard for rotation.</param>
        /// <param name="cameraForwardVector">Optional camera forward vector.</param>
        /// <param name="objectForwardVector">Optional object forward vector.</param>
        /// <param name="result">The <see cref="Matrix{T}"/> for cylindrical billboarding as an output parameter.</param>
        public static void CreateConstrainedBillboard(ref Vec3<T> objectPosition, ref Vec3<T> cameraPosition,
            ref Vec3<T> rotateAxis, Vec3<T>? cameraForwardVector, Vec3<T>? objectForwardVector, out Matrix<T> result)
        {
            T num;
		    Vec3<T> vector;
		    Vec3<T> vector2;
		    Vec3<T> vector3;
		    vector2.X = objectPosition.X - cameraPosition.X;
		    vector2.Y = objectPosition.Y - cameraPosition.Y;
		    vector2.Z = objectPosition.Z - cameraPosition.Z;
		    T num2 = vector2.LengthSquared();
		    if (num2 < T.CreateChecked(0.0001f))
		    {
		        vector2 = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vec3<T>.Forward;
		    }
		    else
		    {
		        Vec3<T>.Multiply(ref vector2, T.One / T.Sqrt(num2), out vector2);
		    }
		    Vec3<T> vector4 = rotateAxis;
		    Vec3<T>.Dot(ref rotateAxis, ref vector2, out num);
		    if (T.Abs(num) > T.CreateChecked(0.9982547f))
		    {
		        if (objectForwardVector.HasValue)
		        {
		            vector = objectForwardVector.Value;
		            Vec3<T>.Dot(ref rotateAxis, ref vector, out num);
		            if (T.Abs(num) > T.CreateChecked(0.9982547f))
		            {
		                num = ((rotateAxis.X * Vec3<T>.Forward.X) + (rotateAxis.Y * Vec3<T>.Forward.Y)) + (rotateAxis.Z * Vec3<T>.Forward.Z);
		                vector = (T.Abs(num) > T.CreateChecked(0.9982547f)) ? Vec3<T>.Right : Vec3<T>.Forward;
		            }
		        }
		        else
		        {
		            num = ((rotateAxis.X * Vec3<T>.Forward.X) + (rotateAxis.Y * Vec3<T>.Forward.Y)) + (rotateAxis.Z * Vec3<T>.Forward.Z);
		            vector = (T.Abs(num) > T.CreateChecked(0.9982547f)) ? Vec3<T>.Right : Vec3<T>.Forward;
		        }
		        Vec3<T>.Cross(ref rotateAxis, ref vector, out vector3);
		        vector3.Normalize();
		        Vec3<T>.Cross(ref vector3, ref rotateAxis, out vector);
		        vector.Normalize();
		    }
		    else
		    {
		        Vec3<T>.Cross(ref rotateAxis, ref vector2, out vector3);
		        vector3.Normalize();
		        Vec3<T>.Cross(ref vector3, ref vector4, out vector);
		        vector.Normalize();
		    }
		    result.M11 = vector3.X;
		    result.M12 = vector3.Y;
		    result.M13 = vector3.Z;
		    result.M14 = T.Zero;
		    result.M21 = vector4.X;
		    result.M22 = vector4.Y;
		    result.M23 = vector4.Z;
		    result.M24 = T.Zero;
		    result.M31 = vector.X;
		    result.M32 = vector.Y;
		    result.M33 = vector.Z;
		    result.M34 = T.Zero;
		    result.M41 = objectPosition.X;
		    result.M42 = objectPosition.Y;
		    result.M43 = objectPosition.Z;
		    result.M44 = T.One;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> which contains the rotation moment around specified axis.
        /// </summary>
        /// <param name="axis">The axis of rotation.</param>
        /// <param name="angle">The angle of rotation in radians.</param>
        /// <returns>The rotation <see cref="Matrix{T}"/>.</returns>
        public static Matrix<T> CreateFromAxisAngle(Vec3<T> axis, T angle)
        {
	        CreateFromAxisAngle(ref axis, angle, out Matrix<T> result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> which contains the rotation moment around specified axis.
        /// </summary>
        /// <param name="axis">The axis of rotation.</param>
        /// <param name="angle">The angle of rotation in radians.</param>
        /// <param name="result">The rotation <see cref="Matrix{T}"/> as an output parameter.</param>
        public static void CreateFromAxisAngle(ref Vec3<T> axis, T angle, out Matrix<T> result)
        {
            T x = axis.X;
		    T y = axis.Y;
		    T z = axis.Z;
		    T num2 = T.Sin(angle);
		    T num = T.Cos(angle);
		    T num11 = x * x;
		    T num10 = y * y;
		    T num9 = z * z;
		    T num8 = x * y;
		    T num7 = x * z;
		    T num6 = y * z;
		    result.M11 = num11 + (num * (T.One - num11));
		    result.M12 = (num8 - (num * num8)) + (num2 * z);
		    result.M13 = (num7 - (num * num7)) - (num2 * y);
		    result.M14 = T.Zero;
		    result.M21 = (num8 - (num * num8)) - (num2 * z);
		    result.M22 = num10 + (num * (T.One - num10));
		    result.M23 = (num6 - (num * num6)) + (num2 * x);
		    result.M24 = T.Zero;
		    result.M31 = (num7 - (num * num7)) + (num2 * y);
		    result.M32 = (num6 - (num * num6)) - (num2 * x);
		    result.M33 = num9 + (num * (T.One - num9));
		    result.M34 = T.Zero;
		    result.M41 = T.Zero;
		    result.M42 = T.Zero;
		    result.M43 = T.Zero;
		    result.M44 = T.One;
        }

        /// <summary>
        /// Creates a new rotation <see cref="Matrix{T}"/> from a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="quaternion"><see cref="Quaternion"/> of rotation moment.</param>
        /// <returns>The rotation <see cref="Matrix{T}"/>.</returns>
        public static Matrix<T> CreateFromQuaternion(Quaternion<T> quaternion)
        {
	        CreateFromQuaternion(ref quaternion, out var result);
            return result;
        }

        /// <summary>
        /// Creates a new rotation <see cref="Matrix{T}"/> from a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="quaternion"><see cref="Quaternion"/> of rotation moment.</param>
        /// <param name="result">The rotation <see cref="Matrix{T}"/> as an output parameter.</param>
        public static void CreateFromQuaternion(ref Quaternion<T> quaternion, out Matrix<T> result)
        {
            T num9 = quaternion.X * quaternion.X;
		    T num8 = quaternion.Y * quaternion.Y;
		    T num7 = quaternion.Z * quaternion.Z;
		    T num6 = quaternion.X * quaternion.Y;
		    T num5 = quaternion.Z * quaternion.W;
		    T num4 = quaternion.Z * quaternion.X;
		    T num3 = quaternion.Y * quaternion.W;
		    T num2 = quaternion.Y * quaternion.Z;
		    T num = quaternion.X * quaternion.W;
		    result.M11 = T.One - (T.CreateChecked(2) * (num8 + num7));
		    result.M12 = T.CreateChecked(2) * (num6 + num5);
		    result.M13 = T.CreateChecked(2) * (num4 - num3);
		    result.M14 = T.Zero;
		    result.M21 = T.CreateChecked(2) * (num6 - num5);
		    result.M22 = T.One - (T.CreateChecked(2) * (num7 + num9));
		    result.M23 = T.CreateChecked(2) * (num2 + num);
		    result.M24 = T.Zero;
		    result.M31 = T.CreateChecked(2) * (num4 + num3);
		    result.M32 = T.CreateChecked(2) * (num2 - num);
		    result.M33 = T.One - (T.CreateChecked(2) * (num8 + num9));
		    result.M34 = T.Zero;
		    result.M41 = T.Zero;
		    result.M42 = T.Zero;
		    result.M43 = T.Zero;
		    result.M44 = T.One;
        }

        /// <summary>
        /// Creates a new rotation <see cref="Matrix{T}"/> from the specified yaw, pitch and roll values.
        /// </summary>
        /// <param name="yaw">The yaw rotation value in radians.</param>
        /// <param name="pitch">The pitch rotation value in radians.</param>
        /// <param name="roll">The roll rotation value in radians.</param>
        /// <returns>The rotation <see cref="Matrix{T}"/>.</returns>
        /// <remarks>For more information about yaw, pitch and roll visit http://en.wikipedia.org/wiki/Euler_angles.
        /// </remarks>
		public static Matrix<T> CreateFromYawPitchRoll(T yaw, T pitch, T roll)
		{
			CreateFromYawPitchRoll(yaw, pitch, roll, out var matrix);
		    return matrix;
		}

        /// <summary>
        /// Creates a new rotation <see cref="Matrix{T}"/> from the specified yaw, pitch and roll values.
        /// </summary>
        /// <param name="yaw">The yaw rotation value in radians.</param>
        /// <param name="pitch">The pitch rotation value in radians.</param>
        /// <param name="roll">The roll rotation value in radians.</param>
        /// <param name="result">The rotation <see cref="Matrix{T}"/> as an output parameter.</param>
        /// <remarks>For more information about yaw, pitch and roll visit http://en.wikipedia.org/wiki/Euler_angles.
        /// </remarks>
		public static void CreateFromYawPitchRoll(T yaw, T pitch, T roll, out Matrix<T> result)
		{
			Quaternion<T>.CreateFromYawPitchRoll(yaw, pitch, roll, out Quaternion<T> quaternion);
		    CreateFromQuaternion(ref quaternion, out result);
		}

        /// <summary>
        /// Creates a new viewing <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="cameraTarget">Lookup vector of the camera.</param>
        /// <param name="cameraUpVector">The direction of the upper edge of the camera.</param>
        /// <returns>The viewing <see cref="Matrix{T}"/>.</returns>
        public static Matrix<T> CreateLookAt(Vec3<T> cameraPosition, Vec3<T> cameraTarget, Vec3<T> cameraUpVector)
        {
	        CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out Matrix<T> matrix);
            return matrix;
        }

        /// <summary>
        /// Creates a new viewing <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="cameraTarget">Lookup vector of the camera.</param>
        /// <param name="cameraUpVector">The direction of the upper edge of the camera.</param>
        /// <param name="result">The viewing <see cref="Matrix{T}"/> as an output parameter.</param>
        public static void CreateLookAt(ref Vec3<T> cameraPosition, ref Vec3<T> cameraTarget, ref Vec3<T> cameraUpVector, out Matrix<T> result)
        {
            Vec3<T> vector = Vec3<T>.Normalize(cameraPosition - cameraTarget);
            Vec3<T> vector2 = Vec3<T>.Normalize(Vec3<T>.Cross(cameraUpVector, vector));
            Vec3<T> vector3 = Vec3<T>.Cross(vector, vector2);
		    result.M11 = vector2.X;
		    result.M12 = vector3.X;
		    result.M13 = vector.X;
		    result.M14 = T.Zero;
		    result.M21 = vector2.Y;
		    result.M22 = vector3.Y;
		    result.M23 = vector.Y;
		    result.M24 = T.Zero;
		    result.M31 = vector2.Z;
		    result.M32 = vector3.Z;
		    result.M33 = vector.Z;
		    result.M34 = T.Zero;
		    result.M41 = -Vec3<T>.Dot(vector2, cameraPosition);
		    result.M42 = -Vec3<T>.Dot(vector3, cameraPosition);
		    result.M43 = -Vec3<T>.Dot(vector, cameraPosition);
		    result.M44 = T.One;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix{T}"/> for orthographic view.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="zNearPlane">Depth of the near plane.</param>
        /// <param name="zFarPlane">Depth of the far plane.</param>
        /// <returns>The new projection <see cref="Matrix{T}"/> for orthographic view.</returns>
        public static Matrix<T> CreateOrthographic(T width, T height, T zNearPlane, T zFarPlane)
        {
	        CreateOrthographic(width, height, zNearPlane, zFarPlane, out Matrix<T> matrix);
		    return matrix;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix{T}"/> for orthographic view.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="zNearPlane">Depth of the near plane.</param>
        /// <param name="zFarPlane">Depth of the far plane.</param>
        /// <param name="result">The new projection <see cref="Matrix{T}"/> for orthographic view as an output parameter.</param>
        public static void CreateOrthographic(T width, T height, T zNearPlane, T zFarPlane, out Matrix<T> result)
        {
            result.M11 = T.CreateChecked(2) / width;
		    result.M12 = result.M13 = result.M14 = T.Zero;
		    result.M22 = T.CreateChecked(2) / height;
		    result.M21 = result.M23 = result.M24 = T.Zero;
		    result.M33 = T.One / (zNearPlane - zFarPlane);
		    result.M31 = result.M32 = result.M34 = T.Zero;
		    result.M41 = result.M42 = T.Zero;
		    result.M43 = zNearPlane / (zNearPlane - zFarPlane);
		    result.M44 = T.One;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix{T}"/> for customized orthographic view.
        /// </summary>
        /// <param name="left">Lower x-value at the near plane.</param>
        /// <param name="right">Upper x-value at the near plane.</param>
        /// <param name="bottom">Lower y-coordinate at the near plane.</param>
        /// <param name="top">Upper y-value at the near plane.</param>
        /// <param name="zNearPlane">Depth of the near plane.</param>
        /// <param name="zFarPlane">Depth of the far plane.</param>
        /// <returns>The new projection <see cref="Matrix{T}"/> for customized orthographic view.</returns>
        public static Matrix<T> CreateOrthographicOffCenter(T left, T right, T bottom, T top, T zNearPlane, T zFarPlane)
        {
	        CreateOrthographicOffCenter(left, right, bottom, top, zNearPlane, zFarPlane, out Matrix<T> matrix);
			return matrix;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix{T}"/> for customized orthographic view.
        /// </summary>
        /// <param name="viewingVolume">The viewing volume.</param>
        /// <param name="zNearPlane">Depth of the near plane.</param>
        /// <param name="zFarPlane">Depth of the far plane.</param>
        /// <returns>The new projection <see cref="Matrix{T}"/> for customized orthographic view.</returns>
        public static Matrix<T> CreateOrthographicOffCenter(Rectangle viewingVolume, T zNearPlane, T zFarPlane)
        {
	        CreateOrthographicOffCenter(T.CreateChecked(viewingVolume.Left), T.CreateChecked(viewingVolume.Right), T.CreateChecked(viewingVolume.Bottom), T.CreateChecked(viewingVolume.Top), zNearPlane, zFarPlane, out Matrix<T> matrix);
            return matrix;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix{T}"/> for customized orthographic view.
        /// </summary>
        /// <param name="left">Lower x-value at the near plane.</param>
        /// <param name="right">Upper x-value at the near plane.</param>
        /// <param name="bottom">Lower y-coordinate at the near plane.</param>
        /// <param name="top">Upper y-value at the near plane.</param>
        /// <param name="zNearPlane">Depth of the near plane.</param>
        /// <param name="zFarPlane">Depth of the far plane.</param>
        /// <param name="result">The new projection <see cref="Matrix{T}"/> for customized orthographic view as an output parameter.</param>
        public static void CreateOrthographicOffCenter(T left, T right, T bottom, T top, T zNearPlane, T zFarPlane, out Matrix<T> result)
        {
	        // needs higher precision?
	        result.M11 = T.CreateChecked(2) / (right - left);
	        result.M12 = T.Zero;
	        result.M13 = T.Zero;
	        result.M14 = T.Zero;
	        result.M21 = T.Zero;
	        result.M22 = T.CreateChecked(2) / (top - bottom);
	        result.M23 = T.Zero;
	        result.M24 = T.Zero;
	        result.M31 = T.Zero;
	        result.M32 = T.Zero;
	        result.M33 = T.One / (zNearPlane - zFarPlane);
	        result.M34 = T.Zero;
	        result.M41 = (left + right) / (left - right);
	        result.M42 = (top + bottom) / (bottom - top);
	        result.M43 = zNearPlane / (zNearPlane - zFarPlane);
	        result.M44 = T.One;
	        
//			result.M11 = (float)(2.0 / ((double)right - (double)left));
//			result.M12 = T.Zero;
//			result.M13 = T.Zero;
//			result.M14 = T.Zero;
//			result.M21 = T.Zero;
//			result.M22 = (float)(2.0 / ((double)top - (double)bottom));
//			result.M23 = T.Zero;
//			result.M24 = T.Zero;
//			result.M31 = T.Zero;
//			result.M32 = T.Zero;
//			result.M33 = (float)(1.0 / ((double)zNearPlane - (double)zFarPlane));
//			result.M34 = T.Zero;
//			result.M41 = (float)(((double)left + (double)right) / ((double)left - (double)right));
//			result.M42 = (float)(((double)top + (double)bottom) / ((double)bottom - (double)top));
//			result.M43 = (float)((double)zNearPlane / ((double)zNearPlane - (double)zFarPlane));
//			result.M44 = T.One;
		}

        /// <summary>
        /// Creates a new projection <see cref="Matrix{T}"/> for perspective view.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane.</param>
        /// <returns>The new projection <see cref="Matrix{T}"/> for perspective view.</returns>
        public static Matrix<T> CreatePerspective(T width, T height, T nearPlaneDistance, T farPlaneDistance)
        {
	        CreatePerspective(width, height, nearPlaneDistance, farPlaneDistance, out Matrix<T> matrix);
		    return matrix;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix{T}"/> for perspective view.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane, or <see cref="float.PositiveInfinity"/>.</param>
        /// <param name="result">The new projection <see cref="Matrix{T}"/> for perspective view as an output parameter.</param>
        public static void CreatePerspective(T width, T height, T nearPlaneDistance, T farPlaneDistance, out Matrix<T> result)
        {
            if (nearPlaneDistance <= T.Zero)
		    {
		        throw new ArgumentException("nearPlaneDistance <= 0");
		    }
		    if (farPlaneDistance <= T.Zero)
		    {
		        throw new ArgumentException("farPlaneDistance <= 0");
		    }
		    if (nearPlaneDistance >= farPlaneDistance)
		    {
		        throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
		    }
			
            T negFarRange = T.IsPositiveInfinity(farPlaneDistance) ? T.NegativeOne : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			
            result.M11 = (T.CreateChecked(2) * nearPlaneDistance) / width;
            result.M12 = result.M13 = result.M14 = T.Zero;
            result.M22 = (T.CreateChecked(2) * nearPlaneDistance) / height;
            result.M21 = result.M23 = result.M24 = T.Zero;           
            result.M33 = negFarRange;
            result.M31 = result.M32 = T.Zero;
            result.M34 = T.NegativeOne;
            result.M41 = result.M42 = result.M44 = T.Zero;
            result.M43 = nearPlaneDistance * negFarRange;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix{T}"/> for perspective view with field of view.
        /// </summary>
        /// <param name="fieldOfView">Field of view in the y direction in radians.</param>
        /// <param name="aspectRatio">Width divided by height of the viewing volume, not the rendered image!</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane, or <see cref="T.PositiveInfinity"/>.</param>
        /// <returns>The new projection <see cref="Matrix{T}"/> for perspective view with FOV.</returns>
        public static Matrix<T> CreatePerspectiveFieldOfView(T fieldOfView, T aspectRatio, T nearPlaneDistance, T farPlaneDistance)
        {
	        CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance, out Matrix<T> result);
            return result;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix{T}"/> for perspective view with field of view.
        /// </summary>
        /// <param name="fieldOfView">Field of view in the y direction in radians.</param>
        /// <param name="aspectRatio">Width divided by height of the viewing volume, not the rendered image!</param>
        /// <param name="nearPlaneDistance">Distance of the near plane.</param>
        /// <param name="farPlaneDistance">Distance of the far plane, or <see cref="T.PositiveInfinity"/>.</param>
        /// <param name="result">The new projection <see cref="Matrix{T}"/> for perspective view with FOV as an output parameter.</param>
        public static void CreatePerspectiveFieldOfView(T fieldOfView, T aspectRatio, T nearPlaneDistance, T farPlaneDistance, out Matrix<T> result)
        {
            if ((fieldOfView <= T.Zero) || (fieldOfView >= T.Pi))
		    {
		        throw new ArgumentException("fieldOfView <= 0 or >= PI");
		    }
		    if (nearPlaneDistance <= T.Zero)
		    {
		        throw new ArgumentException("nearPlaneDistance <= 0");
		    }
		    if (farPlaneDistance <= T.Zero)
		    {
		        throw new ArgumentException("farPlaneDistance <= 0");
		    }
		    if (nearPlaneDistance >= farPlaneDistance)
		    {
		        throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
		    }

            T yScale = T.One / T.Tan(fieldOfView * T.CreateChecked(0.5));
            T xScale = yScale / aspectRatio;
            T negFarRange = T.IsPositiveInfinity(farPlaneDistance) ? T.NegativeOne : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

            result.M11 = xScale;
            result.M12 = result.M13 = result.M14 = T.Zero;
            result.M22 = yScale;
            result.M21 = result.M23 = result.M24 = T.Zero;
            result.M31 = result.M32 = T.Zero;            
            result.M33 = negFarRange;
            result.M34 = T.NegativeOne;
            result.M41 = result.M42 = result.M44 = T.Zero;
            result.M43 = nearPlaneDistance * negFarRange;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix{T}"/> for customized perspective view.
        /// </summary>
        /// <param name="left">Lower x-value at the near plane.</param>
        /// <param name="right">Upper x-value at the near plane.</param>
        /// <param name="bottom">Lower y-coordinate at the near plane.</param>
        /// <param name="top">Upper y-value at the near plane.</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane.</param>
        /// <returns>The new <see cref="Matrix{T}"/> for customized perspective view.</returns>
        public static Matrix<T> CreatePerspectiveOffCenter(T left, T right, T bottom, T top, T nearPlaneDistance, T farPlaneDistance)
        {
	        CreatePerspectiveOffCenter(left, right, bottom, top, nearPlaneDistance, farPlaneDistance, out var result);
            return result;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix{T}"/> for customized perspective view.
        /// </summary>
        /// <param name="viewingVolume">The viewing volume.</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane.</param>
        /// <returns>The new <see cref="Matrix{T}"/> for customized perspective view.</returns>
        public static Matrix<T> CreatePerspectiveOffCenter(Rectangle viewingVolume, T nearPlaneDistance, T farPlaneDistance)
        {
	        CreatePerspectiveOffCenter(T.CreateChecked(viewingVolume.Left), T.CreateChecked(viewingVolume.Right), T.CreateChecked(viewingVolume.Bottom), T.CreateChecked(viewingVolume.Top), nearPlaneDistance, farPlaneDistance, out Matrix<T> result);
            return result;
        }

        /// <summary>
        /// Creates a new projection <see cref="Matrix{T}"/> for customized perspective view.
        /// </summary>
        /// <param name="left">Lower x-value at the near plane.</param>
        /// <param name="right">Upper x-value at the near plane.</param>
        /// <param name="bottom">Lower y-coordinate at the near plane.</param>
        /// <param name="top">Upper y-value at the near plane.</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane.</param>
        /// <param name="result">The new <see cref="Matrix{T}"/> for customized perspective view as an output parameter.</param>
        public static void CreatePerspectiveOffCenter(T left, T right, T bottom, T top, T nearPlaneDistance, T farPlaneDistance, out Matrix<T> result)
        {
            if (nearPlaneDistance <= T.Zero)
		    {
		        throw new ArgumentException("nearPlaneDistance <= 0");
		    }
		    if (farPlaneDistance <= T.Zero)
		    {
		        throw new ArgumentException("farPlaneDistance <= 0");
		    }
		    if (nearPlaneDistance >= farPlaneDistance)
		    {
		        throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
		    }
		    result.M11 = (T.CreateChecked(2) * nearPlaneDistance) / (right - left);
		    result.M12 = result.M13 = result.M14 = T.Zero;
		    result.M22 = (T.CreateChecked(2) * nearPlaneDistance) / (top - bottom);
		    result.M21 = result.M23 = result.M24 = T.Zero;
		    result.M31 = (left + right) / (right - left);
		    result.M32 = (top + bottom) / (top - bottom);
		    result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
		    result.M34 = T.NegativeOne;
		    result.M43 = (nearPlaneDistance * farPlaneDistance) / (nearPlaneDistance - farPlaneDistance);
		    result.M41 = result.M42 = result.M44 = T.Zero;
        }

        /// <summary>
        /// Creates a new rotation <see cref="Matrix{T}"/> around X axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>The rotation <see cref="Matrix{T}"/> around X axis.</returns>
        public static Matrix<T> CreateRotationX(T radians)
        {
	        CreateRotationX(radians, out var result);
            return result;
        }

        /// <summary>
        /// Creates a new rotation <see cref="Matrix{T}"/> around X axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <param name="result">The rotation <see cref="Matrix{T}"/> around X axis as an output parameter.</param>
        public static void CreateRotationX(T radians, out Matrix<T> result)
        {
            result = Matrix<T>.Identity;

			var val1 = T.Cos(radians);
			var val2 = T.Sin(radians);
			
            result.M22 = val1;
            result.M23 = val2;
            result.M32 = -val2;
            result.M33 = val1;
        }

        /// <summary>
        /// Creates a new rotation <see cref="Matrix{T}"/> around Y axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>The rotation <see cref="Matrix{T}"/> around Y axis.</returns>
        public static Matrix<T> CreateRotationY(T radians)
        {
	        CreateRotationY(radians, out var result);
            return result;
        }

        /// <summary>
        /// Creates a new rotation <see cref="Matrix{T}"/> around Y axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <param name="result">The rotation <see cref="Matrix{T}"/> around Y axis as an output parameter.</param>
        public static void CreateRotationY(T radians, out Matrix<T> result)
        {
            result = Matrix<T>.Identity;

            T val1 = T.Cos(radians);
			T val2 = T.Sin(radians);
			
            result.M11 = val1;
            result.M13 = -val2;
            result.M31 = val2;
            result.M33 = val1;
        }

        /// <summary>
        /// Creates a new rotation <see cref="Matrix{T}"/> around Z axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>The rotation <see cref="Matrix{T}"/> around Z axis.</returns>
        public static Matrix<T> CreateRotationZ(T radians)
        {
	        CreateRotationZ(radians, out Matrix<T> result);
            return result;
        }

        /// <summary>
        /// Creates a new rotation <see cref="Matrix{T}"/> around Z axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <param name="result">The rotation <see cref="Matrix{T}"/> around Z axis as an output parameter.</param>
        public static void CreateRotationZ(T radians, out Matrix<T> result)
        {
            result = Matrix<T>.Identity;

			T val1 = T.Cos(radians);
			T val2 = T.Sin(radians);
			
            result.M11 = val1;
            result.M12 = val2;
            result.M21 = -val2;
            result.M22 = val1;
        }

        /// <summary>
        /// Creates a new scaling <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="scale">Scale value for all three axises.</param>
        /// <returns>The scaling <see cref="Matrix{T}"/>.</returns>
        public static Matrix<T> CreateScale(T scale)
        {
	        CreateScale(scale, scale, scale, out Matrix<T> result);
            return result;
        }

        /// <summary>
        /// Creates a new scaling <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="scale">Scale value for all three axises.</param>
        /// <param name="result">The scaling <see cref="Matrix{T}"/> as an output parameter.</param>
        public static void CreateScale(T scale, out Matrix<T> result)
        {
            CreateScale(scale, scale, scale, out result);
        }

        /// <summary>
        /// Creates a new scaling <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="xScale">Scale value for X axis.</param>
        /// <param name="yScale">Scale value for Y axis.</param>
        /// <param name="zScale">Scale value for Z axis.</param>
        /// <returns>The scaling <see cref="Matrix{T}"/>.</returns>
        public static Matrix<T> CreateScale(T xScale, T yScale, T zScale)
        {
	        CreateScale(xScale, yScale, zScale, out var result);
            return result;
        }

        /// <summary>
        /// Creates a new scaling <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="xScale">Scale value for X axis.</param>
        /// <param name="yScale">Scale value for Y axis.</param>
        /// <param name="zScale">Scale value for Z axis.</param>
        /// <param name="result">The scaling <see cref="Matrix{T}"/> as an output parameter.</param>
        public static void CreateScale(T xScale, T yScale, T zScale, out Matrix<T> result)
        {
			result.M11 = xScale;
			result.M12 = T.Zero;
			result.M13 = T.Zero;
			result.M14 = T.Zero;
			result.M21 = T.Zero;
			result.M22 = yScale;
			result.M23 = T.Zero;
			result.M24 = T.Zero;
			result.M31 = T.Zero;
			result.M32 = T.Zero;
			result.M33 = zScale;
			result.M34 = T.Zero;
			result.M41 = T.Zero;
			result.M42 = T.Zero;
			result.M43 = T.Zero;
			result.M44 = T.One;
        }

        /// <summary>
        /// Creates a new scaling <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="scales"><see cref="Vec3{T}"/> representing x,y and z scale values.</param>
        /// <returns>The scaling <see cref="Matrix{T}"/>.</returns>
        public static Matrix<T> CreateScale(Vec3<T> scales)
        {
	        CreateScale(ref scales, out Matrix<T> result);
            return result;
        }

        /// <summary>
        /// Creates a new scaling <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="scales"><see cref="Vec3{T}"/> representing x,y and z scale values.</param>
        /// <param name="result">The scaling <see cref="Matrix{T}"/> as an output parameter.</param>
        public static void CreateScale(ref Vec3<T> scales, out Matrix<T> result)
        {
            result.M11 = scales.X;
            result.M12 = T.Zero;
            result.M13 = T.Zero;
            result.M14 = T.Zero;
            result.M21 = T.Zero;
            result.M22 = scales.Y;
            result.M23 = T.Zero;
            result.M24 = T.Zero;
            result.M31 = T.Zero;
            result.M32 = T.Zero;
            result.M33 = scales.Z;
            result.M34 = T.Zero;
            result.M41 = T.Zero;
            result.M42 = T.Zero;
            result.M43 = T.Zero;
            result.M44 = T.One;
        }


        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> that flattens geometry into a specified <see cref="Plane"/> as if casting a shadow from a specified light source. 
        /// </summary>
        /// <param name="lightDirection">A vector specifying the direction from which the light that will cast the shadow is coming.</param>
        /// <param name="plane">The plane onto which the new matrix should flatten geometry so as to cast a shadow.</param>
        /// <returns>A <see cref="Matrix{T}"/> that can be used to flatten geometry onto the specified plane from the specified direction. </returns>
        public static Matrix<T> CreateShadow(Vec3<T> lightDirection, Plane<T> plane)
        {
	        CreateShadow(ref lightDirection, ref plane, out Matrix<T> result);
            return result;
        }


        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> that flattens geometry into a specified <see cref="Plane"/> as if casting a shadow from a specified light source. 
        /// </summary>
        /// <param name="lightDirection">A vector specifying the direction from which the light that will cast the shadow is coming.</param>
        /// <param name="plane">The plane onto which the new matrix should flatten geometry so as to cast a shadow.</param>
        /// <param name="result">A <see cref="Matrix{T}"/> that can be used to flatten geometry onto the specified plane from the specified direction as an output parameter.</param>
        public static void CreateShadow(ref Vec3<T> lightDirection, ref Plane<T> plane, out Matrix<T> result)
        {
            T dot = (plane.Normal.X * lightDirection.X) + (plane.Normal.Y * lightDirection.Y) + (plane.Normal.Z * lightDirection.Z);
            T x = -plane.Normal.X;
            T y = -plane.Normal.Y;
            T z = -plane.Normal.Z;
            T d = -plane.D;
			
            result.M11 = (x * lightDirection.X) + dot;
            result.M12 = x * lightDirection.Y;
            result.M13 = x * lightDirection.Z;
            result.M14 = T.Zero;
            result.M21 = y * lightDirection.X;
            result.M22 = (y * lightDirection.Y) + dot;
            result.M23 = y * lightDirection.Z;
            result.M24 = T.Zero;
            result.M31 = z * lightDirection.X;
            result.M32 = z * lightDirection.Y;
            result.M33 = (z * lightDirection.Z) + dot;
            result.M34 = T.Zero;
            result.M41 = d * lightDirection.X;
            result.M42 = d * lightDirection.Y;
            result.M43 = d * lightDirection.Z;
            result.M44 = dot;
        }
        
        /// <summary>
        /// Creates a new translation <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="xPosition">X coordinate of translation.</param>
        /// <param name="yPosition">Y coordinate of translation.</param>
        /// <param name="zPosition">Z coordinate of translation.</param>
        /// <returns>The translation <see cref="Matrix{T}"/>.</returns>
        public static Matrix<T> CreateTranslation(T xPosition, T yPosition, T zPosition)
        {
	        CreateTranslation(xPosition, yPosition, zPosition, out Matrix<T> result);
            return result;
        }

        /// <summary>
        /// Creates a new translation <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="position">X,Y and Z coordinates of translation.</param>
        /// <param name="result">The translation <see cref="Matrix{T}"/> as an output parameter.</param>
        public static void CreateTranslation(ref Vec3<T> position, out Matrix<T> result)
        {
            result.M11 = T.One;
            result.M12 = T.Zero;
            result.M13 = T.Zero;
            result.M14 = T.Zero;
            result.M21 = T.Zero;
            result.M22 = T.One;
            result.M23 = T.Zero;
            result.M24 = T.Zero;
            result.M31 = T.Zero;
            result.M32 = T.Zero;
            result.M33 = T.One;
            result.M34 = T.Zero;
            result.M41 = position.X;
            result.M42 = position.Y;
            result.M43 = position.Z;
            result.M44 = T.One;
        }

        /// <summary>
        /// Creates a new translation <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="position">X,Y and Z coordinates of translation.</param>
        /// <returns>The translation <see cref="Matrix{T}"/>.</returns>
        public static Matrix<T> CreateTranslation(Vec3<T> position)
        {
	        CreateTranslation(ref position, out var result);
			return result;
        }

        /// <summary>
        /// Creates a new translation <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="xPosition">X coordinate of translation.</param>
        /// <param name="yPosition">Y coordinate of translation.</param>
        /// <param name="zPosition">Z coordinate of translation.</param>
        /// <param name="result">The translation <see cref="Matrix{T}"/> as an output parameter.</param>
        public static void CreateTranslation(T xPosition, T yPosition, T zPosition, out Matrix<T> result)
        {
            result.M11 = T.One;
			result.M12 = T.Zero;
			result.M13 = T.Zero;
			result.M14 = T.Zero;
			result.M21 = T.Zero;
			result.M22 = T.One;
			result.M23 = T.Zero;
			result.M24 = T.Zero;
			result.M31 = T.Zero;
			result.M32 = T.Zero;
			result.M33 = T.One;
			result.M34 = T.Zero;
			result.M41 = xPosition;
			result.M42 = yPosition;
			result.M43 = zPosition;
			result.M44 = T.One;
        }
        
        /// <summary>
        /// Creates a new reflection <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="value">The plane that used for reflection calculation.</param>
        /// <returns>The reflection <see cref="Matrix{T}"/>.</returns>
        public static Matrix<T> CreateReflection(Plane<T> value)
        {
	        CreateReflection(ref value, out Matrix<T> result);
            return result;
        }

        /// <summary>
        /// Creates a new reflection <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="value">The plane that used for reflection calculation.</param>
        /// <param name="result">The reflection <see cref="Matrix{T}"/> as an output parameter.</param>
        public static void CreateReflection(ref Plane<T> value, out Matrix<T> result)
        {
            Plane<T> plane;
            Plane<T>.Normalize(ref value, out plane);
            T x = plane.Normal.X;
            T y = plane.Normal.Y;
            T z = plane.Normal.Z;
            T num3 = -T.CreateChecked(2) * x;
            T num2 = -T.CreateChecked(2) * y;
            T num = -T.CreateChecked(2) * z;
            result.M11 = (num3 * x) + T.One;
            result.M12 = num2 * x;
            result.M13 = num * x;
            result.M14 = T.Zero;
            result.M21 = num3 * y;
            result.M22 = (num2 * y) + T.One;
            result.M23 = num * y;
            result.M24 = T.Zero;
            result.M31 = num3 * z;
            result.M32 = num2 * z;
            result.M33 = (num * z) + T.One;
            result.M34 = T.Zero;
            result.M41 = num3 * plane.D;
            result.M42 = num2 * plane.D;
            result.M43 = num * plane.D;
            result.M44 = T.One;
        }

        /// <summary>
        /// Creates a new world <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="position">The position vector.</param>
        /// <param name="forward">The forward direction vector.</param>
        /// <param name="up">The upward direction vector. Usually <see cref="Vec3{T}.Up"/>.</param>
        /// <returns>The world <see cref="Matrix{T}"/>.</returns>
        public static Matrix<T> CreateWorld(Vec3<T> position, Vec3<T> forward, Vec3<T> up)
        {
	        CreateWorld(ref position, ref forward, ref up, out Matrix<T> ret);
	        return ret;
        }

        /// <summary>
        /// Creates a new world <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="position">The position vector.</param>
        /// <param name="forward">The forward direction vector.</param>
        /// <param name="up">The upward direction vector. Usually <see cref="Vec3{T}.Up"/>.</param>
        /// <param name="result">The world <see cref="Matrix{T}"/> as an output parameter.</param>
        public static void CreateWorld(ref Vec3<T> position, ref Vec3<T> forward, ref Vec3<T> up, out Matrix<T> result)
        {
        	Vec3<T> x, y, z;
        	Vec3<T>.Normalize(ref forward, out z);
        	Vec3<T>.Cross(ref forward, ref up, out x);
        	Vec3<T>.Cross(ref x, ref forward, out y);
        	x.Normalize();
        	y.Normalize();            
        	
        	result = new Matrix<T> {
	            Right = x,
	            Up = y,
	            Forward = z,
	            Translation = position,
	            M44 = T.One
            };
        }
		
        public Matrix<T> SetPosition(Vec3<T> value) {
	        M41 = value.X;
	        M42 = value.Y;
	        M43 = value.Z;
	        return this;
        }
		
        public Vec3<T> GetPosition(Vec3<T> value) {
	        return new Vec3<T>(M41, M42, M43);
        }
		
        /// <summary>
        /// Decomposes this matrix to translation, rotation and scale elements. Returns <c>true</c> if matrix can be decomposed; <c>false</c> otherwise.
        /// </summary>
        /// <param name="scale">Scale vector as an output parameter.</param>
        /// <param name="rotation">Rotation quaternion as an output parameter.</param>
        /// <param name="translation">Translation vector as an output parameter.</param>
        /// <returns><c>true</c> if matrix can be decomposed; <c>false</c> otherwise.</returns>
        public bool Decompose(out Vec3<T> scale, out Quaternion<T> rotation, out Vec3<T> translation)
        {
            translation.X = M41;
            translation.Y = M42;
            translation.Z = M43;

            T xs = (T.Sign(M11 * M12 * M13 * M14) < 0) ? T.NegativeOne : T.One;
            T ys = (T.Sign(M21 * M22 * M23 * M24) < 0) ? T.NegativeOne : T.One;
            T zs = (T.Sign(M31 * M32 * M33 * M34) < 0) ? T.NegativeOne : T.One;

            scale.X = xs * T.Sqrt(M11 * M11 + M12 * M12 + M13 * M13);
            scale.Y = ys * T.Sqrt(M21 * M21 + M22 * M22 + M23 * M23);
            scale.Z = zs * T.Sqrt(M31 * M31 + M32 * M32 + M33 * M33);

            if (scale.X == T.Zero || scale.Y == T.Zero || scale.Z == T.Zero)
            {
                rotation = Quaternion<T>.Identity;
                return false;
            }

            Matrix<T> m1 = new Matrix<T>(this.M11 / scale.X, M12 / scale.X, M13 / scale.X, T.Zero,
                                   this.M21 / scale.Y, M22 / scale.Y, M23 / scale.Y, T.Zero,
                                   this.M31 / scale.Z, M32 / scale.Z, M33 / scale.Z, T.Zero,
                                   T.Zero, T.Zero, T.Zero, T.One);

            rotation = Quaternion<T>.CreateFromRotationMatrix(m1);
            return true;
        }	

		/// <summary>
        /// Returns a determinant of this <see cref="Matrix{T}"/>.
        /// </summary>
        /// <returns>Determinant of this <see cref="Matrix{T}"/></returns>
        /// <remarks>See more about determinant here - http://en.wikipedia.org/wiki/Determinant.
        /// </remarks>
        public T Determinant()
        {
            T num22 = M11;
		    T num21 = M12;
		    T num20 = M13;
		    T num19 = M14;
		    T num12 = M21;
		    T num11 = M22;
		    T num10 = M23;
		    T num9 = M24;
		    T num8 = M31;
		    T num7 = M32;
		    T num6 = M33;
		    T num5 = M34;
		    T num4 = M41;
		    T num3 = M42;
		    T num2 = M43;
		    T num = M44;
		    T num18 = (num6 * num) - (num5 * num2);
		    T num17 = (num7 * num) - (num5 * num3);
		    T num16 = (num7 * num2) - (num6 * num3);
		    T num15 = (num8 * num) - (num5 * num4);
		    T num14 = (num8 * num2) - (num6 * num4);
		    T num13 = (num8 * num3) - (num7 * num4);
		    return ((((num22 * (((num11 * num18) - (num10 * num17)) + (num9 * num16))) - (num21 * (((num12 * num18) - (num10 * num15)) + (num9 * num14)))) + (num20 * (((num12 * num17) - (num11 * num15)) + (num9 * num13)))) - (num19 * (((num12 * num16) - (num11 * num14)) + (num10 * num13))));
        }

        /// <summary>
        /// Divides the elements of a <see cref="Matrix{T}"/> by the elements of another matrix.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/>.</param>
        /// <param name="matrix2">Divisor <see cref="Matrix{T}"/>.</param>
        /// <returns>The result of dividing the matrix.</returns>
        public static Matrix<T> Divide(Matrix<T> matrix1, Matrix<T> matrix2)
        {
		    matrix1.M11 = matrix1.M11 / matrix2.M11;
		    matrix1.M12 = matrix1.M12 / matrix2.M12;
		    matrix1.M13 = matrix1.M13 / matrix2.M13;
		    matrix1.M14 = matrix1.M14 / matrix2.M14;
		    matrix1.M21 = matrix1.M21 / matrix2.M21;
		    matrix1.M22 = matrix1.M22 / matrix2.M22;
		    matrix1.M23 = matrix1.M23 / matrix2.M23;
		    matrix1.M24 = matrix1.M24 / matrix2.M24;
		    matrix1.M31 = matrix1.M31 / matrix2.M31;
		    matrix1.M32 = matrix1.M32 / matrix2.M32;
		    matrix1.M33 = matrix1.M33 / matrix2.M33;
		    matrix1.M34 = matrix1.M34 / matrix2.M34;
		    matrix1.M41 = matrix1.M41 / matrix2.M41;
		    matrix1.M42 = matrix1.M42 / matrix2.M42;
		    matrix1.M43 = matrix1.M43 / matrix2.M43;
		    matrix1.M44 = matrix1.M44 / matrix2.M44;
		    return matrix1;
        }

        /// <summary>
        /// Divides the elements of a <see cref="Matrix{T}"/> by the elements of another matrix.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/>.</param>
        /// <param name="matrix2">Divisor <see cref="Matrix{T}"/>.</param>
        /// <param name="result">The result of dividing the matrix as an output parameter.</param>
        public static void Divide(ref Matrix<T> matrix1, ref Matrix<T> matrix2, out Matrix<T> result)
        {
            result.M11 = matrix1.M11 / matrix2.M11;
		    result.M12 = matrix1.M12 / matrix2.M12;
		    result.M13 = matrix1.M13 / matrix2.M13;
		    result.M14 = matrix1.M14 / matrix2.M14;
		    result.M21 = matrix1.M21 / matrix2.M21;
		    result.M22 = matrix1.M22 / matrix2.M22;
		    result.M23 = matrix1.M23 / matrix2.M23;
		    result.M24 = matrix1.M24 / matrix2.M24;
		    result.M31 = matrix1.M31 / matrix2.M31;
		    result.M32 = matrix1.M32 / matrix2.M32;
		    result.M33 = matrix1.M33 / matrix2.M33;
		    result.M34 = matrix1.M34 / matrix2.M34;
		    result.M41 = matrix1.M41 / matrix2.M41;
		    result.M42 = matrix1.M42 / matrix2.M42;
		    result.M43 = matrix1.M43 / matrix2.M43;
		    result.M44 = matrix1.M44 / matrix2.M44;
        }

        /// <summary>
        /// Divides the elements of a <see cref="Matrix{T}"/> by a scalar.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <returns>The result of dividing a matrix by a scalar.</returns>
        public static Matrix<T> Divide(Matrix<T> matrix1, T divider)
        {
		    T num = T.One / divider;
		    matrix1.M11 = matrix1.M11 * num;
		    matrix1.M12 = matrix1.M12 * num;
		    matrix1.M13 = matrix1.M13 * num;
		    matrix1.M14 = matrix1.M14 * num;
		    matrix1.M21 = matrix1.M21 * num;
		    matrix1.M22 = matrix1.M22 * num;
		    matrix1.M23 = matrix1.M23 * num;
		    matrix1.M24 = matrix1.M24 * num;
		    matrix1.M31 = matrix1.M31 * num;
		    matrix1.M32 = matrix1.M32 * num;
		    matrix1.M33 = matrix1.M33 * num;
		    matrix1.M34 = matrix1.M34 * num;
		    matrix1.M41 = matrix1.M41 * num;
		    matrix1.M42 = matrix1.M42 * num;
		    matrix1.M43 = matrix1.M43 * num;
		    matrix1.M44 = matrix1.M44 * num;
		    return matrix1;
        }

        /// <summary>
        /// Divides the elements of a <see cref="Matrix{T}"/> by a scalar.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <param name="result">The result of dividing a matrix by a scalar as an output parameter.</param>
        public static void Divide(ref Matrix<T> matrix1, T divider, out Matrix<T> result)
        {
            T num = T.One / divider;
		    result.M11 = matrix1.M11 * num;
		    result.M12 = matrix1.M12 * num;
		    result.M13 = matrix1.M13 * num;
		    result.M14 = matrix1.M14 * num;
		    result.M21 = matrix1.M21 * num;
		    result.M22 = matrix1.M22 * num;
		    result.M23 = matrix1.M23 * num;
		    result.M24 = matrix1.M24 * num;
		    result.M31 = matrix1.M31 * num;
		    result.M32 = matrix1.M32 * num;
		    result.M33 = matrix1.M33 * num;
		    result.M34 = matrix1.M34 * num;
		    result.M41 = matrix1.M41 * num;
		    result.M42 = matrix1.M42 * num;
		    result.M43 = matrix1.M43 * num;
		    result.M44 = matrix1.M44 * num;
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Matrix{T}"/> without any tolerance.
        /// </summary>
        /// <param name="other">The <see cref="Matrix{T}"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Matrix<T> other)
        {
            return ((((((this.M11 == other.M11) && (this.M22 == other.M22)) && ((this.M33 == other.M33) && (this.M44 == other.M44))) && (((this.M12 == other.M12) && (this.M13 == other.M13)) && ((this.M14 == other.M14) && (this.M21 == other.M21)))) && ((((this.M23 == other.M23) && (this.M24 == other.M24)) && ((this.M31 == other.M31) && (this.M32 == other.M32))) && (((this.M34 == other.M34) && (this.M41 == other.M41)) && (this.M42 == other.M42)))) && (this.M43 == other.M43));
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/> without any tolerance.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object? obj)
        {
            bool flag = false;
		    if (obj is Matrix<T> matrix)
		    {
		        flag = this.Equals(matrix);
		    }
		    return flag;
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Matrix{T}"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Matrix{T}"/>.</returns>
        public override int GetHashCode()
        {
            return (((((((((((((((this.M11.GetHashCode() + this.M12.GetHashCode()) + this.M13.GetHashCode()) + this.M14.GetHashCode()) + this.M21.GetHashCode()) + this.M22.GetHashCode()) + this.M23.GetHashCode()) + this.M24.GetHashCode()) + this.M31.GetHashCode()) + this.M32.GetHashCode()) + this.M33.GetHashCode()) + this.M34.GetHashCode()) + this.M41.GetHashCode()) + this.M42.GetHashCode()) + this.M43.GetHashCode()) + this.M44.GetHashCode());
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> which contains inversion of the specified matrix. 
        /// </summary>
        /// <param name="matrix">Source <see cref="Matrix{T}"/>.</param>
        /// <returns>The inverted matrix.</returns>
        public static Matrix<T> Invert(Matrix<T> matrix)
        {
	        Invert(ref matrix, out var result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> which contains inversion of the specified matrix. 
        /// </summary>
        /// <param name="matrix">Source <see cref="Matrix{T}"/>.</param>
        /// <param name="result">The inverted matrix as output parameter.</param>
        public static void Invert(ref Matrix<T> matrix, out Matrix<T> result)
        {
	        T num1 = matrix.M11;
			T num2 = matrix.M12;
			T num3 = matrix.M13;
			T num4 = matrix.M14;
			T num5 = matrix.M21;
			T num6 = matrix.M22;
			T num7 = matrix.M23;
			T num8 = matrix.M24;
			T num9 = matrix.M31;
			T num10 = matrix.M32;
			T num11 = matrix.M33;
			T num12 = matrix.M34;
			T num13 = matrix.M41;
			T num14 = matrix.M42;
			T num15 = matrix.M43;
			T num16 = matrix.M44;
			T num17 = T.CreateChecked(double.CreateChecked(num11) * double.CreateChecked(num16) - double.CreateChecked(num12) * double.CreateChecked(num15));
			T num18 = T.CreateChecked(double.CreateChecked(num10) * double.CreateChecked(num16) - double.CreateChecked(num12) * double.CreateChecked(num14));
			T num19 = T.CreateChecked(double.CreateChecked(num10) * double.CreateChecked(num15) - double.CreateChecked(num11) * double.CreateChecked(num14));
			T num20 = T.CreateChecked(double.CreateChecked(num9) * double.CreateChecked(num16) - double.CreateChecked(num12) * double.CreateChecked(num13));
			T num21 = T.CreateChecked(double.CreateChecked(num9) * double.CreateChecked(num15) - double.CreateChecked(num11) * double.CreateChecked(num13));
			T num22 = T.CreateChecked(double.CreateChecked(num9) * double.CreateChecked(num14) - double.CreateChecked(num10) * double.CreateChecked(num13));
			T num23 = T.CreateChecked(double.CreateChecked(num6) * double.CreateChecked(num17) - double.CreateChecked(num7) * double.CreateChecked(num18) + double.CreateChecked(num8) * double.CreateChecked(num19));
			T num24 = -T.CreateChecked(double.CreateChecked(num5) * double.CreateChecked(num17) - double.CreateChecked(num7) * double.CreateChecked(num20) + double.CreateChecked(num8) * double.CreateChecked(num21));
			T num25 = T.CreateChecked(double.CreateChecked(num5) * double.CreateChecked(num18) - double.CreateChecked(num6) * double.CreateChecked(num20) + double.CreateChecked(num8) * double.CreateChecked(num22));
			T num26 = -T.CreateChecked(double.CreateChecked(num5) * double.CreateChecked(num19) - double.CreateChecked(num6) * double.CreateChecked(num21) + double.CreateChecked(num7) * double.CreateChecked(num22));
			T num27 = T.CreateChecked(1.0d / (double.CreateChecked(num1) * double.CreateChecked(num23) + double.CreateChecked(num2) * double.CreateChecked(num24) + double.CreateChecked(num3) * double.CreateChecked(num25) + double.CreateChecked(num4) * double.CreateChecked(num26)));
			
			result.M11 = num23 * num27;
			result.M21 = num24 * num27;
			result.M31 = num25 * num27;
			result.M41 = num26 * num27;
			result.M12 = -T.CreateChecked(double.CreateChecked(num2) * double.CreateChecked(num17) - double.CreateChecked(num3) * double.CreateChecked(num18) + double.CreateChecked(num4) * double.CreateChecked(num19)) * num27;
			result.M22 = T.CreateChecked(double.CreateChecked(num1) * double.CreateChecked(num17) - double.CreateChecked(num3) * double.CreateChecked(num20) + double.CreateChecked(num4) * double.CreateChecked(num21)) * num27;
			result.M32 = -T.CreateChecked(double.CreateChecked(num1) * double.CreateChecked(num18) - double.CreateChecked(num2) * double.CreateChecked(num20) + double.CreateChecked(num4) * double.CreateChecked(num22)) * num27;
			result.M42 = T.CreateChecked(double.CreateChecked(num1) * double.CreateChecked(num19) - double.CreateChecked(num2) * double.CreateChecked(num21) + double.CreateChecked(num3) * double.CreateChecked(num22)) * num27;
			T num28 = T.CreateChecked(double.CreateChecked(num7) * double.CreateChecked(num16) - double.CreateChecked(num8) * double.CreateChecked(num15));
			T num29 = T.CreateChecked(double.CreateChecked(num6) * double.CreateChecked(num16) - double.CreateChecked(num8) * double.CreateChecked(num14));
			T num30 = T.CreateChecked(double.CreateChecked(num6) * double.CreateChecked(num15) - double.CreateChecked(num7) * double.CreateChecked(num14));
			T num31 = T.CreateChecked(double.CreateChecked(num5) * double.CreateChecked(num16) - double.CreateChecked(num8) * double.CreateChecked(num13));
			T num32 = T.CreateChecked(double.CreateChecked(num5) * double.CreateChecked(num15) - double.CreateChecked(num7) * double.CreateChecked(num13));
			T num33 = T.CreateChecked(double.CreateChecked(num5) * double.CreateChecked(num14) - double.CreateChecked(num6) * double.CreateChecked(num13));
			result.M13 = T.CreateChecked(double.CreateChecked(num2) * double.CreateChecked(num28) - double.CreateChecked(num3) * double.CreateChecked(num29) + double.CreateChecked(num4) * double.CreateChecked(num30)) * num27;
			result.M23 = -T.CreateChecked(double.CreateChecked(num1) * double.CreateChecked(num28) - double.CreateChecked(num3) * double.CreateChecked(num31) + double.CreateChecked(num4) * double.CreateChecked(num32)) * num27;
			result.M33 = T.CreateChecked(double.CreateChecked(num1) * double.CreateChecked(num29) - double.CreateChecked(num2) * double.CreateChecked(num31) + double.CreateChecked(num4) * double.CreateChecked(num33)) * num27;
			result.M43 = -T.CreateChecked(double.CreateChecked(num1) * double.CreateChecked(num30) - double.CreateChecked(num2) * double.CreateChecked(num32) + double.CreateChecked(num3) * double.CreateChecked(num33)) * num27;
			T num34 = T.CreateChecked(double.CreateChecked(num7) * double.CreateChecked(num12) - double.CreateChecked(num8) * double.CreateChecked(num11));
			T num35 = T.CreateChecked(double.CreateChecked(num6) * double.CreateChecked(num12) - double.CreateChecked(num8) * double.CreateChecked(num10));
			T num36 = T.CreateChecked(double.CreateChecked(num6) * double.CreateChecked(num11) - double.CreateChecked(num7) * double.CreateChecked(num10));
			T num37 = T.CreateChecked(double.CreateChecked(num5) * double.CreateChecked(num12) - double.CreateChecked(num8) * double.CreateChecked(num9));
			T num38 = T.CreateChecked(double.CreateChecked(num5) * double.CreateChecked(num11) - double.CreateChecked(num7) * double.CreateChecked(num9));
			T num39 = T.CreateChecked(double.CreateChecked(num5) * double.CreateChecked(num10) - double.CreateChecked(num6) * double.CreateChecked(num9));
			result.M14 = -T.CreateChecked(double.CreateChecked(num2) * double.CreateChecked(num34) - double.CreateChecked(num3) * double.CreateChecked(num35) + double.CreateChecked(num4) * double.CreateChecked(num36)) * num27;
			result.M24 = T.CreateChecked(double.CreateChecked(num1) * double.CreateChecked(num34) - double.CreateChecked(num3) * double.CreateChecked(num37) + double.CreateChecked(num4) * double.CreateChecked(num38)) * num27;
			result.M34 = -T.CreateChecked(double.CreateChecked(num1) * double.CreateChecked(num35) - double.CreateChecked(num2) * double.CreateChecked(num37) + double.CreateChecked(num4) * double.CreateChecked(num39)) * num27;
			result.M44 = T.CreateChecked(double.CreateChecked(num1) * double.CreateChecked(num36) - double.CreateChecked(num2) * double.CreateChecked(num38) + double.CreateChecked(num3) * double.CreateChecked(num39)) * num27;
	        
//			T num1 = matrix.M11;
//			T num2 = matrix.M12;
//			T num3 = matrix.M13;
//			T num4 = matrix.M14;
//			T num5 = matrix.M21;
//			T num6 = matrix.M22;
//			T num7 = matrix.M23;
//			T num8 = matrix.M24;
//			T num9 = matrix.M31;
//			T num10 = matrix.M32;
//			T num11 = matrix.M33;
//			T num12 = matrix.M34;
//			T num13 = matrix.M41;
//			T num14 = matrix.M42;
//			T num15 = matrix.M43;
//			T num16 = matrix.M44;
//			T num17 = (float) ((double) num11 * (double) num16 - (double) num12 * (double) num15);
//			T num18 = (float) ((double) num10 * (double) num16 - (double) num12 * (double) num14);
//			T num19 = (float) ((double) num10 * (double) num15 - (double) num11 * (double) num14);
//			T num20 = (float) ((double) num9 * (double) num16 - (double) num12 * (double) num13);
//			T num21 = (float) ((double) num9 * (double) num15 - (double) num11 * (double) num13);
//			T num22 = (float) ((double) num9 * (double) num14 - (double) num10 * (double) num13);
//			T num23 = (float) ((double) num6 * (double) num17 - (double) num7 * (double) num18 + (double) num8 * (double) num19);
//			T num24 = (float) -((double) num5 * (double) num17 - (double) num7 * (double) num20 + (double) num8 * (double) num21);
//			T num25 = (float) ((double) num5 * (double) num18 - (double) num6 * (double) num20 + (double) num8 * (double) num22);
//			T num26 = (float) -((double) num5 * (double) num19 - (double) num6 * (double) num21 + (double) num7 * (double) num22);
//			T num27 = (float) (1.0 / ((double) num1 * (double) num23 + (double) num2 * (double) num24 + (double) num3 * (double) num25 + (double) num4 * (double) num26));
//			
//			result.M11 = num23 * num27;
//			result.M21 = num24 * num27;
//			result.M31 = num25 * num27;
//			result.M41 = num26 * num27;
//			result.M12 = (float) -((double) num2 * (double) num17 - (double) num3 * (double) num18 + (double) num4 * (double) num19) * num27;
//			result.M22 = (float) ((double) num1 * (double) num17 - (double) num3 * (double) num20 + (double) num4 * (double) num21) * num27;
//			result.M32 = (float) -((double) num1 * (double) num18 - (double) num2 * (double) num20 + (double) num4 * (double) num22) * num27;
//			result.M42 = (float) ((double) num1 * (double) num19 - (double) num2 * (double) num21 + (double) num3 * (double) num22) * num27;
//			float num28 = (float) ((double) num7 * (double) num16 - (double) num8 * (double) num15);
//			float num29 = (float) ((double) num6 * (double) num16 - (double) num8 * (double) num14);
//			float num30 = (float) ((double) num6 * (double) num15 - (double) num7 * (double) num14);
//			float num31 = (float) ((double) num5 * (double) num16 - (double) num8 * (double) num13);
//			float num32 = (float) ((double) num5 * (double) num15 - (double) num7 * (double) num13);
//			float num33 = (float) ((double) num5 * (double) num14 - (double) num6 * (double) num13);
//			result.M13 = (float) ((double) num2 * (double) num28 - (double) num3 * (double) num29 + (double) num4 * (double) num30) * num27;
//			result.M23 = (float) -((double) num1 * (double) num28 - (double) num3 * (double) num31 + (double) num4 * (double) num32) * num27;
//			result.M33 = (float) ((double) num1 * (double) num29 - (double) num2 * (double) num31 + (double) num4 * (double) num33) * num27;
//			result.M43 = (float) -((double) num1 * (double) num30 - (double) num2 * (double) num32 + (double) num3 * (double) num33) * num27;
//			float num34 = (float) ((double) num7 * (double) num12 - (double) num8 * (double) num11);
//			float num35 = (float) ((double) num6 * (double) num12 - (double) num8 * (double) num10);
//			float num36 = (float) ((double) num6 * (double) num11 - (double) num7 * (double) num10);
//			float num37 = (float) ((double) num5 * (double) num12 - (double) num8 * (double) num9);
//			float num38 = (float) ((double) num5 * (double) num11 - (double) num7 * (double) num9);
//			float num39 = (float) ((double) num5 * (double) num10 - (double) num6 * (double) num9);
//			result.M14 = (float) -((double) num2 * (double) num34 - (double) num3 * (double) num35 + (double) num4 * (double) num36) * num27;
//			result.M24 = (float) ((double) num1 * (double) num34 - (double) num3 * (double) num37 + (double) num4 * (double) num38) * num27;
//			result.M34 = (float) -((double) num1 * (double) num35 - (double) num2 * (double) num37 + (double) num4 * (double) num39) * num27;
//			result.M44 = (float) ((double) num1 * (double) num36 - (double) num2 * (double) num38 + (double) num3 * (double) num39) * num27;
			
			
			/*
			
			
            ///
            // Use Laplace expansion theorem to calculate the inverse of a 4x4 matrix
            // 
            // 1. Calculate the 2x2 determinants needed the 4x4 determinant based on the 2x2 determinants 
            // 3. Create the adjugate matrix, which satisfies: A * adj(A) = det(A) * I
            // 4. Divide adjugate matrix with the determinant to find the inverse
            
            float det1, det2, det3, det4, det5, det6, det7, det8, det9, det10, det11, det12;
            float detMatrix;
            FindDeterminants(ref matrix, out detMatrix, out det1, out det2, out det3, out det4, out det5, out det6, 
                             out det7, out det8, out det9, out det10, out det11, out det12);
            
            float invDetMatrix = 1f / detMatrix;
            
            Matrix ret; // Allow for matrix and result to point to the same structure
            
            ret.M11 = (matrix.M22*det12 - matrix.M23*det11 + matrix.M24*det10) * invDetMatrix;
            ret.M12 = (-matrix.M12*det12 + matrix.M13*det11 - matrix.M14*det10) * invDetMatrix;
            ret.M13 = (matrix.M42*det6 - matrix.M43*det5 + matrix.M44*det4) * invDetMatrix;
            ret.M14 = (-matrix.M32*det6 + matrix.M33*det5 - matrix.M34*det4) * invDetMatrix;
            ret.M21 = (-matrix.M21*det12 + matrix.M23*det9 - matrix.M24*det8) * invDetMatrix;
            ret.M22 = (matrix.M11*det12 - matrix.M13*det9 + matrix.M14*det8) * invDetMatrix;
            ret.M23 = (-matrix.M41*det6 + matrix.M43*det3 - matrix.M44*det2) * invDetMatrix;
            ret.M24 = (matrix.M31*det6 - matrix.M33*det3 + matrix.M34*det2) * invDetMatrix;
            ret.M31 = (matrix.M21*det11 - matrix.M22*det9 + matrix.M24*det7) * invDetMatrix;
            ret.M32 = (-matrix.M11*det11 + matrix.M12*det9 - matrix.M14*det7) * invDetMatrix;
            ret.M33 = (matrix.M41*det5 - matrix.M42*det3 + matrix.M44*det1) * invDetMatrix;
            ret.M34 = (-matrix.M31*det5 + matrix.M32*det3 - matrix.M34*det1) * invDetMatrix;
            ret.M41 = (-matrix.M21*det10 + matrix.M22*det8 - matrix.M23*det7) * invDetMatrix;
            ret.M42 = (matrix.M11*det10 - matrix.M12*det8 + matrix.M13*det7) * invDetMatrix;
            ret.M43 = (-matrix.M41*det4 + matrix.M42*det2 - matrix.M43*det1) * invDetMatrix;
            ret.M44 = (matrix.M31*det4 - matrix.M32*det2 + matrix.M33*det1) * invDetMatrix;
            
            result = ret;
            */
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> that contains linear interpolation of the values in specified matrixes.
        /// </summary>
        /// <param name="matrix1">The first <see cref="Matrix{T}"/>.</param>
        /// <param name="matrix2">The second <see cref="Vec2{T}"/>.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>>The result of linear interpolation of the specified matrixes.</returns>
        public static Matrix<T> Lerp(Matrix<T> matrix1, Matrix<T> matrix2, T amount)
        {
		    matrix1.M11 = matrix1.M11 + ((matrix2.M11 - matrix1.M11) * amount);
		    matrix1.M12 = matrix1.M12 + ((matrix2.M12 - matrix1.M12) * amount);
		    matrix1.M13 = matrix1.M13 + ((matrix2.M13 - matrix1.M13) * amount);
		    matrix1.M14 = matrix1.M14 + ((matrix2.M14 - matrix1.M14) * amount);
		    matrix1.M21 = matrix1.M21 + ((matrix2.M21 - matrix1.M21) * amount);
		    matrix1.M22 = matrix1.M22 + ((matrix2.M22 - matrix1.M22) * amount);
		    matrix1.M23 = matrix1.M23 + ((matrix2.M23 - matrix1.M23) * amount);
		    matrix1.M24 = matrix1.M24 + ((matrix2.M24 - matrix1.M24) * amount);
		    matrix1.M31 = matrix1.M31 + ((matrix2.M31 - matrix1.M31) * amount);
		    matrix1.M32 = matrix1.M32 + ((matrix2.M32 - matrix1.M32) * amount);
		    matrix1.M33 = matrix1.M33 + ((matrix2.M33 - matrix1.M33) * amount);
		    matrix1.M34 = matrix1.M34 + ((matrix2.M34 - matrix1.M34) * amount);
		    matrix1.M41 = matrix1.M41 + ((matrix2.M41 - matrix1.M41) * amount);
		    matrix1.M42 = matrix1.M42 + ((matrix2.M42 - matrix1.M42) * amount);
		    matrix1.M43 = matrix1.M43 + ((matrix2.M43 - matrix1.M43) * amount);
		    matrix1.M44 = matrix1.M44 + ((matrix2.M44 - matrix1.M44) * amount);
		    return matrix1;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> that contains linear interpolation of the values in specified matrixes.
        /// </summary>
        /// <param name="matrix1">The first <see cref="Matrix{T}"/>.</param>
        /// <param name="matrix2">The second <see cref="Vec2{T}"/>.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified matrixes as an output parameter.</param>
        public static void Lerp(ref Matrix<T> matrix1, ref Matrix<T> matrix2, T amount, out Matrix<T> result)
        {
            result.M11 = matrix1.M11 + ((matrix2.M11 - matrix1.M11) * amount);
		    result.M12 = matrix1.M12 + ((matrix2.M12 - matrix1.M12) * amount);
		    result.M13 = matrix1.M13 + ((matrix2.M13 - matrix1.M13) * amount);
		    result.M14 = matrix1.M14 + ((matrix2.M14 - matrix1.M14) * amount);
		    result.M21 = matrix1.M21 + ((matrix2.M21 - matrix1.M21) * amount);
		    result.M22 = matrix1.M22 + ((matrix2.M22 - matrix1.M22) * amount);
		    result.M23 = matrix1.M23 + ((matrix2.M23 - matrix1.M23) * amount);
		    result.M24 = matrix1.M24 + ((matrix2.M24 - matrix1.M24) * amount);
		    result.M31 = matrix1.M31 + ((matrix2.M31 - matrix1.M31) * amount);
		    result.M32 = matrix1.M32 + ((matrix2.M32 - matrix1.M32) * amount);
		    result.M33 = matrix1.M33 + ((matrix2.M33 - matrix1.M33) * amount);
		    result.M34 = matrix1.M34 + ((matrix2.M34 - matrix1.M34) * amount);
		    result.M41 = matrix1.M41 + ((matrix2.M41 - matrix1.M41) * amount);
		    result.M42 = matrix1.M42 + ((matrix2.M42 - matrix1.M42) * amount);
		    result.M43 = matrix1.M43 + ((matrix2.M43 - matrix1.M43) * amount);
		    result.M44 = matrix1.M44 + ((matrix2.M44 - matrix1.M44) * amount);
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> that contains a multiplication of two matrix.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/>.</param>
        /// <param name="matrix2">Source <see cref="Matrix{T}"/>.</param>
        /// <returns>Result of the matrix multiplication.</returns>
        public static Matrix<T> Multiply(Matrix<T> matrix1, Matrix<T> matrix2)
        {
            var m11 = (((matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21)) + (matrix1.M13 * matrix2.M31)) + (matrix1.M14 * matrix2.M41);
            var m12 = (((matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22)) + (matrix1.M13 * matrix2.M32)) + (matrix1.M14 * matrix2.M42);
            var m13 = (((matrix1.M11 * matrix2.M13) + (matrix1.M12 * matrix2.M23)) + (matrix1.M13 * matrix2.M33)) + (matrix1.M14 * matrix2.M43);
            var m14 = (((matrix1.M11 * matrix2.M14) + (matrix1.M12 * matrix2.M24)) + (matrix1.M13 * matrix2.M34)) + (matrix1.M14 * matrix2.M44);
            var m21 = (((matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21)) + (matrix1.M23 * matrix2.M31)) + (matrix1.M24 * matrix2.M41);
            var m22 = (((matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22)) + (matrix1.M23 * matrix2.M32)) + (matrix1.M24 * matrix2.M42);
            var m23 = (((matrix1.M21 * matrix2.M13) + (matrix1.M22 * matrix2.M23)) + (matrix1.M23 * matrix2.M33)) + (matrix1.M24 * matrix2.M43);
            var m24 = (((matrix1.M21 * matrix2.M14) + (matrix1.M22 * matrix2.M24)) + (matrix1.M23 * matrix2.M34)) + (matrix1.M24 * matrix2.M44);
            var m31 = (((matrix1.M31 * matrix2.M11) + (matrix1.M32 * matrix2.M21)) + (matrix1.M33 * matrix2.M31)) + (matrix1.M34 * matrix2.M41);
            var m32 = (((matrix1.M31 * matrix2.M12) + (matrix1.M32 * matrix2.M22)) + (matrix1.M33 * matrix2.M32)) + (matrix1.M34 * matrix2.M42);
            var m33 = (((matrix1.M31 * matrix2.M13) + (matrix1.M32 * matrix2.M23)) + (matrix1.M33 * matrix2.M33)) + (matrix1.M34 * matrix2.M43);
            var m34 = (((matrix1.M31 * matrix2.M14) + (matrix1.M32 * matrix2.M24)) + (matrix1.M33 * matrix2.M34)) + (matrix1.M34 * matrix2.M44);
            var m41 = (((matrix1.M41 * matrix2.M11) + (matrix1.M42 * matrix2.M21)) + (matrix1.M43 * matrix2.M31)) + (matrix1.M44 * matrix2.M41);
            var m42 = (((matrix1.M41 * matrix2.M12) + (matrix1.M42 * matrix2.M22)) + (matrix1.M43 * matrix2.M32)) + (matrix1.M44 * matrix2.M42);
            var m43 = (((matrix1.M41 * matrix2.M13) + (matrix1.M42 * matrix2.M23)) + (matrix1.M43 * matrix2.M33)) + (matrix1.M44 * matrix2.M43);
           	var m44 = (((matrix1.M41 * matrix2.M14) + (matrix1.M42 * matrix2.M24)) + (matrix1.M43 * matrix2.M34)) + (matrix1.M44 * matrix2.M44);
            matrix1.M11 = m11;
			matrix1.M12 = m12;
			matrix1.M13 = m13;
			matrix1.M14 = m14;
			matrix1.M21 = m21;
			matrix1.M22 = m22;
			matrix1.M23 = m23;
			matrix1.M24 = m24;
			matrix1.M31 = m31;
			matrix1.M32 = m32;
			matrix1.M33 = m33;
			matrix1.M34 = m34;
			matrix1.M41 = m41;
			matrix1.M42 = m42;
			matrix1.M43 = m43;
			matrix1.M44 = m44;
			return matrix1;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> that contains a multiplication of two matrix.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/>.</param>
        /// <param name="matrix2">Source <see cref="Matrix{T}"/>.</param>
        /// <param name="result">Result of the matrix multiplication as an output parameter.</param>
        public static void Multiply(ref Matrix<T> matrix1, ref Matrix<T> matrix2, out Matrix<T> result)
        {
            var m11 = (((matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21)) + (matrix1.M13 * matrix2.M31)) + (matrix1.M14 * matrix2.M41);
            var m12 = (((matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22)) + (matrix1.M13 * matrix2.M32)) + (matrix1.M14 * matrix2.M42);
            var m13 = (((matrix1.M11 * matrix2.M13) + (matrix1.M12 * matrix2.M23)) + (matrix1.M13 * matrix2.M33)) + (matrix1.M14 * matrix2.M43);
            var m14 = (((matrix1.M11 * matrix2.M14) + (matrix1.M12 * matrix2.M24)) + (matrix1.M13 * matrix2.M34)) + (matrix1.M14 * matrix2.M44);
            var m21 = (((matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21)) + (matrix1.M23 * matrix2.M31)) + (matrix1.M24 * matrix2.M41);
            var m22 = (((matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22)) + (matrix1.M23 * matrix2.M32)) + (matrix1.M24 * matrix2.M42);
            var m23 = (((matrix1.M21 * matrix2.M13) + (matrix1.M22 * matrix2.M23)) + (matrix1.M23 * matrix2.M33)) + (matrix1.M24 * matrix2.M43);
            var m24 = (((matrix1.M21 * matrix2.M14) + (matrix1.M22 * matrix2.M24)) + (matrix1.M23 * matrix2.M34)) + (matrix1.M24 * matrix2.M44);
            var m31 = (((matrix1.M31 * matrix2.M11) + (matrix1.M32 * matrix2.M21)) + (matrix1.M33 * matrix2.M31)) + (matrix1.M34 * matrix2.M41);
            var m32 = (((matrix1.M31 * matrix2.M12) + (matrix1.M32 * matrix2.M22)) + (matrix1.M33 * matrix2.M32)) + (matrix1.M34 * matrix2.M42);
            var m33 = (((matrix1.M31 * matrix2.M13) + (matrix1.M32 * matrix2.M23)) + (matrix1.M33 * matrix2.M33)) + (matrix1.M34 * matrix2.M43);
            var m34 = (((matrix1.M31 * matrix2.M14) + (matrix1.M32 * matrix2.M24)) + (matrix1.M33 * matrix2.M34)) + (matrix1.M34 * matrix2.M44);
            var m41 = (((matrix1.M41 * matrix2.M11) + (matrix1.M42 * matrix2.M21)) + (matrix1.M43 * matrix2.M31)) + (matrix1.M44 * matrix2.M41);
            var m42 = (((matrix1.M41 * matrix2.M12) + (matrix1.M42 * matrix2.M22)) + (matrix1.M43 * matrix2.M32)) + (matrix1.M44 * matrix2.M42);
            var m43 = (((matrix1.M41 * matrix2.M13) + (matrix1.M42 * matrix2.M23)) + (matrix1.M43 * matrix2.M33)) + (matrix1.M44 * matrix2.M43);
           	var m44 = (((matrix1.M41 * matrix2.M14) + (matrix1.M42 * matrix2.M24)) + (matrix1.M43 * matrix2.M34)) + (matrix1.M44 * matrix2.M44);
            result.M11 = m11;
			result.M12 = m12;
			result.M13 = m13;
			result.M14 = m14;
			result.M21 = m21;
			result.M22 = m22;
			result.M23 = m23;
			result.M24 = m24;
			result.M31 = m31;
			result.M32 = m32;
			result.M33 = m33;
			result.M34 = m34;
			result.M41 = m41;
			result.M42 = m42;
			result.M43 = m43;
			result.M44 = m44;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> that contains a multiplication of <see cref="Matrix{T}"/> and a scalar.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <returns>Result of the matrix multiplication with a scalar.</returns>
        public static Matrix<T> Multiply(Matrix<T> matrix1, T scaleFactor)
        {
            matrix1.M11 *= scaleFactor;
            matrix1.M12 *= scaleFactor;
            matrix1.M13 *= scaleFactor;
            matrix1.M14 *= scaleFactor;
            matrix1.M21 *= scaleFactor;
            matrix1.M22 *= scaleFactor;
            matrix1.M23 *= scaleFactor;
            matrix1.M24 *= scaleFactor;
            matrix1.M31 *= scaleFactor;
            matrix1.M32 *= scaleFactor;
            matrix1.M33 *= scaleFactor;
            matrix1.M34 *= scaleFactor;
            matrix1.M41 *= scaleFactor;
            matrix1.M42 *= scaleFactor;
            matrix1.M43 *= scaleFactor;
            matrix1.M44 *= scaleFactor;
            return matrix1;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> that contains a multiplication of <see cref="Matrix{T}"/> and a scalar.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">Result of the matrix multiplication with a scalar as an output parameter.</param>
        public static void Multiply(ref Matrix<T> matrix1, T scaleFactor, out Matrix<T> result)
        {
            result.M11 = matrix1.M11 * scaleFactor;
            result.M12 = matrix1.M12 * scaleFactor;
            result.M13 = matrix1.M13 * scaleFactor;
            result.M14 = matrix1.M14 * scaleFactor;
            result.M21 = matrix1.M21 * scaleFactor;
            result.M22 = matrix1.M22 * scaleFactor;
            result.M23 = matrix1.M23 * scaleFactor;
            result.M24 = matrix1.M24 * scaleFactor;
            result.M31 = matrix1.M31 * scaleFactor;
            result.M32 = matrix1.M32 * scaleFactor;
            result.M33 = matrix1.M33 * scaleFactor;
            result.M34 = matrix1.M34 * scaleFactor;
            result.M41 = matrix1.M41 * scaleFactor;
            result.M42 = matrix1.M42 * scaleFactor;
            result.M43 = matrix1.M43 * scaleFactor;
            result.M44 = matrix1.M44 * scaleFactor;

        }

        /// <summary>
        /// Copy the values of specified <see cref="Matrix{T}"/> to the float array.
        /// </summary>
        /// <param name="matrix">The source <see cref="Matrix{T}"/>.</param>
        /// <returns>The array which matrix values will be stored.</returns>
        /// <remarks>
        /// Required for OpenGL 2.0 projection matrix stuff.
        /// </remarks>
        public static float[] ToFloatArray(Matrix<T> matrix)
        {
            float[] matarray = {
									float.CreateChecked(matrix.M11), float.CreateChecked(matrix.M12), float.CreateChecked(matrix.M13), float.CreateChecked(matrix.M14),
									float.CreateChecked(matrix.M21), float.CreateChecked(matrix.M22), float.CreateChecked(matrix.M23), float.CreateChecked(matrix.M24),
									float.CreateChecked(matrix.M31), float.CreateChecked(matrix.M32), float.CreateChecked(matrix.M33), float.CreateChecked(matrix.M34),
									float.CreateChecked(matrix.M41), float.CreateChecked(matrix.M42), float.CreateChecked(matrix.M43), float.CreateChecked(matrix.M44)
								};
            return matarray;
        }

        /// <summary>
        /// Returns a matrix with the all values negated.
        /// </summary>
        /// <param name="matrix">Source <see cref="Matrix{T}"/>.</param>
        /// <returns>Result of the matrix negation.</returns>
        public static Matrix<T> Negate(Matrix<T> matrix)
        {
		    matrix.M11 = -matrix.M11;
		    matrix.M12 = -matrix.M12;
		    matrix.M13 = -matrix.M13;
		    matrix.M14 = -matrix.M14;
		    matrix.M21 = -matrix.M21;
		    matrix.M22 = -matrix.M22;
		    matrix.M23 = -matrix.M23;
		    matrix.M24 = -matrix.M24;
		    matrix.M31 = -matrix.M31;
		    matrix.M32 = -matrix.M32;
		    matrix.M33 = -matrix.M33;
		    matrix.M34 = -matrix.M34;
		    matrix.M41 = -matrix.M41;
		    matrix.M42 = -matrix.M42;
		    matrix.M43 = -matrix.M43;
		    matrix.M44 = -matrix.M44;
		    return matrix;
        }

        /// <summary>
        /// Returns a matrix with the all values negated.
        /// </summary>
        /// <param name="matrix">Source <see cref="Matrix{T}"/>.</param>
        /// <param name="result">Result of the matrix negation as an output parameter.</param>
        public static void Negate(ref Matrix<T> matrix, out Matrix<T> result)
        {
            result.M11 = -matrix.M11;
		    result.M12 = -matrix.M12;
		    result.M13 = -matrix.M13;
		    result.M14 = -matrix.M14;
		    result.M21 = -matrix.M21;
		    result.M22 = -matrix.M22;
		    result.M23 = -matrix.M23;
		    result.M24 = -matrix.M24;
		    result.M31 = -matrix.M31;
		    result.M32 = -matrix.M32;
		    result.M33 = -matrix.M33;
		    result.M34 = -matrix.M34;
		    result.M41 = -matrix.M41;
		    result.M42 = -matrix.M42;
		    result.M43 = -matrix.M43;
		    result.M44 = -matrix.M44;
        }

        /// <summary>
        /// Converts a <see cref="System.Numerics.Matrix4x4"/> to a <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="value">The converted value.</param>
        public static implicit operator Matrix<T>(Matrix4x4 value)
        {
            return new Matrix<T>(
                T.CreateChecked(value.M11), T.CreateChecked(value.M12), T.CreateChecked(value.M13), T.CreateChecked(value.M14),
                T.CreateChecked(value.M21), T.CreateChecked(value.M22), T.CreateChecked(value.M23), T.CreateChecked(value.M24),
                T.CreateChecked(value.M31), T.CreateChecked(value.M32), T.CreateChecked(value.M33), T.CreateChecked(value.M34),
                T.CreateChecked(value.M41), T.CreateChecked(value.M42), T.CreateChecked(value.M43), T.CreateChecked(value.M44));
        }

        /// <summary>
        /// Adds two matrixes.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/> on the left of the add sign.</param>
        /// <param name="matrix2">Source <see cref="Matrix{T}"/> on the right of the add sign.</param>
        /// <returns>Sum of the matrixes.</returns>
        public static Matrix<T> operator +(Matrix<T> matrix1, Matrix<T> matrix2)
        {
            matrix1.M11 = matrix1.M11 + matrix2.M11;
            matrix1.M12 = matrix1.M12 + matrix2.M12;
            matrix1.M13 = matrix1.M13 + matrix2.M13;
            matrix1.M14 = matrix1.M14 + matrix2.M14;
            matrix1.M21 = matrix1.M21 + matrix2.M21;
            matrix1.M22 = matrix1.M22 + matrix2.M22;
            matrix1.M23 = matrix1.M23 + matrix2.M23;
            matrix1.M24 = matrix1.M24 + matrix2.M24;
            matrix1.M31 = matrix1.M31 + matrix2.M31;
            matrix1.M32 = matrix1.M32 + matrix2.M32;
            matrix1.M33 = matrix1.M33 + matrix2.M33;
            matrix1.M34 = matrix1.M34 + matrix2.M34;
            matrix1.M41 = matrix1.M41 + matrix2.M41;
            matrix1.M42 = matrix1.M42 + matrix2.M42;
            matrix1.M43 = matrix1.M43 + matrix2.M43;
            matrix1.M44 = matrix1.M44 + matrix2.M44;
            return matrix1;
        }

        /// <summary>
        /// Divides the elements of a <see cref="Matrix{T}"/> by the elements of another <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/> on the left of the div sign.</param>
        /// <param name="matrix2">Divisor <see cref="Matrix{T}"/> on the right of the div sign.</param>
        /// <returns>The result of dividing the matrixes.</returns>
        public static Matrix<T> operator /(Matrix<T> matrix1, Matrix<T> matrix2)
        {
		    matrix1.M11 = matrix1.M11 / matrix2.M11;
		    matrix1.M12 = matrix1.M12 / matrix2.M12;
		    matrix1.M13 = matrix1.M13 / matrix2.M13;
		    matrix1.M14 = matrix1.M14 / matrix2.M14;
		    matrix1.M21 = matrix1.M21 / matrix2.M21;
		    matrix1.M22 = matrix1.M22 / matrix2.M22;
		    matrix1.M23 = matrix1.M23 / matrix2.M23;
		    matrix1.M24 = matrix1.M24 / matrix2.M24;
		    matrix1.M31 = matrix1.M31 / matrix2.M31;
		    matrix1.M32 = matrix1.M32 / matrix2.M32;
		    matrix1.M33 = matrix1.M33 / matrix2.M33;
		    matrix1.M34 = matrix1.M34 / matrix2.M34;
		    matrix1.M41 = matrix1.M41 / matrix2.M41;
		    matrix1.M42 = matrix1.M42 / matrix2.M42;
		    matrix1.M43 = matrix1.M43 / matrix2.M43;
		    matrix1.M44 = matrix1.M44 / matrix2.M44;
		    return matrix1;
        }

        /// <summary>
        /// Divides the elements of a <see cref="Matrix{T}"/> by a scalar.
        /// </summary>
        /// <param name="matrix">Source <see cref="Matrix{T}"/> on the left of the div sign.</param>
        /// <param name="divider">Divisor scalar on the right of the div sign.</param>
        /// <returns>The result of dividing a matrix by a scalar.</returns>
        public static Matrix<T> operator /(Matrix<T> matrix, T divider)
        {
		    T num = T.One / divider;
		    matrix.M11 = matrix.M11 * num;
		    matrix.M12 = matrix.M12 * num;
		    matrix.M13 = matrix.M13 * num;
		    matrix.M14 = matrix.M14 * num;
		    matrix.M21 = matrix.M21 * num;
		    matrix.M22 = matrix.M22 * num;
		    matrix.M23 = matrix.M23 * num;
		    matrix.M24 = matrix.M24 * num;
		    matrix.M31 = matrix.M31 * num;
		    matrix.M32 = matrix.M32 * num;
		    matrix.M33 = matrix.M33 * num;
		    matrix.M34 = matrix.M34 * num;
		    matrix.M41 = matrix.M41 * num;
		    matrix.M42 = matrix.M42 * num;
		    matrix.M43 = matrix.M43 * num;
		    matrix.M44 = matrix.M44 * num;
		    return matrix;
        }

        /// <summary>
        /// Compares whether two <see cref="Matrix{T}"/> instances are equal without any tolerance.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/> on the left of the equal sign.</param>
        /// <param name="matrix2">Source <see cref="Matrix{T}"/> on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Matrix<T> matrix1, Matrix<T> matrix2)
        {
            return (
                matrix1.M11 == matrix2.M11 &&
                matrix1.M12 == matrix2.M12 &&
                matrix1.M13 == matrix2.M13 &&
                matrix1.M14 == matrix2.M14 &&
                matrix1.M21 == matrix2.M21 &&
                matrix1.M22 == matrix2.M22 &&
                matrix1.M23 == matrix2.M23 &&
                matrix1.M24 == matrix2.M24 &&
                matrix1.M31 == matrix2.M31 &&
                matrix1.M32 == matrix2.M32 &&
                matrix1.M33 == matrix2.M33 &&
                matrix1.M34 == matrix2.M34 &&
                matrix1.M41 == matrix2.M41 &&
                matrix1.M42 == matrix2.M42 &&
                matrix1.M43 == matrix2.M43 &&
                matrix1.M44 == matrix2.M44                  
                );
        }

        /// <summary>
        /// Compares whether two <see cref="Matrix{T}"/> instances are not equal without any tolerance.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/> on the left of the not equal sign.</param>
        /// <param name="matrix2">Source <see cref="Matrix{T}"/> on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(Matrix<T> matrix1, Matrix<T> matrix2)
        {
            return (
                matrix1.M11 != matrix2.M11 ||
                matrix1.M12 != matrix2.M12 ||
                matrix1.M13 != matrix2.M13 ||
                matrix1.M14 != matrix2.M14 ||
                matrix1.M21 != matrix2.M21 ||
                matrix1.M22 != matrix2.M22 ||
                matrix1.M23 != matrix2.M23 ||
                matrix1.M24 != matrix2.M24 ||
                matrix1.M31 != matrix2.M31 ||
                matrix1.M32 != matrix2.M32 ||
                matrix1.M33 != matrix2.M33 ||
                matrix1.M34 != matrix2.M34 || 
                matrix1.M41 != matrix2.M41 ||
                matrix1.M42 != matrix2.M42 ||
                matrix1.M43 != matrix2.M43 ||
                matrix1.M44 != matrix2.M44                  
                );
        }

        /// <summary>
        /// Multiplies two matrixes.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/> on the left of the mul sign.</param>
        /// <param name="matrix2">Source <see cref="Matrix{T}"/> on the right of the mul sign.</param>
        /// <returns>Result of the matrix multiplication.</returns>
        /// <remarks>
        /// Using matrix multiplication algorithm - see http://en.wikipedia.org/wiki/Matrix_multiplication.
        /// </remarks>
        public static Matrix<T> operator *(Matrix<T> matrix1, Matrix<T> matrix2)
        {
            var m11 = (((matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21)) + (matrix1.M13 * matrix2.M31)) + (matrix1.M14 * matrix2.M41);
            var m12 = (((matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22)) + (matrix1.M13 * matrix2.M32)) + (matrix1.M14 * matrix2.M42);
            var m13 = (((matrix1.M11 * matrix2.M13) + (matrix1.M12 * matrix2.M23)) + (matrix1.M13 * matrix2.M33)) + (matrix1.M14 * matrix2.M43);
            var m14 = (((matrix1.M11 * matrix2.M14) + (matrix1.M12 * matrix2.M24)) + (matrix1.M13 * matrix2.M34)) + (matrix1.M14 * matrix2.M44);
            var m21 = (((matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21)) + (matrix1.M23 * matrix2.M31)) + (matrix1.M24 * matrix2.M41);
            var m22 = (((matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22)) + (matrix1.M23 * matrix2.M32)) + (matrix1.M24 * matrix2.M42);
            var m23 = (((matrix1.M21 * matrix2.M13) + (matrix1.M22 * matrix2.M23)) + (matrix1.M23 * matrix2.M33)) + (matrix1.M24 * matrix2.M43);
            var m24 = (((matrix1.M21 * matrix2.M14) + (matrix1.M22 * matrix2.M24)) + (matrix1.M23 * matrix2.M34)) + (matrix1.M24 * matrix2.M44);
            var m31 = (((matrix1.M31 * matrix2.M11) + (matrix1.M32 * matrix2.M21)) + (matrix1.M33 * matrix2.M31)) + (matrix1.M34 * matrix2.M41);
            var m32 = (((matrix1.M31 * matrix2.M12) + (matrix1.M32 * matrix2.M22)) + (matrix1.M33 * matrix2.M32)) + (matrix1.M34 * matrix2.M42);
            var m33 = (((matrix1.M31 * matrix2.M13) + (matrix1.M32 * matrix2.M23)) + (matrix1.M33 * matrix2.M33)) + (matrix1.M34 * matrix2.M43);
            var m34 = (((matrix1.M31 * matrix2.M14) + (matrix1.M32 * matrix2.M24)) + (matrix1.M33 * matrix2.M34)) + (matrix1.M34 * matrix2.M44);
            var m41 = (((matrix1.M41 * matrix2.M11) + (matrix1.M42 * matrix2.M21)) + (matrix1.M43 * matrix2.M31)) + (matrix1.M44 * matrix2.M41);
            var m42 = (((matrix1.M41 * matrix2.M12) + (matrix1.M42 * matrix2.M22)) + (matrix1.M43 * matrix2.M32)) + (matrix1.M44 * matrix2.M42);
            var m43 = (((matrix1.M41 * matrix2.M13) + (matrix1.M42 * matrix2.M23)) + (matrix1.M43 * matrix2.M33)) + (matrix1.M44 * matrix2.M43);
           	var m44 = (((matrix1.M41 * matrix2.M14) + (matrix1.M42 * matrix2.M24)) + (matrix1.M43 * matrix2.M34)) + (matrix1.M44 * matrix2.M44);
            matrix1.M11 = m11;
			matrix1.M12 = m12;
			matrix1.M13 = m13;
			matrix1.M14 = m14;
			matrix1.M21 = m21;
			matrix1.M22 = m22;
			matrix1.M23 = m23;
			matrix1.M24 = m24;
			matrix1.M31 = m31;
			matrix1.M32 = m32;
			matrix1.M33 = m33;
			matrix1.M34 = m34;
			matrix1.M41 = m41;
			matrix1.M42 = m42;
			matrix1.M43 = m43;
			matrix1.M44 = m44;
			return matrix1;
        }

        /// <summary>
        /// Multiplies the elements of matrix by a scalar.
        /// </summary>
        /// <param name="matrix">Source <see cref="Matrix{T}"/> on the left of the mul sign.</param>
        /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
        /// <returns>Result of the matrix multiplication with a scalar.</returns>
        public static Matrix<T> operator *(Matrix<T> matrix, T scaleFactor)
        {
		    matrix.M11 = matrix.M11 * scaleFactor;
		    matrix.M12 = matrix.M12 * scaleFactor;
		    matrix.M13 = matrix.M13 * scaleFactor;
		    matrix.M14 = matrix.M14 * scaleFactor;
		    matrix.M21 = matrix.M21 * scaleFactor;
		    matrix.M22 = matrix.M22 * scaleFactor;
		    matrix.M23 = matrix.M23 * scaleFactor;
		    matrix.M24 = matrix.M24 * scaleFactor;
		    matrix.M31 = matrix.M31 * scaleFactor;
		    matrix.M32 = matrix.M32 * scaleFactor;
		    matrix.M33 = matrix.M33 * scaleFactor;
		    matrix.M34 = matrix.M34 * scaleFactor;
		    matrix.M41 = matrix.M41 * scaleFactor;
		    matrix.M42 = matrix.M42 * scaleFactor;
		    matrix.M43 = matrix.M43 * scaleFactor;
		    matrix.M44 = matrix.M44 * scaleFactor;
		    return matrix;
        }

        /// <summary>
        /// Subtracts the values of one <see cref="Matrix{T}"/> from another <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="matrix1">Source <see cref="Matrix{T}"/> on the left of the sub sign.</param>
        /// <param name="matrix2">Source <see cref="Matrix{T}"/> on the right of the sub sign.</param>
        /// <returns>Result of the matrix subtraction.</returns>
        public static Matrix<T> operator -(Matrix<T> matrix1, Matrix<T> matrix2)
        {
		    matrix1.M11 = matrix1.M11 - matrix2.M11;
		    matrix1.M12 = matrix1.M12 - matrix2.M12;
		    matrix1.M13 = matrix1.M13 - matrix2.M13;
		    matrix1.M14 = matrix1.M14 - matrix2.M14;
		    matrix1.M21 = matrix1.M21 - matrix2.M21;
		    matrix1.M22 = matrix1.M22 - matrix2.M22;
		    matrix1.M23 = matrix1.M23 - matrix2.M23;
		    matrix1.M24 = matrix1.M24 - matrix2.M24;
		    matrix1.M31 = matrix1.M31 - matrix2.M31;
		    matrix1.M32 = matrix1.M32 - matrix2.M32;
		    matrix1.M33 = matrix1.M33 - matrix2.M33;
		    matrix1.M34 = matrix1.M34 - matrix2.M34;
		    matrix1.M41 = matrix1.M41 - matrix2.M41;
		    matrix1.M42 = matrix1.M42 - matrix2.M42;
		    matrix1.M43 = matrix1.M43 - matrix2.M43;
		    matrix1.M44 = matrix1.M44 - matrix2.M44;
		    return matrix1;
        }

        /// <summary>
        /// Inverts values in the specified <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="matrix">Source <see cref="Matrix{T}"/> on the right of the sub sign.</param>
        /// <returns>Result of the inversion.</returns>
        public static Matrix<T> operator -(Matrix<T> matrix)
        {
		    matrix.M11 = -matrix.M11;
		    matrix.M12 = -matrix.M12;
		    matrix.M13 = -matrix.M13;
		    matrix.M14 = -matrix.M14;
		    matrix.M21 = -matrix.M21;
		    matrix.M22 = -matrix.M22;
		    matrix.M23 = -matrix.M23;
		    matrix.M24 = -matrix.M24;
		    matrix.M31 = -matrix.M31;
		    matrix.M32 = -matrix.M32;
		    matrix.M33 = -matrix.M33;
		    matrix.M34 = -matrix.M34;
		    matrix.M41 = -matrix.M41;
		    matrix.M42 = -matrix.M42;
		    matrix.M43 = -matrix.M43;
		    matrix.M44 = -matrix.M44;
			return matrix;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> that contains subtraction of one matrix from another.
        /// </summary>
        /// <param name="matrix1">The first <see cref="Matrix{T}"/>.</param>
        /// <param name="matrix2">The second <see cref="Matrix{T}"/>.</param>
        /// <returns>The result of the matrix subtraction.</returns>
        public static Matrix<T> Subtract(Matrix<T> matrix1, Matrix<T> matrix2)
        {
		    matrix1.M11 = matrix1.M11 - matrix2.M11;
		    matrix1.M12 = matrix1.M12 - matrix2.M12;
		    matrix1.M13 = matrix1.M13 - matrix2.M13;
		    matrix1.M14 = matrix1.M14 - matrix2.M14;
		    matrix1.M21 = matrix1.M21 - matrix2.M21;
		    matrix1.M22 = matrix1.M22 - matrix2.M22;
		    matrix1.M23 = matrix1.M23 - matrix2.M23;
		    matrix1.M24 = matrix1.M24 - matrix2.M24;
		    matrix1.M31 = matrix1.M31 - matrix2.M31;
		    matrix1.M32 = matrix1.M32 - matrix2.M32;
		    matrix1.M33 = matrix1.M33 - matrix2.M33;
		    matrix1.M34 = matrix1.M34 - matrix2.M34;
		    matrix1.M41 = matrix1.M41 - matrix2.M41;
		    matrix1.M42 = matrix1.M42 - matrix2.M42;
		    matrix1.M43 = matrix1.M43 - matrix2.M43;
		    matrix1.M44 = matrix1.M44 - matrix2.M44;
		    return matrix1;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix{T}"/> that contains subtraction of one matrix from another.
        /// </summary>
        /// <param name="matrix1">The first <see cref="Matrix{T}"/>.</param>
        /// <param name="matrix2">The second <see cref="Matrix{T}"/>.</param>
        /// <param name="result">The result of the matrix subtraction as an output parameter.</param>
        public static void Subtract(ref Matrix<T> matrix1, ref Matrix<T> matrix2, out Matrix<T> result)
        {
            result.M11 = matrix1.M11 - matrix2.M11;
		    result.M12 = matrix1.M12 - matrix2.M12;
		    result.M13 = matrix1.M13 - matrix2.M13;
		    result.M14 = matrix1.M14 - matrix2.M14;
		    result.M21 = matrix1.M21 - matrix2.M21;
		    result.M22 = matrix1.M22 - matrix2.M22;
		    result.M23 = matrix1.M23 - matrix2.M23;
		    result.M24 = matrix1.M24 - matrix2.M24;
		    result.M31 = matrix1.M31 - matrix2.M31;
		    result.M32 = matrix1.M32 - matrix2.M32;
		    result.M33 = matrix1.M33 - matrix2.M33;
		    result.M34 = matrix1.M34 - matrix2.M34;
		    result.M41 = matrix1.M41 - matrix2.M41;
		    result.M42 = matrix1.M42 - matrix2.M42;
		    result.M43 = matrix1.M43 - matrix2.M43;
		    result.M44 = matrix1.M44 - matrix2.M44;
        }

        internal string DebugDisplayString
        {
            get
            {
                if (this == Identity)
                {
                    return "Identity";
                }

                return string.Concat(
                     "( ", this.M11.ToString(), "  ", this.M12.ToString(), "  ", this.M13.ToString(), "  ", this.M14.ToString(), " )  \r\n",
                     "( ", this.M21.ToString(), "  ", this.M22.ToString(), "  ", this.M23.ToString(), "  ", this.M24.ToString(), " )  \r\n",
                     "( ", this.M31.ToString(), "  ", this.M32.ToString(), "  ", this.M33.ToString(), "  ", this.M34.ToString(), " )  \r\n",
                     "( ", this.M41.ToString(), "  ", this.M42.ToString(), "  ", this.M43.ToString(), "  ", this.M44.ToString(), " )");
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="Matrix{T}"/> in the format:
        /// {M11:[<see cref="M11"/>] M12:[<see cref="M12"/>] M13:[<see cref="M13"/>] M14:[<see cref="M14"/>]}
        /// {M21:[<see cref="M21"/>] M12:[<see cref="M22"/>] M13:[<see cref="M23"/>] M14:[<see cref="M24"/>]}
        /// {M31:[<see cref="M31"/>] M32:[<see cref="M32"/>] M33:[<see cref="M33"/>] M34:[<see cref="M34"/>]}
        /// {M41:[<see cref="M41"/>] M42:[<see cref="M42"/>] M43:[<see cref="M43"/>] M44:[<see cref="M44"/>]}
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="Matrix{T}"/>.</returns>
        public override string ToString()
        {
            return "{M11:" + M11 + " M12:" + M12 + " M13:" + M13 + " M14:" + M14 + "}"
                + " {M21:" + M21 + " M22:" + M22 + " M23:" + M23 + " M24:" + M24 + "}"
                + " {M31:" + M31 + " M32:" + M32 + " M33:" + M33 + " M34:" + M34 + "}"
                + " {M41:" + M41 + " M42:" + M42 + " M43:" + M43 + " M44:" + M44 + "}";
        }

        /// <summary>
        /// Swap the matrix rows and columns.
        /// </summary>
        /// <param name="matrix">The matrix for transposing operation.</param>
        /// <returns>The new <see cref="Matrix{T}"/> which contains the transposing result.</returns>
        public static Matrix<T> Transpose(Matrix<T> matrix)
        {
	        Transpose(ref matrix, out var ret);
            return ret;
        }

        /// <summary>
        /// Swap the matrix rows and columns.
        /// </summary>
        /// <param name="matrix">The matrix for transposing operation.</param>
        /// <param name="result">The new <see cref="Matrix{T}"/> which contains the transposing result as an output parameter.</param>
        public static void Transpose(ref Matrix<T> matrix, out Matrix<T> result)
        {
            Matrix<T> ret;
            
            ret.M11 = matrix.M11;
            ret.M12 = matrix.M21;
            ret.M13 = matrix.M31;
            ret.M14 = matrix.M41;

            ret.M21 = matrix.M12;
            ret.M22 = matrix.M22;
            ret.M23 = matrix.M32;
            ret.M24 = matrix.M42;

            ret.M31 = matrix.M13;
            ret.M32 = matrix.M23;
            ret.M33 = matrix.M33;
            ret.M34 = matrix.M43;

            ret.M41 = matrix.M14;
            ret.M42 = matrix.M24;
            ret.M43 = matrix.M34;
            ret.M44 = matrix.M44;
            
            result = ret;
        }

        /// <summary>
        /// Returns a <see cref="System.Numerics.Matrix4x4"/>.
        /// </summary>
        public Matrix4x4 ToNumerics()
        {
            return new Matrix4x4(
                float.CreateChecked(M11), float.CreateChecked(M12), float.CreateChecked(M13), float.CreateChecked(M14),
                float.CreateChecked(M21), float.CreateChecked(M22), float.CreateChecked(M23), float.CreateChecked(M24),
                float.CreateChecked(M31), float.CreateChecked(M32), float.CreateChecked(M33), float.CreateChecked(M34),
                float.CreateChecked(M41), float.CreateChecked(M42), float.CreateChecked(M43), float.CreateChecked(M44));
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Helper method for using the Laplace expansion theorem using two rows expansions to calculate major and 
        /// minor determinants of a 4x4 matrix. This method is used for inverting a matrix.
        /// </summary>
        private static void FindDeterminants(ref Matrix<T> matrix, out T major, 
                                             out T minor1, out T minor2, out T minor3, out T minor4, out T minor5, out T minor6,
                                             out T minor7, out T minor8, out T minor9, out T minor10, out T minor11, out T minor12)
        {
	        // needs higher precision?
	        T det1 = matrix.M11 * matrix.M22 - matrix.M12 * matrix.M21;
            T det2 = matrix.M11 * matrix.M23 - matrix.M13 * matrix.M21;
            T det3 = matrix.M11 * matrix.M24 - matrix.M14 * matrix.M21;
            T det4 = matrix.M12 * matrix.M23 - matrix.M13 * matrix.M22;
            T det5 = matrix.M12 * matrix.M24 - matrix.M14 * matrix.M22;
            T det6 = matrix.M13 * matrix.M24 - matrix.M14 * matrix.M23;
            T det7 = matrix.M31 * matrix.M42 - matrix.M32 * matrix.M41;
            T det8 = matrix.M31 * matrix.M43 - matrix.M33 * matrix.M41;
            T det9 = matrix.M31 * matrix.M44 - matrix.M34 * matrix.M41;
            T det10 = matrix.M32 * matrix.M43 - matrix.M33 * matrix.M42;
            T det11 = matrix.M32 * matrix.M44 - matrix.M34 * matrix.M42;
            T det12 = matrix.M33 * matrix.M44 - matrix.M34 * matrix.M43;
	        
            major = det1 * det12 - det2 * det11 + det3 * det10 + det4 * det9 - det5 * det8 + det6 * det7;
            minor1 = det1;
            minor2 = det2;
            minor3 = det3;
            minor4 = det4;
            minor5 = det5;
            minor6 = det6;
            minor7 = det7;
            minor8 = det8;
            minor9 = det9;
            minor10 = det10;
            minor11 = det11;
            minor12 = det12;
				
//                double det1 = (double)matrix.M11 * (double)matrix.M22 - (double)matrix.M12 * (double)matrix.M21;
//                double det2 = (double)matrix.M11 * (double)matrix.M23 - (double)matrix.M13 * (double)matrix.M21;
//                double det3 = (double)matrix.M11 * (double)matrix.M24 - (double)matrix.M14 * (double)matrix.M21;
//                double det4 = (double)matrix.M12 * (double)matrix.M23 - (double)matrix.M13 * (double)matrix.M22;
//                double det5 = (double)matrix.M12 * (double)matrix.M24 - (double)matrix.M14 * (double)matrix.M22;
//                double det6 = (double)matrix.M13 * (double)matrix.M24 - (double)matrix.M14 * (double)matrix.M23;
//                double det7 = (double)matrix.M31 * (double)matrix.M42 - (double)matrix.M32 * (double)matrix.M41;
//                double det8 = (double)matrix.M31 * (double)matrix.M43 - (double)matrix.M33 * (double)matrix.M41;
//                double det9 = (double)matrix.M31 * (double)matrix.M44 - (double)matrix.M34 * (double)matrix.M41;
//                double det10 = (double)matrix.M32 * (double)matrix.M43 - (double)matrix.M33 * (double)matrix.M42;
//                double det11 = (double)matrix.M32 * (double)matrix.M44 - (double)matrix.M34 * (double)matrix.M42;
//                double det12 = (double)matrix.M33 * (double)matrix.M44 - (double)matrix.M34 * (double)matrix.M43;
//                
//                major = (float)(det1*det12 - det2*det11 + det3*det10 + det4*det9 - det5*det8 + det6*det7);
//                minor1 = (float)det1;
//                minor2 = (float)det2;
//                minor3 = (float)det3;
//                minor4 = (float)det4;
//                minor5 = (float)det5;
//                minor6 = (float)det6;
//                minor7 = (float)det7;
//                minor8 = (float)det8;
//                minor9 = (float)det9;
//                minor10 = (float)det10;
//                minor11 = (float)det11;
//                minor12 = (float)det12;
        }
		
        #endregion
        
    }
}
