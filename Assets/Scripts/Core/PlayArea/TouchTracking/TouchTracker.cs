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

/*
 * TODO need to be refactored!
 */
namespace Core.PlayArea.TouchTracking{

    public interface IContinueTrackOnEnter{
        
    }
    
    public class TouchTracker : MonoBehaviour, ITutorialControllable{
        public float minDistance = 5f;
        public LineRenderer lineRenderer;
        public new Camera camera;
        public LineColliderGenerator lineColliderGenerator;
        public TouchCollider touchCollider;
        public CircleCollider polygonCollider;
        public float totalLineLength = 5f;
        public ProgressBar progressBar;
        public PointDisplayer pointDisplayer;
        public StageManager stageManager;
        private CircleDetector _circleDetector;
        private float _currentLineLength;
        private Game _game;
        private bool _isInTutorial;
        public bool tutorKeepLine = false;
        public bool isAcceptingInput = true;


        public bool isTracing;
        private RectTransform _rect;
        private bool _mouseOnBall = false;


        private void Start(){
            _rect = GetComponent<RectTransform>();
            _circleDetector = new CircleDetector();
            touchCollider.SetEnabled(false);
            _game = GameManager.shared.game;
        }

        private void Update(){
            if (!isAcceptingInput) return;
            TraceTouchPosition();
        }
        
        private void OnMouseDown(){
            if (!isAcceptingInput) return;
            StartTracking();
        }

        private IEnumerator OnMouseExit(){
            if (!isAcceptingInput) yield break;
            yield return new WaitForEndOfFrame();
            if (_mouseOnBall) yield break;
            StopTracking();
        }

        private void OnMouseUp(){
            if (!isAcceptingInput) return;
            StopTracking();
        }

        public void HandOverControlTo(TutorialBase tutorial){
            _isInTutorial = true;
        }

        public void GainBackControlFrom(TutorialBase tutorial){
            _isInTutorial = false;
        }

        public event TutorialControllableEvent OnTouchEnd;
        public event TutorialControllableEvent OnTouchStart;

        public void StartTracking(){
            isTracing = true;
            // touchCollider.SetEnabled(true);
            lineRenderer.positionCount = 0;
            _circleDetector.points.Clear();
            _currentLineLength = 0;
            OnTouchStart?.Invoke(this);
        }

        public void StopTracking(){
            if (!isTracing) return;
            isTracing = false;
            touchCollider.SetEnabled(false);
            if (_isInTutorial) OnTouchEnd?.Invoke(this);
            if (_game.player.hitBalls.Count == 0 && _game.player.circledBalls.Count == 0) return;

            isAcceptingInput = false;
            StartCoroutine(HideLine());
            GameManager.shared.OnPlayerFinishInput();
        }

        private IEnumerator HideLine(){
            if (_isInTutorial){
                yield return new WaitWhile(() => tutorKeepLine);
            }
            yield return CoroutineUtility.Delayed(0.1f, () => lineRenderer.positionCount = 0);
        }
        
        

        private Vector2 GetCurrentTouchPosition(){
            if (Input.touchCount == 0) return Vector2.zero;
            return Input.touches[0].position;
        }

        private void TraceTouchPosition(){
            if (!isTracing) return;
            if (_currentLineLength >= totalLineLength) return;
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
            touchCollider.transform.SetPositionAndRotation(worldPosition, Quaternion.Euler(0, 0, 0));

            // TODO change point displayer position

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
            if (circle != null)
                polygonCollider.SetCircle(circle
                    .Select(v => (Vector2)polygonCollider.transform.InverseTransformPoint(v)).ToArray());

            // Add point to lineRenderer
            lineRenderer.positionCount = positionCount + 1;
            lineRenderer.SetPosition(positionCount, worldPosition);

            // Add length
            _currentLineLength += curSegLength;
            progressBar.Percentage = 100 - _currentLineLength / totalLineLength * 100;
        }

        public void ContinueTrackOnMouseExit(){
            if (!isTracing) return;
            if (_currentLineLength >= totalLineLength) return;
            _mouseOnBall = true;
        }

        public void OnMouseExitBall(){
            if (!isTracing) return;
            _mouseOnBall = false;
        }

        public void OnMouseUpBall(){
            if (!isTracing) return;
            _mouseOnBall = false;
            OnMouseUp();
        }
    }
}