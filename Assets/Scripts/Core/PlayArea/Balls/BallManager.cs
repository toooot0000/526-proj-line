using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.DisplayArea.Stage;
using Core.PlayArea.TouchTracking;
using Model;
using Model.Mechanics.PlayableObjects;
using Tutorial;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.PlayArea.Balls{
    public class BallManager : PlayableViewManager<Ball>, ITutorialControllable, IPlayableViewManager{
        public GameObject ballPrefab;
        public int randomSpawnNumber = 5;
        public ComboDisplayer comboDisplayer;
        public IEnumerable<BallView> balls => views.Select(v => v as BallView);
        public TouchTracker touchTracker;
        public PlayAreaManager playAreaManager;

        private bool _isInTutorial = false;

        private Rect GenerateRandomLocalPosition(IReadOnlyList<Vector2Int> emptyPositions){
            var gridPosition = emptyPositions[Random.Range(0, emptyPositions.Count)];
            return playAreaManager.GridRectToRect(new RectInt(gridPosition, Vector2Int.one));
        }

        public void SpawnBalls(){
            foreach (var ballView in balls){
                ballView.gameObject.SetActive(false);
            }
            var skillBallModels = GameManager.shared.game.GetAllBalls();
            foreach (var skillBallModel in skillBallModels){
                Place(skillBallModel);
            }
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

        public override PlayableObjectViewWithModel<Ball> Place(Ball model){
            var ret =  base.Place(model) as BallView;
            ret!.UpdateConfig();
            ret!.ResetView();
            return ret;
        }

        protected override PlayableObjectViewWithModel<Ball> GenerateNewObject(){
            var newBallObject = Instantiate(ballPrefab, transform, false);
            var ballView = newBallObject.GetComponent<BallView>();
            if (ballView == null) throw new Exception("Ball Prefab doesn't have a ballView component!");
            ballView.OnBallSliced += OnBallSliced;
            ballView.OnBallCircled += OnBallCircled;
            return ballView;
        }
    }
}