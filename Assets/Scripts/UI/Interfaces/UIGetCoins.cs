using TMPro;
using UnityEngine;
using Utility;

namespace UI.Interfaces{
    public class UIGetCoins : UIBase{
        public TextMeshProUGUI mesh;
        private CanvasGroup _canvasGroup;

        private int _coinNum;
        private bool _inAnimation;

        private void Start(){
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        public override void Open(object nextStageChoice){
            base.Open(nextStageChoice);
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 0,
                i => _canvasGroup.alpha = i,
                () => _inAnimation = false
            );
            StartCoroutine(coroutine());
            _coinNum = (int)nextStageChoice;
            UpdateContent();
        }

        public override void Close(){
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 1,
                i => _canvasGroup.alpha = 1 - i,
                () => {
                    _inAnimation = false;
                    base.Close();
                    GameManager.shared.Delayed(0.1f, ()=>UIManager.shared.OpenUI("UISelectStage", GameManager.shared.game.currentStage.nextStageChoice));
                    Destroy(gameObject);
                });
            StartCoroutine(coroutine());
        }

        public void ConfirmButtonClicked(){
            GameManager.shared.game.player.Coin += _coinNum;
            Close();
        }

        private void UpdateContent(){
            mesh.text = $"x {_coinNum.ToString()}";
        }
    }
}