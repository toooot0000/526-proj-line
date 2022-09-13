using System;
using Core.Balls;
using Unity.VisualScripting;
using UnityEngine;

namespace Core{
    public class Wall: MonoBehaviour{
        public Vector2 velocityChangeRate = Vector2.one;


        private void OnTriggerEnter2D(Collider2D col){
            var ball = col.GetComponent<Ball>();
            if (ball == null) return;
            ball.OnHittingWall(this);
        }
    }
}