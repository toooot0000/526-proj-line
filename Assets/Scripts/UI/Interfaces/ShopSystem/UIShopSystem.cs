using System.Collections.Generic;
using System.Linq;
using BackendApi;
using Model;
using UI.Container;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace UI.Interfaces.ShopSystem {
    public class UIShopSystem : UIBase {
        public Gear[] items; // only contains gears which the player haven't got
        private CanvasGroup _canvasGroup;
        private bool _inAnimation;
        private UIShopPanel[] _panels;
        private List<int> soldItems;

        private void Start() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _panels = transform.GetComponentsInChildren<UIShopPanel>();
            soldItems = new List<int>();
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

            foreach (var gear in items)
            {
                var gearPanel = _panels[curPanelInd];
                gearPanel.Show = true;
                gearPanel.Model = gear;
                if (gear.id <= GameManager.shared.game.player.Coin) gearPanel.OnClick += PurchaseSelectedGear;
                else gearPanel.coinWithNumber.color = Color.red;
                if (soldItems.Contains(gear.id)) gearPanel.soldOut.enabled = true;
                curPanelInd++;
            }
            for (; curPanelInd < _panels.Length; curPanelInd++) _panels[curPanelInd].Show = false;
        }

        public void ConfirmButtonEvent() {
            // StartCoroutine(CoroutineUtility.Delayed(GameManager.shared.game.GoToNextStage));
            Close();
        }
        

        private void PurchaseSelectedGear(UIShopPanel clickedPanel)
        {
            if (GameManager.shared.game.player.Coin < clickedPanel.Model.id) return; // change gear.id to gear.price later
            GameManager.shared.game.player.Coin -= clickedPanel.Model.id; // change gear.id to gear.price later
            GameManager.shared.game.player.AddGear(clickedPanel.Model);
            soldItems.Add(clickedPanel.Model.id);
            // clickedPanel.soldOut.enabled = true;
            UpdateGearPanel();
        }
    }
}