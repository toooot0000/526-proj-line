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

        public override void Open(object coins){
            base.Open(coins);
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 0,
                i => _canvasGroup.alpha = i,
                () => _inAnimation = false
            );
            StartCoroutine(coroutine());
            _coinNum = (int)coins;
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
                    Destroy(gameObject);
                });
            StartCoroutine(coroutine());
        }

        public void ConfirmButtonClicked(){
            StartCoroutine(CoroutineUtility.Delayed(GameManager.shared.GoToNextStage));
            GameManager.shared.game.player.Coin += _coinNum;
            Close();
        }

        private void UpdateContent(){
            mesh.text = $"x {_coinNum.ToString()}";
        }
    }
}