using UnityEngine;

namespace UI.Common{
    public class Shade : MonoBehaviour{
        public SpriteRenderer image;
        private Coroutine _coroutine;
        public float transitionTime = 0.2f;
        private bool _isToShow = false;
        private Color _shadeColor;
        
        private void Awake(){
            _shadeColor = image.color;
            image.color = new Color(_shadeColor.r, _shadeColor.g, _shadeColor.b, 0);
        }

        public void Update(){
            var spd = _shadeColor.a / transitionTime * (_isToShow ? 1 : -1);
            var curAlpha = image.color.a;
            image.color = new Color(_shadeColor.r, _shadeColor.g, _shadeColor.b, Mathf.Clamp(curAlpha + spd * Time.deltaTime, 0, _shadeColor.a));
        }
        
        public void SetActive(bool val){
            _isToShow = val;
        }
    }
}