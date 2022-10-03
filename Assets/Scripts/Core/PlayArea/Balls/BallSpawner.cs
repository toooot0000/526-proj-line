using System;
using System.Collections.Generic;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.PlayArea.Balls{
    public class BallSpawner: MonoBehaviour{
        public GameObject ballPrefab;
        public int randomSpawnNumber = 5;
        public readonly List<BallConfig> ballConfigs = new();


        private void Start(){
            GameManager.shared.game.OnTurnChanged += game => {
                if (game.turn == Game.Turn.Player){
                    SpawnBalls();
                }
            };
            SpawnBalls();
        }

        public void Spawn(global::Model.Ball ball){
            var newBallObject = Instantiate(ballPrefab, transform, false);
            var newBallConfig = newBallObject.GetComponent<BallConfig>();
            newBallConfig.modelBall = ball;
            newBallObject.transform.localPosition = GenerateRandomLocalPosition();
            newBallConfig.UpdateConfig();
        }
        
        public void SpawnRandom()       // Temp
        {
            for (int i = 0; i < randomSpawnNumber; i++)
            {
                var model = new global::Model.Ball(GameManager.shared.game)
                {
                    point = Random.Range(1, 10),
                    size = Random.Range(1, 3) / 2.0f,
                    speed = Random.Range(1, 10) / 2.0f
                };
                Spawn(model);
            }
        }

        private Vector3 GenerateRandomLocalPosition(){
            
            return new Vector3(){
                x = Random.Range(-1, 1),
                y = Random.Range(-1, 1),
                z = transform.position.z
            }; // TODO
        }

        public void SpawnBalls(){
            var skillBalls = GameManager.shared.game.GetAllSkillBalls();
            int i = 0;
            for (; i < skillBalls.Count; i++){
                BallConfig config = null;
                if (i < ballConfigs.Count){
                    config = ballConfigs[i];
                } else{
                    var newBallObject = Instantiate(ballPrefab, transform, false);
                    config = newBallObject.GetComponent<BallConfig>();
                    ballConfigs.Add(config);
                }
                config.gameObject.SetActive(true);
                config.modelBall = skillBalls[i];
                config.transform.localPosition = GenerateRandomLocalPosition();
                config.UpdateConfig();
                config.ball.Reset();
            }

            for (; i < ballConfigs.Count; i++){
                ballConfigs[i].gameObject.SetActive(false);
            }
        }
    }
}