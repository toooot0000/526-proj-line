using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Bezier {
    public class BezierCurve: MonoBehaviour{
        public Vector3[] points;

        void Awake(){
            if(points == null || points.Length < 4){
                Reset();
            }
        }

        void Reset(){
            if(points.Length == 0){
                points = new Vector3[]{
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0)
                };
            }
        }

        public Vector3 GetPoint(float t){
            return transform.TransformPoint(BezierLerp.GetPoint(points[0], points[1], points[2], points[3], t));
        }

        public Vector3 GetVelocity(float t){
            return transform.TransformPoint(BezierLerp.GetFirstDerivative(points[0], points[1], points[2], points[3], t));
        }

        public Vector3 GetDirection(float t){
            return GetVelocity(t).normalized;
        }

    }


    static class BezierLerp{
        public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * oneMinusT * p0 +
                3f * oneMinusT * oneMinusT * t * p1 +
                3f * oneMinusT * t * t * p2 +
                t * t * t * p3;
        }

        public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                3f * oneMinusT * oneMinusT * (p1 - p0) +
                6f * oneMinusT * t * (p2 - p1) +
                3f * t * t * (p3 - p2);
        }
    }
}