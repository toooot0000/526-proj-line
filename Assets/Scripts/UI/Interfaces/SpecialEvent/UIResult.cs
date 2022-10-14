using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;

namespace UI.Interfaces.SpecialEvent
{
    public class UIResult: UIBase
    {
        private CanvasGroup _canvasGroup;
        private bool _inAnimation;
        public Image shade;
        public TextMeshProUGUI text;
        
        
        void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            //text.text = "You have chosen an answer.";

        }
        
        public override void Open(object nextStageChoice) {
            base.Open(nextStageChoice);
            text.text = nextStageChoice.ToString();
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 0,
                i => _canvasGroup.alpha = i,
                () => _inAnimation = false
            );
            StartCoroutine(coroutine());
        }

        public override void Close() {
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
        
        public void TapToContinue() {
            Close();
        }
        
    }
}