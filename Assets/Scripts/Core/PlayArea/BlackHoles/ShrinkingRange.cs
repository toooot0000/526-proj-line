using UnityEngine;
using Utility.Animation;

namespace Core.PlayArea.BlackHoles{
    public struct Vec3Wrapper: ILerpable<Vec3Wrapper>{
        public Vector3 value;
        public Vec3Wrapper(Vector3 val){
            value = val;
        }
        public Vec3Wrapper Add(Vec3Wrapper other){
            return new Vec3Wrapper(value + other.value);
        }

        public Vec3Wrapper Time(float k){
            return new Vec3Wrapper(value * k);
        }
    }
    
    public class ShrinkingRange: Tween<Vec3Wrapper>{
        protected override void OnValueChange(Vec3Wrapper current){
            transform.localScale = current.value;
        }
    }
}