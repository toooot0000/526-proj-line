using BackendApi;
using Model;
using UI.Container;
using UnityEngine;
using Utility;

namespace UI.Interfaces.ShopSystem {
    public class UIShopSystem : UIBase {
        public UIContainerFlexBox container;
        public Gear[] items;
        private CanvasGroup _canvasGroup;
        private bool _inAnimation;
        private UIShopPanel[] _panels;
        // private UIShopPanel _selected;

        private void Start() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _panels = transform.GetComponentsInChildren<UIShopPanel>();
        }

        public override void Open(object gears) {
            base.Open(gears);
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 0,
                i => _canvasGroup.alpha = i,
                () => _inAnimation = false
            );
            items = gears as Gear[];
            UpdateGearPanel();
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
        } // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateGearPanel() {
            var curPanelInd = 0;
            if (items.Length > 6) {
                Debug.LogError("Gears in Shop more than 6!");
                return;
            }

            foreach (var gear in items) {
                var gearPanel = _panels[curPanelInd];
                gearPanel.OnClick += PurchaseSelectedGear;
                gearPanel.Show = true;
                gearPanel.Model = gear;
                curPanelInd++;
                if (gear.rarity > GameManager.shared.game.player.Coin) gearPanel.highLight.enabled = false;
            } // change gear.rarity to gear.price later

            for (; curPanelInd < _panels.Length; curPanelInd++) _panels[curPanelInd].Show = false;
            container.UpdateLayout();
        }

        public void ConfirmButtonEvent() {
            // StartCoroutine(CoroutineUtility.Delayed(GameManager.shared.game.GoToNextStage));
            Close();
        }
        

        private void PurchaseSelectedGear(UIShopPanel clickedPanel)
        {
            if (GameManager.shared.game.player.Coin < clickedPanel.Model.rarity) return; // change gear.rarity to gear.price later
            GameManager.shared.game.player.Coin -= clickedPanel.Model.rarity; // change gear.rarity to gear.price later
            GameManager.shared.game.player.AddGear(clickedPanel.Model);
            UpdateGearPanel();
        }
    }
}