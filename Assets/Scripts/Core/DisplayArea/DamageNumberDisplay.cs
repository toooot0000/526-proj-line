using System;
using TMPro;
using UnityEngine;

namespace Core.DisplayArea{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DamageNumberDisplay: MonoBehaviour{

        private TextMeshProUGUI _textMesh;
        public int Number{
            set{
                _textMesh.text = $"-{value.ToString()}";
                StartFloating();
            }
        }

        private Vector2 _velocity = Vector2.up;

        public float maxSpeed = 5f;

        private float _curSpeed = 0;
        private float _curTime = 0;

        public float totalTime = 0.5f;

        public AnimationCurve curve;

        private void Start(){
            _textMesh = GetComponent<TextMeshProUGUI>();
        }

        private void Update(){
            if (_curTime >= totalTime || _curSpeed < Mathf.Epsilon) return;
            _curTime += Time.deltaTime;
            var k = _curTime / totalTime;
            _curSpeed = maxSpeed * curve.Evaluate(k);
            _textMesh.alpha = 1 - k;
            (transform as RectTransform)!.anchoredPosition += _velocity * (_curSpeed * Time.deltaTime);
        }

        private void StartFloating(){
            (transform as RectTransform)!.anchoredPosition = Vector2.zero;
            _curSpeed = maxSpeed;
            _curTime = 0;
            _textMesh.alpha = 1;
        }
        
    }
}