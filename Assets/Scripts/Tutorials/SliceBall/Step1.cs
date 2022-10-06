using UnityEngine;

namespace Tutorials.SliceBall{
    public partial class TutorialSliceBall{
        private partial class Step1: StepBase{
            public override void SetUp(TutorialBase tutorial){
                var ttr = (TutorialSliceBall)tutorial;
                ttr._attackBallView.ControlledByTutorial(ttr);
                ttr._attackBallView.tutorCanBeCircled = false;

                ttr._defendBallView.ControlledByTutorial(ttr);
                ttr._defendBallView.tutorCanBeHit = false;
                ttr._defendBallView.tutorCanBeCircled = false;

                ttr._tracker.ControlledByTutorial(ttr);

                ttr.LiftToFront(ttr._attackBallView.gameObject);
                ttr.LiftToFront(ttr._tracker.gameObject, -1);

                var ballPosition = ttr.ballPoint.position;
                var attackBallTransform = ttr._attackBallView.transform;
                var position = attackBallTransform.position;
                position =
                    new Vector3(ballPosition.x, ballPosition.y, position.z);
                attackBallTransform.position = position;
                ttr._defendBallView.transform.position = position + new Vector3(0, -1);
                ttr.movingPointer.Positions = new[]{ ttr.start.position, ttr.end.position };
                ttr.movingPointer.StartMoving();
                
                
                
                
                ttr._tracker.OnTouchEnd += ttr.StepComplete;
            }

            public override void Complete(TutorialBase tutorial){
                var ttr = (TutorialSliceBall)tutorial;
                ttr._attackBallView.GainBackControl(ttr);
                ttr._defendBallView.GainBackControl(ttr);
                ttr._tracker.GainBackControl(ttr);
                ttr.PutToBack(ttr._attackBallView.gameObject);
                ttr.PutToBack(ttr._tracker.gameObject);
                ttr._tracker.OnTouchEnd -= ttr.StepComplete;
            }
        }
    }
}