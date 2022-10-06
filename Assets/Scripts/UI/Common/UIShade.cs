using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Common{
    public class UIShade: MonoBehaviour{
        public Image image;
        private Coroutine _coroutine;
        public float maxShade = 0.5f;
        public float transitionTime = 0.2f;
        public Color shadeColor = Color.black;

        private void Start(){
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
                        () => image.color = new Color(shadeColor.r, shadeColor.g, shadeColor.b, 0),
                        i => image.color = new Color(shadeColor.r, shadeColor.g, shadeColor.b, maxShade * i),
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
                        () => image.color = new Color(shadeColor.r, shadeColor.g, shadeColor.b, maxShade),
                        i => image.color = new Color(shadeColor.r, shadeColor.g, shadeColor.b, maxShade * (1 - i)),
                        () => gameObject.SetActive(false)
                    )()
                );
            }
        }
    }
}