using System;
using System.Collections;
using UnityEngine;
using Utility;

namespace Core{
    readonly struct Vector3Wrapper : ITweenArithmetic<Vector3Wrapper>{
        public readonly Vector3 vec;

        public Vector3Wrapper(Vector3 otherVec){
            vec = otherVec;
        }
        public Vector3Wrapper Add(Vector3Wrapper other) => new Vector3Wrapper(vec + other.vec);
        
        public Vector3Wrapper Sub(Vector3Wrapper other) => new Vector3Wrapper(vec - other.vec);

        public Vector3Wrapper Mul(float other) => new Vector3Wrapper(vec * other);
    }


    [RequireComponent(typeof(LineRenderer))]
    public class LengthBar: MonoBehaviour{

        public AnimationCurve tweenCurve;
        public float tweenTime = 0.2f;

        private float _percentage = 100.0f;

        public float Percentage{
            get => _percentage;
            set{
                var newPer = Mathf.Clamp(value, .0f, 100f);
                UpdateLine(newPer);
                _percentage = newPer;
            }
        }

        private LineRenderer _line;
        private Coroutine _coroutine;

        private void Start(){
            var rect = ((RectTransform)transform).rect;
            _line = GetComponent<LineRenderer>();
            _line.widthCurve = AnimationCurve.Constant(0, 1, rect.size.y);
            _line.positionCount = 2;
            _line.useWorldSpace = false;
            _line.SetPosition(0, PercentageToLocalPosition(0));
            _line.SetPosition(1, PercentageToLocalPosition(100));
        }

        private Vector3 PercentageToLocalPosition(float percentage){
            var rect = ((RectTransform)transform).rect;
            return new Vector3(
                Mathf.Lerp(rect.position.x, rect.position.x + rect.size.x, percentage / 100.0f),
                rect.position.y + rect.size.y/2f, transform.position.z);
        }

        private void UpdateLine(float newPer){
            if (_coroutine != null){
                StopCoroutine(_coroutine);   
            }

            var start = new Vector3Wrapper(PercentageToLocalPosition(_percentage));
            var end = new Vector3Wrapper(PercentageToLocalPosition(newPer));
            var tween = new Tween<Vector3Wrapper>(start, end, tweenTime, tweenCurve);
            var enumerator = TweenUtility.MakeEnumerator(tween, wrapper => {
                _line.SetPosition(1, wrapper.vec);
            });
            _coroutine = StartCoroutine(enumerator);
        }

    }
}