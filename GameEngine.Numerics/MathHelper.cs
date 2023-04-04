using System.Numerics;

namespace GameEngine.Numerics
{
    /// <summary>
    /// Contains commonly used precalculated values and mathematical operations.
    /// </summary>
    public static class MathHelper
    {
    	/// <summary>
        /// Represents the mathematical constant e(2.71828175).
        /// </summary>
        public const float E = MathF.E;
        
        /// <summary>
        /// Represents the log base ten of e(0.4342945).
        /// </summary>
        public const float LOG10_E = 0.4342945f;
        
        /// <summary>
        /// Represents the log base two of e(1.442695).
        /// </summary>
        public const float LOG2_E = 1.442695f;
        
        /// <summary>
        /// Represents the value of pi(3.14159274).
        /// </summary>
        public const float PI = MathF.PI;
        
        /// <summary>
        /// Represents the value of pi divided by two(1.57079637).
        /// </summary>
        public const float PI_OVER2 = (float)(Math.PI / 2.0);
        
        /// <summary>
        /// Represents the value of pi divided by four(0.7853982).
        /// </summary>
        public const float PI_OVER4 = (float)(Math.PI / 4.0);
        
        /// <summary>
        /// Represents the value of pi times two(6.28318548).
        /// </summary>
        public const float TWO_PI = (float)(Math.PI * 2.0);
        
        /// <summary>
        /// Represents the value of pi times two(6.28318548).
        /// This is an alias of TwoPi.
        /// </summary>
        public const float TAU = TWO_PI;
        
        /// <summary>
        /// Returns the Cartesian coordinate for one axis of a point that is defined by a given triangle and two normalized barycentric (areal) coordinates.
        /// </summary>
        /// <param name="value1">The coordinate on one axis of vertex 1 of the defining triangle.</param>
        /// <param name="value2">The coordinate on the same axis of vertex 2 of the defining triangle.</param>
        /// <param name="value3">The coordinate on the same axis of vertex 3 of the defining triangle.</param>
        /// <param name="amount1">The normalized barycentric (areal) coordinate b2, equal to the weighting factor for vertex 2, the coordinate of which is specified in value2.</param>
        /// <param name="amount2">The normalized barycentric (areal) coordinate b3, equal to the weighting factor for vertex 3, the coordinate of which is specified in value3.</param>
        /// <returns>Cartesian coordinate of the specified point with respect to the axis being used.</returns>
        public static T Barycentric<T>(T value1, T value2, T value3, T amount1, T amount2) where T : IFloatingPointIeee754<T>
        {
            return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
        }

	    /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions.
        /// </summary>
        /// <param name="value1">The first position in the interpolation.</param>
        /// <param name="value2">The second position in the interpolation.</param>
        /// <param name="value3">The third position in the interpolation.</param>
        /// <param name="value4">The fourth position in the interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>A position that is the result of the Catmull-Rom interpolation.</returns>
        public static T CatmullRom<T>(T value1, T value2, T value3, T value4, T amount) where T : IFloatingPointIeee754<T>
        {
            // Using formula from http://www.mvps.org/directx/articles/catmull/
            // Internally using doubles not to lose precission
            double amountSquared = double.CreateChecked(amount) * double.CreateChecked(amount);
            double amountCubed = double.CreateChecked(amountSquared) * double.CreateChecked(amount);
            return T.CreateChecked(
                0.5d * (
                    2.0d * double.CreateChecked(value2) +
                    double.CreateChecked(value3 - value1) * double.CreateChecked(amount) +
                    (2.0d * double.CreateChecked(value1) - 5.0d * double.CreateChecked(value2) + 4.0d * double.CreateChecked(value3) - double.CreateChecked(value4)) * amountSquared +
                    (3.0d * double.CreateChecked(value2) - double.CreateChecked(value1) - 3.0d * double.CreateChecked(value3) + double.CreateChecked(value4)) * amountCubed
                    ));
        }
        
        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value. If <c>value</c> is less than <c>min</c>, <c>min</c> will be returned.</param>
        /// <param name="max">The maximum value. If <c>value</c> is greater than <c>max</c>, <c>max</c> will be returned.</param>
        /// <returns>The clamped value.</returns>
        public static T Clamp<T>(T value, T min, T max) where T : INumber<T>
        { 
            value = (value > max) ? max : value; 
            value = (value < min) ? min : value; 
            return value;
        }
        
        /// <summary>
        /// Calculates the absolute value of the difference of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>Distance between the two values.</returns>
        public static T Distance<T>(T value1, T value2) where T : IFloatingPointIeee754<T>
        {
            return T.Abs(value1 - value2);
        }
        
        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="value1">Source position.</param>
        /// <param name="tangent1">Source tangent.</param>
        /// <param name="value2">Source position.</param>
        /// <param name="tangent2">Source tangent.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The result of the Hermite spline interpolation.</returns>
        public static T Hermite<T>(T value1, T tangent1, T value2, T tangent2, T amount) where T : IFloatingPointIeee754<T>
        {
            // All transformed to double not to lose precission
            // Otherwise, for high numbers of param:amount the result is NaN instead of Infinity
            double v1 = double.CreateChecked(value1);
            double v2 = double.CreateChecked(value2);
            double t1 = double.CreateChecked(tangent1);
            double t2 = double.CreateChecked(tangent2);
            double s = double.CreateChecked(amount);
            double sCubed = s * s * s;
            double sSquared = s * s;
            
            if(amount == T.Zero)
                return value1;
            if(amount == T.One)
                return value2;
            
            double result = (2 * v1 - 2 * v2 + t2 + t1) * sCubed +
                           (3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared +
                           t1 * s +
                           v1;
            return T.CreateChecked(result);
        }
        
        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Destination value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns> 
        /// <remarks>This method performs the linear interpolation based on the following formula:
        /// <code>value1 + (value2 - value1) * amount</code>.
        /// Passing amount a value of 0 will cause value1 to be returned, a value of 1 will cause value2 to be returned.
        /// See <see cref="MathHelper.LerpPrecise{T}"/> for a less efficient version with more precision around edge cases.
        /// </remarks>
        public static T Lerp<T>(T value1, T value2, T amount) where T : IFloatingPointIeee754<T>
        {
            return value1 + (value2 - value1) * amount;
        }


        /// <summary>
        /// Linearly interpolates between two values.
        /// This method is a less efficient, more precise version of <see cref="MathHelper.Lerp{T}"/>.
        /// See remarks for more info.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Destination value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns>
        /// <remarks>This method performs the linear interpolation based on the following formula:
        /// <code>((1 - amount) * value1) + (value2 * amount)</code>.
        /// Passing amount a value of 0 will cause value1 to be returned, a value of 1 will cause value2 to be returned.
        /// This method does not have the floating point precision issue that <see cref="MathHelper.Lerp{T}"/> has.
        /// i.e. If there is a big gap between value1 and value2 in magnitude (e.g. value1=10000000000000000, value2=1),
        /// right at the edge of the interpolation range (amount=1), <see cref="MathHelper.Lerp{T}"/> will return 0 (whereas it should return 1).
        /// This also holds for value1=10^17, value2=10; value1=10^18,value2=10^2... so on.
        /// For an in depth explanation of the issue, see below references:
        /// Relevant Wikipedia Article: https://en.wikipedia.org/wiki/Linear_interpolation#Programming_language_support
        /// Relevant StackOverflow Answer: http://stackoverflow.com/questions/4353525/floating-point-linear-interpolation#answer-23716956
        /// </remarks>
        public static T LerpPrecise<T>(T value1, T value2, T amount) where T : IFloatingPointIeee754<T>
        {
            return ((T.One - amount) * value1) + (value2 * amount);
        }

        /// <summary>
        /// Returns the greater of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>The greater value.</returns>
        public static T Max<T>(T value1, T value2) where T : INumber<T>
        {
            return value1 > value2 ? value1 : value2;
        }
        
        /// <summary>
        /// Returns the lesser of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>The lesser value.</returns>
        public static T Min<T>(T value1, T value2) where T : INumber<T>
        {
            return value1 < value2 ? value1 : value2;
        }
        
        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Weighting value.</param>
        /// <returns>Interpolated value.</returns>
        public static T SmoothStep<T>(T value1, T value2, T amount) where T : IFloatingPointIeee754<T>
        {
            // It is expected that 0 < amount < 1
            // If amount < 0, return value1
            // If amount > 1, return value2
            T result = T.Clamp(amount, T.Zero, T.One);
            result = MathHelper.Hermite(value1, T.Zero, value2, T.Zero, result);
            
            return result;
        }
        
        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians">The angle in radians.</param>
        /// <returns>The angle in degrees.</returns>
        /// <remarks>
        /// This method uses double precission internally,
        /// though it returns single float
        /// Factor = 180 / pi
        /// </remarks>
        public static float ToDegrees(float radians)
        { 
            return (float)(radians * 57.295779513082320876798154814105);
        }
        
        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        /// <remarks>
        /// This method uses double precission internally,
        /// though it returns single float
        /// Factor = pi / 180
        /// </remarks>
        public static float ToRadians(float degrees)
        { 
            return (float)(degrees * 0.017453292519943295769236907684886);
        }
	 
        /// <summary>
        /// Reduces a given angle to a value between π and -π.
        /// </summary>
        /// <param name="angle">The angle to reduce, in radians.</param>
        /// <returns>The new angle, in radians.</returns>
        public static float WrapAngle(float angle)
        {
            if ((angle > -PI) && (angle <= PI))
                return angle;
            angle %= TWO_PI;
            if (angle <= -PI)
                return angle + TWO_PI;
            if (angle > PI)
                return angle - TWO_PI;
            return angle;
        }

 	/// <summary>
        /// Determines if value is powered by two.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <returns><c>true</c> if <c>value</c> is powered by two; otherwise <c>false</c>.</returns>
	public static bool IsPowerOfTwo(int value)
	{
	     return (value > 0) && ((value & (value - 1)) == 0);
	}
    }
}
