using Core.PlayArea.Balls;
using UnityEngine;

namespace Core.PlayArea{
    public class Wall : MonoBehaviour{
        public Vector2 velocityChangeRate = Vector2.one;


        private void OnTriggerEnter2D(Collider2D col){
            var ball = col.GetComponent<IMovable>();
            if (ball == null) return;
            var velocity = ball.Velocity;
            ball.Velocity = new Vector2(velocity.x * velocityChangeRate.x, velocity.y * velocityChangeRate.y);
        }
    }
}