using UnityEngine;
using Utility.Extensions;

namespace Utility{
    public static class Vector2Utility{
        public static Vector2 RandomDirection => Vector2.one.Rotated(Random.Range(0, 360)).normalized;
    }
}