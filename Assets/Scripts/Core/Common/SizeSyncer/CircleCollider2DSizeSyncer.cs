using UnityEngine;

namespace Core.Common.SizeSyncer{
    public class CircleCollider2DSizeSyncer: BaseColliderSizeSyncer{
        protected override Collider2D GetCollider2D(){
            return GetComponent<CircleCollider2D>();
        }

        public override void SyncSize(){
            ((CircleCollider2D)collider).radius = rect.rect.height / 2;
            collider.offset = Vector2.zero;
        }
    }
}