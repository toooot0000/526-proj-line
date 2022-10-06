using UnityEngine;
using Utility;

namespace UI{
    public class Shade : MonoBehaviour{
        private SpriteRenderer _renderer;

        private void Start(){
            _renderer = GetComponent<SpriteRenderer>();
            gameObject.SetActive(false);
        }

        public void SetActive(bool val){
            if (val){
                if (gameObject.activeSelf) return;
                gameObject.SetActive(true);
                var col = _renderer.color;
                StartCoroutine(
                    TweenUtility.Lerp(
                        0.2f,
                        () => _renderer.color = new Color(col.r, col.g, col.b, 0),
                        i => _renderer.color = new Color(col.r, col.g, col.b, 0.5f * i),
                        () => { }
                    )()
                );
            } else{
                if (!gameObject.activeSelf) return;
                var col = _renderer.color;
                StartCoroutine(
                    TweenUtility.Lerp(
                        0.2f,
                        () => _renderer.color = new Color(col.r, col.g, col.b, 0.5f),
                        i => _renderer.color = new Color(col.r, col.g, col.b, 0.5f * (1 - i)),
                        () => gameObject.SetActive(false)
                    )()
                );
            }
        }
    }
}