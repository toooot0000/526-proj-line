﻿using Core.PlayArea.Balls;
using UnityEngine;

namespace Core.PlayArea{
    public class Wall: MonoBehaviour{
        public Vector2 velocityChangeRate = Vector2.one;


        private void OnTriggerEnter2D(Collider2D col){
            var ball = col.GetComponent<Ball>();
            if (ball == null) return;
            ball.OnHittingWall(this);
        }
    }
}