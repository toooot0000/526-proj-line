using Core.Common.SizeSyncer;
using UnityEngine;

namespace Core.PlayArea.Blocks{
    public class BlockView: PlayableObjectViewBase{
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

        private Model.Mechanics.PlayableObjects.Block _model;
        public Model.Mechanics.PlayableObjects.Block Model{
            get => _model;
            set{
                _model = value;
                _model.gameObject = gameObject;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision){
            var otherCollider = collision.collider;
            var ball = otherCollider.GetComponent<IMovableView>();
            if (ball == null) return;
            var contact = collision.GetContact(0);
            var normal = contact.normal;
            var newVelocity = Vector2.Reflect(ball.Velocity, normal);
            ball.Velocity = newVelocity;
        }
    }
}