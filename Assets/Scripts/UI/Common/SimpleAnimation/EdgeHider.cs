using System;
using UnityEngine;
using Utility.Extensions;

namespace UI.Common.SimpleAnimation{

    public enum EdgeHiderDirection{
        Up,
        Left,
        Right,
        Bottom
    }

    [RequireComponent(typeof(RectTransform))]
    public class EdgeHider: MonoBehaviour{
        public bool isMoving = false;
        public AnimationCurve curve = AnimationCurve.Constant(0, 1, 1);
        public float transitionTime = 0.2f;
        public EdgeHiderDirection direction = EdgeHiderDirection.Up;
        public float padding = 10;

        private RectTransform _rect;
        private float _curTime = 0;
        private bool _isToShow = false;
        private Vector2 _oriPosition;

        private void Awake(){
            _rect = GetComponent<RectTransform>();
            _oriPosition = _rect.anchoredPosition;
        }

        private void Update(){
            if (!isMoving) return;
            var rect = _rect.rect;
            var hidePosition = direction switch{          
                EdgeHiderDirection.Up => _oriPosition +  new Vector2(0, rect.height + padding),
                EdgeHiderDirection.Left => _oriPosition + new Vector2(- rect.width - padding, 0),
                EdgeHiderDirection.Right => _oriPosition + new Vector2(rect.width + padding, 0),
                EdgeHiderDirection.Bottom => _oriPosition + new Vector2(0, - rect.height - padding),
                _ => throw new ArgumentOutOfRangeException()
            };
            _rect.anchoredPosition = Vector2.Lerp(hidePosition, _oriPosition,  _isToShow ? (_curTime / transitionTime) : (1 - _curTime / transitionTime));
            _curTime = Mathf.Clamp(_curTime + Time.deltaTime, 0, transitionTime);
            if (_curTime.AlmostEquals(1)) isMoving = false;
        }

        public void Hide(){
            _isToShow = false;
            isMoving = true;
        }

        public void Show(){
            _isToShow = true;
            isMoving = true;
        }
    }
}