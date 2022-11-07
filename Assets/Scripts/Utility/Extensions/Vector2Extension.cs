using UnityEngine;

namespace Utility.Extensions{
    public static class Vector2Extension{
        /// <summary>
        /// Rotate the vector in degrees, clockwise; 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector2 Rotated(this Vector2 v, float degrees){
            var sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            var cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            var tx = v.x;
            var ty = v.y;
            v.x = cos * tx - sin * ty;
            v.y = sin * tx + cos * ty;
            return v;
        }
        
        /// <summary>
        /// Align the vector to axis directions
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Vector2 Aligned(this Vector2 vec){
            if (vec.magnitude == 0) return Vector2.zero;
            vec = vec.Rotated(-45);
            if (vec.x > 0){
                if (vec.y > 0)
                    return Vector2.up;
                return Vector2.right;
            }

            if (vec.y > 0)
                return Vector2.left;
            return Vector2.down;
        }
    }
}