using Model;
using TMPro;
using UI.Container;
using UnityEngine;

namespace UI.GearDisplayer{
    public class GearDisplayer: MonoBehaviour{
        // public TextMeshProUGUI desc;
        // public SpriteRenderer sprite;
        public UIContainerBase container;
        public GameObject gearItemPrefab;
        
        // public Gear Gear{
        //     set{
        //         desc.text = $"{value.name}\nAtt: {value.ball.point.ToString()}\nBall Num: {value.ballNum.ToString()}";
        //         var textureSprite = Resources.Load<Sprite>(value.imgPath);
        //         sprite.sprite = textureSprite;
        //     }
        // }

        private void Start(){
            GameManager.shared.game.player.OnGearChanged += UpdateGears;
            UpdateGears(GameManager.shared.game, GameManager.shared.game.player);
        }

        private void UpdateGears(Game game, GameModel player) {
            var gears = (player as Player)!.CurrentGears;
            var i = 0;
            for(;i< gears.Length; i++) {
                GearItem item;
                if (i >= container.children.Count) {
                    item = Instantiate(gearItemPrefab, transform).GetComponent<GearItem>();
                    container.AddChild(item);
                }
                else {
                    item = (container.children[i] as GearItem)!;
                    item.gameObject.SetActive(true);
                    container.AttachChild(i);
                }
                item.Model = gears[i];
            }
            while (i < container.transform.childCount) {
                container.UntachChild(i);
                container.children[i].gameObject.SetActive(false);
            }
            
            container.UpdateLayout();
        }
    }
}