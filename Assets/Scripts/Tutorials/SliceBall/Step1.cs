using UnityEngine;

namespace Tutorials.SliceBall{
    public partial class TutorialSliceBall{
        private partial class Step1: StepBase{
            public override void SetUp(TutorialBase tutorial){
                var ttr = (TutorialSliceBall)tutorial;
                ttr._attackBallView.HandOverControlTo(ttr);
                ttr._attackBallView.tutorCanBeCircled = false;

                ttr._defendBallView.HandOverControlTo(ttr);
                ttr._defendBallView.tutorCanBeHit = false;
                ttr._defendBallView.tutorCanBeCircled = false;

                ttr._tracker.HandOverControlTo(ttr);

                ttr.LiftToFront(ttr._attackBallView.gameObject);
                ttr.LiftToFront(ttr._tracker.gameObject, -1);

                var ballPosition = ttr.ballPoint.position;
                var attackBallTransform = ttr._attackBallView.transform;
                var position = attackBallTransform.position;
                position =
                    new Vector3(ballPosition.x, ballPosition.y, position.z);
                attackBallTransform.position = position;
                var defTransform = ttr._defendBallView.transform;
                var positionZ = defTransform.position.z;
                defTransform.position = new Vector3( position.x, position.y -1.2f, positionZ);
                ttr.movingPointer.Positions = new[]{ ttr.start.position, ttr.end.position };
                ttr.movingPointer.StartMoving();
                ttr._tracker.OnTouchEnd += ttr.StepComplete;
            }

            public override void Complete(TutorialBase tutorial){
                var ttr = (TutorialSliceBall)tutorial;
                ttr._attackBallView.GainBackControlFrom(ttr);
                ttr._defendBallView.GainBackControlFrom(ttr);
                ttr._tracker.GainBackControlFrom(ttr);
                ttr.PutToBack(ttr._attackBallView.gameObject);
                ttr.PutToBack(ttr._tracker.gameObject);
                ttr._tracker.OnTouchEnd -= ttr.StepComplete;
            }
        }
    }
}