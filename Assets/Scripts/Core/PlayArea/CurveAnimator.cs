using System;
using UnityEngine;
using Utility;
using Utility.Bezier;

namespace Core.PlayArea{
    public class CurveAnimator: MonoBehaviour{
        public AnimationCurve curve;
        
        public void FlyToLocation(float seconds, Vector3 targetWorldLocation, Action start, Action completion){
            var startWorldLocation = transform.position;
            var p1 = new Vector3{
                x = startWorldLocation.x,
                y = (startWorldLocation.y + targetWorldLocation.y) / 2,
                z = startWorldLocation.z
            };
            var p2 = new Vector3{
                x = (startWorldLocation.x + targetWorldLocation.x) / 2,
                y = targetWorldLocation.y,
                z = startWorldLocation.z
            };

            var lerp = TweenUtility.Lerp(
                seconds,
                start,
                i => {
                    i = curve.Evaluate(i);
                    transform.position = BezierLerp.GetPoint(startWorldLocation, p1, p2, targetWorldLocation, i);
                },
                completion);
            StartCoroutine(lerp());
        }
    }
}