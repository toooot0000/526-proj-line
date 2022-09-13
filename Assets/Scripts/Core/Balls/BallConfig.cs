using System;
using UnityEngine;

namespace Core.Balls{
    public class BallConfig: MonoBehaviour{
        public Model.Ball ball;

        private void Start(){
            var component = GetComponent<CircleCollider2D>();
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(ball.size / 2, ball.size/2, ball.size/2);
        }
    }
}