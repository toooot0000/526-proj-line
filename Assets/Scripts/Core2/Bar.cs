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

        public TouchTrigger trigger;

        private void Start(){
            _transform = GetComponent<RectTransform>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.freezeRotation = true;
            trigger.PointerDown += _ => OnPointerDown();
            trigger.PointerUp += _ => OnPointerUp();
        }

        private void FixedUpdate(){
            if (!_isPressed){
                return;
            }
            var dis = Mathf.Abs(Input.mousePosition.x - _originPosition.x);
            var sign = Mathf.Sign(Input.mousePosition.x - _originPosition.x);
            var speed = sign * maxSpeed * speedCurve.Evaluate(dis / maxDistance);
            _rigidbody2D.velocity = new Vector2(speed, 0);
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
                _rigidbody2D.velocity = new Vector2(-_rigidbody2D.velocity.x, 0);
            }
        }
    }
}