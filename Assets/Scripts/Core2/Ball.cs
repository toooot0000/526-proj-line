using System;
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

        private void Update(){}


        private void OnCollisionEnter2D(Collision2D col){
            var contact = col.GetContact(0);
            var p = (Vector2)_transform.position - contact.point;
            p.Normalize();
            var proj = (Vector2)Vector3.Project(velocity, p);
            velocity -= proj * 2;
            velocity = velocity.normalized * _velocityMag;
            _rigidbody2D.velocity = velocity;
        }
    }
}