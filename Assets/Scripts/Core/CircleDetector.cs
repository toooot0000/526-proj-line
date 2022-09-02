using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LineUtility = Utility.LineUtility;

namespace Core{
    public class CircleDetector{

        public List<Vector2> points;

        public CircleDetector(){
            points = new List<Vector2>();
        }

        public void AddPoint(Vector2 point){
            points.Add(point);
        }

        public IEnumerable<Vector2> DetectCircle(){
            if (points.Count < 3) return null;
            var last = points[^2];
            var point = points[^1];
            for (var i = 0; i < points.Count-2; i++){
                var intersectPoint = LineUtility.GetIntersectPoint(points[i], points[i + 1], last, point);
                if (intersectPoint != null && intersectPoint != last){
                    return CalculateCircle(i + 1, points.Count - 2, (Vector2)intersectPoint);
                }
            }
            return null;
        }

        private IEnumerable<Vector2> CalculateCircle(int startIndex, int endIndex, Vector2 intersectPoint){
            var count = endIndex - startIndex + 2;
            var ret = new Vector2[count];
            points.CopyTo(startIndex, ret, 0, count - 1);
            ret[count - 1] = intersectPoint;
            RemoveCircle(startIndex, endIndex, intersectPoint);
            return ret;
        }


        private void RemoveCircle(int startIndex, int endIndex, Vector2 intersectPoint){
            points.RemoveRange(startIndex, endIndex - startIndex + 1);
            points.Add(intersectPoint);
            (points[^1], points[^2]) = (points[^2], points[^1]);
        }
    }
}