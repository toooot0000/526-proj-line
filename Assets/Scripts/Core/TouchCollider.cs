using System;
using UnityEngine;

namespace Core{
    public class TouchCollider: MonoBehaviour{
        private CircleCollider2D _collider2D;

        public void SetEnabled(bool value){
            enabled = value;
            _collider2D.enabled = value;
        }

        private void Start(){
            _collider2D = GetComponent<CircleCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D col){
            var ball = col.GetComponent<Ball>();
            if (ball == null || ball.currentState != Ball.State.Free) return;
            ball.OnBeingTouched();
        }

    }
}