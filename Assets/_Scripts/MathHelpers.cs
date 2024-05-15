using UnityEngine;

namespace JustGame.Scripts.Managers
{
    public static class MathHelpers
    {
        public static bool IsContainBound(this Bounds bound, Bounds target)
        {
            return (bound.Contains(target.min) && bound.Contains(target.max));
        }

        /// <summary>
        /// Check whether 2 rect collide with each other. Each rect has its position as its center not top-right
        /// </summary>
        /// <param name="rect1Pos"></param>
        /// <param name="rect1Size"></param>
        /// <param name="rect2Pos"></param>
        /// <param name="rect2Size"></param>
        /// <returns></returns>
        public static bool Is2RectCollided(Vector2 rect1Pos, Vector2 rect1Size, Vector2 rect2Pos, Vector2 rect2Size)
        {
            // Calculate the minimum and maximum coordinates of each rectangle
            float rect1MinX = rect1Pos.x;
            float rect1MaxX = rect1Pos.x + rect1Size.x;
            float rect1MinY = rect1Pos.y;
            float rect1MaxY = rect1Pos.y + rect1Size.y;

            float rect2MinX = rect2Pos.x;
            float rect2MaxX = rect2Pos.x + rect2Size.x;
            float rect2MinY = rect2Pos.y;
            float rect2MaxY = rect2Pos.y + rect2Size.y;

            // Check for overlap in the X-axis
            bool isOverlapX = rect1MinX <= rect2MaxX && rect1MaxX >= rect2MinX;

            // Check for overlap in the Y-axis
            bool isOverlapY = rect1MinY <= rect2MaxY && rect1MaxY >= rect2MinY;

            // If there is overlap in both X and Y axes, a collision has occurred
            bool isCollision = isOverlapX && isOverlapY;

            return isCollision;
        }
        

        /// <summary>
        /// Remaps a value x in interval [A,B], to the proportional value in interval [C,D]
        /// </summary>
        /// <param name="x">The value to remap.</param>
        /// <param name="A">the minimum bound of interval [A,B] that contains the x value</param>
        /// <param name="B">the maximum bound of interval [A,B] that contains the x value</param>
        /// <param name="C">the minimum bound of target interval [C,D]</param>
        /// <param name="D">the maximum bound of target interval [C,D]</param>
        public static float Remap(float x, float A, float B, float C, float D)
        {
            var remappedValue = C + (x-A)/(B-A) * (D - C);
            return remappedValue;
        }
        
        /// <summary>
        /// Returns the angle of this vector, in radians
        /// </summary>
        /// <param name="v">The vector to get the angle of. It does not have to be normalized</param>
        /// <returns></returns>
        public static float GetAngle( this Vector2 v ) => Mathf.Atan2( v.y, v.x );
        public static float DegToRad( this float angDegrees ) => angDegrees * Mathf.Deg2Rad;
        public static float RadToDeg( this float angRadians ) => angRadians * Mathf.Rad2Deg;

        public static bool IsInclusiveRange(this float f, float min, float max)
        {
            return (min <= f) && (f <= max);
        }
        public static bool IsExclusiveRange(this float f, float min, float max)
        {
            return (min < f) && (f < max);
        }

        /// <summary>
        /// Reset position, localScale and rotation value to default value. <br/>
        /// Position => Vector3 (0,0,0) <br/>
        /// LocalScale => Vector3(1,1,1) <br/>
        /// Rotation => Quaternion Identity <br/>
        /// </summary>
        /// <param name="tf"></param>
        public static void Reset(this Transform tf, bool ignoreScale = false)
        {
            tf.position = Vector3.zero;
            if (!ignoreScale)
            {
                tf.localScale = Vector3.one;
            }
            tf.rotation = Quaternion.identity;
        }

        /// <summary>
        /// Convert number to percentage
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float PercentOf(float value)
        {
            return value / 100;
        }

        /// <summary>
        /// Return percent of value x
        /// </summary>
        /// <param name="x"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static float Percent(float x, float percent)
        {
            return (x / 100f * percent);
        }
    }
}
