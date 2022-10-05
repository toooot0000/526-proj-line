using UnityEngine;

namespace Utility.Extensions{
    public static class FloatExtension{
        public static bool AlmostEquals(this float a, float other){
            return Mathf.Abs(a - other) <= Mathf.Epsilon;
        }
    }
}