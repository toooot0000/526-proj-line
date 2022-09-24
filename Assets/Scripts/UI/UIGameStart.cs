using System;
using UnityEngine;
using Utility;

namespace UI {
    public class UIGameStart: UIBase {
        
        private bool _inAnimation = false;
        private CanvasGroup _canvasGroup;
        private void Start() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 1;
        }

        public override void Close(){
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f, 
                () => _canvasGroup.alpha = 1, 
                i => _canvasGroup.alpha = 1 - i, 
                () => {
                    _inAnimation = false;
                    base.Close();
                    Destroy(gameObject);
                });
            StartCoroutine(coroutine());
        }
    }
}