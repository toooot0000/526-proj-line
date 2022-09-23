using UnityEngine;

namespace Utility.Extensions{
    public static class Vector2Extension {
     
        public static Vector2 Rotate(this Vector2 v, float degrees) {
            var sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            var cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
         
            var tx = v.x;
            var ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }

        public static Vector2 Align(this Vector2 vec){
            if (vec.magnitude == 0){
                return Vector2.zero;
            }
            vec = vec.Rotate(-45);
            if (vec.x > 0){
                if (vec.y > 0){
                    return Vector2.up;
                } else{
                    return Vector2.right;
                }
            } else{
                if (vec.y > 0){
                    return Vector2.left;
                } else{
                    return Vector2.down;
                }
            }
        }
    }
}