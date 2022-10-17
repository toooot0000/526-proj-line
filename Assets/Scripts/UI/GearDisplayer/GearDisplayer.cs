using System;
using Model;
using UI.Common.SimpleAnimation;
using UI.Container;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GearDisplayer{
    public class GearDisplayer : UIComponent{
        public UIContainerBase container;
        public GameObject gearItemPrefab;
        private EdgeHider _edgeHider;

        private void Awake(){
            _edgeHider = GetComponent<EdgeHider>();
        }

        private void Start(){
            GameManager.shared.game.player.OnGearChanged += UpdateGears;
            UIManager.shared.RegisterComponent(this);
            UpdateGears(GameManager.shared.game, GameManager.shared.game.player);
            var scrollRect = GetComponentInChildren<ScrollRect>();
            scrollRect.verticalNormalizedPosition = 1;
            scrollRect.horizontalNormalizedPosition = 0;
        }
        
        public override void Hide(){
            _edgeHider.Hide();
        }

        public override void Show(){
            _edgeHider.Show();
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