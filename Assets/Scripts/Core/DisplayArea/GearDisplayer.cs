using System;
using Model;
using TMPro;
using UnityEngine;

namespace Core.DisplayArea{
    public class GearDisplayer: MonoBehaviour{
        public TextMeshProUGUI desc;
        public SpriteRenderer sprite;

        public Gear Gear{
            set{
                desc.text = $"{value.name}\nAtt: {value.ball.point.ToString()}\nBall Num: {value.ballNum.ToString()}";
                var textureSprite = Resources.Load<Sprite>(value.imgPath);
                sprite.sprite = textureSprite;
            }
        }
        
        private void Start(){
            GameManager.shared.game.player.OnGearChanged += (g, m) => {
                Gear = (m as Player)?.CurrentGear;
            };
            Gear = GameManager.shared.game.player.CurrentGear;
        }
    }
}