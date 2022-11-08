using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private Rect GenerateRandomLocalPosition(Vector2Int[] emptyPositions){
            var gridPosition = emptyPositions[Random.Range(0, emptyPositions.Length)];
            return playAreaManager.GridRectToRect(new RectInt(gridPosition, Vector2Int.one));
        }

        public void SpawnBalls(){
            var emptyPositions = GameManager.shared.game.playArea.GetEmptyGridPositions().ToArray();
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
                    curBallView.OnBallSliced += OnBallSliced;
                    curBallView.OnBallCircled += OnBallCircled;
                    balls.Add(curBallView);
                }

                curBallView.gameObject.SetActive(true);
                curBallView.Model = skillBallModels[i];
                var rect = GenerateRandomLocalPosition(emptyPositions);
                ((RectTransform)curBallView.transform).anchoredPosition = rect.position;
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