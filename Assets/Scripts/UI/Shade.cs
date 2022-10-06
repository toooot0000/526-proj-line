using UnityEngine;
using Utility;

namespace UI{
    public class Shade : MonoBehaviour{
        private SpriteRenderer _renderer;
        private Coroutine _coroutine;
        public float maxShade = 0.5f;
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
                if(_coroutine!=null) StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(
                    TweenUtility.Lerp(
                        transitionTime,
                        () => _renderer.color = new Color(shadeColor.r, shadeColor.g, shadeColor.b, 0),
                        i => _renderer.color = new Color(shadeColor.r, shadeColor.g, shadeColor.b, maxShade * i),
                        () => { }
                    )()
                );
            } else{
                if (!gameObject.activeSelf) return;
                if(_coroutine!=null) StopCoroutine(_coroutine);
                StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(
                    TweenUtility.Lerp(
                        transitionTime,
                        () => _renderer.color = new Color(shadeColor.r, shadeColor.g, shadeColor.b, maxShade),
                        i => _renderer.color = new Color(shadeColor.r, shadeColor.g, shadeColor.b, maxShade * (1 - i)),
                        () => gameObject.SetActive(false)
                    )()
                );
            }
        }
    }
}