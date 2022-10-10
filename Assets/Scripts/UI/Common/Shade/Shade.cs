using UnityEngine;
using Utility.Extensions;

namespace UI.Common.Shade{
    public class Shade : MonoBehaviour{
        public SpriteRenderer image;
        private Coroutine _coroutine;
        public float transitionTime = 0.2f;
        public Collider2D collider2D;
        private bool _isToShow = false;
        private Color _shadeColor;
        
        private void Awake(){
            _shadeColor = image.color;
            image.color = new Color(_shadeColor.r, _shadeColor.g, _shadeColor.b, 0);
            collider2D.enabled = false;
        }

        public void Update(){
            var spd = _shadeColor.a / transitionTime * (_isToShow ? 1 : -1);
            var curAlpha = image.color.a;
            image.color = new Color(_shadeColor.r, _shadeColor.g, _shadeColor.b, Mathf.Clamp(curAlpha + spd * Time.deltaTime, 0, _shadeColor.a));
            if (image.color.a.AlmostEquals(0) && !_isToShow){
                collider2D.enabled = false;
            }
        }
        
        public void SetActive(bool val){
            _isToShow = val;
            if (val){
                collider2D.enabled = true;
            }
        }
    }
}