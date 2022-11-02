using Core.Common.SizeSyncer;
using Core.PlayArea.Balls;
using UnityEngine;

namespace Core.PlayArea.Block{
    public class Block: MonoBehaviour{
        public RectInt rectInt;
        public SpriteSizeSyncer sizeSyncer;
        public BoxCollider2DSizeSyncer collider2DSizeSyncer;

        public Rect Rect{
            set{
                var trans = (RectTransform)transform;
                trans.sizeDelta = value.size;
                trans.anchoredPosition = value.position;
                sizeSyncer.SyncSize();
                collider2DSizeSyncer.SyncSize();
            }
        }
        
        private void OnCollisionEnter2D(Collision2D collision){
            var otherCollider = collision.collider;
            var ball = otherCollider.GetComponent<BallView>();
            if (ball == null) return;
            var contact = collision.GetContact(0);
            var normal = contact.normal;
            var newVelocity = Vector2.Reflect(ball.velocity, normal);
            ball.velocity = newVelocity;
        }
    }
}