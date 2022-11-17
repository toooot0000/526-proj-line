using System.Collections;
using System.Collections.Generic;
using Core.PlayArea.Balls;
using UnityEngine;

namespace Core.PlayArea.TouchTracking{
    public class CircleCollider : MonoBehaviour{
        public ChargeDisplayer chargeDisplayer;
        private Vector2 _centeredLocalPosition;
        private PolygonCollider2D _collider2D;
        private readonly HashSet<ICircleableView> _seen = new();

        private void Start(){
            _collider2D = GetComponent<PolygonCollider2D>();
        }

        private void OnTriggerStay2D(Collider2D col){
            var circleable = col.GetComponent<ICircleableView>();
            if (circleable == null || _seen.Contains(circleable)) return;
            if (circleable is PlayableObjectViewBase{ IsInTutorial: true, tutorIsCircleable: false }) return;
            _seen.Add(circleable);
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
            _seen.Clear();
            StartCoroutine(ScheduledDisable());
        }
    }
}