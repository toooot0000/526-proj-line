using System;
using UnityEngine;
using Utility.Extensions;

namespace Tutorial.Common{
    public class TutorialMovingPointer : MonoBehaviour{
        public SpriteRenderer pointerSpr;
        public float seconds = 3f;
        public bool repeat = true;
        public AnimationCurve curve;
        private float _curTime;

        private bool _isMoving;
        // public Vector3 startPosition;
        // public Vector3 endPosition;

        private Vector3[] _positions;
        private float _totalLength;

        private void Awake(){
            Enabled = false;
        }

        public Vector3[] Positions{
            set{
                _positions = value;
                _totalLength = 0;
                for (var i = 0; i < value.Length - 1; i++) _totalLength += (value[i + 1] - value[i]).magnitude;
            }
            private get => _positions;
        }

        public bool Enabled{
            set{
                pointerSpr.enabled = value;
                enabled = value;
            }
            get => enabled;
        }

        private void Update(){
            if (!_isMoving) return;
            if (Positions.Length <= 1) return;
            _curTime += Time.deltaTime;
            _curTime = Math.Min(seconds, _curTime);
            UpdatePosition(curve.Evaluate(_curTime / seconds));
            if (Mathf.Abs(_curTime - seconds) <= Mathf.Epsilon)
                if (repeat)
                    _curTime = 0;
        }

        public void StartMoving(){
            _isMoving = true;
        }

        public void EndMoving(){
            _isMoving = false;
        }

        private void UpdatePosition(float i){
            var j = 0;
            for (; j < Positions.Length - 1; j++){
                var curMag = (Positions[j + 1] - Positions[j]).magnitude;
                var k = curMag / _totalLength;
                if (k > i || k.AlmostEquals(i)){
                    i *= _totalLength / curMag;
                    transform.position = Vector3.Lerp(Positions[j], Positions[j + 1], i);
                    return;
                }

                i -= k;
            }
        }
    }
}