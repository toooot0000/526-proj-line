using UnityEngine;

namespace Core{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxCollider2DSyncSizeWithRectTransform : MonoBehaviour{
        private BoxCollider2D _collider;

        private RectTransform _rectTransform;

        void Start(){
            _collider = GetComponent<BoxCollider2D>();
            _rectTransform = GetComponent<RectTransform>();
            SyncSize();
        }

        public void SyncSize(){
            _collider.size = _rectTransform.rect.size;
            _collider.offset = Vector2.zero;
        }
    }
}
