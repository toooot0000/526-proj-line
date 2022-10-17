using System.Linq;
using Core.PlayArea.Balls;
using Model;
using Tutorial.Common;
using Tutorial.Utility;
using UnityEngine;

namespace Tutorial.Tutorials.Combo{
    public class TutorialCombo: TutorialBase{

        public TutorialText[] texts;
        public TutorialTapCatcher tapCatcher;
        public TutorialMovingPointer movingPointer;
        public Transform startPosition;
        public Transform endPosition;


        private StepBase[] _step;
        private BallView _attBall;
        private BallView _defBall;
        protected override StepBase[] Steps => _step;

        public override void OnLoaded(TutorialManager mng){
            mng.stageManager.HandOverControlTo(this);
            mng.stageManager.TutorSetPause(true);
            mng.ballManager.HandOverControlTo(this);
            mng.tracker.HandOverControlTo(this);

            _step = new StepBase[]{
                new StepTapToContinue<TutorialCombo>(texts[0], tapCatcher, mng.stageManager.enemyView.gameObject),
                new StepTapToContinue<TutorialCombo>(texts[1], null, 
                    setUp:(combo, step) => { 
                        // Set up the moving pointer
                        var start = combo.startPosition.position;
                        var end = combo.endPosition.position;
                        combo.movingPointer.Positions = new[]{ start, end };
                        combo.movingPointer.Enabled = true;
                        
                        // freeze the setup the ball position
                        var balls = combo.tutorialManager.ballManager.balls; 
                        combo._attBall = balls.First(b => b.Model.type == BallType.Physics);
                        combo._defBall = balls.First(b => b.Model.type == BallType.Defend);
                        combo._attBall.transform.position = start;
                        combo._defBall.transform.position = end;
                        combo._attBall.tutorCanBeCircled = false;
                        combo._defBall.tutorCanBeCircled = false;
                        combo._attBall.tutorCanBeHit = true;
                        combo._defBall.tutorCanBeHit = true;
                        
                        // highlight balls
                        step.AddHighlightObject(combo._attBall.gameObject);
                        step.AddHighlightObject(combo._defBall.gameObject);
                        step.HighlightAll(combo);
                        
                        // tracker line keeping
                        combo.tutorialManager.tracker.tutorKeepLine = true;
                    },
                    bind: (t, s) => {
                        t.tutorialManager.tracker.OnTouchEnd += t.StepComplete;
                    },
                    cleanUp: (t, s) => {
                        s.LowlightAll(t);
                    },
                    unbind: (t, s) => {
                        t.tutorialManager.tracker.OnTouchEnd -= t.StepComplete;
                    }
                )
            };
            
            base.OnLoaded(mng);
        }

        public override void StepComplete(ITutorialControllable controllable){
            if (GameManager.shared.game.player.hitBalls.Count != 2){
                _attBall.CurrentState = BallView.State.Controlled;
                _defBall.CurrentState = BallView.State.Controlled;
            } else{
                base.StepComplete(controllable);
            }
        }

        protected override void Complete(){
            tutorialManager.stageManager.TutorSetPause(false);
            tutorialManager.stageManager.GainBackControlFrom(this);
            tutorialManager.ballManager.GainBackControlFrom(this);
            base.Complete();
        }
    }
}