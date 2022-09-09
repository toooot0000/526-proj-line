using System;
using Extensions;
using UnityEngine;

namespace Core2{
    public class Ball: MonoBehaviour{

        public Vector2 velocity;
        private Rigidbody2D _rigidbody2D;
        private RectTransform _transform;
        private float _velocityMag;

        private void Start(){
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.velocity = velocity;
            _transform = GetComponent<RectTransform>();
            _velocityMag = velocity.magnitude;
        }

        private void FixedUpdate(){
            velocity = _rigidbody2D.velocity;
        }


        private void OnCollisionEnter2D(Collision2D col){
            var contact = col.GetContact(col.contactCount - 1);
            var normal = ((Vector2)_transform.position - contact.point).Align().normalized;
            var surface = normal.Rotate(90);
            var proj = (Vector2)Vector3.Project(velocity, surface);
            velocity = (proj * 2 - velocity).normalized * _velocityMag;
            _rigidbody2D.velocity = velocity;
        }
    }
}