using System;
using Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Balls{
    public class BallConfig: MonoBehaviour{
        public Model.Ball modelBall;
        private void Start(){
            var component = GetComponent<CircleCollider2D>();
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(modelBall.size / 2, modelBall.size/2, modelBall.size/2);
            var ball = GetComponent<Ball>();
            ball.velocity = Vector2.one.Rotate(Random.Range(0, 360)).normalized * modelBall.speed;
        }
    }
}