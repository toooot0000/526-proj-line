using System;
using System.Collections;
using System.Linq;
using Core.Common;
using Core.DisplayArea.Stage;
using Core.PlayArea.Balls;
using Model;
using Tutorial;
using UnityEngine;
using Utility;

namespace Core.PlayArea.TouchTracking{

    public class TouchTracker : MonoBehaviour, ITutorialControllable{
        public float minDistance = 5f;
        public LineRenderer lineRenderer;
        public new Camera camera;
        public CircleCollider circleCollider;
        public ProgressBar progressBar;
        private readonly CircleDetector _circleDetector = new CircleDetector();
        private float _currentLineLength;
        private bool _isInTutorial;
        [HideInInspector]
        public bool tutorKeepLine = false;
        [HideInInspector]
        public bool isAcceptingInput = false;
        [HideInInspector]
        public bool isTracing;
        private bool _continueTracking = false;
        private float TotalLineLength => initLineLength + lineLengthAdder;

        [HideInInspector] 
        public float initLineLength = 10f;
        [HideInInspector] 
        public float lineLengthAdder = 0;


        public bool IsReachingLineLengthLimit() => _currentLineLength >= TotalLineLength;

        private void Update(){
            if (!isTracing) return;
            TraceTouchPosition();
            if(IsReachingLineLengthLimit()) StopTracking();
        }
        
        private void OnMouseDown(){
            ResetState();
            StartTracking();
        }

        private IEnumerator OnMouseExit(){
            yield return new WaitForEndOfFrame();
            if (_continueTracking){
                _continueTracking = false;
                yield break;
            }
            StopTracking();
            SendInput();
            if (_isInTutorial) yield return new WaitWhile(() => tutorKeepLine);
            yield return HideLine();
        }

        public IEnumerator OnMouseUp(){
            StopTracking();
            SendInput();
            if (_isInTutorial) yield return new WaitWhile(() => tutorKeepLine);
            yield return HideLine();
        }

        public void HandOverControlTo(TutorialBase tutorial){
            _isInTutorial = true;
        }

        public void GainBackControlFrom(TutorialBase tutorial){
            _isInTutorial = false;
        }

        public event TutorialControllableEvent OnInputReadyToSent;

        public void StartTracking(){
            if (!isAcceptingInput) return;
            ResetState();
            isTracing = true;
        }

        public bool StopTracking(){
            if (!isTracing) return false;
            isTracing = false;
            return true;
        }

        private IEnumerator HideLine(){
            yield return CoroutineUtility.Delayed(0.1f, () => lineRenderer.positionCount = 0);
        }
        
        private Vector2 GetCurrentTouchPosition(){
            if (Input.touchCount == 0) return Vector2.zero;
            return Input.touches[0].position;
        }

        private void TraceTouchPosition(){
#if UNITY_STANDALONE || UNITY_WEBGL
            var inputPosition = Input.mousePosition;
#else
            var inputPosition = GetCurrentTouchPosition();
#endif
            var worldPosition = camera.ScreenToWorldPoint(new Vector3(inputPosition.x,
                inputPosition.y,
                camera.nearClipPlane));
            if (!_isInTutorial) worldPosition.z = -0.1f;
            var positionCount = lineRenderer.positionCount;
            
            var curSegLength = PointDistanceValidate(worldPosition);
            if (curSegLength < 0) return;

            ActivateCircleCollider(worldPosition);
            
            // Add point to lineRenderer
            lineRenderer.positionCount = positionCount + 1;
            lineRenderer.SetPosition(positionCount, worldPosition);

            // Add length
            _currentLineLength += curSegLength;
            progressBar.Percentage = 100 - _currentLineLength / TotalLineLength * 100;
        }

        private void SendInput(){
            if (_isInTutorial) OnInputReadyToSent?.Invoke(this);
            GameManager.shared.OnPlayerFinishDrawing();
        }

        public void ContinueTracking(){
            _continueTracking = true;
        }

        private void ActivateCircleCollider(Vector3 worldPosition){
            // Circle handling 
            _circleDetector.AddPoint(worldPosition);
            var circle = _circleDetector.DetectCircle();
            if (circle != null)
                circleCollider.SetCircle(circle
                    .Select(v => (Vector2)circleCollider.transform.InverseTransformPoint(v)).ToArray());
        }

        private float PointDistanceValidate(Vector3 worldPosition){
            var positionCount = lineRenderer.positionCount;
            // Length validate
            if (positionCount > 0){
                var remain = TotalLineLength - _currentLineLength;
                var curSegLength = (worldPosition - lineRenderer.GetPosition(positionCount - 1)).magnitude;
                if (curSegLength < minDistance) return -1;
                curSegLength = Mathf.Min(curSegLength, remain);
                return curSegLength;
            }
            return 0;
        }

        private void ResetState(){
            lineRenderer.positionCount = 0;
            _circleDetector.points.Clear();
            _currentLineLength = 0;
        }
    }
}