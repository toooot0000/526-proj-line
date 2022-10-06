using Model;
using UnityEngine;
using Utility.Extensions;

namespace Core.PlayArea.Balls{
    public class BallConfig : MonoBehaviour{
        public Ball modelBall;

        public SpriteRenderer icon;

        public SpriteRenderer sprite;
        public BallView ballView;


        public void UpdateConfig(){
            var rectTransform = transform as RectTransform;
            rectTransform!.localScale = new Vector3(modelBall.size, modelBall.size, 1);
            ballView.velocity = Vector2.one.Rotate(Random.Range(0, 360)).normalized * modelBall.speed;
            sprite.color = Color.white;
            icon.sprite = Resources.Load<Sprite>((modelBall.parent as Gear)!.imgPath);
            icon.color = Color.white;
        }
    }
}