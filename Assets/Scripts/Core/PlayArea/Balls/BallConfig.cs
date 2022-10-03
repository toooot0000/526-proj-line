using System.Collections.Generic;
using Model;
using UnityEngine;
using Utility.Extensions;
using Random = UnityEngine.Random;

namespace Core.PlayArea.Balls{
    public class BallConfig: MonoBehaviour{

        public static Dictionary<global::Model.BallType, Color> colors = new(){
            {global::Model.BallType.Physics, Color.white},
            {global::Model.BallType.Magic, Color.blue},
            {global::Model.BallType.Defend, Color.yellow}
        };
        public global::Model.Ball modelBall;

        public SpriteRenderer icon;
        private Vector2 _originIconSize = new Vector2(1.5f, 1.5f);

        public SpriteRenderer sprite;
        public BallView ballView;
        
        
        private void Start(){
            _originIconSize = new Vector2(1.5f, 1.5f);
        }
        
        public void UpdateConfig(){
            var rectTransform = transform as RectTransform;
            rectTransform!.localScale = new Vector3(modelBall.size, modelBall.size, 1);
            ballView.velocity = Vector2.one.Rotate(Random.Range(0, 360)).normalized * modelBall.speed;
            sprite.color = colors[modelBall.type];
            icon.size = _originIconSize * modelBall.size;
            icon.sprite = Resources.Load<Sprite>((modelBall.parent as Gear)!.imgPath);
            icon.color = Color.white;
        }
    }
}