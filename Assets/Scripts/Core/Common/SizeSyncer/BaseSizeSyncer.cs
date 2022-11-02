using UnityEngine;

namespace Core.Common.SizeSyncer{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class BaseColliderSizeSyncer : MonoBehaviour{
        protected new Collider2D collider;
        protected RectTransform rect;

        private void Awake(){
            collider = GetCollider2D();
            rect = GetComponent<RectTransform>();
            SyncSize();
        }

        protected abstract Collider2D GetCollider2D();

        public abstract void SyncSize();
    }
}