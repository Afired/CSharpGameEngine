using System.Diagnostics;
using System.Numerics;
using GameEngine.Core.Serialization;

namespace GameEngine.Numerics
{
    /// <summary>
    /// Describes a sphere in 3D-space for bounding operations.
    /// </summary>
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct BoundingSphere<T> : IEquatable<BoundingSphere<T>> where T : struct, IFloatingPointIeee754<T>
    {
        /// <summary>
        /// The sphere center.
        /// </summary>
        [Serialized] public Vec3<T> Center;

        /// <summary>
        /// The sphere radius.
        /// </summary>
        [Serialized] public T Radius;

        internal string DebugDisplayString
        {
            get {
                return string.Empty;
//                return string.Concat(
//                    "Center( ", this.Center.DebugDisplayString, " )  \r\n",
//                    "Radius( ", this.Radius.ToString(), " )"
//                    );
            }
        }

        /// <summary>
        /// Constructs a bounding sphere with the specified center and radius.  
        /// </summary>
        /// <param name="center">The sphere center.</param>
        /// <param name="radius">The sphere radius.</param>
        public BoundingSphere(Vec3<T> center, T radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        /// <summary>
        /// Test if a bounding box is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="box">The box for testing.</param>
        /// <returns>The containment type.</returns>
        public ContainmentType Contains(BoundingBox<T> box)
        {
            //check if all corner is in sphere
            bool inside = true;
            foreach (Vec3<T> corner in box.GetCorners())
            {
                if (this.Contains(corner) == ContainmentType.Disjoint)
                {
                    inside = false;
                    break;
                }
            }

            if (inside)
                return ContainmentType.Contains;

            //check if the distance from sphere center to cube face < radius
            T min = T.Zero;

            if (Center.X < box.Min.X)
				min += (Center.X - box.Min.X) * (Center.X - box.Min.X);

			else if (Center.X > box.Max.X)
					min += (Center.X - box.Max.X) * (Center.X - box.Max.X);

			if (Center.Y < box.Min.Y)
				min += (Center.Y - box.Min.Y) * (Center.Y - box.Min.Y);

			else if (Center.Y > box.Max.Y)
				min += (Center.Y - box.Max.Y) * (Center.Y - box.Max.Y);

			if (Center.Z < box.Min.Z)
				min += (Center.Z - box.Min.Z) * (Center.Z - box.Min.Z);

			else if (Center.Z > box.Max.Z)
				min += (Center.Z - box.Max.Z) * (Center.Z - box.Max.Z);

			if (min <= Radius * Radius) 
				return ContainmentType.Intersects;
            
            //else disjoint
            return ContainmentType.Disjoint;
        }

        /// <summary>
        /// Test if a bounding box is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="box">The box for testing.</param>
        /// <param name="result">The containment type as an output parameter.</param>
        public void Contains(ref BoundingBox<T> box, out ContainmentType result)
        {
            result = this.Contains(box);
        }

        /// <summary>
        /// Test if a frustum is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="frustum">The frustum for testing.</param>
        /// <returns>The containment type.</returns>
        public ContainmentType Contains(BoundingFrustum<T> frustum)
        {
            //check if all corner is in sphere
            bool inside = true;

            Vec3<T>[] corners = frustum.GetCorners();
            foreach (Vec3<T> corner in corners)
            {
                if (this.Contains(corner) == ContainmentType.Disjoint)
                {
                    inside = false;
                    break;
                }
            }
            if (inside)
                return ContainmentType.Contains;

            //check if the distance from sphere center to frustrum face < radius
            T min = T.Zero;
            //TODO : calcul dmin

            if (min <= Radius * Radius)
                return ContainmentType.Intersects;

            //else disjoint
            return ContainmentType.Disjoint;
        }

        /// <summary>
        /// Test if a frustum is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="frustum">The frustum for testing.</param>
        /// <param name="result">The containment type as an output parameter.</param>
        public void Contains(ref BoundingFrustum<T> frustum,out ContainmentType result)
        {
            result = this.Contains(frustum);
        }

        /// <summary>
        /// Test if a sphere is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="sphere">The other sphere for testing.</param>
        /// <returns>The containment type.</returns>
        public ContainmentType Contains(BoundingSphere<T> sphere)
        {
            ContainmentType result;
            Contains(ref sphere, out result);
            return result;
        }

        /// <summary>
        /// Test if a sphere is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="sphere">The other sphere for testing.</param>
        /// <param name="result">The containment type as an output parameter.</param>
        public void Contains(ref BoundingSphere<T> sphere, out ContainmentType result)
        {
            T sqDistance;
            Vec3<T>.DistanceSquared(ref sphere.Center, ref Center, out sqDistance);

            if (sqDistance > (sphere.Radius + Radius) * (sphere.Radius + Radius))
                result = ContainmentType.Disjoint;

            else if (sqDistance <= (Radius - sphere.Radius) * (Radius - sphere.Radius))
                result = ContainmentType.Contains;

            else
                result = ContainmentType.Intersects;
        }

        /// <summary>
        /// Test if a point is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="point">The vector in 3D-space for testing.</param>
        /// <returns>The containment type.</returns>
        public ContainmentType Contains(Vec3<T> point)
        {
            ContainmentType result;
            Contains(ref point, out result);
            return result;
        }

        /// <summary>
        /// Test if a point is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="point">The vector in 3D-space for testing.</param>
        /// <param name="result">The containment type as an output parameter.</param>
        public void Contains(ref Vec3<T> point, out ContainmentType result)
        {
            T sqRadius = Radius * Radius;
            T sqDistance;
            Vec3<T>.DistanceSquared(ref point, ref Center, out sqDistance);
            
            if (sqDistance > sqRadius)
                result = ContainmentType.Disjoint;

            else if (sqDistance < sqRadius)
                result = ContainmentType.Contains;

            else 
                result = ContainmentType.Intersects;
        }
        
        /// <summary>
        /// Creates the smallest <see cref="BoundingSphere{T}"/> that can contain a specified <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="box">The box to create the sphere from.</param>
        /// <returns>The new <see cref="BoundingSphere{T}"/>.</returns>
        public static BoundingSphere<T> CreateFromBoundingBox(BoundingBox<T> box)
        {
            BoundingSphere<T> result;
            CreateFromBoundingBox(ref box, out result);
            return result;
        }

        /// <summary>
        /// Creates the smallest <see cref="BoundingSphere{T}"/> that can contain a specified <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="box">The box to create the sphere from.</param>
        /// <param name="result">The new <see cref="BoundingSphere{T}"/> as an output parameter.</param>
        public static void CreateFromBoundingBox(ref BoundingBox<T> box, out BoundingSphere<T> result)
        {
            // Find the center of the box.
            Vec3<T> center = new Vec3<T>((box.Min.X + box.Max.X) / T.CreateChecked(2),
                                         (box.Min.Y + box.Max.Y) / T.CreateChecked(2),
                                         (box.Min.Z + box.Max.Z) / T.CreateChecked(2));

            // Find the distance between the center and one of the corners of the box.
            T radius = Vec3<T>.Distance(center, box.Max);

            result = new BoundingSphere<T>(center, radius);
        }

        /// <summary>
        /// Creates the smallest <see cref="BoundingSphere{T}"/> that can contain a specified <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="frustum">The frustum to create the sphere from.</param>
        /// <returns>The new <see cref="BoundingSphere{T}"/>.</returns>
        public static BoundingSphere<T> CreateFromFrustum(BoundingFrustum<T> frustum)
        {
            return CreateFromPoints(frustum.GetCorners());
        }

        /// <summary>
        /// Creates the smallest <see cref="BoundingSphere{T}"/> that can contain a specified list of points in 3D-space. 
        /// </summary>
        /// <param name="points">List of point to create the sphere from.</param>
        /// <returns>The new <see cref="BoundingSphere{T}"/>.</returns>
        public static BoundingSphere<T> CreateFromPoints(IEnumerable<Vec3<T>> points)
        {
            if (points == null )
                throw new ArgumentNullException(nameof(points));

            // From "Real-Time Collision Detection" (Page 89)

            Vec3<T> minx = new Vec3<T>(T.PositiveInfinity, T.PositiveInfinity, T.PositiveInfinity);
            Vec3<T> maxx = -minx;
            Vec3<T> miny = minx;
            Vec3<T> maxy = -minx;
            Vec3<T> minz = minx;
            Vec3<T> maxz = -minx;

            // Find the most extreme points along the principle axis.
            var numPoints = 0;           
            foreach (Vec3<T> pt in points)
            {
                ++numPoints;

                if (pt.X < minx.X) 
                    minx = pt;
                if (pt.X > maxx.X) 
                    maxx = pt;
                if (pt.Y < miny.Y) 
                    miny = pt;
                if (pt.Y > maxy.Y) 
                    maxy = pt;
                if (pt.Z < minz.Z) 
                    minz = pt;
                if (pt.Z > maxz.Z) 
                    maxz = pt;
            }

            if (numPoints == 0)
                throw new ArgumentException("You should have at least one point in points.");

            var sqDistX = Vec3<T>.DistanceSquared(maxx, minx);
            var sqDistY = Vec3<T>.DistanceSquared(maxy, miny);
            var sqDistZ = Vec3<T>.DistanceSquared(maxz, minz);

            // Pick the pair of most distant points.
            Vec3<T> min = minx;
            Vec3<T> max = maxx;
            if (sqDistY > sqDistX && sqDistY > sqDistZ) 
            {
                max = maxy;
                min = miny;
            }
            if (sqDistZ > sqDistX && sqDistZ > sqDistY) 
            {
                max = maxz;
                min = minz;
            }
            
            Vec3<T> center = (min + max) * T.CreateChecked(0.5);
            T radius = Vec3<T>.Distance(max, center);
            
            // Test every point and expand the sphere.
            // The current bounding sphere is just a good approximation and may not enclose all points.            
            // From: Mathematics for 3D Game Programming and Computer Graphics, Eric Lengyel, Third Edition.
            // Page 218
            T sqRadius = radius * radius;
            foreach (Vec3<T> pt in points)
            {
                Vec3<T> diff = (pt - center);
                T sqDist = diff.LengthSquared();
                if (sqDist > sqRadius)
                {
                    T distance = T.Sqrt(sqDist); // equal to diff.Length();
                    Vec3<T> direction = diff / distance;
                    Vec3<T> g = center - radius * direction;
                    center = (g + pt) / T.CreateChecked(2);
                    radius = Vec3<T>.Distance(pt, center);
                    sqRadius = radius * radius;
                }
            }

            return new BoundingSphere<T>(center, radius);
        }

        /// <summary>
        /// Creates the smallest <see cref="BoundingSphere{T}"/> that can contain two spheres.
        /// </summary>
        /// <param name="original">First sphere.</param>
        /// <param name="additional">Second sphere.</param>
        /// <returns>The new <see cref="BoundingSphere{T}"/>.</returns>
        public static BoundingSphere<T> CreateMerged(BoundingSphere<T> original, BoundingSphere<T> additional)
        {
            BoundingSphere<T> result;
            CreateMerged(ref original, ref additional, out result);
            return result;
        }

        /// <summary>
        /// Creates the smallest <see cref="BoundingSphere{T}"/> that can contain two spheres.
        /// </summary>
        /// <param name="original">First sphere.</param>
        /// <param name="additional">Second sphere.</param>
        /// <param name="result">The new <see cref="BoundingSphere{T}"/> as an output parameter.</param>
        public static void CreateMerged(ref BoundingSphere<T> original, ref BoundingSphere<T> additional, out BoundingSphere<T> result)
        {
            Vec3<T> ocenterToaCenter = Vec3<T>.Subtract(additional.Center, original.Center);
            T distance = ocenterToaCenter.Length();
            if (distance <= original.Radius + additional.Radius)//intersect
            {
                if (distance <= original.Radius - additional.Radius)//original contain additional
                {
                    result = original;
                    return;
                }
                if (distance <= additional.Radius - original.Radius)//additional contain original
                {
                    result = additional;
                    return;
                }
            }
            //else find center of new sphere and radius
            T leftRadius = T.Max(original.Radius - distance, additional.Radius);
            T rightRadius = T.Max(original.Radius + distance, additional.Radius);
            ocenterToaCenter = ocenterToaCenter + (((leftRadius - rightRadius) / (T.CreateChecked(2) * ocenterToaCenter.Length())) * ocenterToaCenter);//oCenterToResultCenter

            result = new BoundingSphere<T>();
            result.Center = original.Center + ocenterToaCenter;
            result.Radius = (leftRadius + rightRadius) / T.CreateChecked(2);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="BoundingSphere{T}"/>.
        /// </summary>
        /// <param name="other">The <see cref="BoundingSphere{T}"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(BoundingSphere<T> other)
        {
            return this.Center == other.Center && this.Radius == other.Radius;
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is BoundingSphere<T> sphere)
                return this.Equals(sphere);

            return false;
        }

        /// <summary>
        /// Gets the hash code of this <see cref="BoundingSphere{T}"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="BoundingSphere{T}"/>.</returns>
        public override int GetHashCode()
        {
            return this.Center.GetHashCode() + this.Radius.GetHashCode();
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingBox{T}"/> intersects with this sphere.
        /// </summary>
        /// <param name="box">The box for testing.</param>
        /// <returns><c>true</c> if <see cref="BoundingBox{T}"/> intersects with this sphere; <c>false</c> otherwise.</returns>
        public bool Intersects(BoundingBox<T> box)
        {
			return box.Intersects(this);
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingBox{T}"/> intersects with this sphere.
        /// </summary>
        /// <param name="box">The box for testing.</param>
        /// <param name="result"><c>true</c> if <see cref="BoundingBox{T}"/> intersects with this sphere; <c>false</c> otherwise. As an output parameter.</param>
        public void Intersects(ref BoundingBox<T> box, out bool result)
        {
            box.Intersects(ref this, out result);
        }

        /*
        TODO : Make the public bool Intersects(BoundingFrustum frustum) overload

        public bool Intersects(BoundingFrustum frustum)
        {
            if (frustum == null)
                throw new NullReferenceException();

            throw new NotImplementedException();
        }

        */

        /// <summary>
        /// Gets whether or not the other <see cref="BoundingSphere{T}"/> intersects with this sphere.
        /// </summary>
        /// <param name="sphere">The other sphere for testing.</param>
        /// <returns><c>true</c> if other <see cref="BoundingSphere{T}"/> intersects with this sphere; <c>false</c> otherwise.</returns>
        public bool Intersects(BoundingSphere<T> sphere)
        {
            bool result;
            Intersects(ref sphere, out result);
            return result;
        }

        /// <summary>
        /// Gets whether or not the other <see cref="BoundingSphere{T}"/> intersects with this sphere.
        /// </summary>
        /// <param name="sphere">The other sphere for testing.</param>
        /// <param name="result"><c>true</c> if other <see cref="BoundingSphere{T}"/> intersects with this sphere; <c>false</c> otherwise. As an output parameter.</param>
        public void Intersects(ref BoundingSphere<T> sphere, out bool result)
        {
            T sqDistance;
            Vec3<T>.DistanceSquared(ref sphere.Center, ref Center, out sqDistance);

            if (sqDistance > (sphere.Radius + Radius) * (sphere.Radius + Radius))
                result = false;
            else
                result = true;
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="Plane"/> intersects with this sphere.
        /// </summary>
        /// <param name="plane">The plane for testing.</param>
        /// <returns>Type of intersection.</returns>
        public PlaneIntersectionType Intersects(Plane<T> plane)
        {
            var result = default(PlaneIntersectionType);
            // TODO: we might want to inline this for performance reasons
            this.Intersects(ref plane, out result);
            return result;
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="Plane"/> intersects with this sphere.
        /// </summary>
        /// <param name="plane">The plane for testing.</param>
        /// <param name="result">Type of intersection as an output parameter.</param>
        public void Intersects(ref Plane<T> plane, out PlaneIntersectionType result)
        {
            T distance = T.Zero;
            // TODO: we might want to inline this for performance reasons
            Vec3<T>.Dot(ref plane.Normal, ref this.Center, out distance);
            distance += plane.D;
            if (distance > this.Radius)
                result = PlaneIntersectionType.Front;
            else if (distance < -this.Radius)
                result = PlaneIntersectionType.Back;
            else
                result = PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="Ray{T}"/> intersects with this sphere.
        /// </summary>
        /// <param name="ray">The ray for testing.</param>
        /// <returns>Distance of ray intersection or <c>null</c> if there is no intersection.</returns>
        public T? Intersects(Ray<T> ray)
        {
            return ray.Intersects(this);
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="Ray{T}"/> intersects with this sphere.
        /// </summary>
        /// <param name="ray">The ray for testing.</param>
        /// <param name="result">Distance of ray intersection or <c>null</c> if there is no intersection as an output parameter.</param>
        public void Intersects(ref Ray<T> ray, out T? result)
        {
            ray.Intersects(ref this, out result);
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="BoundingSphere{T}"/> in the format:
        /// {Center:[<see cref="Center"/>] Radius:[<see cref="Radius"/>]}
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="BoundingSphere{T}"/>.</returns>
        public override string ToString()
        {
            return "{Center:" + this.Center + " Radius:" + this.Radius + "}";
        }

        /// <summary>
        /// Creates a new <see cref="BoundingSphere{T}"/> that contains a transformation of translation and scale from this sphere by the specified <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
        /// <returns>Transformed <see cref="BoundingSphere{T}"/>.</returns>
        public BoundingSphere<T> Transform(Matrix<T> matrix)
        {
            BoundingSphere<T> sphere = new BoundingSphere<T>();
            sphere.Center = Vec3<T>.Transform(this.Center, matrix);
            sphere.Radius = this.Radius * T.Sqrt(T.Max(((matrix.M11 * matrix.M11) + (matrix.M12 * matrix.M12)) + (matrix.M13 * matrix.M13), T.Max(((matrix.M21 * matrix.M21) + (matrix.M22 * matrix.M22)) + (matrix.M23 * matrix.M23), ((matrix.M31 * matrix.M31) + (matrix.M32 * matrix.M32)) + (matrix.M33 * matrix.M33))));
            return sphere;
        }

        /// <summary>
        /// Creates a new <see cref="BoundingSphere{T}"/> that contains a transformation of translation and scale from this sphere by the specified <see cref="Matrix{T}"/>.
        /// </summary>
        /// <param name="matrix">The transformation <see cref="Matrix{T}"/>.</param>
        /// <param name="result">Transformed <see cref="BoundingSphere{T}"/> as an output parameter.</param>
        public void Transform(ref Matrix<T> matrix, out BoundingSphere<T> result)
        {
            result.Center = Vec3<T>.Transform(this.Center, matrix);
            result.Radius = this.Radius * T.Sqrt(T.Max(((matrix.M11 * matrix.M11) + (matrix.M12 * matrix.M12)) + (matrix.M13 * matrix.M13), T.Max(((matrix.M21 * matrix.M21) + (matrix.M22 * matrix.M22)) + (matrix.M23 * matrix.M23), ((matrix.M31 * matrix.M31) + (matrix.M32 * matrix.M32)) + (matrix.M33 * matrix.M33))));
        }

        /// <summary>
        /// Deconstruction method for <see cref="BoundingSphere{T}"/>.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public void Deconstruct(out Vec3<T> center, out T radius)
        {
            center = Center;
            radius = Radius;
        }

        /// <summary>
        /// Compares whether two <see cref="BoundingSphere{T}"/> instances are equal.
        /// </summary>
        /// <param name="a"><see cref="BoundingSphere{T}"/> instance on the left of the equal sign.</param>
        /// <param name="b"><see cref="BoundingSphere{T}"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator == (BoundingSphere<T> a, BoundingSphere<T> b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Compares whether two <see cref="BoundingSphere{T}"/> instances are not equal.
        /// </summary>
        /// <param name="a"><see cref="BoundingSphere{T}"/> instance on the left of the not equal sign.</param>
        /// <param name="b"><see cref="BoundingSphere{T}"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator != (BoundingSphere<T> a, BoundingSphere<T> b)
        {
            return !a.Equals(b);
        }

    }
}
