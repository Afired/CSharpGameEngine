using System.Diagnostics;
using System.Numerics;

namespace GameEngine.Numerics
{
    /// <summary>
    /// Defines a viewing frustum for intersection operations.
    /// </summary>
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public class BoundingFrustum<T> : IEquatable<BoundingFrustum<T>> where T : struct, IFloatingPointIeee754<T>
    {
        private Matrix<T> _matrix;
        private readonly Vec3<T>[] _corners = new Vec3<T>[CORNER_COUNT];
        private readonly Plane<T>[] _planes = new Plane<T>[PLANE_COUNT];

        /// <summary>
        /// The number of planes in the frustum.
        /// </summary>
        public const int PLANE_COUNT = 6;

        /// <summary>
        /// The number of corner points in the frustum.
        /// </summary>
        public const int CORNER_COUNT = 8;

        /// <summary>
        /// Gets or sets the <see cref="Matrix"/> of the frustum.
        /// </summary>
        public Matrix<T> Matrix
        {
            get { return this._matrix; }
            set
            {
                this._matrix = value;
                this.CreatePlanes();    // FIXME: The odds are the planes will be used a lot more often than the matrix
                this.CreateCorners();   // is updated, so this should help performance. I hope ;)
            }
        }

        /// <summary>
        /// Gets the near plane of the frustum.
        /// </summary>
        public Plane<T> Near
        {
            get { return this._planes[0]; }
        }

        /// <summary>
        /// Gets the far plane of the frustum.
        /// </summary>
        public Plane<T> Far
        {
            get { return this._planes[1]; }
        }

        /// <summary>
        /// Gets the left plane of the frustum.
        /// </summary>
        public Plane<T> Left
        {
            get { return this._planes[2]; }
        }

        /// <summary>
        /// Gets the right plane of the frustum.
        /// </summary>
        public Plane<T> Right
        {
            get { return this._planes[3]; }
        }

        /// <summary>
        /// Gets the top plane of the frustum.
        /// </summary>
        public Plane<T> Top
        {
            get { return this._planes[4]; }
        }

        /// <summary>
        /// Gets the bottom plane of the frustum.
        /// </summary>
        public Plane<T> Bottom
        {
            get { return this._planes[5]; }
        }

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    "Near( ", this._planes[0].DebugDisplayString, " )  \r\n",
                    "Far( ", this._planes[1].DebugDisplayString, " )  \r\n",
                    "Left( ", this._planes[2].DebugDisplayString, " )  \r\n",
                    "Right( ", this._planes[3].DebugDisplayString, " )  \r\n",
                    "Top( ", this._planes[4].DebugDisplayString, " )  \r\n",
                    "Bottom( ", this._planes[5].DebugDisplayString, " )  "
                    );
            }
        }

        /// <summary>
        /// Constructs the frustum by extracting the view planes from a matrix.
        /// </summary>
        /// <param name="value">Combined matrix which usually is (View * Projection).</param>
        public BoundingFrustum(Matrix<T> value)
        {
            this._matrix = value;
            this.CreatePlanes();
            this.CreateCorners();
        }

        /// <summary>
        /// Compares whether two <see cref="BoundingFrustum{T}"/> instances are equal.
        /// </summary>
        /// <param name="a"><see cref="BoundingFrustum{T}"/> instance on the left of the equal sign.</param>
        /// <param name="b"><see cref="BoundingFrustum{T}"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(BoundingFrustum<T> a, BoundingFrustum<T>? b)
        {
            if (Equals(a, null))
                return (Equals(b, null));

            if (Equals(b, null))
                return (Equals(a, null));

            return a._matrix == (b._matrix);
        }

        /// <summary>
        /// Compares whether two <see cref="BoundingFrustum{T}"/> instances are not equal.
        /// </summary>
        /// <param name="a"><see cref="BoundingFrustum{T}"/> instance on the left of the not equal sign.</param>
        /// <param name="b"><see cref="BoundingFrustum{T}"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(BoundingFrustum<T> a, BoundingFrustum<T> b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustum{T}"/> and specified <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBox{T}"/> for testing.</param>
        /// <returns>Result of testing for containment between this <see cref="BoundingFrustum{T}"/> and specified <see cref="BoundingBox{T}"/>.</returns>
        public ContainmentType Contains(BoundingBox<T> box)
        {
            var result = default(ContainmentType);
            this.Contains(ref box, out result);
            return result;
        }

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustum{T}"/> and specified <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBox{T}"/> for testing.</param>
        /// <param name="result">Result of testing for containment between this <see cref="BoundingFrustum{T}"/> and specified <see cref="BoundingBox{T}"/> as an output parameter.</param>
        public void Contains(ref BoundingBox<T> box, out ContainmentType result)
        {
            var intersects = false;
            for (var i = 0; i < PLANE_COUNT; ++i)
            {
                var planeIntersectionType = default(PlaneIntersectionType);
                box.Intersects(ref this._planes[i], out planeIntersectionType);
                switch (planeIntersectionType)
                {
                case PlaneIntersectionType.Front:
                    result = ContainmentType.Disjoint; 
                    return;
                case PlaneIntersectionType.Intersecting:
                    intersects = true;
                    break;
                }
            }
            result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustum{T}"/> and specified <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="frustum">A <see cref="BoundingFrustum{T}"/> for testing.</param>
        /// <returns>Result of testing for containment between this <see cref="BoundingFrustum{T}"/> and specified <see cref="BoundingFrustum{T}"/>.</returns>
        public ContainmentType Contains(BoundingFrustum<T> frustum)
        {
            if (this == frustum)                // We check to see if the two frustums are equal
                return ContainmentType.Contains;// If they are, there's no need to go any further.

            var intersects = false;
            for (var i = 0; i < PLANE_COUNT; ++i)
            {
                PlaneIntersectionType planeIntersectionType;
                frustum.Intersects(ref _planes[i], out planeIntersectionType);
                switch (planeIntersectionType)
                {
                    case PlaneIntersectionType.Front:
                        return ContainmentType.Disjoint;
                    case PlaneIntersectionType.Intersecting:
                        intersects = true;
                        break;
                }
            }
            return intersects ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustum{T}"/> and specified <see cref="BoundingSphere{T}"/>.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere{T}"/> for testing.</param>
        /// <returns>Result of testing for containment between this <see cref="BoundingFrustum{T}"/> and specified <see cref="BoundingSphere{T}"/>.</returns>
        public ContainmentType Contains(BoundingSphere<T> sphere)
        {
            var result = default(ContainmentType);
            this.Contains(ref sphere, out result);
            return result;
        }

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustum{T}"/> and specified <see cref="BoundingSphere{T}"/>.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere{T}"/> for testing.</param>
        /// <param name="result">Result of testing for containment between this <see cref="BoundingFrustum{T}"/> and specified <see cref="BoundingSphere{T}"/> as an output parameter.</param>
        public void Contains(ref BoundingSphere<T> sphere, out ContainmentType result)
        {
            var intersects = false;
            for (var i = 0; i < PLANE_COUNT; ++i) 
            {
                var planeIntersectionType = default(PlaneIntersectionType);

                // TODO: we might want to inline this for performance reasons
                sphere.Intersects(ref this._planes[i], out planeIntersectionType);
                switch (planeIntersectionType)
                {
                case PlaneIntersectionType.Front:
                    result = ContainmentType.Disjoint; 
                    return;
                case PlaneIntersectionType.Intersecting:
                    intersects = true;
                    break;
                }
            }
            result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustum{T}"/> and specified <see cref="Vec3{T}"/>.
        /// </summary>
        /// <param name="point">A <see cref="Vec3{T}"/> for testing.</param>
        /// <returns>Result of testing for containment between this <see cref="BoundingFrustum{T}"/> and specified <see cref="Vec3{T}"/>.</returns>
        public ContainmentType Contains(Vec3<T> point)
        {
            var result = default(ContainmentType);
            this.Contains(ref point, out result);
            return result;
        }

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustum{T}"/> and specified <see cref="Vec3{T}"/>.
        /// </summary>
        /// <param name="point">A <see cref="Vec3{T}"/> for testing.</param>
        /// <param name="result">Result of testing for containment between this <see cref="BoundingFrustum{T}"/> and specified <see cref="Vec3{T}"/> as an output parameter.</param>
        public void Contains(ref Vec3<T> point, out ContainmentType result)
        {
            for (var i = 0; i < PLANE_COUNT; ++i)
            {
                // TODO: we might want to inline this for performance reasons
                if (PlaneHelper.ClassifyPoint(ref point, ref _planes[i]) > T.Zero)
                {   
                    result = ContainmentType.Disjoint;
                    return;
                }
            }
            result = ContainmentType.Contains;
        }
        
        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="other">The <see cref="BoundingFrustum{T}"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(BoundingFrustum<T>? other)
        {
            return (this == other);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object? obj)
        {
            return (obj is BoundingFrustum<T> frustum) && this == frustum;
        }

        /// <summary>
        /// Returns a copy of internal corners array.
        /// </summary>
        /// <returns>The array of corners.</returns>
        public Vec3<T>[] GetCorners()
        {
            return (Vec3<T>[])this._corners.Clone();
        }

        /// <summary>
        /// Returns a copy of internal corners array.
        /// </summary>
        /// <param name="corners">The array which values will be replaced to corner values of this instance. It must have size of <see cref="CORNER_COUNT"/>.</param>
		public void GetCorners(Vec3<T>[] corners)
        {
			if (corners == null) throw new ArgumentNullException(nameof(corners));
		    if (corners.Length < CORNER_COUNT) throw new ArgumentOutOfRangeException(nameof(corners));

            this._corners.CopyTo(corners, 0);
        }

        /// <summary>
        /// Gets the hash code of this <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="BoundingFrustum{T}"/>.</returns>
        public override int GetHashCode()
        {
            return _matrix.GetHashCode();
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingBox{T}"/> intersects with this <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBox{T}"/> for intersection test.</param>
        /// <returns><c>true</c> if specified <see cref="BoundingBox{T}"/> intersects with this <see cref="BoundingFrustum{T}"/>; <c>false</c> otherwise.</returns>
        public bool Intersects(BoundingBox<T> box)
        {
			var result = false;
			this.Intersects(ref box, out result);
			return result;
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingBox{T}"/> intersects with this <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBox{T}"/> for intersection test.</param>
        /// <param name="result"><c>true</c> if specified <see cref="BoundingBox{T}"/> intersects with this <see cref="BoundingFrustum{T}"/>; <c>false</c> otherwise as an output parameter.</param>
        public void Intersects(ref BoundingBox<T> box, out bool result)
        {
			var containment = default(ContainmentType);
			this.Contains(ref box, out containment);
			result = containment != ContainmentType.Disjoint;
		}

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingFrustum{T}"/> intersects with this <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="frustum">An other <see cref="BoundingFrustum{T}"/> for intersection test.</param>
        /// <returns><c>true</c> if other <see cref="BoundingFrustum{T}"/> intersects with this <see cref="BoundingFrustum{T}"/>; <c>false</c> otherwise.</returns>
        public bool Intersects(BoundingFrustum<T> frustum)
        {
            return Contains(frustum) != ContainmentType.Disjoint;
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingSphere{T}"/> intersects with this <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere{T}"/> for intersection test.</param>
        /// <returns><c>true</c> if specified <see cref="BoundingSphere{T}"/> intersects with this <see cref="BoundingFrustum{T}"/>; <c>false</c> otherwise.</returns>
        public bool Intersects(BoundingSphere<T> sphere)
        {
            var result = default(bool);
            this.Intersects(ref sphere, out result);
            return result;
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingSphere{T}"/> intersects with this <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphere{T}"/> for intersection test.</param>
        /// <param name="result"><c>true</c> if specified <see cref="BoundingSphere{T}"/> intersects with this <see cref="BoundingFrustum{T}"/>; <c>false</c> otherwise as an output parameter.</param>
        public void Intersects(ref BoundingSphere<T> sphere, out bool result)
        {
            var containment = default(ContainmentType);
            this.Contains(ref sphere, out containment);
            result = containment != ContainmentType.Disjoint;
        }

        /// <summary>
        /// Gets type of intersection between specified <see cref="Plane"/> and this <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="plane">A <see cref="Plane"/> for intersection test.</param>
        /// <returns>A plane intersection type.</returns>
        public PlaneIntersectionType Intersects(Plane<T> plane)
        {
            PlaneIntersectionType result;
            Intersects(ref plane, out result);
            return result;
        }

        /// <summary>
        /// Gets type of intersection between specified <see cref="Plane"/> and this <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="plane">A <see cref="Plane"/> for intersection test.</param>
        /// <param name="result">A plane intersection type as an output parameter.</param>
        public void Intersects(ref Plane<T> plane, out PlaneIntersectionType result)
        {
            result = plane.Intersects(ref _corners[0]);
            for (int i = 1; i < _corners.Length; i++)
                if (plane.Intersects(ref _corners[i]) != result)
                    result = PlaneIntersectionType.Intersecting;
        }
        
        /// <summary>
        /// Gets the distance of intersection of <see cref="Ray{T}"/> and this <see cref="BoundingFrustum{T}"/> or null if no intersection happens.
        /// </summary>
        /// <param name="ray">A <see cref="Ray{T}"/> for intersection test.</param>
        /// <returns>Distance at which ray intersects with this <see cref="BoundingFrustum{T}"/> or null if no intersection happens.</returns>
        public bool Intersects(Ray<T> ray, out T? result)
        {
            return Intersects(ref ray, out result);
        }

        /// <summary>
        /// Gets the distance of intersection of <see cref="Ray{T}"/> and this <see cref="BoundingFrustum{T}"/> or null if no intersection happens.
        /// </summary>
        /// <param name="ray">A <see cref="Ray{T}"/> for intersection test.</param>
        /// <param name="result">True and Distance at which ray intersects with this <see cref="BoundingFrustum{T}"/> or false and if no intersection happens.</param>
        public bool Intersects(ref Ray<T> ray, out T? result)
        {
            ContainmentType ctype;
            this.Contains(ref ray.Position, out ctype);

            switch (ctype)
            {
                case ContainmentType.Disjoint:
                    result = default;
                    return false;
                case ContainmentType.Contains:
                    result = T.Zero;
                    return true;
                case ContainmentType.Intersects:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        } 

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="BoundingFrustum{T}"/> in the format:
        /// {Near:[nearPlane] Far:[farPlane] Left:[leftPlane] Right:[rightPlane] Top:[topPlane] Bottom:[bottomPlane]}
        /// </summary>
        /// <returns><see cref="String"/> representation of this <see cref="BoundingFrustum{T}"/>.</returns>
        public override string ToString()
        {
            return "{Near: " + this._planes[0] +
                   " Far:" + this._planes[1] +
                   " Left:" + this._planes[2] +
                   " Right:" + this._planes[3] +
                   " Top:" + this._planes[4] +
                   " Bottom:" + this._planes[5] +
                   "}";
        }

        private void CreateCorners()
        {
            IntersectionPoint(ref this._planes[0], ref this._planes[2], ref this._planes[4], out this._corners[0]);
            IntersectionPoint(ref this._planes[0], ref this._planes[3], ref this._planes[4], out this._corners[1]);
            IntersectionPoint(ref this._planes[0], ref this._planes[3], ref this._planes[5], out this._corners[2]);
            IntersectionPoint(ref this._planes[0], ref this._planes[2], ref this._planes[5], out this._corners[3]);
            IntersectionPoint(ref this._planes[1], ref this._planes[2], ref this._planes[4], out this._corners[4]);
            IntersectionPoint(ref this._planes[1], ref this._planes[3], ref this._planes[4], out this._corners[5]);
            IntersectionPoint(ref this._planes[1], ref this._planes[3], ref this._planes[5], out this._corners[6]);
            IntersectionPoint(ref this._planes[1], ref this._planes[2], ref this._planes[5], out this._corners[7]);
        }

        private void CreatePlanes()
        {            
            this._planes[0] = new Plane<T>(-this._matrix.M13, -this._matrix.M23, -this._matrix.M33, -this._matrix.M43);
            this._planes[1] = new Plane<T>(this._matrix.M13 - this._matrix.M14, this._matrix.M23 - this._matrix.M24, this._matrix.M33 - this._matrix.M34, this._matrix.M43 - this._matrix.M44);
            this._planes[2] = new Plane<T>(-this._matrix.M14 - this._matrix.M11, -this._matrix.M24 - this._matrix.M21, -this._matrix.M34 - this._matrix.M31, -this._matrix.M44 - this._matrix.M41);
            this._planes[3] = new Plane<T>(this._matrix.M11 - this._matrix.M14, this._matrix.M21 - this._matrix.M24, this._matrix.M31 - this._matrix.M34, this._matrix.M41 - this._matrix.M44);
            this._planes[4] = new Plane<T>(this._matrix.M12 - this._matrix.M14, this._matrix.M22 - this._matrix.M24, this._matrix.M32 - this._matrix.M34, this._matrix.M42 - this._matrix.M44);
            this._planes[5] = new Plane<T>(-this._matrix.M14 - this._matrix.M12, -this._matrix.M24 - this._matrix.M22, -this._matrix.M34 - this._matrix.M32, -this._matrix.M44 - this._matrix.M42);
            
            this.NormalizePlane(ref this._planes[0]);
            this.NormalizePlane(ref this._planes[1]);
            this.NormalizePlane(ref this._planes[2]);
            this.NormalizePlane(ref this._planes[3]);
            this.NormalizePlane(ref this._planes[4]);
            this.NormalizePlane(ref this._planes[5]);
        }

        private static void IntersectionPoint(ref Plane<T> a, ref Plane<T> b, ref Plane<T> c, out Vec3<T> result)
        {
            // Formula used
            //                d1 ( N2 * N3 ) + d2 ( N3 * N1 ) + d3 ( N1 * N2 )
            //P =   -------------------------------------------------------------------------
            //                             N1 . ( N2 * N3 )
            //
            // Note: N refers to the normal, d refers to the displacement. '.' means dot product. '*' means cross product
            
            Vec3<T> v1, v2, v3;
            Vec3<T> cross;
            
            Vec3<T>.Cross(ref b.Normal, ref c.Normal, out cross);
            
            T f;
            Vec3<T>.Dot(ref a.Normal, ref cross, out f);
            f *= T.NegativeOne;
            
            Vec3<T>.Cross(ref b.Normal, ref c.Normal, out cross);
            Vec3<T>.Multiply(ref cross, a.D, out v1);
            //v1 = (a.D * (Vector3.Cross(b.Normal, c.Normal)));
            
            
            Vec3<T>.Cross(ref c.Normal, ref a.Normal, out cross);
            Vec3<T>.Multiply(ref cross, b.D, out v2);
            //v2 = (b.D * (Vector3.Cross(c.Normal, a.Normal)));
            
            
            Vec3<T>.Cross(ref a.Normal, ref b.Normal, out cross);
            Vec3<T>.Multiply(ref cross, c.D, out v3);
            //v3 = (c.D * (Vector3.Cross(a.Normal, b.Normal)));
            
            result.X = (v1.X + v2.X + v3.X) / f;
            result.Y = (v1.Y + v2.Y + v3.Y) / f;
            result.Z = (v1.Z + v2.Z + v3.Z) / f;
        }
        
        private void NormalizePlane(ref Plane<T> p)
        {
            T factor = T.One / p.Normal.Length();
            p.Normal.X *= factor;
            p.Normal.Y *= factor;
            p.Normal.Z *= factor;
            p.D *= factor;
        }

    }
}
