using System.Collections;
using Core.PlayArea.Balls;
using Core.PlayArea.Mine;
using UnityEngine;

namespace Core.PlayArea.TouchTracking{
    public class CircleCollider : MonoBehaviour{
        public ChargeDisplayer chargeDisplayer;
        private Vector2 _centeredLocalPosition;

        private PolygonCollider2D _collider2D;

        private void Start(){
            _collider2D = GetComponent<PolygonCollider2D>();
        }

        private void OnTriggerStay2D(Collider2D col){
            // var ball = col.GetComponent<BallView>();
            // if (ball != null) ball.OnCircled();
            // var mine = col.GetComponent<MineView>();
            // if(mine!= null) mine.OnBeingCircled();
            var circleable = col.GetComponent<ICircleable>();
            circleable.OnCircled();
        }


        private IEnumerator ScheduledDisable(){
            yield return new WaitForSeconds(0.1f);
            _collider2D.enabled = false;
            var position = transform.TransformPoint(_centeredLocalPosition);
            position.z = -0.5f;
            chargeDisplayer.Show(GameManager.shared.game.player.circledBalls.Count, position);
        }

        public void SetCircle(Vector2[] circlePoints){
            _collider2D.points = circlePoints;
            _collider2D.enabled = true;
            _centeredLocalPosition = Vector2.zero;
            foreach (var vec in circlePoints) _centeredLocalPosition += vec;
            _centeredLocalPosition /= circlePoints.Length;
            StartCoroutine(ScheduledDisable());
        }
    }
}