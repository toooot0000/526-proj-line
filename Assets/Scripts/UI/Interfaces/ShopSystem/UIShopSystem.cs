using System.Collections.Generic;
using System.Linq;
using BackendApi;
using Model;
using UI.Container;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace UI.Interfaces.ShopSystem {
    public class UIShopSystem : UIBase
    {
        private List<Gear> items;
        public Game game;
        private CanvasGroup _canvasGroup;
        private bool _inAnimation;
        private UIShopPanel[] _panels;
        private List<int> playerOwnedItems;
        private List<int> soldItems;

        private void Start() {
            items = new List<Gear>();
            playerOwnedItems = new List<int>();
            foreach (var gear in GameManager.shared.game.player.gears)
            {
                playerOwnedItems.Add(gear.id);
            }

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
            items = gears as List<Gear>;
            
            int index = 0;
            for (int j = -2; j <= 7; j++)
            {
                if(j == 0) continue;
                if (index < 6 && !playerOwnedItems.Contains(j))
                {
                    items.Add(new Gear(game, j));
                    index += 1;
                }
            }
            
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
            if (items.Count > 6) {
                Debug.LogError("Gears in Shop more than 6!");
                return;
            }

            foreach (var gear in items)
            {
                var gearPanel = _panels[curPanelInd];
                gearPanel.Show = true;
                gearPanel.Model = gear;
                if (2 <= GameManager.shared.game.player.Coin) gearPanel.OnClick += PurchaseSelectedGear;
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
            if (GameManager.shared.game.player.Coin < 2) return; // change gear.id to gear.price later
            GameManager.shared.game.player.Coin -= 2; // change gear.id to gear.price later
            GameManager.shared.game.player.AddGear(clickedPanel.Model);
            soldItems.Add(clickedPanel.Model.id);
            // clickedPanel.soldOut.enabled = true;
            UpdateGearPanel();
        }
    }
}