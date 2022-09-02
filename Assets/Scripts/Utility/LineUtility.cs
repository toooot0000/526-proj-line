using UnityEngine;

namespace Utility{
    public class LineUtility{
        public static Vector2? GetIntersectPoint(Vector2 s1, Vector2 e1, Vector2 s2, Vector2 e2){
            // Vector2 d1 = e1 - s1, d2 = e2 - s2;
            // if (Mathf.Approximately(Vector3.Cross(d1, d2).magnitude, 0.0f)) return null;
            var det = (e1.x - s1.x) * (e2.y - s2.y) - (e2.x - s2.x) * (e1.y - s1.y);
            if (Mathf.Approximately(det, 0.0f)) return null;
            var t = ((s2.x - s1.x) * (e2.y - s2.y) - (s2.y - s1.y) * (e2.x - s2.x))/det;
            var p = ((s2.x-s1.x)*(e1.y-s1.y)-(s2.y-s1.y)*(e1.x-s1.x))/det;
            if (t is < 0 or > 1 || p is < 0 or > 1) return null;
            return s1 + (e1 - s1) * t;
        }
    }
}