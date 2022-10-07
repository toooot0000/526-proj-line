using UnityEngine;
using Utility;

namespace Core.Common{
    internal readonly struct Vector3Wrapper : ITweenArithmetic<Vector3Wrapper>{
        public readonly Vector3 vec;

        public Vector3Wrapper(Vector3 otherVec){
            vec = otherVec;
        }

        public Vector3Wrapper Add(Vector3Wrapper other){
            return new(vec + other.vec);
        }

        public Vector3Wrapper Sub(Vector3Wrapper other){
            return new(vec - other.vec);
        }

        public Vector3Wrapper Mul(float other){
            return new(vec * other);
        }
    }


    [RequireComponent(typeof(LineRenderer))]
    public class ProgressBar : MonoBehaviour{
        public AnimationCurve tweenCurve;
        public float tweenMaxSpeed = 20f;
        private Coroutine _coroutine;
        private float _currentPercentage = 100f;

        private LineRenderer _line;

        private float _percentage = 100.0f;

        public float Percentage{
            get => _percentage;
            set => _percentage = Mathf.Clamp(value, .0f, 100f);
        }

        private void Start(){
            var rect = ((RectTransform)transform).rect;
            _line = GetComponent<LineRenderer>();
            _line.widthCurve = AnimationCurve.Constant(0, 1, rect.size.y);
            _line.positionCount = 2;
            _line.useWorldSpace = false;
            _line.SetPosition(0, PercentageToLocalPosition(0));
            _line.SetPosition(1, PercentageToLocalPosition(100));
        }

        private void Update(){
            var diff = _percentage - _currentPercentage;
            if (Mathf.Abs(diff) < Mathf.Epsilon) return;
            var speed = Mathf.Sign(diff) * tweenCurve.Evaluate(Mathf.Abs(diff) / 100) * tweenMaxSpeed;
            _currentPercentage += speed * Time.deltaTime;
            _currentPercentage = Mathf.Clamp(_currentPercentage, 0, 100f);
            _line.SetPosition(0, PercentageToLocalPosition(0));
            _line.SetPosition(1, PercentageToLocalPosition(_currentPercentage));
        }

        private Vector3 PercentageToLocalPosition(float percentage){
            var rect = ((RectTransform)transform).rect;
            return new Vector3(
                Mathf.Lerp(rect.position.x, rect.position.x + rect.size.x, percentage / 100.0f),
                rect.position.y + rect.size.y / 2f, transform.position.z);
        }
    }
}