using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

namespace Core{
    public class TouchTracking : MonoBehaviour{
        public float minDistance = 5f;
        public LineRenderer lineRenderer;
        public new Camera camera;
        public LineColliderGenerator lineColliderGenerator;
        public GameObject touchCollider;
        public CircleCollider polygonCollider;
        
        private Boolean _isTracing = false;
        private RectTransform _rect;
        private CircleDetector _circleDetector;

        private void Start(){
            _rect = GetComponent<RectTransform>();
            _circleDetector = new CircleDetector();
        }

        private void Update(){
            TraceTouchPosition();
        }

        private void FixedUpdate(){
            if (!_isTracing) return;
        }

        public void StartTracking(){
            _isTracing = true;
            lineRenderer.positionCount = 0;
            _circleDetector.points.Clear();
        }

        public void StopTracking(){
            if (!_isTracing) return;
            _isTracing = false;
        }

        private Vector2 GetCurrentTouchPosition(){
            if(Input.touchCount == 0) return Vector2.zero;
            return Input.touches[0].position;
        }

        private void OnMouseDown(){
            StartTracking();
        }

        private void OnMouseExit(){
            StopTracking();
        }

        private void OnMouseUp(){
            StopTracking();
        }

        private void TraceTouchPosition(){
            if (!_isTracing) return;
#if UNITY_STANDALONE || UNITY_WEBGL
            var inputPosition = Input.mousePosition;
#else
            var inputPosition = GetCurrentTouchPosition();
#endif
            var worldPosition = camera.ScreenToWorldPoint(new Vector3(inputPosition.x,
                inputPosition.y,
                camera.nearClipPlane));
            worldPosition.z = -.1f;
            var positionCount = lineRenderer.positionCount;
            if (positionCount > 0 &&
                (worldPosition - lineRenderer.GetPosition(positionCount - 1)).magnitude < minDistance)
                return;
            
            touchCollider.transform.SetPositionAndRotation(worldPosition, Quaternion.Euler(0, 0, 0));

            _circleDetector.AddPoint(worldPosition);
            var circle = _circleDetector.DetectCircle();
            if (circle != null){
                var count = _circleDetector.points.Count;
                lineRenderer.positionCount = count;
                for (int i = 0; i < count; i++){
                    lineRenderer.SetPosition(i, _circleDetector.points[i]);
                }
                // TODO Add the circled balls detection
                polygonCollider.SetCircle(circle.Select(v => (Vector2)polygonCollider.transform.InverseTransformPoint(v)).ToArray());
            } else{
                lineRenderer.positionCount = positionCount + 1;
                lineRenderer.SetPosition(positionCount, worldPosition);
            }
        }


    }
}