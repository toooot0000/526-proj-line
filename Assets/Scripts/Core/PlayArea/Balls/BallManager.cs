using System.Collections;
using System.Collections.Generic;
using Core.DisplayArea.Stage;
using Core.PlayArea.TouchTracking;
using Model;
using Model.Mechanics.PlayableObjects;
using Tutorial;
using UnityEngine;

namespace Core.PlayArea.Balls{
    public class BallManager : MonoBehaviour, ITutorialControllable{
        public GameObject ballPrefab;
        public int randomSpawnNumber = 5;
        public ComboDisplayer comboDisplayer;
        public readonly List<BallView> balls = new();
        public TouchTracker touchTracker;
        public PlayAreaManager playAreaManager;

        private bool _isInTutorial = false;

        public void SpawnNewBallView(Ball ball){
            var newBallObject = Instantiate(ballPrefab, transform, false);
            var newBallConfig = newBallObject.GetComponent<BallConfig>();
            newBallConfig.modelBall = ball;
            newBallObject.transform.localPosition = GenerateRandomLocalPosition();
            newBallConfig.ResetView();
        }

        public void SpawnRandom() // Temp
        {
            for (var i = 0; i < randomSpawnNumber; i++){
                var model = new Ball(GameManager.shared.game){
                    point = Random.Range(1, 10),
                    size = Random.Range(1, 3) / 2.0f,
                    speed = Random.Range(1, 10) / 2.0f
                };
                SpawnNewBallView(model);
            }
        }

        private Vector3 GenerateRandomLocalPosition(){
            return new Vector3{
                x = Random.Range(-1, 1),
                y = Random.Range(-1, 1),
                z = transform.position.z
            }; // TODO
        }

        public void SpawnBalls(){
            var skillBallModels = GameManager.shared.game.GetAllSkillBalls();
            var i = 0;
            for (; i < skillBallModels.Length; i++){
                BallView curBallView = null;
                if (i < balls.Count){
                    curBallView = balls[i];
                    curBallView.ResetView();
                } else{
                    var newBallObject = Instantiate(ballPrefab, transform, false);
                    curBallView = newBallObject.GetComponent<BallView>();
                    curBallView.OnSliced += OnBallSliced;
                    curBallView.OnCircled += OnBallCircled;
                    curBallView.OnMouseEnterBall += touchTracker.OnMouseEnterBall;
                    curBallView.OnMouseExitBall += touchTracker.OnMouseExitBall;
                    curBallView.OnMouseUpBall += touchTracker.OnMouseUpBall;
                    balls.Add(curBallView);
                }

                curBallView.gameObject.SetActive(true);
                curBallView.Model = skillBallModels[i];
                curBallView.transform.localPosition = GenerateRandomLocalPosition();
                curBallView.UpdateConfig();
            }

            for (; i < balls.Count; i++) balls[i].gameObject.SetActive(false);
        }

        public void FlyAllBalls(StageManager stageManager, float seconds){
            foreach (var ball in balls){
                if (ball.CurrentState == BallView.State.Free){
                    ball.FadeOut(seconds);
                    continue;
                }
                if (ball.Model.type == BallType.Debuff) continue;

                var target = stageManager.enemyView.transform.position;
                if (ball.Model.type == BallType.Defend) target = stageManager.playerView.transform.position;
                ball.FlyToLocation(seconds, target);
            }
        }

        public IEnumerator FlyAllBallsCoroutine(StageManager stageManager, float seconds){
            FlyAllBalls(stageManager, seconds);
            yield return new WaitForSeconds(seconds);
        }

        private void OnBallSliced(BallView view){
            comboDisplayer.Show(view.Model.currentGame.player.hitBalls.Count, view.transform.position);
            view.CurrentState = BallView.State.Touched;
            UpdateBallState();
        }

        private void UpdateBallState(){
            foreach (var ball in balls){
                if (((Gear)ball.Model.parent).IsComboIng() && ball.CurrentState == BallView.State.Touched){
                    ball.CurrentState = BallView.State.Combo;
                }

                if (((Gear)ball.Model.parent).IsCharged() && ball.CurrentState == BallView.State.Circled){
                    ball.CurrentState = BallView.State.Charged;
                }
            }
        }

        private void OnBallCircled(BallView view){
            view.CurrentState = BallView.State.Circled;
            UpdateBallState();
        }

        public void HandOverControlTo(TutorialBase tutorial){
            _isInTutorial = true;
            foreach (var ball in balls){
                ball.HandOverControlTo(tutorial);
            }
        }

        public void GainBackControlFrom(TutorialBase tutorial){
            _isInTutorial = false;
            foreach (var ball in balls){
                ball.GainBackControlFrom(tutorial);
            }
        }
    }
}