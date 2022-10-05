using System;
using System.Linq;
using Core.PlayArea.Balls;
using Core.PlayArea.TouchTracking;
using Model;
using UI;
using UnityEngine;

namespace Tutorials{

    public class TutorialContextSliceBall : TutorialContextBase{
        public BallManager ballManager;
        public TouchTracker tracker;
    }

    
    
    public class TutorialSliceBall: TutorialBase{
        public MovingPointer movingPointer;
        
        private BallView _attackBallView;
        private BallView _defendBallView;
        private TouchTracker _tracker;

        public Transform start;
        public Transform end;
        public Transform ballPoint;
        
        private readonly TutorialStepBase[] _steps ={
            new TutorialSliceBallStep1()
        };

        private int _currentStepIndex = 0;

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

        private void WrappedComplete(ITutorialControllable controllable){
            _steps[_currentStepIndex].Complete(this);
            _currentStepIndex++;
            if (_currentStepIndex == _steps.Length){
                Complete();
            } else{
                _steps[_currentStepIndex].SetUp(this);
            }
        }
        
        
        private class TutorialSliceBallStep1 : TutorialStepBase{
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
                
                // movingPointer.startPosition = start.position;
                // movingPointer.endPosition = end.position;
                ttr.movingPointer.Positions = new[]{ ttr.start.position, ttr.end.position };
                ttr.movingPointer.StartMoving();

                ttr._tracker.OnTouchEnd += ttr.WrappedComplete;
            }
            public override void Complete(TutorialBase tutorial){
                var ttr = (TutorialSliceBall)tutorial;
                ttr._attackBallView.GainBackControl(tutorial);
                ttr._defendBallView.GainBackControl(tutorial);
                ttr._tracker.GainBackControl(tutorial);
                ttr.PutToBack(ttr._attackBallView.gameObject);
                ttr.PutToBack(ttr._tracker.gameObject);
                ttr._tracker.OnTouchEnd -= ttr.WrappedComplete;
            }
        }
    }
}