using Tutorial;
using UnityEngine;

namespace Tutorials.Charge{
    public partial class TutorialCharge{
        private partial class Step2: IStepBase{
            private TutorialCharge _ttr;
            public virtual void SetUp(TutorialBase tutorial){
                _ttr = (TutorialCharge)tutorial;
                _ttr.meshes[1].enabled = true;
                _ttr.touchCatcher.Enabled = false;
                
                


                var startPosition = _ttr.startPoint.transform.position;
                var endPosition = _ttr.endPoint.transform.position;
                _ttr._attBallView.transform.position = endPosition;
                _ttr._defBallView.transform.position = startPosition;
                
                _ttr.LiftToFront(_ttr._defBallView.gameObject);
                _ttr.LiftToFront(_ttr.tutorialManager.tracker.gameObject, -1);

                _ttr._attBallView.tutorCanBeHit = false;
                _ttr._attBallView.tutorCanBeCircled = false;
                _ttr._defBallView.tutorCanBeHit = false;
                _ttr._defBallView.tutorCanBeCircled = true;

                _ttr.tutorialManager.tracker.tutorKeepLine = true;

                _ttr.tutorialMovingPointer.Enabled = true;

                var positions = new Vector3[10];
                var curDeg = -90f;
                for (var i = 0; i < 10; i++){
                    positions[i] = startPosition + new Vector3(){
                        x = Mathf.Cos(Mathf.Deg2Rad * curDeg),
                        y = Mathf.Sin(Mathf.Deg2Rad * curDeg)
                    };
                    curDeg += 360f/9;
                }

                _ttr.tutorialMovingPointer.Positions = positions;
                
                _ttr.tutorialMovingPointer.StartMoving();

                _ttr.tutorialManager.tracker.OnTouchEnd += WrappedStepComplete;
            }

            private void WrappedStepComplete(ITutorialControllable controllable){
                if (GameManager.shared.game.player.circledBalls.Count == 1){
                    _ttr.StepComplete(controllable);
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