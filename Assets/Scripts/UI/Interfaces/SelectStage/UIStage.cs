using UnityEngine;
using Utility;

namespace UI.Interfaces.SelectStage
{
    public class UIStage : UIBase
    {
        private CanvasGroup _canvasGroup;

        private bool _inAnimation;

        private void Start(){
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
                    GameManager.shared.GameStart();
                });
            StartCoroutine(coroutine()); 
        }
        
        public void GotoStage(int stageId)
        {
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 1,
                i => _canvasGroup.alpha = 1 - i,
                () => {
                    _inAnimation = false;
                    base.Close();
                    Destroy(gameObject);
                    GameManager.shared.GotoStage(stageId);
                });
            StartCoroutine(coroutine()); 
        }
        
    }

}
