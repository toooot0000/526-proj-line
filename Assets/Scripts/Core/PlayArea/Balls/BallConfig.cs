using System.Collections.Generic;
using Model;
using UnityEngine;
using Utility.Extensions;
using Random = UnityEngine.Random;

namespace Core.PlayArea.Balls{
    public class BallConfig: MonoBehaviour{

        public static Dictionary<global::Model.Ball.Type, Color> colors = new(){
            {global::Model.Ball.Type.Physics, Color.white},
            {global::Model.Ball.Type.Magic, Color.blue},
            {global::Model.Ball.Type.Defend, Color.yellow}
        };
        public global::Model.Ball modelBall;

        public SpriteRenderer icon;
        private Vector2 _originIconSize;

        public SpriteRenderer sprite;
        public BallView ballView;
        
        
        private void Start(){
            _originIconSize = icon.size;
        }
        
        public void UpdateConfig(){
            var rectTransform = transform as RectTransform;
            rectTransform!.localScale = new Vector3(modelBall.size / 2, modelBall.size/2, modelBall.size/2);
            ballView.velocity = Vector2.one.Rotate(Random.Range(0, 360)).normalized * modelBall.speed;
            sprite.color = colors[modelBall.type];
            icon.size *= modelBall.size;
            icon.sprite = Resources.Load<Sprite>((modelBall.parent as Gear)!.imgPath);
            icon.color = Color.white;
        }
    }
}