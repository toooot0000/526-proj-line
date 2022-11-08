using System;
using System.Collections.Generic;
using System.Linq;
using BackendApi;
using Model;
using TMPro;
using UI.Common;
using UI.Container;
using Unity.VisualScripting;
using UnityEngine;
using Utility;
using Utility.Loader;
using Random = UnityEngine.Random;

namespace UI.Interfaces.ShopSystem {
    public class UIShopSystem : UIBase{
        private const int UnifiedPrice = 2;
        private int RefreshPrice = 1;
        public List<Gear> _items;
        private static Game Game => GameManager.shared.game;
        private CanvasGroup _canvasGroup;
        private bool _inAnimation;
        private UIShopPanel[] _panels;
        public TextMeshProUGUI coinWithNumber;
        public TextMeshProUGUI refreshCoinWithNumber;

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
                curPanelInd++;
            }
            for (; curPanelInd < _panels.Length; curPanelInd++) _panels[curPanelInd].Show = false;
            coinWithNumber.text = $"{GameManager.shared.game.player.Coin.ToString()}";
            refreshCoinWithNumber.text = $"{RefreshPrice.ToString()}";
        }

        public void ConfirmButtonEvent() {
            // StartCoroutine(CoroutineUtility.Delayed(GameManager.shared.game.GoToNextStage));
            Close();
        }
        

        public void PurchaseSelectedGear(UIShopPanel clickedPanel)
        {
            if (GameManager.shared.game.player.Coin < clickedPanel.Price) return; 
            GameManager.shared.game.player.Coin -= clickedPanel.Price; 
            GameManager.shared.game.player.AddGear(clickedPanel.Model);
            clickedPanel.soldOut.enabled = true;
            UpdatePriceTags();
        }

        public void UpdatePriceTags(){
            foreach (var uiShopPanel in _panels){
                uiShopPanel.UpdatePriceColor();
            }
            coinWithNumber.text = $"{GameManager.shared.game.player.Coin.ToString()}";
        }

        public void Refresh()
        {
            if (GameManager.shared.game.player.Coin < RefreshPrice) return;
            GameManager.shared.game.player.Coin -= RefreshPrice;
            RefreshPrice += 1;
            coinWithNumber.text = $"{GameManager.shared.game.player.Coin.ToString()}";
            refreshCoinWithNumber.text = $"{RefreshPrice.ToString()}";
            if (GameManager.shared.game.player.Coin < RefreshPrice) refreshCoinWithNumber.color = Color.red;
            _items = RandomSelectItems();
            UpdateGearPanel();
        }

        private List<Gear> RandomSelectItems()
        {
            List<int> _candidates = GetAllGearsPlayerNotOwned(-1);
            List<Gear> _res = new List<Gear>();
            for (int i = 0; i < 3; i++)
            {
                while (true)
                {
                    int rand = Random.Range(0, _candidates.Count);
                    Gear temp = new Gear(Game, _candidates[rand]);
                    if (temp.rarity == i)
                    {
                        _res.Add(temp);
                        break;
                    }
                }
            }
            return _res;
        }
    }
}