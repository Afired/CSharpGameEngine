using System.Diagnostics;
using System.Numerics;
using GameEngine.Core.Serialization;

namespace GameEngine.Numerics
{
	internal static class PlaneHelper
    {
        /// <summary>
        /// Returns a value indicating what side (positive/negative) of a plane a point is
        /// </summary>
        /// <param name="point">The point to check with</param>
        /// <param name="plane">The plane to check against</param>
        /// <returns>Greater than zero if on the positive side, less than zero if on the negative size, 0 otherwise</returns>
        internal static T ClassifyPoint<T>(ref Vec3<T> point, ref Plane<T> plane) where T : struct, IFloatingPointIeee754<T>
        {
            return point.X * plane.Normal.X + point.Y * plane.Normal.Y + point.Z * plane.Normal.Z + plane.D;
        }

        /// <summary>
        /// Returns the perpendicular distance from a point to a plane
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <param name="plane">The place to check</param>
        /// <returns>The perpendicular distance from the point to the plane</returns>
        internal static T PerpendicularDistance<T>(ref Vec3<T> point, ref Plane<T> plane) where T : struct, IFloatingPointIeee754<T>
        {
            // dist = (ax + by + cz + d) / sqrt(a*a + b*b + c*c)
            return T.Abs((plane.Normal.X * point.X + plane.Normal.Y * point.Y + plane.Normal.Z * point.Z)
                                    / T.Sqrt(plane.Normal.X * plane.Normal.X + plane.Normal.Y * plane.Normal.Y + plane.Normal.Z * plane.Normal.Z));
        }
    }
	
    /// <summary>
    /// A plane in 3d space, represented by its normal away from the origin and its distance from the origin, D.
    /// </summary>
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct Plane<T> : IEquatable<Plane<T>> where T : struct, IFloatingPointIeee754<T>
    {
        /// <summary>
        /// The distance of the <see cref="Plane"/> to the origin.
        /// </summary>
        [Serialized] public T D;

        /// <summary>
        /// The normal of the <see cref="Plane"/>.
        /// </summary>
        [Serialized] public Vec3<T> Normal;

        /// <summary>
        /// Create a <see cref="Plane"/> with the first three components of the specified <see cref="Vec4{T}"/>
        /// as the normal and the last component as the distance to the origin.
        /// </summary>
        /// <param name="value">A vector holding the normal and distance to origin.</param>
        public Plane(Vec4<T> value)
            : this(new Vec3<T>(value.X, value.Y, value.Z), value.W)
        {

        }

        /// <summary>
        /// Create a <see cref="Plane"/> with the specified normal and distance to the origin.
        /// </summary>
        /// <param name="normal">The normal of the plane.</param>
        /// <param name="d">The distance to the origin.</param>
        public Plane(Vec3<T> normal, T d)
        {
            Normal = normal;
            D = d;
        }

        /// <summary>
        /// Create the <see cref="Plane"/> that contains the three specified points.
        /// </summary>
        /// <param name="a">A point the created <see cref="Plane"/> should contain.</param>
        /// <param name="b">A point the created <see cref="Plane"/> should contain.</param>
        /// <param name="c">A point the created <see cref="Plane"/> should contain.</param>
        public Plane(Vec3<T> a, Vec3<T> b, Vec3<T> c)
        {
            Vec3<T> ab = b - a;
            Vec3<T> ac = c - a;

            Vec3<T> cross = Vec3<T>.Cross(ab, ac);
            Vec3<T>.Normalize(ref cross, out Normal);
            D = -(Vec3<T>.Dot(Normal, a));
        }

        /// <summary>
        /// Create a <see cref="Plane"/> with the first three values as the X, Y and Z
        /// components of the normal and the last value as the distance to the origin.
        /// </summary>
        /// <param name="a">The X component of the normal.</param>
        /// <param name="b">The Y component of the normal.</param>
        /// <param name="c">The Z component of the normal.</param>
        /// <param name="d">The distance to the origin.</param>
        public Plane(T a, T b, T c, T d)
            : this(new Vec3<T>(a, b, c), d)
        {

        }

        /// <summary>
        /// Create a <see cref="Plane"/> that contains the specified point and has the specified <see cref="Normal"/> vector.
        /// </summary>
        /// <param name="pointOnPlane">A point the created <see cref="Plane"/> should contain.</param>
        /// <param name="normal">The normal of the plane.</param>
        public Plane(Vec3<T> pointOnPlane, Vec3<T> normal)
        {
            Normal = normal;
            D = -(
                pointOnPlane.X * normal.X +
                pointOnPlane.Y * normal.Y +
                pointOnPlane.Z * normal.Z
            );
        }

        /// <summary>
        /// Get the dot product of a <see cref="Vec4{T}"/> with this <see cref="Plane"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vec4{T}"/> to calculate the dot product with.</param>
        /// <returns>The dot product of the specified <see cref="Vec4{T}"/> and this <see cref="Plane"/>.</returns>
        public T Dot(Vec4<T> value)
        {
            return ((((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z)) + (this.D * value.W));
        }

        /// <summary>
        /// Get the dot product of a <see cref="Vec4{T}"/> with this <see cref="Plane"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vec4{T}"/> to calculate the dot product with.</param>
        /// <param name="result">
        /// The dot product of the specified <see cref="Vec4{T}"/> and this <see cref="Plane"/>.
        /// </param>
        public void Dot(ref Vec4<T> value, out T result)
        {
            result = (((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z)) + (this.D * value.W);
        }

        /// <summary>
        /// Get the dot product of a <see cref="Vec3{T}"/> with
        /// the <see cref="Normal"/> vector of this <see cref="Plane"/>
        /// plus the <see cref="D"/> value of this <see cref="Plane"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vec3{T}"/> to calculate the dot product with.</param>
        /// <returns>
        /// The dot product of the specified <see cref="Vec3{T}"/> and the normal of this <see cref="Plane"/>
        /// plus the <see cref="D"/> value of this <see cref="Plane"/>.
        /// </returns>
        public T DotCoordinate(Vec3<T> value)
        {
            return ((((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z)) + this.D);
        }

        /// <summary>
        /// Get the dot product of a <see cref="Vec3{T}"/> with
        /// the <see cref="Normal"/> vector of this <see cref="Plane"/>
        /// plus the <see cref="D"/> value of this <see cref="Plane"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vec3{T}"/> to calculate the dot product with.</param>
        /// <param name="result">
        /// The dot product of the specified <see cref="Vec3{T}"/> and the normal of this <see cref="Plane"/>
        /// plus the <see cref="D"/> value of this <see cref="Plane"/>.
        /// </param>
        public void DotCoordinate(ref Vec3<T> value, out T result)
        {
            result = (((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z)) + this.D;
        }

        /// <summary>
        /// Get the dot product of a <see cref="Vec3{T}"/> with
        /// the <see cref="Normal"/> vector of this <see cref="Plane"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vec3{T}"/> to calculate the dot product with.</param>
        /// <returns>
        /// The dot product of the specified <see cref="Vec3{T}"/> and the normal of this <see cref="Plane"/>.
        /// </returns>
        public T DotNormal(Vec3<T> value)
        {
            return (((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z));
        }

        /// <summary>
        /// Get the dot product of a <see cref="Vec3{T}"/> with
        /// the <see cref="Normal"/> vector of this <see cref="Plane"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vec3{T}"/> to calculate the dot product with.</param>
        /// <param name="result">
        /// The dot product of the specified <see cref="Vec3{T}"/> and the normal of this <see cref="Plane"/>.
        /// </param>
        public void DotNormal(ref Vec3<T> value, out T result)
        {
            result = ((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z);
        }

        /// <summary>
        /// Transforms a normalized plane by a matrix.
        /// </summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed plane.</returns>
        public static Plane<T> Transform(Plane<T> plane, Matrix<T> matrix)
        {
            Plane<T> result;
            Transform(ref plane, ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Transforms a normalized plane by a matrix.
        /// </summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <param name="result">The transformed plane.</param>
        public static void Transform(ref Plane<T> plane, ref Matrix<T> matrix, out Plane<T> result)
        {
            // See "Transforming Normals" in http://www.glprogramming.com/red/appendixf.html
            // for an explanation of how this works.

            Matrix<T> transformedMatrix;
            Matrix<T>.Invert(ref matrix, out transformedMatrix);
            Matrix<T>.Transpose(ref transformedMatrix, out transformedMatrix);

            var vector = new Vec4<T>(plane.Normal, plane.D);

            Vec4<T> transformedVector;
            Vec4<T>.Transform(ref vector, ref transformedMatrix, out transformedVector);

            result = new Plane<T>(transformedVector);
        }

        /// <summary>
        /// Transforms a normalized plane by a quaternion rotation.
        /// </summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="rotation">The quaternion rotation.</param>
        /// <returns>The transformed plane.</returns>
        public static Plane<T> Transform(Plane<T> plane, Quaternion<T> rotation)
        {
            Plane<T> result;
            Transform(ref plane, ref rotation, out result);
            return result;
        }

        /// <summary>
        /// Transforms a normalized plane by a quaternion rotation.
        /// </summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="rotation">The quaternion rotation.</param>
        /// <param name="result">The transformed plane.</param>
        public static void Transform(ref Plane<T> plane, ref Quaternion<T> rotation, out Plane<T> result)
        {
            Vec3<T>.Transform(ref plane.Normal, ref rotation, out result.Normal);
            result.D = plane.D;
        }

        /// <summary>
        /// Normalize the normal vector of this plane.
        /// </summary>
        public void Normalize()
        {
            T length = Normal.Length();
            T factor =  T.One / length;            
            Vec3<T>.Multiply(ref Normal, factor, out Normal);
            D = D * factor;
        }

        /// <summary>
        /// Get a normalized version of the specified plane.
        /// </summary>
        /// <param name="value">The <see cref="Plane"/> to normalize.</param>
        /// <returns>A normalized version of the specified <see cref="Plane"/>.</returns>
        public static Plane<T> Normalize(Plane<T> value)
        {
			Plane<T> ret;
			Normalize(ref value, out ret);
			return ret;
        }

        /// <summary>
        /// Get a normalized version of the specified plane.
        /// </summary>
        /// <param name="value">The <see cref="Plane"/> to normalize.</param>
        /// <param name="result">A normalized version of the specified <see cref="Plane"/>.</param>
        public static void Normalize(ref Plane<T> value, out Plane<T> result)
        {
            T length = value.Normal.Length();
            T factor =  T.One / length;            
            Vec3<T>.Multiply(ref value.Normal, factor, out result.Normal);
            result.D = value.D * factor;
        }

        /// <summary>
        /// Check if two planes are not equal.
        /// </summary>
        /// <param name="plane1">A <see cref="Plane"/> to check for inequality.</param>
        /// <param name="plane2">A <see cref="Plane"/> to check for inequality.</param>
        /// <returns><code>true</code> if the two planes are not equal, <code>false</code> if they are.</returns>
        public static bool operator !=(Plane<T> plane1, Plane<T> plane2)
        {
            return !plane1.Equals(plane2);
        }

        /// <summary>
        /// Check if two planes are equal.
        /// </summary>
        /// <param name="plane1">A <see cref="Plane"/> to check for equality.</param>
        /// <param name="plane2">A <see cref="Plane"/> to check for equality.</param>
        /// <returns><code>true</code> if the two planes are equal, <code>false</code> if they are not.</returns>
        public static bool operator ==(Plane<T> plane1, Plane<T> plane2)
        {
            return plane1.Equals(plane2);
        }

        /// <summary>
        /// Check if this <see cref="Plane"/> is equal to another <see cref="Plane"/>.
        /// </summary>
        /// <param name="other">An <see cref="Object"/> to check for equality with this <see cref="Plane"/>.</param>
        /// <returns>
        /// <code>true</code> if the specified <see cref="object"/> is equal to this <see cref="Plane"/>,
        /// <code>false</code> if it is not.
        /// </returns>
        public override bool Equals(object? other)
        {
            return (other is Plane plane) && this.Equals(plane);
        }

        /// <summary>
        /// Check if this <see cref="Plane"/> is equal to another <see cref="Plane"/>.
        /// </summary>
        /// <param name="other">A <see cref="Plane"/> to check for equality with this <see cref="Plane"/>.</param>
        /// <returns>
        /// <code>true</code> if the specified <see cref="Plane"/> is equal to this one,
        /// <code>false</code> if it is not.
        /// </returns>
        public bool Equals(Plane<T> other)
        {
            return ((Normal == other.Normal) && (D == other.D));
        }

        /// <summary>
        /// Get a hash code for this <see cref="Plane"/>.
        /// </summary>
        /// <returns>A hash code for this <see cref="Plane"/>.</returns>
        public override int GetHashCode()
        {
            return Normal.GetHashCode() ^ D.GetHashCode();
        }


        /// <summary>
        /// Check if this <see cref="Plane"/> intersects a <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox{T}"/> to test for intersection.</param>
        /// <returns>
        /// The type of intersection of this <see cref="Plane"/> with the specified <see cref="BoundingBox{T}"/>.
        /// </returns>
        public PlaneIntersectionType Intersects(BoundingBox<T> box)
        {
            return box.Intersects(this);
        }

        /// <summary>
        /// Check if this <see cref="Plane"/> intersects a <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox{T}"/> to test for intersection.</param>
        /// <param name="result">
        /// The type of intersection of this <see cref="Plane"/> with the specified <see cref="BoundingBox{T}"/>.
        /// </param>
        public void Intersects(ref BoundingBox<T> box, out PlaneIntersectionType result)
        {
            box.Intersects (ref this, out result);
        }

        /// <summary>
        /// Check if this <see cref="Plane"/> intersects a <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="frustum">The <see cref="BoundingFrustum{T}"/> to test for intersection.</param>
        /// <returns>
        /// The type of intersection of this <see cref="Plane"/> with the specified <see cref="BoundingFrustum{T}"/>.
        /// </returns>
        public PlaneIntersectionType Intersects(BoundingFrustum<T> frustum)
        {
            return frustum.Intersects(this);
        }

        /// <summary>
        /// Check if this <see cref="Plane"/> intersects a <see cref="BoundingSphere{T}"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingSphere{T}"/> to test for intersection.</param>
        /// <returns>
        /// The type of intersection of this <see cref="Plane"/> with the specified <see cref="BoundingSphere{T}"/>.
        /// </returns>
        public PlaneIntersectionType Intersects(BoundingSphere<T> sphere)
        {
            return sphere.Intersects(this);
        }

        /// <summary>
        /// Check if this <see cref="Plane"/> intersects a <see cref="BoundingSphere{T}"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingSphere{T}"/> to test for intersection.</param>
        /// <param name="result">
        /// The type of intersection of this <see cref="Plane"/> with the specified <see cref="BoundingSphere{T}"/>.
        /// </param>
        public void Intersects(ref BoundingSphere<T> sphere, out PlaneIntersectionType result)
        {
            sphere.Intersects(ref this, out result);
        }

        internal PlaneIntersectionType Intersects(ref Vec3<T> point)
        {
            T distance;
            DotCoordinate(ref point, out distance);

            if (distance > T.Zero)
                return PlaneIntersectionType.Front;

            if (distance < T.Zero)
                return PlaneIntersectionType.Back;

            return PlaneIntersectionType.Intersecting;
        }

        internal string DebugDisplayString
        {
            get {
                return string.Empty;
//                return string.Concat(
//                    this.Normal.DebugDisplayString, "  ",
//                    this.D.ToString()
//                    );
            }
        }

        /// <summary>
        /// Get a <see cref="String"/> representation of this <see cref="Plane"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="Plane"/>.</returns>
        public override string ToString()
        {
            return "{Normal:" + Normal + " D:" + D + "}";
        }

        /// <summary>
        /// Deconstruction method for <see cref="Plane"/>.
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="d"></param>
        public void Deconstruct(out Vec3<T> normal, out T d)
        {
            normal = Normal;
            d = D;
        }

        /// <summary>
        /// Returns a <see cref="System.Numerics.Plane"/>.
        /// </summary>
        public Plane ToNumerics()
        {
            return new Plane(float.CreateChecked(this.Normal.X), float.CreateChecked(this.Normal.Y), float.CreateChecked(this.Normal.Z), float.CreateChecked(this.D));
        }

        /// <summary>
        /// Converts a <see cref="System.Numerics.Plane"/> to a <see cref="Plane"/>.
        /// </summary>
        /// <param name="value">The converted value.</param>
        public static implicit operator Plane<T>(Plane value)
        {
            return new Plane<T>(value.Normal, T.CreateChecked(value.D));
        }

    }
}
