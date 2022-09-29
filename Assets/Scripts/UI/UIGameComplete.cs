using UnityEngine;
using Utility;

namespace UI{
    public class UIGameComplete: UIBase{
        
        private bool _inAnimation = false;
        private CanvasGroup _canvasGroup;
        private void Start() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 1;
        }
        public override void Open(){
            base.Open();
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f, 
                () => _canvasGroup.alpha = 0, 
                i => _canvasGroup.alpha = i, 
                () => _inAnimation = false
            );
            StartCoroutine(coroutine());
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

        public void OnBtnClicked(){
            GameManager.shared.game.Restart();
            Close();
        }
    }
}