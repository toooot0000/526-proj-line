using UnityEngine;

namespace Core.Common.SizeSyncer{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class CapsuleCollider2DSizeSyncer : BaseColliderSizeSyncer{
        protected override Collider2D GetCollider2D(){
            return GetComponent<CapsuleCollider2D>();
        }

        public override void SyncSize(){
            ((CapsuleCollider2D)collider).size = rect.rect.size;
            collider.offset = Vector2.zero;
        }
    }
}