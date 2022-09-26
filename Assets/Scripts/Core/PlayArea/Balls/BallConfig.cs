using System.Collections.Generic;
using UnityEngine;
using Utility.Extensions;
using Random = UnityEngine.Random;

namespace Core.PlayArea.Balls{
    public class BallConfig: MonoBehaviour{

        public static Dictionary<Model.Ball.Type, Color> colors = new(){
            {Model.Ball.Type.Physics, Color.white},
            {Model.Ball.Type.Magic, Color.blue},
            {Model.Ball.Type.Defend, Color.yellow}
        };
        public Model.Ball modelBall;

        private void Start(){
            UpdateConfig();
        }
        
        public void UpdateConfig(){
            var rectTransform = transform as RectTransform;
            rectTransform!.localScale = new Vector3(modelBall.size / 2, modelBall.size/2, modelBall.size/2);
            var ball = GetComponent<Ball>();
            ball.velocity = Vector2.one.Rotate(Random.Range(0, 360)).normalized * modelBall.speed;
            var sprite = GetComponent<SpriteRenderer>();
            sprite.color = colors[modelBall.type];
        }
    }
}