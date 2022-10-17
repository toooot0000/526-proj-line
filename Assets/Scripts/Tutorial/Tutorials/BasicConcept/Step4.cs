using Core.PlayArea.Balls;
using UnityEngine;

namespace Tutorial.Tutorials.BasicConcept{
    public class Step4: StepBase{
        private BallView _ball;
        private TutorialBasicConcept _ttr;
        public override void SetUp(TutorialBase tutorial){
            _ttr = (TutorialBasicConcept)tutorial;
            _ttr.desc[3].Enabled = true;
            _ball = _ttr.tutorialManager.ballManager.balls[0];
            _ttr.LiftToFront(_ball.gameObject);
            
            var position = _ttr.ballPosition.position;
            _ball.HandOverControlTo(_ttr);
            _ttr.tutorialManager.tracker.HandOverControlTo(_ttr);
            _ttr.LiftToFront(_ttr.tutorialManager.tracker.gameObject);
            _ball.transform.position = position;
            _ttr.movingPointer.Enabled = true;
            _ttr.movingPointer.Positions = new[]{
                position - new Vector3(1, 0, 0),
                position + new Vector3(1, 0, 0)
            };
            _ttr.movingPointer.StartMoving();
            _ball.tutorCanBeCircled = false;
            _ball.tutorCanBeHit = true;
            _ttr.tutorialManager.tracker.OnTouchEnd += WrappedComplete;
        }

        private void WrappedComplete(ITutorialControllable controllable){
            if (GameManager.shared.game.player.hitBalls.Count == 1){
                _ttr.StepComplete(controllable);
            } else{
                _ball.CurrentState = BallView.State.Controlled;
            }
        }

        public override void Complete(TutorialBase tutorial){
            _ball.GainBackControlFrom(_ttr);
            _ttr.tutorialManager.tracker.GainBackControlFrom(_ttr);
            _ttr.PutToBack(_ball.gameObject);
            _ttr.PutToBack(_ttr.tutorialManager.tracker.gameObject);
            _ttr.desc[3].Enabled = false;
            _ttr.movingPointer.Enabled = false;
            tutorial.tutorialManager.tracker.OnTouchEnd -= WrappedComplete;
        }
    }
}