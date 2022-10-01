using System.Collections.Generic;
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
        
        
        private void Start(){
            UpdateConfig();
            _originIconSize = icon.size;
        }
        
        public void UpdateConfig(){
            var rectTransform = transform as RectTransform;
            rectTransform!.localScale = new Vector3(modelBall.size / 2, modelBall.size/2, modelBall.size/2);
            var ball = GetComponent<Ball>();
            ball.velocity = Vector2.one.Rotate(Random.Range(0, 360)).normalized * modelBall.speed;
            var sprite = GetComponent<SpriteRenderer>();
            sprite.color = colors[modelBall.type];
            icon.size = modelBall.size * _originIconSize;
        }
    }
}