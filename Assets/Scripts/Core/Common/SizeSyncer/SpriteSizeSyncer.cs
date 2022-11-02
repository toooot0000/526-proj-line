using UnityEngine;

namespace Core.Common.SizeSyncer{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(RectTransform))]
    public class SpriteSizeSyncer : MonoBehaviour{
        private RectTransform _rectTransform;
        private SpriteRenderer _spriteRenderer;

        private void Awake(){
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rectTransform = GetComponent<RectTransform>();
            SyncSize();
        }

        public void SyncSize(){
            _spriteRenderer.size = _rectTransform.rect.size;
        }
    }
}