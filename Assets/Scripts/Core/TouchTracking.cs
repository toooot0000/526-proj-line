using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Core{
    public class TouchTracking : MonoBehaviour{
        public float minDistance = 5f;
        public LineRenderer lineRenderer;
        public new Camera camera;
        
        private Touch _touch;
        private Boolean _isTracing;
        private RectTransform _rect;

        private void Start(){
            _rect = GetComponent<RectTransform>();
        }

        private void Update(){
            if (!_isTracing) return;
#if UNITY_STANDALONE || UNITY_WEBGL
            var inputPosition = Input.mousePosition;
#else
            var inputPosition = GetCurrentTouchPosition();
#endif
            var worldPosition = camera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, camera.nearClipPlane));
            worldPosition.z = -.1f;
            var positionCount = lineRenderer.positionCount;
            if (positionCount > 0 &&
                (worldPosition - lineRenderer.GetPosition(positionCount - 1)).magnitude < minDistance) return;
            lineRenderer.positionCount = positionCount + 1;
            lineRenderer.SetPosition(positionCount, worldPosition);
        }

        private void FixedUpdate(){
            if (!_isTracing) return;
        }

        public void StartTracking(){
            _isTracing = true;
            lineRenderer.positionCount = 0;
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
    }
}