using System.Diagnostics;
using System.Numerics;
using GameEngine.Core.Serialization;

namespace GameEngine.Numerics
{
    /// <summary>
    /// Represents an axis-aligned bounding box (AABB) in 3D space.
    /// </summary>
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct BoundingBox<T> : IEquatable<BoundingBox<T>> where T : struct, IFloatingPointIeee754<T>
    {
        /// <summary>
        ///   The minimum extent of this <see cref="BoundingBox{T}"/>.
        /// </summary>
        [Serialized] public Vec3<T> Min;
      
        /// <summary>
        ///   The maximum extent of this <see cref="BoundingBox{T}"/>.
        /// </summary>
        [Serialized] public Vec3<T> Max;

        /// <summary>
        ///   The number of corners in a <see cref="BoundingBox{T}"/>. This is equal to 8.
        /// </summary>
        public const int CORNER_COUNT = 8;

        /// <summary>
        ///   Create a <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="min">The minimum extent of the <see cref="BoundingBox{T}"/>.</param>
        /// <param name="max">The maximum extent of the <see cref="BoundingBox{T}"/>.</param>
        public BoundingBox(Vec3<T> min, Vec3<T> max)
        {
            this.Min = min;
            this.Max = max;
        }
        
        public static Vec3<T> MaxVector3 => new Vec3<T>(T.PositiveInfinity);
        public static Vec3<T> MinVector3 => new Vec3<T>(T.NegativeInfinity);

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> contains another <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox{T}"/> to test for overlap.</param>
        /// <returns>
        ///   A value indicating if this <see cref="BoundingBox{T}"/> contains,
        ///   intersects with or is disjoint with <paramref name="box"/>.
        /// </returns>
        public ContainmentType Contains(BoundingBox<T> box)
        {
            //test if all corner is in the same side of a face by just checking min and max
            if (box.Max.X < Min.X
                || box.Min.X > Max.X
                || box.Max.Y < Min.Y
                || box.Min.Y > Max.Y
                || box.Max.Z < Min.Z
                || box.Min.Z > Max.Z)
                return ContainmentType.Disjoint;


            if (box.Min.X >= Min.X
                && box.Max.X <= Max.X
                && box.Min.Y >= Min.Y
                && box.Max.Y <= Max.Y
                && box.Min.Z >= Min.Z
                && box.Max.Z <= Max.Z)
                return ContainmentType.Contains;

            return ContainmentType.Intersects;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> contains another <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox{T}"/> to test for overlap.</param>
        /// <param name="result">
        ///   A value indicating if this <see cref="BoundingBox{T}"/> contains,
        ///   intersects with or is disjoint with <paramref name="box"/>.
        /// </param>
        public void Contains(ref BoundingBox<T> box, out ContainmentType result)
        {
            result = Contains(box);
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> contains a <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="frustum">The <see cref="BoundingFrustum{T}"/> to test for overlap.</param>
        /// <returns>
        ///   A value indicating if this <see cref="BoundingBox{T}"/> contains,
        ///   intersects with or is disjoint with <paramref name="frustum"/>.
        /// </returns>
        public ContainmentType Contains(BoundingFrustum<T> frustum)
        {
            //TODO: bad done here need a fix. 
            //Because question is not frustum contain box but reverse and this is not the same
            int i;
            ContainmentType contained;
            Vec3<T>[] corners = frustum.GetCorners();

            // First we check if frustum is in box
            for (i = 0; i < corners.Length; i++)
            {
                this.Contains(ref corners[i], out contained);
                if (contained == ContainmentType.Disjoint)
                    break;
            }

            if (i == corners.Length) // This means we checked all the corners and they were all contain or instersect
                return ContainmentType.Contains;

            if (i != 0)             // if i is not equal to zero, we can fastpath and say that this box intersects
                return ContainmentType.Intersects;


            // If we get here, it means the first (and only) point we checked was actually contained in the frustum.
            // So we assume that all other points will also be contained. If one of the points is disjoint, we can
            // exit immediately saying that the result is Intersects
            i++;
            for (; i < corners.Length; i++)
            {
                this.Contains(ref corners[i], out contained);
                if (contained != ContainmentType.Contains)
                    return ContainmentType.Intersects;

            }

            // If we get here, then we know all the points were actually contained, therefore result is Contains
            return ContainmentType.Contains;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> contains a <see cref="BoundingSphere{T}"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingSphere{T}"/> to test for overlap.</param>
        /// <returns>
        ///   A value indicating if this <see cref="BoundingBox{T}"/> contains,
        ///   intersects with or is disjoint with <paramref name="sphere"/>.
        /// </returns>
        public ContainmentType Contains(BoundingSphere<T> sphere)
        {
            if (sphere.Center.X - Min.X >= sphere.Radius
                && sphere.Center.Y - Min.Y >= sphere.Radius
                && sphere.Center.Z - Min.Z >= sphere.Radius
                && Max.X - sphere.Center.X >= sphere.Radius
                && Max.Y - sphere.Center.Y >= sphere.Radius
                && Max.Z - sphere.Center.Z >= sphere.Radius)
                return ContainmentType.Contains;
            
            
            T min = T.Zero;
            
            T e = sphere.Center.X - Min.X;
            if (e < T.Zero)
            {
                if (e < -sphere.Radius)
                {
                    return ContainmentType.Disjoint;
                }
                min += e * e;
            }
            else
            {
                e = sphere.Center.X - Max.X;
                if (e > T.Zero)
                {
                    if (e > sphere.Radius)
                    {
                        return ContainmentType.Disjoint;
                    }
                    min += e * e;
                }
            }

            e = sphere.Center.Y - Min.Y;
            if (e < T.Zero)
            {
                if (e < -sphere.Radius)
                {
                    return ContainmentType.Disjoint;
                }
                min += e * e;
            }
            else
            {
                e = sphere.Center.Y - Max.Y;
                if (e > T.Zero)
                {
                    if (e > sphere.Radius)
                    {
                        return ContainmentType.Disjoint;
                    }
                    min += e * e;
                }
            }

            e = sphere.Center.Z - Min.Z;
            if (e < T.Zero)
            {
                if (e < -sphere.Radius)
                {
                    return ContainmentType.Disjoint;
                }
                min += e * e;
            }
            else
            {
                e = sphere.Center.Z - Max.Z;
                if (e > T.Zero)
                {
                    if (e > sphere.Radius)
                    {
                        return ContainmentType.Disjoint;
                    }
                    min += e * e;
                }
            }

            if (min <= sphere.Radius * sphere.Radius)
                return ContainmentType.Intersects;

            return ContainmentType.Disjoint;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> contains a <see cref="BoundingSphere{T}"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingSphere{T}"/> to test for overlap.</param>
        /// <param name="result">
        ///   A value indicating if this <see cref="BoundingBox{T}"/> contains,
        ///   intersects with or is disjoint with <paramref name="sphere"/>.
        /// </param>
        public void Contains(ref BoundingSphere<T> sphere, out ContainmentType result)
        {
            result = this.Contains(sphere);
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> contains a point.
        /// </summary>
        /// <param name="point">The <see cref="Vec3{T}"/> to test.</param>
        /// <returns>
        ///   <see cref="ContainmentType.Contains"/> if this <see cref="BoundingBox{T}"/> contains
        ///   <paramref name="point"/> or <see cref="ContainmentType.Disjoint"/> if it does not.
        /// </returns>
        public ContainmentType Contains(Vec3<T> point)
        {
            ContainmentType result;
            this.Contains(ref point, out result);
            return result;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> contains a point.
        /// </summary>
        /// <param name="point">The <see cref="Vec3{T}"/> to test.</param>
        /// <param name="result">
        ///   <see cref="ContainmentType.Contains"/> if this <see cref="BoundingBox{T}"/> contains
        ///   <paramref name="point"/> or <see cref="ContainmentType.Disjoint"/> if it does not.
        /// </param>
        public void Contains(ref Vec3<T> point, out ContainmentType result)
        {
            //first we get if point is out of box
            if (point.X < this.Min.X
                || point.X > this.Max.X
                || point.Y < this.Min.Y
                || point.Y > this.Max.Y
                || point.Z < this.Min.Z
                || point.Z > this.Max.Z)
            {
                result = ContainmentType.Disjoint;
            }
            else
            {
                result = ContainmentType.Contains;
            }
        }
        
        /// <summary>
        /// Create a bounding box from the given list of points.
        /// </summary>
        /// <param name="points">The array of Vector3 instances defining the point cloud to bound</param>
        /// <param name="index">The base index to start iterating from</param>
        /// <param name="count">The number of points to iterate</param>
        /// <returns>A bounding box that encapsulates the given point cloud.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the given array is null or has no points.</exception>
        public static BoundingBox<T> CreateFromPoints(Vec3<T>[] points, int index = 0, int count = -1)
        {
            if (points == null || points.Length == 0)
                throw new ArgumentException();

            if (count == -1)
                count = points.Length;

            var minVec = MaxVector3;
            var maxVec = MinVector3;
            for (int i = index; i < count; i++)
            {                
                minVec.X = (minVec.X < points[i].X) ? minVec.X : points[i].X;
                minVec.Y = (minVec.Y < points[i].Y) ? minVec.Y : points[i].Y;
                minVec.Z = (minVec.Z < points[i].Z) ? minVec.Z : points[i].Z;

                maxVec.X = (maxVec.X > points[i].X) ? maxVec.X : points[i].X;
                maxVec.Y = (maxVec.Y > points[i].Y) ? maxVec.Y : points[i].Y;
                maxVec.Z = (maxVec.Z > points[i].Z) ? maxVec.Z : points[i].Z;
            }

            return new BoundingBox<T>(minVec, maxVec);
        }


        /// <summary>
        /// Create a bounding box from the given list of points.
        /// </summary>
        /// <param name="points">The list of Vector3 instances defining the point cloud to bound</param>
        /// <param name="index">The base index to start iterating from</param>
        /// <param name="count">The number of points to iterate</param>
        /// <returns>A bounding box that encapsulates the given point cloud.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the given list is null or has no points.</exception>
        public static BoundingBox<T> CreateFromPoints(List<Vec3<T>> points, int index = 0, int count = -1)
        {
            if (points == null || points.Count == 0)
                throw new ArgumentException();

            if (count == -1)
                count = points.Count;

            var minVec = MaxVector3;
            var maxVec = MinVector3;
            for (int i = index; i < count; i++)
            {
                minVec.X = (minVec.X < points[i].X) ? minVec.X : points[i].X;
                minVec.Y = (minVec.Y < points[i].Y) ? minVec.Y : points[i].Y;
                minVec.Z = (minVec.Z < points[i].Z) ? minVec.Z : points[i].Z;

                maxVec.X = (maxVec.X > points[i].X) ? maxVec.X : points[i].X;
                maxVec.Y = (maxVec.Y > points[i].Y) ? maxVec.Y : points[i].Y;
                maxVec.Z = (maxVec.Z > points[i].Z) ? maxVec.Z : points[i].Z;
            }

            return new BoundingBox<T>(minVec, maxVec);
        }


        /// <summary>
        ///   Create the enclosing <see cref="BoundingBox{T}"/> from the given list of points.
        /// </summary>
        /// <param name="points">The list of <see cref="Vec3{T}"/> instances defining the point cloud to bound.</param>
        /// <returns>A <see cref="BoundingBox{T}"/> that encloses the given point cloud.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the given list has no points.</exception>
        public static BoundingBox<T> CreateFromPoints(IEnumerable<Vec3<T>> points)
        {
            if (points == null)
                throw new ArgumentNullException();

            var empty = true;
            var minVec = MaxVector3;
            var maxVec = MinVector3;
            foreach (var ptVector in points)
            {
                minVec.X = (minVec.X < ptVector.X) ? minVec.X : ptVector.X;
                minVec.Y = (minVec.Y < ptVector.Y) ? minVec.Y : ptVector.Y;
                minVec.Z = (minVec.Z < ptVector.Z) ? minVec.Z : ptVector.Z;

                maxVec.X = (maxVec.X > ptVector.X) ? maxVec.X : ptVector.X;
                maxVec.Y = (maxVec.Y > ptVector.Y) ? maxVec.Y : ptVector.Y;
                maxVec.Z = (maxVec.Z > ptVector.Z) ? maxVec.Z : ptVector.Z;

                empty = false;
            }
            if (empty)
                throw new ArgumentException();

            return new BoundingBox<T>(minVec, maxVec);
        }

        /// <summary>
        ///   Create the enclosing <see cref="BoundingBox{T}"/> of a <see cref="BoundingSphere{T}"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingSphere{T}"/> to enclose.</param>
        /// <returns>A <see cref="BoundingBox{T}"/> enclosing <paramref name="sphere"/>.</returns>
        public static BoundingBox<T> CreateFromSphere(BoundingSphere<T> sphere)
        {
            BoundingBox<T> result;
            CreateFromSphere(ref sphere, out result);
            return result;
        }

        /// <summary>
        ///   Create the enclosing <see cref="BoundingBox{T}"/> of a <see cref="BoundingSphere{T}"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingSphere{T}"/> to enclose.</param>
        /// <param name="result">A <see cref="BoundingBox{T}"/> enclosing <paramref name="sphere"/>.</param>
        public static void CreateFromSphere(ref BoundingSphere<T> sphere, out BoundingBox<T> result)
        {
            var corner = new Vec3<T>(sphere.Radius);
            result.Min = sphere.Center - corner;
            result.Max = sphere.Center + corner;
        }

        /// <summary>
        ///   Create the <see cref="BoundingBox{T}"/> enclosing two other <see cref="BoundingBox{T}"/> instances.
        /// </summary>
        /// <param name="original">A <see cref="BoundingBox{T}"/> to enclose.</param>
        /// <param name="additional">A <see cref="BoundingBox{T}"/> to enclose.</param>
        /// <returns>
        ///   The <see cref="BoundingBox{T}"/> enclosing <paramref name="original"/> and <paramref name="additional"/>.
        /// </returns>
        public static BoundingBox<T> CreateMerged(BoundingBox<T> original, BoundingBox<T> additional)
        {
            BoundingBox<T> result;
            CreateMerged(ref original, ref additional, out result);
            return result;
        }

        /// <summary>
        ///   Create the <see cref="BoundingBox{T}"/> enclosing two other <see cref="BoundingBox{T}"/> instances.
        /// </summary>
        /// <param name="original">A <see cref="BoundingBox{T}"/> to enclose.</param>
        /// <param name="additional">A <see cref="BoundingBox{T}"/> to enclose.</param>
        /// <param name="result">
        ///   The <see cref="BoundingBox{T}"/> enclosing <paramref name="original"/> and <paramref name="additional"/>.
        /// </param>
        public static void CreateMerged(ref BoundingBox<T> original, ref BoundingBox<T> additional, out BoundingBox<T> result)
        {
            result.Min.X = T.Min(original.Min.X, additional.Min.X);
            result.Min.Y = T.Min(original.Min.Y, additional.Min.Y);
            result.Min.Z = T.Min(original.Min.Z, additional.Min.Z);
            result.Max.X = T.Max(original.Max.X, additional.Max.X);
            result.Max.Y = T.Max(original.Max.Y, additional.Max.Y);
            result.Max.Z = T.Max(original.Max.Z, additional.Max.Z);
        }

        /// <summary>
        ///   Check if two <see cref="BoundingBox{T}"/> instances are equal.
        /// </summary>
        /// <param name="other">The <see cref="BoundingBox{T}"/> to compare with this <see cref="BoundingBox{T}"/>.</param>
        /// <returns>
        ///   <code>true</code> if <see cref="other"/> is equal to this <see cref="BoundingBox{T}"/>,
        ///   <code>false</code> if it is not.
        /// </returns>
        public bool Equals(BoundingBox<T> other)
        {
            return (this.Min == other.Min) && (this.Max == other.Max);
        }

        /// <summary>
        ///   Check if two <see cref="BoundingBox{T}"/> instances are equal.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with this <see cref="BoundingBox{T}"/>.</param>
        /// <returns>
        ///   <code>true</code> if <see cref="obj"/> is equal to this <see cref="BoundingBox{T}"/>,
        ///   <code>false</code> if it is not.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return (obj is BoundingBox<T> box) && this.Equals(box);
        }

        /// <summary>
        ///   Get an array of <see cref="Vec3{T}"/> containing the corners of this <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <returns>An array of <see cref="Vec3{T}"/> containing the corners of this <see cref="BoundingBox{T}"/>.</returns>
        public Vec3<T>[] GetCorners()
        {
            return new Vec3<T>[] {
                new Vec3<T>(this.Min.X, this.Max.Y, this.Max.Z), 
                new Vec3<T>(this.Max.X, this.Max.Y, this.Max.Z),
                new Vec3<T>(this.Max.X, this.Min.Y, this.Max.Z), 
                new Vec3<T>(this.Min.X, this.Min.Y, this.Max.Z), 
                new Vec3<T>(this.Min.X, this.Max.Y, this.Min.Z),
                new Vec3<T>(this.Max.X, this.Max.Y, this.Min.Z),
                new Vec3<T>(this.Max.X, this.Min.Y, this.Min.Z),
                new Vec3<T>(this.Min.X, this.Min.Y, this.Min.Z)
            };
        }

        /// <summary>
        ///   Fill the first 8 places of an array of <see cref="Vec3{T}"/>
        ///   with the corners of this <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="corners">The array to fill.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="corners"/> is <code>null</code>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   If <paramref name="corners"/> has a length of less than 8.
        /// </exception>
        public void GetCorners(Vec3<T>[] corners)
        {
            if (corners == null)
            {
                throw new ArgumentNullException(nameof(corners));
            }
            if (corners.Length < 8)
            {
                throw new ArgumentOutOfRangeException(nameof(corners), "Not Enought Corners");
            }
            corners[0].X = this.Min.X;
            corners[0].Y = this.Max.Y;
            corners[0].Z = this.Max.Z;
            corners[1].X = this.Max.X;
            corners[1].Y = this.Max.Y;
            corners[1].Z = this.Max.Z;
            corners[2].X = this.Max.X;
            corners[2].Y = this.Min.Y;
            corners[2].Z = this.Max.Z;
            corners[3].X = this.Min.X;
            corners[3].Y = this.Min.Y;
            corners[3].Z = this.Max.Z;
            corners[4].X = this.Min.X;
            corners[4].Y = this.Max.Y;
            corners[4].Z = this.Min.Z;
            corners[5].X = this.Max.X;
            corners[5].Y = this.Max.Y;
            corners[5].Z = this.Min.Z;
            corners[6].X = this.Max.X;
            corners[6].Y = this.Min.Y;
            corners[6].Z = this.Min.Z;
            corners[7].X = this.Min.X;
            corners[7].Y = this.Min.Y;
            corners[7].Z = this.Min.Z;
        }

        /// <summary>
        ///   Get the hash code for this <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <returns>A hash code for this <see cref="BoundingBox{T}"/>.</returns>
        public override int GetHashCode()
        {
            return this.Min.GetHashCode() + this.Max.GetHashCode();
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> intersects another <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox{T}"/> to test for intersection.</param>
        /// <returns>
        ///   <code>true</code> if this <see cref="BoundingBox{T}"/> intersects <paramref name="box"/>,
        ///   <code>false</code> if it does not.
        /// </returns>
        public bool Intersects(BoundingBox<T> box)
        {
            bool result;
            Intersects(ref box, out result);
            return result;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> intersects another <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox{T}"/> to test for intersection.</param>
        /// <param name="result">
        ///   <code>true</code> if this <see cref="BoundingBox{T}"/> intersects <paramref name="box"/>,
        ///   <code>false</code> if it does not.
        /// </param>
        public void Intersects(ref BoundingBox<T> box, out bool result)
        {
            if ((this.Max.X >= box.Min.X) && (this.Min.X <= box.Max.X))
            {
                if ((this.Max.Y < box.Min.Y) || (this.Min.Y > box.Max.Y))
                {
                    result = false;
                    return;
                }

                result = (this.Max.Z >= box.Min.Z) && (this.Min.Z <= box.Max.Z);
                return;
            }

            result = false;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> intersects a <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="frustum">The <see cref="BoundingFrustum{T}"/> to test for intersection.</param>
        /// <returns>
        ///   <code>true</code> if this <see cref="BoundingBox{T}"/> intersects <paramref name="frustum"/>,
        ///   <code>false</code> if it does not.
        /// </returns>
        public bool Intersects(BoundingFrustum<T> frustum)
        {
            return frustum.Intersects(this);
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> intersects a <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingFrustum{T}"/> to test for intersection.</param>
        /// <returns>
        ///   <code>true</code> if this <see cref="BoundingBox{T}"/> intersects <paramref name="sphere"/>,
        ///   <code>false</code> if it does not.
        /// </returns>
        public bool Intersects(BoundingSphere<T> sphere)
        {
            bool result;
            Intersects(ref sphere, out result);
            return result;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> intersects a <see cref="BoundingFrustum{T}"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingFrustum{T}"/> to test for intersection.</param>
        /// <param name="result">
        ///   <code>true</code> if this <see cref="BoundingBox{T}"/> intersects <paramref name="sphere"/>,
        ///   <code>false</code> if it does not.
        /// </param>
        public void Intersects(ref BoundingSphere<T> sphere, out bool result)
        {
            T squareDistance = T.Zero;
            Vec3<T> point = sphere.Center;
            if (point.X < Min.X) squareDistance += (Min.X - point.X) * (Min.X - point.X);
            if (point.X > Max.X) squareDistance += (point.X - Max.X) * (point.X - Max.X);
            if (point.Y < Min.Y) squareDistance += (Min.Y - point.Y) * (Min.Y - point.Y);
            if (point.Y > Max.Y) squareDistance += (point.Y - Max.Y) * (point.Y - Max.Y);
            if (point.Z < Min.Z) squareDistance += (Min.Z - point.Z) * (Min.Z - point.Z);
            if (point.Z > Max.Z) squareDistance += (point.Z - Max.Z) * (point.Z - Max.Z);
            result = squareDistance <= sphere.Radius * sphere.Radius;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> intersects a <see cref="Plane"/>.
        /// </summary>
        /// <param name="plane">The <see cref="Plane"/> to test for intersection.</param>
        /// <returns>
        ///   <code>true</code> if this <see cref="BoundingBox{T}"/> intersects <paramref name="plane"/>,
        ///   <code>false</code> if it does not.
        /// </returns>
        public PlaneIntersectionType Intersects(Plane<T> plane)
        {
            PlaneIntersectionType result;
            Intersects(ref plane, out result);
            return result;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> intersects a <see cref="Plane"/>.
        /// </summary>
        /// <param name="plane">The <see cref="Plane"/> to test for intersection.</param>
        /// <param name="result">
        ///   <code>true</code> if this <see cref="BoundingBox{T}"/> intersects <paramref name="plane"/>,
        ///   <code>false</code> if it does not.
        /// </param>
        public void Intersects(ref Plane<T> plane, out PlaneIntersectionType result)
        {
            // See http://zach.in.tu-clausthal.de/teaching/cg_literatur/lighthouse3d_view_frustum_culling/index.html

            Vec3<T> positiveVertex;
            Vec3<T> negativeVertex;

            if (plane.Normal.X >= T.Zero)
            {
                positiveVertex.X = Max.X;
                negativeVertex.X = Min.X;
            }
            else
            {
                positiveVertex.X = Min.X;
                negativeVertex.X = Max.X;
            }

            if (plane.Normal.Y >= T.Zero)
            {
                positiveVertex.Y = Max.Y;
                negativeVertex.Y = Min.Y;
            }
            else
            {
                positiveVertex.Y = Min.Y;
                negativeVertex.Y = Max.Y;
            }

            if (plane.Normal.Z >= T.Zero)
            {
                positiveVertex.Z = Max.Z;
                negativeVertex.Z = Min.Z;
            }
            else
            {
                positiveVertex.Z = Min.Z;
                negativeVertex.Z = Max.Z;
            }

            // Inline Vector3.Dot(plane.Normal, negativeVertex) + plane.D;
            var distance = plane.Normal.X * negativeVertex.X + plane.Normal.Y * negativeVertex.Y + plane.Normal.Z * negativeVertex.Z + plane.D;
            if (distance > T.Zero)
            {
                result = PlaneIntersectionType.Front;
                return;
            }

            // Inline Vector3.Dot(plane.Normal, positiveVertex) + plane.D;
            distance = plane.Normal.X * positiveVertex.X + plane.Normal.Y * positiveVertex.Y + plane.Normal.Z * positiveVertex.Z + plane.D;
            if (distance < T.Zero)
            {
                result = PlaneIntersectionType.Back;
                return;
            }

            result = PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> intersects a <see cref="Ray{T}"/>.
        /// </summary>
        /// <param name="ray">The <see cref="Ray{T}"/> to test for intersection.</param>
        /// <returns>
        ///   The distance along the <see cref="Ray{T}"/> to the intersection point or
        ///   <code>null</code> if the <see cref="Ray{T}"/> does not intesect this <see cref="BoundingBox{T}"/>.
        /// </returns>
        public T? Intersects(Ray<T> ray)
        {
            return ray.Intersects(this);
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox{T}"/> intersects a <see cref="Ray{T}"/>.
        /// </summary>
        /// <param name="ray">The <see cref="Ray{T}"/> to test for intersection.</param>
        /// <param name="result">
        ///   The distance along the <see cref="Ray{T}"/> to the intersection point or
        ///   <code>null</code> if the <see cref="Ray{T}"/> does not intesect this <see cref="BoundingBox{T}"/>.
        /// </param>
        public void Intersects(ref Ray<T> ray, out T? result)
        {
            result = Intersects(ray);
        }

        /// <summary>
        ///   Check if two <see cref="BoundingBox{T}"/> instances are equal.
        /// </summary>
        /// <param name="a">A <see cref="BoundingBox{T}"/> to compare the other.</param>
        /// <param name="b">A <see cref="BoundingBox{T}"/> to compare the other.</param>
        /// <returns>
        ///   <code>true</code> if <see cref="a"/> is equal to this <see cref="b"/>,
        ///   <code>false</code> if it is not.
        /// </returns>
        public static bool operator ==(BoundingBox<T> a, BoundingBox<T> b)
        {
            return a.Equals(b);
        }

        /// <summary>
        ///   Check if two <see cref="BoundingBox{T}"/> instances are not equal.
        /// </summary>
        /// <param name="a">A <see cref="BoundingBox{T}"/> to compare the other.</param>
        /// <param name="b">A <see cref="BoundingBox{T}"/> to compare the other.</param>
        /// <returns>
        ///   <code>true</code> if <see cref="a"/> is not equal to this <see cref="b"/>,
        ///   <code>false</code> if it is.
        /// </returns>
        public static bool operator !=(BoundingBox<T> a, BoundingBox<T> b)
        {
            return !a.Equals(b);
        }

        internal string DebugDisplayString
        {
            get {
                return string.Empty;
//                return string.Concat(
//                    "Min( ", this.Min.DebugDisplayString, " )  \r\n",
//                    "Max( ",this.Max.DebugDisplayString, " )"
//                    );
            }
        }

        /// <summary>
        /// Get a <see cref="String"/> representation of this <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="BoundingBox{T}"/>.</returns>
        public override string ToString()
        {
            return "{{Min:" + this.Min.ToString() + " Max:" + this.Max.ToString() + "}}";
        }

        /// <summary>
        /// Deconstruction method for <see cref="BoundingBox{T}"/>.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void Deconstruct(out Vec3<T> min, out Vec3<T> max)
        {
            min = Min;
            max = Max;
        }

    }
}
