using Model;
using UI.Common;
using UnityEngine;
using Utility;

namespace UI.StatusBar{
    public class CoinDisplayer: UIComponent{

        private bool _isHidden = false;
        private Coroutine _coroutine = null;
        private Vector2 _prevAnchoredPosition;
        
        
        public CoinWithNumber coin;

        private void Start(){
            UIManager.shared.RegisterComponent(this);
            GameManager.shared.game.player.OnCoinChanged += (game, model) => coin.Number = ((Player)model).Coin;
            coin.Number = GameManager.shared.game.player.Coin;
        }
        
        public override void Hide(){
            if (_isHidden) return;
            _isHidden = true;

            var rectTrans = (RectTransform)transform;
            _prevAnchoredPosition = rectTrans.anchoredPosition;
            var target = _prevAnchoredPosition + new Vector2(0, rectTrans.rect.height);
            if(_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(TweenUtility.Move(UIManager.UITransitionTime, rectTrans, _prevAnchoredPosition, target));
        }

        public override void Show(){
            if (!_isHidden) return;
            _isHidden = false;
            var rectTrans = (RectTransform)transform;
            if(_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(TweenUtility.Move(UIManager.UITransitionTime, rectTrans, rectTrans.anchoredPosition, _prevAnchoredPosition));
        }
    }
}