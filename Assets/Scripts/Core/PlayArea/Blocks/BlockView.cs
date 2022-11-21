using Core.Common.SizeSyncer;
using Model.Mechanics.PlayableObjects;
using TMPro;
using UnityEngine;

namespace Core.PlayArea.Blocks{
    public class BlockView: PlayableObjectViewWithModel<Block>, IOnPlayerTurnEnd{
        public SpriteSizeSyncer sizeSyncer;
        public BoxCollider2DSizeSyncer collider2DSizeSyncer;
        public TextMeshProUGUI turnNum;

        public Rect Rect{
            set{
                var trans = (RectTransform)transform;
                trans.sizeDelta = value.size;
                trans.anchoredPosition = value.position;
                sizeSyncer.SyncSize();
                collider2DSizeSyncer.SyncSize();
            }
        }

        private Block _model;
        public override Block Model{
            get => _model;
            set{
                _model = value;
                turnNum.text =  $"{Model.remainingTurn.ToString()}";
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

        public void OnPlayerTurnEnd(){
            Model.remainingTurn--;
            turnNum.text = $"{Model.remainingTurn.ToString()}";
            if (Model.remainingTurn == 0){
                gameObject.SetActive(false);
            }
        }
    }
}