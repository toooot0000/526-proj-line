using Model;
using Tutorials;
using UI.Container;
using UnityEngine;
using Utility;

namespace UI.GearDisplayer{
    public class GearDisplayer : UIComponent{
        public UIContainerBase container;
        public GameObject gearItemPrefab;
        
        
        private Coroutine _coroutine;
        private Vector2 _prevAnchoredPosition;
        private bool _isHidden = false;

        private void Start(){
            GameManager.shared.game.player.OnGearChanged += UpdateGears;
            UIManager.shared.RegisterComponent(this);
            UpdateGears(GameManager.shared.game, GameManager.shared.game.player);
        }

        public override void HandOverControlTo(TutorialBase tutorial){ }

        public override void GainBackControlFrom(TutorialBase tutorial){ }

        public override void Hide(){
            if (_isHidden) return;
            _isHidden = true;
            var rectTrans = (RectTransform)transform;
            _prevAnchoredPosition = rectTrans.anchoredPosition;
            var target = _prevAnchoredPosition + new Vector2(-rectTrans.rect.width, 0);
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

        private void UpdateGears(Game game, GameModel player){
            var gears = (player as Player)!.CurrentGears;
            var i = 0;
            for (; i < gears.Length; i++){
                GearItem item;
                if (i >= container.children.Count){
                    item = Instantiate(gearItemPrefab, transform).GetComponent<GearItem>();
                    container.AddChild(item);
                } else{
                    item = (container.children[i] as GearItem)!;
                    item.gameObject.SetActive(true);
                    container.AttachChild(i);
                }

                item.Model = gears[i];
            }

            while (i < container.transform.childCount){
                container.UntachChild(i);
                container.children[i].gameObject.SetActive(false);
            }

            container.UpdateLayout();
        }
    }
}