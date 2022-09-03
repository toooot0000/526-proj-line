using System;
using UnityEngine;

namespace Core{
    public class TouchCollider: MonoBehaviour{
        private void OnTriggerEnter2D(Collider2D col){
            var ball = col.GetComponent<Ball>();
            if (ball == null || ball.currentState != Ball.State.Free) return;
            ball.OnBeingTouched();
        }
    }
}