using System;
using System.Collections.Generic;
using Core.DisplayArea.Stage;
using Core.DisplayArea.Stage.Enemy;
using Core.DisplayArea.Stage.Player;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.PlayArea.Balls{
    public class BallManager: MonoBehaviour{
        public GameObject ballPrefab;
        public int randomSpawnNumber = 5;
        public readonly List<BallView> balls = new();
        public ComboDisplayer comboDisplayer;


        private void Start(){
            GameManager.shared.game.OnTurnChanged += game => {
                if (game.turn == Game.Turn.Player){
                    SpawnBalls();
                }
            };
            SpawnBalls();
        }

        public void SpawnNewBallView(global::Model.Ball ball){
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
                SpawnNewBallView(model);
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
            var skillBallModels = GameManager.shared.game.GetAllSkillBalls();
            int i = 0;
            for (; i < skillBallModels.Count; i++){
                BallView curBallView = null;
                if (i < balls.Count){
                    curBallView = balls[i];
                    curBallView.ResetView();
                } else{
                    var newBallObject = Instantiate(ballPrefab, transform, false);
                    curBallView = newBallObject.GetComponent<BallView>();
                    curBallView.OnHitted += view => {
                        comboDisplayer.Show(view.Model.currentGame.player.hitBalls.Count, view.transform.position);
                    };
                    balls.Add(curBallView);
                }
                curBallView.gameObject.SetActive(true);
                curBallView.Model = skillBallModels[i];
                curBallView.transform.localPosition = GenerateRandomLocalPosition();
                curBallView.UpdateConfig();
            }

            for (; i < balls.Count; i++){
                balls[i].gameObject.SetActive(false);
            }
        }

        public void FlyAllBalls(StageManager stageManager, float seconds){
            foreach (var ball in balls){
                if (ball.currentState == BallView.State.Free) continue;
                if (ball.Model.type == BallType.Debuff) continue;
                var target = stageManager.enemyView.transform.position;
                if (ball.Model.type == BallType.Defend){
                    target = stageManager.playerView.transform.position;
                }
                ball.FlyToLocation(seconds, target);
            }
        }
    }
}