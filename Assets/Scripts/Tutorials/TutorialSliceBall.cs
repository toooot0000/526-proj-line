using System;
using System.Linq;
using Core.DisplayArea.Stage;
using Core.DisplayArea.Stage.Enemy;
using Core.DisplayArea.Stage.Player;
using Core.PlayArea.Balls;
using Core.PlayArea.TouchTracking;
using Model;
using UnityEngine;

namespace Tutorials{
    public class TutorialContextSliceBall : TutorialContextBase{
        public BallManager ballManager;
        public StageManager stageManager;
        public TouchTracker tracker;
    }


    public class TutorialSliceBall : TutorialBase{
        public MovingPointer movingPointer;

        public Transform start;
        public Transform end;
        public Transform ballPoint;


        private readonly StepBase<TutorialSliceBall>[] _steps ={
            new SliceBallStep1()
        };

        private BallView _attackBallView;

        private int _currentStepIndex;
        private BallView _defendBallView;
        private EnemyView _enemy;
        private PlayerView _player;
        private TouchTracker _tracker;

        public override void Load(TutorialContextBase context){
            base.Load(context);
            // 1. 重置球的位置; attack: local(0, 0); defend: local(1, 1)
            // 2. 
            var balls = ((TutorialContextSliceBall)context).ballManager.balls;
            _attackBallView = balls.First(b => b.Model.type == BallType.Physics);
            _defendBallView = balls.First(b => b.Model.type == BallType.Defend);
            _tracker = ((TutorialContextSliceBall)context).tracker;
            _currentStepIndex = 0;
            _steps[0].SetUp(this);
        }

        private void Complete(ITutorialControllable controllable){
            _steps[_currentStepIndex].Complete(this);
            _currentStepIndex++;
            if (_currentStepIndex == _steps.Length)
                base.Complete();
            else
                _steps[_currentStepIndex].SetUp(this);
        }

        /// <summary>
        ///     Show how to slice ball
        /// </summary>
        private class SliceBallStep1 : StepBase<TutorialSliceBall>{
            public override void SetUp(TutorialSliceBall ttr){
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

                // movingPointer.startPosition = start.position;
                // movingPointer.endPosition = end.position;
                ttr.movingPointer.Positions = new[]{ ttr.start.position, ttr.end.position };
                ttr.movingPointer.StartMoving();

                ttr._tracker.OnTouchEnd += ttr.Complete;
            }

            public override void Complete(TutorialSliceBall ttr){
                ttr._attackBallView.GainBackControl(ttr);
                ttr._defendBallView.GainBackControl(ttr);
                ttr._tracker.GainBackControl(ttr);
                ttr.PutToBack(ttr._attackBallView.gameObject);
                ttr.PutToBack(ttr._tracker.gameObject);
                ttr._tracker.OnTouchEnd -= ttr.Complete;
            }
        }

        /// <summary>
        ///     Show Enemy lose and intention
        /// </summary>
        private class SliceBallStep2 : StepBase<TutorialSliceBall>{
            public override void SetUp(TutorialSliceBall tutorial){
                
            }

            public override void Complete(TutorialSliceBall tutorial){
                
            }
        }
    }
}