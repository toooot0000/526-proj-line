using System.Collections;
using Core.PlayArea.Balls;
using UnityEngine;

namespace Core.PlayArea.TouchTracking{
    public class CircleCollider: MonoBehaviour{

        private PolygonCollider2D _collider2D;

        private void Start(){
            _collider2D = GetComponent<PolygonCollider2D>();
        }


        private IEnumerator ScheduledDisable(){
            yield return new WaitForSeconds(0.1f);
            _collider2D.enabled = false;
        }

        public void SetCircle(Vector2[] circlePoints){
            _collider2D.points = circlePoints;
            _collider2D.enabled = true;
            StartCoroutine(ScheduledDisable());
        }
        
        private void OnTriggerStay2D(Collider2D col){
            var ball = col.GetComponent<Ball>();
            if (ball == null || ball.currentState != Ball.State.Free) return;
            ball.OnBeingCircled();
        }
        
    }
}