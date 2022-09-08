using UnityEngine;

namespace Core.SizeSyncer{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(RectTransform))]
    public class SpriteSizeSyncer : MonoBehaviour{
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
