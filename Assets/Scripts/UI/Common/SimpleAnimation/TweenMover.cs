using System;
using UnityEngine;

namespace UI.Common.SimpleAnimation{

    [RequireComponent(typeof(RectTransform))]
    public class TweenMover: MonoBehaviour{
        public Vector2 TargetAnchoredPosition{
            set{
                _target = value;
                _direction = (value - _rect.anchoredPosition).normalized;
            }    
        }

        private Vector2 _direction;
        private Vector2 _target;
        
        public bool isMoving = false;
        public AnimationCurve curve = AnimationCurve.Constant(0, 1, 1);
        public float maxSpeed = 5;

        private RectTransform _rect;

        private void Awake(){
            _rect = GetComponent<RectTransform>();
        }

        private void Update(){
            if (!isMoving) return;
            var dis = (_rect.anchoredPosition - _target).magnitude;
            if (dis < double.Epsilon){
                isMoving = false;
                return;
            }
            var spd = Mathf.Lerp(maxSpeed, 0, curve.Evaluate(Mathf.Min(dis / 1000, 1)));
            _rect.anchoredPosition += spd * Time.deltaTime * _direction;
        }
    }
}