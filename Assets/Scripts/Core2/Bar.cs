using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Core2{
    public class Bar: MonoBehaviour{

        public float maxSpeed = 10f;
        public AnimationCurve speedCurve;
        private Vector3 _originPosition;
        private bool _isPressed = false;
        
        public float maxDistance = 10;

        private RectTransform _transform;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _prevVelocity;

        public TouchTrigger trigger;

        private void Start(){
            _transform = GetComponent<RectTransform>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.freezeRotation = true;
            trigger.PointerDown += _ => OnPointerDown();
            trigger.PointerUp += _ => OnPointerUp();
            _prevVelocity = Vector2.zero;
        }

        private void FixedUpdate(){
            if (!_isPressed){
                _prevVelocity = _rigidbody2D.velocity;
                return;
            }
            var dis = Mathf.Abs(Input.mousePosition.x - _originPosition.x);
            var sign = Mathf.Sign(Input.mousePosition.x - _originPosition.x);
            var speed = sign * maxSpeed * speedCurve.Evaluate(dis / maxDistance);
            _prevVelocity = new Vector2(speed, 0);
            _rigidbody2D.velocity = _prevVelocity;
        }

        private void OnPointerDown(){
            _isPressed = true;
            _originPosition = Input.mousePosition;
        }

        private void OnPointerUp(){
            _isPressed = false;
        }

        private void OnCollisionEnter2D(Collision2D col){
            if (col.collider.CompareTag("Ball")) return;
            if (col.collider.gameObject.layer == LayerMask.NameToLayer("Wall")){
                _prevVelocity = new Vector2(-_prevVelocity.x, 0);
                _rigidbody2D.velocity = _prevVelocity;
            }
        }
    }
}