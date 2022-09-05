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
        public TouchCollider touchCollider;
        public CircleCollider polygonCollider;
        public float totalLineLength;
        public LengthBar lengthBar;


        private Boolean _isTracing = false;
        private RectTransform _rect;
        private CircleDetector _circleDetector;
        private float _currentLineLength = 0;

        private void Start(){
            _rect = GetComponent<RectTransform>();
            _circleDetector = new CircleDetector();
            touchCollider.SetEnabled(false);
        }

        private void Update(){
            TraceTouchPosition();
        }

        private void FixedUpdate(){
            if (!_isTracing) return;
        }

        public void StartTracking(){
            _isTracing = true;
            touchCollider.SetEnabled(true);
            lineRenderer.positionCount = 0;
            _circleDetector.points.Clear();
            _currentLineLength = 0;
        }

        public void StopTracking(){
            if (!_isTracing) return;
            _isTracing = false;
            touchCollider.SetEnabled(false);
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
            if (_currentLineLength >= totalLineLength) return;
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
            touchCollider.transform.SetPositionAndRotation(worldPosition, Quaternion.Euler(0, 0, 0));
            
            // Length validate
            var curSegLength = 0.0f;
            if (positionCount > 0){
                var remain = totalLineLength - _currentLineLength;
                curSegLength = (worldPosition - lineRenderer.GetPosition(positionCount - 1)).magnitude;
                if (curSegLength < minDistance) return;
                curSegLength = Mathf.Min(curSegLength, remain);
            }
            
            // Circle handling 
            _circleDetector.AddPoint(worldPosition);
            var circle = _circleDetector.DetectCircle();
            if (circle != null){
                // TODO Add the circled balls detection
                polygonCollider.SetCircle(circle
                    .Select(v => (Vector2)polygonCollider.transform.InverseTransformPoint(v)).ToArray());
            }
            
            // Add point to lineRenderer
            lineRenderer.positionCount = positionCount + 1;
            lineRenderer.SetPosition(positionCount, worldPosition);
            
            // Add length
            _currentLineLength += curSegLength;
            lengthBar.Percentage = 100 - _currentLineLength / totalLineLength * 100;
        }
    }
}