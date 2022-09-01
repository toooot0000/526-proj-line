using UnityEngine;

namespace Core{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(RectTransform))]
    public class SpriteSyncSizeWithRectTransform : MonoBehaviour{
        private SpriteRenderer _spriteRenderer;

        private RectTransform _rectTransform;

        void Start(){
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rectTransform = GetComponent<RectTransform>();
            SyncSize();
        }

        public void SyncSize(){
            _spriteRenderer.size = _rectTransform.rect.size;
        }
    }
}
