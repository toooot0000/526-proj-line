using UnityEngine;
using Utility;

namespace UI{
    public class Shade : MonoBehaviour{
        private SpriteRenderer _renderer;
        private Coroutine _coroutine;
        public float transitionTime = 0.2f;
        public Color shadeColor = Color.black;


        private void Start(){
            _renderer = GetComponent<SpriteRenderer>();
            gameObject.SetActive(false);
        }

        public void SetActive(bool val){
            if (val){
                if (gameObject.activeSelf) return;
                gameObject.SetActive(true);
                if(_coroutine != null) StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(
                    TweenUtility.Lerp(
                        transitionTime,
                        () => _renderer.color = new Color(shadeColor.r, shadeColor.g, shadeColor.b, 0),
                        i => _renderer.color = new Color(shadeColor.r, shadeColor.g, shadeColor.b, shadeColor.a * i),
                        () => { _renderer.color = shadeColor; }
                    )()
                );
            } else{
                if (!gameObject.activeSelf) return;
                if(_coroutine != null) StopCoroutine(_coroutine);
                _coroutine =StartCoroutine(
                    TweenUtility.Lerp(
                        transitionTime,
                        () => _renderer.color = new Color(shadeColor.r, shadeColor.g, shadeColor.b, shadeColor.a),
                        i => _renderer.color = new Color(shadeColor.r, shadeColor.g, shadeColor.b, shadeColor.a * (1 - i)),
                        () => gameObject.SetActive(false)
                    )()
                );
            }
        }
    }
}