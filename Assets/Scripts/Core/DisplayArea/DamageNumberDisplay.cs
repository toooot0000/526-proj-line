using TMPro;
using UnityEngine;

namespace Core.DisplayArea{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DamageNumberDisplay : MonoBehaviour{
        public float maxSpeed = 5f;

        public float totalTime = 0.5f;

        public AnimationCurve curve;

        private readonly Vector2 _velocity = Vector2.up;

        private float _curSpeed;
        private float _curTime;

        private TextMeshProUGUI _textMesh;

        public int Number{
            set{
                _textMesh.text = $"-{value.ToString()}";
                StartFloating();
            }
        }

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