using System.Linq;
using Core.PlayArea.Balls;
using Model;
using Tutorial;

namespace Tutorials.Turn2{
    public partial class TutorialTurn2{
        /// <summary>
        ///     Combo the balls
        /// </summary>
        private partial class Step2: IStepBase{
            private TutorialTurn2 _ttr;
            public virtual void SetUp(TutorialBase tutorial){
                _ttr = (TutorialTurn2)tutorial;
                _ttr.meshes[1].enabled = true;
                _ttr.touchCatcher.Enabled = false;
                
                var startPosition = _ttr.startPoint.transform.position;
                _ttr._attBallView.transform.position = startPosition;
                var endPosition = _ttr.endPoint.transform.position;
                _ttr._defBallView.transform.position = endPosition;
                _ttr._attBallView.HandOverControlTo(_ttr);
                _ttr._defBallView.HandOverControlTo(_ttr);
                
                _ttr.LiftToFront(_ttr._attBallView.gameObject);
                _ttr.LiftToFront(_ttr._defBallView.gameObject);
                _ttr.LiftToFront(_ttr.tutorialManager.tracker.gameObject, -1);

                _ttr._attBallView.tutorCanBeHit = true;
                _ttr._attBallView.tutorCanBeCircled = false;
                _ttr._defBallView.tutorCanBeHit = true;
                _ttr._defBallView.tutorCanBeCircled = false;

                _ttr.tutorialManager.tracker.tutorKeepLine = true;

                _ttr.tutorialMovingPointer.Enabled = true;
                _ttr.tutorialMovingPointer.Positions = new[]{
                    startPosition,
                    endPosition
                };
                
                _ttr.tutorialMovingPointer.StartMoving();

                _ttr.tutorialManager.tracker.OnTouchEnd += WrappedStepComplete;
            }

            private void WrappedStepComplete(ITutorialControllable controllable){
                if (GameManager.shared.game.player.hitBalls.Count == 2){
                    _ttr.StepComplete(controllable);
                }
                else {
                    _ttr._attBallView.CurrentState = BallView.State.Controlled;
                    _ttr._attBallView.CurrentState = BallView.State.Controlled;
                }
            }

            public virtual void Complete(TutorialBase tutorial){
                _ttr.meshes[1].enabled = false;
                _ttr.tutorialMovingPointer.Enabled = false;
                _ttr.PutToBack(_ttr.tutorialManager.tracker.gameObject);
                _ttr.tutorialManager.tracker.OnTouchEnd -= WrappedStepComplete;
            }
        }
    }
}