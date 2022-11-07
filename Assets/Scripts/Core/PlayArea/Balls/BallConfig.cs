using System.Collections.Generic;
using Model;
using Model.Mechanics.PlayableObjects;
using UnityEngine;
using Utility.Extensions;

namespace Core.PlayArea.Balls{
    public class BallConfig : MonoBehaviour{
        public Ball modelBall;
        public SpriteRenderer icon;
        public SpriteRenderer bg;
        public BallView ballView;
        [Header("State Sprites")] 
        public Sprite sprNormal;
        public Sprite sprDebuff;
        public Sprite sprCombo;
        public Sprite sprCharge;
        
        public void ResetView(){
            var rectTransform = transform as RectTransform;
            rectTransform!.localScale = new Vector3(modelBall.size, modelBall.size, 1);
            ballView.velocity = Vector2.one.Rotated(Random.Range(0, 360)).normalized * modelBall.speed;
            bg.sprite = sprNormal;
            bg.color = Color.white;
            if (modelBall.type == BallType.Debuff) bg.sprite = sprDebuff;
            icon.sprite = Resources.Load<Sprite>((modelBall.parent as Gear)!.imgPath);
            icon.color = Color.white;
        }
    }
}