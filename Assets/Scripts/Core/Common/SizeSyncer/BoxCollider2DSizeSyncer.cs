using UnityEngine;

namespace Core.Common.SizeSyncer{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxCollider2DSizeSyncer : BaseColliderSizeSyncer{
        protected override Collider2D GetCollider2D(){
            return GetComponent<BoxCollider2D>();
        }

        public override void SyncSize(){
            ((BoxCollider2D)collider).size = rect.rect.size;
            collider.offset = Vector2.zero;
        }
    }


    
}
