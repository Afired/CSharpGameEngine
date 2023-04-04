using System.Diagnostics;
using System.Numerics;
using GameEngine.Core.Serialization;

namespace GameEngine.Numerics
{
    /// <summary>
    /// Represents a ray with an origin and a direction in 3D space.
    /// </summary>
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct Ray<T> : IEquatable<Ray<T>> where T : struct, IFloatingPointIeee754<T>
    {
        /// <summary>
        /// The direction of this <see cref="Ray{T}"/>.
        /// </summary>
        [Serialized] public Vec3<T> Direction;
      
        /// <summary>
        /// The origin of this <see cref="Ray{T}"/>.
        /// </summary>
        [Serialized] public Vec3<T> Position;

        /// <summary>
        /// Create a <see cref="Ray{T}"/>.
        /// </summary>
        /// <param name="position">The origin of the <see cref="Ray{T}"/>.</param>
        /// <param name="direction">The direction of the <see cref="Ray{T}"/>.</param>
        public Ray(Vec3<T> position, Vec3<T> direction)
        {
            this.Position = position;
            this.Direction = direction;
        }
        
        /// <summary>
        /// Check if the specified <see cref="Object"/> is equal to this <see cref="Ray{T}"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to test for equality with this <see cref="Ray{T}"/>.</param>
        /// <returns>
        /// <code>true</code> if the specified <see cref="Object"/> is equal to this <see cref="Ray{T}"/>,
        /// <code>false</code> if it is not.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return (obj is Ray<T> ray) && this.Equals(ray);
        }

        /// <summary>
        /// Check if the specified <see cref="Ray{T}"/> is equal to this <see cref="Ray{T}"/>.
        /// </summary>
        /// <param name="other">The <see cref="Ray{T}"/> to test for equality with this <see cref="Ray{T}"/>.</param>
        /// <returns>
        /// <code>true</code> if the specified <see cref="Ray{T}"/> is equal to this <see cref="Ray{T}"/>,
        /// <code>false</code> if it is not.
        /// </returns>
        public bool Equals(Ray<T> other)
        {
            return this.Position.Equals(other.Position) && this.Direction.Equals(other.Direction);
        }

        /// <summary>
        /// Get a hash code for this <see cref="Ray{T}"/>.
        /// </summary>
        /// <returns>A hash code for this <see cref="Ray{T}"/>.</returns>
        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ Direction.GetHashCode();
        }

        // adapted from http://www.scratchapixel.com/lessons/3d-basic-lessons/lesson-7-intersecting-simple-shapes/ray-box-intersection/
        /// <summary>
        /// Check if this <see cref="Ray{T}"/> intersects the specified <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox{T}"/> to test for intersection.</param>
        /// <returns>
        /// The distance along the ray of the intersection or <code>null</code> if this
        /// <see cref="Ray{T}"/> does not intersect the <see cref="BoundingBox{T}"/>.
        /// </returns>
        public T? Intersects(BoundingBox<T> box)
        {
            const float EPSILON = 1e-6f;

            T? tMin = null;
            T? tMax = null;

            if (T.Abs(Direction.X) < T.Epsilon)
            {
                if (Position.X < box.Min.X || Position.X > box.Max.X)
                    return null;
            }
            else
            {
                tMin = (box.Min.X - Position.X) / Direction.X;
                tMax = (box.Max.X - Position.X) / Direction.X;

                if (tMin > tMax)
                {
                    var temp = tMin;
                    tMin = tMax;
                    tMax = temp;
                }
            }

            if (T.Abs(Direction.Y) < T.Epsilon)
            {
                if (Position.Y < box.Min.Y || Position.Y > box.Max.Y)
                    return null;
            }
            else
            {
                var tMinY = (box.Min.Y - Position.Y) / Direction.Y;
                var tMaxY = (box.Max.Y - Position.Y) / Direction.Y;

                if (tMinY > tMaxY)
                {
                    var temp = tMinY;
                    tMinY = tMaxY;
                    tMaxY = temp;
                }

                if ((tMin.HasValue && tMin > tMaxY) || (tMax.HasValue && tMinY > tMax))
                    return null;

                if (!tMin.HasValue || tMinY > tMin) tMin = tMinY;
                if (!tMax.HasValue || tMaxY < tMax) tMax = tMaxY;
            }

            if (T.Abs(Direction.Z) < T.Epsilon)
            {
                if (Position.Z < box.Min.Z || Position.Z > box.Max.Z)
                    return null;
            }
            else
            {
                var tMinZ = (box.Min.Z - Position.Z) / Direction.Z;
                var tMaxZ = (box.Max.Z - Position.Z) / Direction.Z;

                if (tMinZ > tMaxZ)
                {
                    var temp = tMinZ;
                    tMinZ = tMaxZ;
                    tMaxZ = temp;
                }

                if ((tMin.HasValue && tMin > tMaxZ) || (tMax.HasValue && tMinZ > tMax))
                    return null;

                if (!tMin.HasValue || tMinZ > tMin) tMin = tMinZ;
                if (!tMax.HasValue || tMaxZ < tMax) tMax = tMaxZ;
            }

            // having a positive tMax and a negative tMin means the ray is inside the box
            // we expect the intesection distance to be 0 in that case
            if ((tMin.HasValue && tMin < T.Zero) && tMax > T.Zero) return T.Zero;

            // a negative tMin means that the intersection point is behind the ray's origin
            // we discard these as not hitting the AABB
            if (tMin < T.Zero) return null;

            return tMin;
        }

        /// <summary>
        /// Check if this <see cref="Ray{T}"/> intersects the specified <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox{T}"/> to test for intersection.</param>
        /// <param name="result">
        /// The distance along the ray of the intersection or <code>null</code> if this
        /// <see cref="Ray{T}"/> does not intersect the <see cref="BoundingBox{T}"/>.
        /// </param>
        public void Intersects(ref BoundingBox<T> box, out T? result)
        {
			result = Intersects(box);
        }

        /*
        public float? Intersects(BoundingFrustum frustum)
        {
            if (frustum == null)
			{
				throw new ArgumentNullException("frustum");
			}
			
			return frustum.Intersects(this);			
        }
        */

        /// <summary>
        /// Check if this <see cref="Ray{T}"/> intersects the specified <see cref="BoundingSphere{T}"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingBox{T}"/> to test for intersection.</param>
        /// <returns>
        /// The distance along the ray of the intersection or <code>null</code> if this
        /// <see cref="Ray{T}"/> does not intersect the <see cref="BoundingSphere{T}"/>.
        /// </returns>
        public T? Intersects(BoundingSphere<T> sphere)
        {
            T? result;
            Intersects(ref sphere, out result);
            return result;
        }

        /// <summary>
        /// Check if this <see cref="Ray{T}"/> intersects the specified <see cref="Plane"/>.
        /// </summary>
        /// <param name="plane">The <see cref="Plane"/> to test for intersection.</param>
        /// <returns>
        /// The distance along the ray of the intersection or <code>null</code> if this
        /// <see cref="Ray{T}"/> does not intersect the <see cref="Plane"/>.
        /// </returns>
        public T? Intersects(Plane<T> plane)
        {
            T? result;
            Intersects(ref plane, out result);
            return result;
        }

        /// <summary>
        /// Check if this <see cref="Ray{T}"/> intersects the specified <see cref="Plane"/>.
        /// </summary>
        /// <param name="plane">The <see cref="Plane"/> to test for intersection.</param>
        /// <param name="result">
        /// The distance along the ray of the intersection or <code>null</code> if this
        /// <see cref="Ray{T}"/> does not intersect the <see cref="Plane"/>.
        /// </param>
        public void Intersects(ref Plane<T> plane, out T? result)
        {
            var den = Vec3<T>.Dot(Direction, plane.Normal);
            if (T.Abs(den) < T.CreateChecked(0.00001f))
            {
                result = null;
                return;
            }

            result = (-plane.D - Vec3<T>.Dot(plane.Normal, Position)) / den;

            if (result < T.Zero)
            {
                if (result < -T.CreateChecked(0.00001f))
                {
                    result = null;
                    return;
                }

                result = T.Zero;
            }
        }

        /// <summary>
        /// Check if this <see cref="Ray{T}"/> intersects the specified <see cref="BoundingSphere{T}"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingBox{T}"/> to test for intersection.</param>
        /// <param name="result">
        /// The distance along the ray of the intersection or <code>null</code> if this
        /// <see cref="Ray{T}"/> does not intersect the <see cref="BoundingSphere{T}"/>.
        /// </param>
        public void Intersects(ref BoundingSphere<T> sphere, out T? result)
        {
            // Find the vector between where the ray starts the the sphere's centre
            Vec3<T> difference = sphere.Center - this.Position;

            T differenceLengthSquared = difference.LengthSquared();
            T sphereRadiusSquared = sphere.Radius * sphere.Radius;

            T distanceAlongRay;

            // If the distance between the ray start and the sphere's centre is less than
            // the radius of the sphere, it means we've intersected. N.B. checking the LengthSquared is faster.
            if (differenceLengthSquared < sphereRadiusSquared) {
                result = T.Zero;
                return;
            }
            
            Vec3<T>.Dot(ref this.Direction, ref difference, out distanceAlongRay);
            // If the ray is pointing away from the sphere then we don't ever intersect
            if(distanceAlongRay < T.Zero) {
                result = null;
                return;
            }

            // Next we kinda use Pythagoras to check if we are within the bounds of the sphere
            // if x = radius of sphere
            // if y = distance between ray position and sphere centre
            // if z = the distance we've travelled along the ray
            // if x^2 + z^2 - y^2 < 0, we do not intersect
            T dist = sphereRadiusSquared + distanceAlongRay * distanceAlongRay - differenceLengthSquared;

            result = (dist < T.Zero) ? null : distanceAlongRay - (T?)T.Sqrt(dist);
        }

        /// <summary>
        /// Check if two rays are not equal.
        /// </summary>
        /// <param name="a">A ray to check for inequality.</param>
        /// <param name="b">A ray to check for inequality.</param>
        /// <returns><code>true</code> if the two rays are not equal, <code>false</code> if they are.</returns>
        public static bool operator !=(Ray<T> a, Ray<T> b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Check if two rays are equal.
        /// </summary>
        /// <param name="a">A ray to check for equality.</param>
        /// <param name="b">A ray to check for equality.</param>
        /// <returns><code>true</code> if the two rays are equals, <code>false</code> if they are not.</returns>
        public static bool operator ==(Ray<T> a, Ray<T> b)
        {
            return a.Equals(b);
        }

        internal string DebugDisplayString
        {
            get {
                return string.Empty;
//                return string.Concat(
//                    "Pos( ", this.Position.DebugDisplayString, " )  \r\n",
//                    "Dir( ", this.Direction.DebugDisplayString, " )"
//                );
            }
        }

        /// <summary>
        /// Get a <see cref="String"/> representation of this <see cref="Ray{T}"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="Ray{T}"/>.</returns>
        public override string ToString()
        {
            return "{{Position:" + Position.ToString() + " Direction:" + Direction.ToString() + "}}";
        }

        /// <summary>
        /// Deconstruction method for <see cref="Ray{T}"/>.
        /// </summary>
        /// <param name="position">Receives the start position of the ray.</param>
        /// <param name="direction">Receives the direction of the ray.</param>
        public void Deconstruct(out Vec3<T> position, out Vec3<T> direction)
        {
            position = Position;
            direction = Direction;
        }

    }
}
