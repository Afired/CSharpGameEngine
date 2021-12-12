using System;
using System.Globalization;

namespace GameEngine.Numerics;

public struct Quaternion : IEquatable<Quaternion> {
    
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }

    /// <summary>
    /// Constructs a Quaternion from the given components.
    /// </summary>
    /// <param name="x">The X component of the Quaternion.</param>
    /// <param name="y">The Y component of the Quaternion.</param>
    /// <param name="z">The Z component of the Quaternion.</param>
    /// <param name="w">The W component of the Quaternion.</param>
    public Quaternion(float x, float y, float z, float w) {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// Constructs a Quaternion from the given vector and rotation parts.
    /// </summary>
    /// <param name="vectorPart">The vector part of the Quaternion.</param>
    /// <param name="scalarPart">The rotation part of the Quaternion.</param>
    public Quaternion(Vector3 vectorPart, float scalarPart) {
        X = vectorPart.X;
        Y = vectorPart.Y;
        Z = vectorPart.Z;
        W = scalarPart;
    }
    
    /// <summary>
    /// Returns a Quaternion representing no rotation. 
    /// </summary>
    public static Quaternion Identity => new Quaternion(0, 0, 0, 1);

    /// <summary>
    /// Returns whether the Quaternion is the identity Quaternion.
    /// </summary>
    public bool IsIdentity => X == 0.0f && Y == 0.0f && Z == 0.0f && W == 1.0f;

    /// <summary>
    /// Calculates the length of the Quaternion.
    /// </summary>
    /// <returns>The computed length of the Quaternion.</returns>
    public float Length() => (float) Math.Sqrt(Dot(this, this));

    /// <summary>
    /// Calculates the length squared of the Quaternion. This operation is cheaper than Length().
    /// </summary>
    /// <returns>The length squared of the Quaternion.</returns>
    public float LengthSquared() => Dot(this, this);

    public Quaternion Normalized {
        get {
            float invNorm = 1.0f / (float) Math.Sqrt(Dot(this, this));

            Quaternion result = Identity;
            result.X = X * invNorm;
            result.Y = Y * invNorm;
            result.Z = Z * invNorm;
            result.W = W * invNorm;
            return result;
        }
    }
    
    /// <summary>
    /// Divides each component of the Quaternion by the length of the Quaternion.
    /// </summary>
    /// <param name="quaternion">The source Quaternion.</param>
    /// <returns>The normalized Quaternion.</returns>
    public static Quaternion Normalize(Quaternion quaternion) {
        return quaternion.Normalized;
    }

    /// <summary>
    /// Creates the conjugate of a specified Quaternion.
    /// </summary>
    /// <param name="value">The Quaternion of which to return the conjugate.</param>
    /// <returns>A new Quaternion that is the conjugate of the specified one.</returns>
    public static Quaternion Conjugate(Quaternion value) {
        Quaternion result = Identity;
        result.X = -value.X;
        result.Y = -value.Y;
        result.Z = -value.Z;
        result.W = value.W;
        return result;
    }

    /// <summary>
    /// Returns the inverse of a Quaternion.
    /// </summary>
    /// <param name="value">The source Quaternion.</param>
    /// <returns>The inverted Quaternion.</returns>
    public static Quaternion Inverse(Quaternion value) {
        Quaternion result = Identity;
        float invNorm = 1.0f / Dot(value, value);
        result.X = -value.X * invNorm;
        result.Y = -value.Y * invNorm;
        result.Z = -value.Z * invNorm;
        result.W = value.W * invNorm;
        return result;
    }

    /// <summary>
    /// Creates a Quaternion from a vector and an angle to rotate about the vector.
    /// </summary>
    /// <param name="axis">The vector to rotate around.</param>
    /// <param name="angle">The angle, in radians, to rotate around the vector.</param>
    /// <returns>The created Quaternion.</returns>
    public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle) {
        Quaternion result = Identity;
        float halfAngle = angle * 0.5f;
        float s = (float) Math.Sin(halfAngle);
        float c = (float) Math.Cos(halfAngle);
        result.X = axis.X * s;
        result.Y = axis.Y * s;
        result.Z = axis.Z * s;
        result.W = c;
        return result;
    }

    /// <summary>
    /// Creates a new Quaternion from the given yaw, pitch, and roll, in radians.
    /// </summary>
    /// <param name="yaw">The yaw angle, in radians, around the Y-axis.</param>
    /// <param name="pitch">The pitch angle, in radians, around the X-axis.</param>
    /// <param name="roll">The roll angle, in radians, around the Z-axis.</param>
    /// <returns></returns>
    public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll) {
        //  Roll first, about axis the object is facing, then
        //  pitch upward, then yaw to face into the new heading
        float sr, cr, sp, cp, sy, cy;

        float halfRoll = roll * 0.5f;
        sr = (float) Math.Sin(halfRoll);
        cr = (float) Math.Cos(halfRoll);

        float halfPitch = pitch * 0.5f;
        sp = (float) Math.Sin(halfPitch);
        cp = (float) Math.Cos(halfPitch);

        float halfYaw = yaw * 0.5f;
        sy = (float) Math.Sin(halfYaw);
        cy = (float) Math.Cos(halfYaw);

        Quaternion result = Identity;

        result.X = cy * sp * cr + sy * cp * sr;
        result.Y = sy * cp * cr - cy * sp * sr;
        result.Z = cy * cp * sr - sy * sp * cr;
        result.W = cy * cp * cr + sy * sp * sr;

        return result;
    }

    /// <summary>
    /// Creates a Quaternion from the given rotation matrix.
    /// </summary>
    /// <param name="matrix">The rotation matrix.</param>
    /// <returns>The created Quaternion.</returns>
    public static Quaternion CreateFromRotationMatrix(Matrix4x4 matrix) {
        float trace = matrix.M11 + matrix.M22 + matrix.M33;

        Quaternion result = new Quaternion();

        if (trace > 0.0f) {
            float s = (float)Math.Sqrt(trace + 1.0f);
            result.W = s * 0.5f;
            s = 0.5f / s;
            result.X = (matrix.M23 - matrix.M32) * s;
            result.Y = (matrix.M31 - matrix.M13) * s;
            result.Z = (matrix.M12 - matrix.M21) * s;
        } else {
            if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33) {
                float s = (float)Math.Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33);
                float invS = 0.5f / s;
                result.X = 0.5f * s;
                result.Y = (matrix.M12 + matrix.M21) * invS;
                result.Z = (matrix.M13 + matrix.M31) * invS;
                result.W = (matrix.M23 - matrix.M32) * invS;
            } else if (matrix.M22 > matrix.M33) {
                float s = (float)Math.Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33);
                float invS = 0.5f / s;
                result.X = (matrix.M21 + matrix.M12) * invS;
                result.Y = 0.5f * s;
                result.Z = (matrix.M32 + matrix.M23) * invS;
                result.W = (matrix.M31 - matrix.M13) * invS;
            } else {
                float s = (float)Math.Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22);
                float invS = 0.5f / s;
                result.X = (matrix.M31 + matrix.M13) * invS;
                result.Y = (matrix.M32 + matrix.M23) * invS;
                result.Z = 0.5f * s;
                result.W = (matrix.M12 - matrix.M21) * invS;
            }
        }
        return result;
    }

    /// <summary>
    /// Calculates the dot product of two Quaternions.
    /// </summary>
    /// <param name="quaternion1">The first source Quaternion.</param>
    /// <param name="quaternion2">The second source Quaternion.</param>
    /// <returns>The dot product of the Quaternions.</returns>
    public static float Dot(Quaternion quaternion1, Quaternion quaternion2) {
        return quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;
    }

    /// <summary>
    /// Interpolates between two quaternions, using spherical linear interpolation.
    /// </summary>
    /// <param name="quaternion1">The first source Quaternion.</param>
    /// <param name="quaternion2">The second source Quaternion.</param>
    /// <param name="amount">The relative weight of the second source Quaternion in the interpolation.</param>
    /// <returns>The interpolated Quaternion.</returns>
    public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, float amount) {
        const float EPSILON = 1e-6f;

        float t = amount;

        float cosOmega = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y +
                         quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;

        bool flip = false;

        if (cosOmega < 0.0f) {
            flip = true;
            cosOmega = -cosOmega;
        }

        float s1, s2;

        if (cosOmega > (1.0f - EPSILON)) {
            // Too close, do straight linear interpolation.
            s1 = 1.0f - t;
            s2 = (flip) ? -t : t;
        } else {
            float omega = (float) Math.Acos(cosOmega);
            float invSinOmega = (float) (1 / Math.Sin(omega));

            s1 = (float) Math.Sin((1.0f - t) * omega) * invSinOmega;
            s2 = (flip)
                ? (float)-Math.Sin(t * omega) * invSinOmega
                : (float)Math.Sin(t * omega) * invSinOmega;
        }

        Quaternion result = Identity;
        result.X = s1 * quaternion1.X + s2 * quaternion2.X;
        result.Y = s1 * quaternion1.Y + s2 * quaternion2.Y;
        result.Z = s1 * quaternion1.Z + s2 * quaternion2.Z;
        result.W = s1 * quaternion1.W + s2 * quaternion2.W;
        return result;
    }

    /// <summary>
    ///  Linearly interpolates between two quaternions.
    /// </summary>
    /// <param name="quaternion1">The first source Quaternion.</param>
    /// <param name="quaternion2">The second source Quaternion.</param>
    /// <param name="amount">The relative weight of the second source Quaternion in the interpolation.</param>
    /// <returns>The interpolated Quaternion.</returns>
    public static Quaternion Lerp(Quaternion quaternion1, Quaternion quaternion2, float amount) {
        float t = amount;
        float t1 = 1.0f - t;

        Quaternion result = Identity;

        float dot = Dot(quaternion1, quaternion2);

        if (dot >= 0.0f) {
            result.X = t1 * quaternion1.X + t * quaternion2.X;
            result.Y = t1 * quaternion1.Y + t * quaternion2.Y;
            result.Z = t1 * quaternion1.Z + t * quaternion2.Z;
            result.W = t1 * quaternion1.W + t * quaternion2.W;
        } else {
            result.X = t1 * quaternion1.X - t * quaternion2.X;
            result.Y = t1 * quaternion1.Y - t * quaternion2.Y;
            result.Z = t1 * quaternion1.Z - t * quaternion2.Z;
            result.W = t1 * quaternion1.W - t * quaternion2.W;
        }

        return result.Normalized;
    }

    /// <summary>
    /// Concatenates two Quaternions; the result represents the value1 rotation followed by the value2 rotation.
    /// </summary>
    /// <param name="value1">The first Quaternion rotation in the series.</param>
    /// <param name="value2">The second Quaternion rotation in the series.</param>
    /// <returns>A new Quaternion representing the concatenation of the value1 rotation followed by the value2 rotation.</returns>
    public static Quaternion Concatenate(Quaternion value1, Quaternion value2) {
        Quaternion result = Identity;

        // Concatenate rotation is actually q2 * q1 instead of q1 * q2.
        // So that's why value2 goes q1 and value1 goes q2.
        float q1x = value2.X;
        float q1y = value2.Y;
        float q1z = value2.Z;
        float q1w = value2.W;

        float q2x = value1.X;
        float q2y = value1.Y;
        float q2z = value1.Z;
        float q2w = value1.W;

        // cross(av, bv)
        float cx = q1y * q2z - q1z * q2y;
        float cy = q1z * q2x - q1x * q2z;
        float cz = q1x * q2y - q1y * q2x;

        float dot = q1x * q2x + q1y * q2y + q1z * q2z;

        result.X = q1x * q2w + q2x * q1w + cx;
        result.Y = q1y * q2w + q2y * q1w + cy;
        result.Z = q1z * q2w + q2z * q1w + cz;
        result.W = q1w * q2w - dot;

        return result;
    }

    /// <summary>
    /// Flips the sign of each component of the quaternion.
    /// </summary>
    /// <param name="value">The source Quaternion.</param>
    /// <returns>The negated Quaternion.</returns>
    public static Quaternion operator -(Quaternion value) {
        Quaternion result = Identity;
        result.X = -value.X;
        result.Y = -value.Y;
        result.Z = -value.Z;
        result.W = -value.W;
        return result;
    }

    /// <summary>
    /// Adds two Quaternions element-by-element.
    /// </summary>
    /// <param name="value1">The first source Quaternion.</param>
    /// <param name="value2">The second source Quaternion.</param>
    /// <returns>The result of adding the Quaternions.</returns>
    public static Quaternion operator +(Quaternion value1, Quaternion value2) {
        Quaternion result = Identity;
        result.X = value1.X + value2.X;
        result.Y = value1.Y + value2.Y;
        result.Z = value1.Z + value2.Z;
        result.W = value1.W + value2.W;
        return result;
    }

    /// <summary>
    /// Subtracts one Quaternion from another.
    /// </summary>
    /// <param name="value1">The first source Quaternion.</param>
    /// <param name="value2">The second Quaternion, to be subtracted from the first.</param>
    /// <returns>The result of the subtraction.</returns>
    public static Quaternion operator -(Quaternion value1, Quaternion value2) {
        Quaternion result = Identity;
        result.X = value1.X - value2.X;
        result.Y = value1.Y - value2.Y;
        result.Z = value1.Z - value2.Z;
        result.W = value1.W - value2.W;
        return result;
    }

    /// <summary>
    /// Multiplies two Quaternions together.
    /// </summary>
    /// <param name="value1">The Quaternion on the left side of the multiplication.</param>
    /// <param name="value2">The Quaternion on the right side of the multiplication.</param>
    /// <returns>The result of the multiplication.</returns>
    public static Quaternion operator *(Quaternion value1, Quaternion value2) {
        Quaternion result = Identity;

        float q1x = value1.X;
        float q1y = value1.Y;
        float q1z = value1.Z;
        float q1w = value1.W;

        float q2x = value2.X;
        float q2y = value2.Y;
        float q2z = value2.Z;
        float q2w = value2.W;

        // cross(av, bv)
        float cx = q1y * q2z - q1z * q2y;
        float cy = q1z * q2x - q1x * q2z;
        float cz = q1x * q2y - q1y * q2x;

        float dot = q1x * q2x + q1y * q2y + q1z * q2z;

        result.X = q1x * q2w + q2x * q1w + cx;
        result.Y = q1y * q2w + q2y * q1w + cy;
        result.Z = q1z * q2w + q2z * q1w + cz;
        result.W = q1w * q2w - dot;

        return result;
    }

    /// <summary>
    /// Multiplies a Quaternion by a scalar value.
    /// </summary>
    /// <param name="value1">The source Quaternion.</param>
    /// <param name="value2">The scalar value.</param>
    /// <returns>The result of the multiplication.</returns>
    public static Quaternion operator *(Quaternion value1, float value2) {
        Quaternion result = Identity;
        result.X = value1.X * value2;
        result.Y = value1.Y * value2;
        result.Z = value1.Z * value2;
        result.W = value1.W * value2;
        return result;
    }

    /// <summary>
    /// Divides a Quaternion by another Quaternion.
    /// </summary>
    /// <param name="value1">The source Quaternion.</param>
    /// <param name="value2">The divisor.</param>
    /// <returns>The result of the division.</returns>
    public static Quaternion operator /(Quaternion value1, Quaternion value2)
    {
        Quaternion result = Identity;

        float q1x = value1.X;
        float q1y = value1.Y;
        float q1z = value1.Z;
        float q1w = value1.W;

        //-------------------------------------
        // Inverse part.
        float invNorm = 1.0f / Dot(value2, value2);

        float q2x = -value2.X * invNorm;
        float q2y = -value2.Y * invNorm;
        float q2z = -value2.Z * invNorm;
        float q2w = value2.W * invNorm;

        //-------------------------------------
        // Multiply part.

        // cross(av, bv)
        float cx = q1y * q2z - q1z * q2y;
        float cy = q1z * q2x - q1x * q2z;
        float cz = q1x * q2y - q1y * q2x;

        float dot = q1x * q2x + q1y * q2y + q1z * q2z;

        result.X = q1x * q2w + q2x * q1w + cx;
        result.Y = q1y * q2w + q2y * q1w + cy;
        result.Z = q1z * q2w + q2z * q1w + cz;
        result.W = q1w * q2w - dot;

        return result;
    }

    /// <summary>
    /// Returns a boolean indicating whether the two given Quaternions are equal.
    /// </summary>
    /// <param name="value1">The first Quaternion to compare.</param>
    /// <param name="value2">The second Quaternion to compare.</param>
    /// <returns>True if the Quaternions are equal; False otherwise.</returns>
    public static bool operator ==(Quaternion value1, Quaternion value2) {
        return (value1.X == value2.X &&
                value1.Y == value2.Y &&
                value1.Z == value2.Z &&
                value1.W == value2.W);
    }

    /// <summary>
    /// Returns a boolean indicating whether the two given Quaternions are not equal.
    /// </summary>
    /// <param name="value1">The first Quaternion to compare.</param>
    /// <param name="value2">The second Quaternion to compare.</param>
    /// <returns>True if the Quaternions are not equal; False if they are equal.</returns>
    public static bool operator !=(Quaternion value1, Quaternion value2) {
        return (value1.X != value2.X ||
                value1.Y != value2.Y ||
                value1.Z != value2.Z ||
                value1.W != value2.W);
    }

    /// <summary>
    /// Returns a boolean indicating whether the given Quaternion is equal to this Quaternion instance.
    /// </summary>
    /// <param name="other">The Quaternion to compare this instance to.</param>
    /// <returns>True if the other Quaternion is equal to this instance; False otherwise.</returns>
    public bool Equals(Quaternion other) {
        return (X == other.X &&
                Y == other.Y &&
                Z == other.Z &&
                W == other.W);
    }

    /// <summary>
    /// Returns a boolean indicating whether the given Object is equal to this Quaternion instance.
    /// </summary>
    /// <param name="obj">The Object to compare against.</param>
    /// <returns>True if the Object is equal to this Quaternion; False otherwise.</returns>
    public override bool Equals(object obj) {
        if (obj is Quaternion)
            return Equals((Quaternion)obj);
        return false;
    }

    /// <summary>
    /// Returns a String representing this Quaternion instance.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString() {
        CultureInfo ci = CultureInfo.CurrentCulture;
        return String.Format(ci, "{{X:{0} Y:{1} Z:{2} W:{3}}}", X.ToString(ci), Y.ToString(ci), Z.ToString(ci), W.ToString(ci));
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode() {
        return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
    }
    
}
