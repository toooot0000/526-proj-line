using System;
using System.Collections.Generic;
using System.Linq;
using BackendApi;
using Model;
using UI.Container;
using Unity.VisualScripting;
using UnityEngine;
using Utility;
using Utility.Loader;

namespace UI.Interfaces.ShopSystem {
    public class UIShopSystem : UIBase{
        private const int UnifiedPrice = 6;
        private List<Gear> _items;
        private static Game Game => GameManager.shared.game;
        private CanvasGroup _canvasGroup;
        private bool _inAnimation;
        private UIShopPanel[] _panels;
        private readonly List<int> _soldItems = new();

        private void Start() {
            _items = new List<Gear>();

            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _panels = GetComponentsInChildren<UIShopPanel>();
        }

        public override void Open(object gears) {
            base.Open(gears);
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 0,
                i => _canvasGroup.alpha = i,
                () => _inAnimation = false
            );
            UpdateDisplayedGears();
            UpdateGearPanel();
            StartCoroutine(coroutine());
        }

        private void UpdateDisplayedGears(){
            _items = GetAllGearsPlayerNotOwned(3).Select(i => new Gear(Game, i)).ToList();
        }

        private static List<int> GetAllGearsPlayerNotOwned(int length = -1){
            var gearIds = CsvLoader.Load("Configs/gears").Keys;
            var ret = new List<int>();
            var playerOwned = new HashSet<int>(GameManager.shared.game.player.gears.Select(g => g.id));
            foreach (var id in gearIds){
                if (playerOwned.Contains(id)) continue;
                ret.Add(id);
                if (length > 0 && ret.Count >= length) break;
            }
            return ret;
        }

        public override void Close() {
            _inAnimation = true;
            var coroutine = TweenUtility.Lerp(0.2f,
                () => _canvasGroup.alpha = 1,
                i => _canvasGroup.alpha = 1 - i,
                () => {
                    _inAnimation = false;
                    base.Close();
                    foreach (var uiShopPanel in _panels){
                        Destroy(uiShopPanel.gameObject);
                    }
                    Destroy(gameObject);
                });
            StartCoroutine(coroutine());
        }
        
        public void UpdateGearPanel(){
            var curPanelInd = 0;
            foreach (var gear in _items)
            {
                var gearPanel = _panels[curPanelInd];
                gearPanel.Show = true;
                gearPanel.Model = gear;
                gearPanel.Price = UnifiedPrice;
                gearPanel.OnClick += PurchaseSelectedGear;
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
            if (GameManager.shared.game.player.Coin < clickedPanel.Price) return; 
            GameManager.shared.game.player.Coin -= clickedPanel.Price; 
            GameManager.shared.game.player.AddGear(clickedPanel.Model);
            _soldItems.Add(clickedPanel.Model.id);
            clickedPanel.soldOut.enabled = true;
            UpdatePriceTags();
        }

        private void UpdatePriceTags(){
            foreach (var uiShopPanel in _panels){
                uiShopPanel.UpdatePriceColor();
            }
        }
    }
}