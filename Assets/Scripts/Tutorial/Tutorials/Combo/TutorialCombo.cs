using System.Linq;
using Core.PlayArea.Balls;
using Model;
using Tutorial.Common;
using Tutorial.Utility;
using UI;
using UI.GearDisplayer;
using UnityEngine;

namespace Tutorial.Tutorials.Combo{
    public class TutorialCombo: TutorialBase{
        public new const string PrefabName = "TutorialCombo";

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

            var intentionDisplayerGameObject = mng.stageManager.enemyView.intentionDisplayer.gameObject;
            
            _step = new StepBase[]{
                new StepTapToContinue<TutorialCombo>(texts[0], tapCatcher, intentionDisplayerGameObject),
                new StepTapToContinue<TutorialCombo>(texts[1], tapCatcher, intentionDisplayerGameObject),
                new StepTapToContinue<TutorialCombo>(texts[2], null, 
                    setUp:(combo, step) => {
                        step.SetTextEnabled(true);
                        // Set up the moving pointer
                        var start = combo.startPosition.position;
                        var end = combo.endPosition.position;
                        combo.movingPointer.Positions = new[]{ start, end };
                        combo.movingPointer.Enabled = true;
                        combo.movingPointer.StartMoving();
                        
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
                        // step.AddHighlightObject(combo._attBall.gameObject);
                        // step.AddHighlightObject(combo._defBall.gameObject);
                        // step.HighlightAll(combo);
                        combo.LiftToFront(combo._attBall.gameObject, -50);
                        combo.LiftToFront(combo._defBall.gameObject, -50);
                        
                        // tracker
                        combo.LiftToFront(combo.tutorialManager.tracker.gameObject, 50);
                        combo.tutorialManager.tracker.tutorKeepLine = true;
                    },
                    bind: (t, s) => {
                        t.tutorialManager.tracker.OnTouchEnd += t.WrappedStepComplete;
                    },
                    cleanUp: (t, s) => {
                        s.SetTextEnabled(false);
                        t.movingPointer.Enabled = false;
                        t.PutToBack(t.tutorialManager.tracker.gameObject);
                    },
                    unbind: (t, s) => {
                        t.tutorialManager.tracker.OnTouchEnd -= t.WrappedStepComplete;
                    }
                ),
                new StepTapToContinue<TutorialCombo>(texts[3], tapCatcher,
                    setUp: StepTapToContinue<TutorialCombo>.DefaultSetUp,
                    cleanUp: (t, s) => {
                        t.tutorialManager.tracker.tutorKeepLine = false;
                        s.LowlightAll(t);
                        s.SetTextEnabled(false);
                        t.PutToBack(t._attBall.gameObject);
                        t.PutToBack(t._defBall.gameObject);
                    }
                ),
                new StepTapToContinue<TutorialCombo>(texts[4], tapCatcher, 
                    setUp: (t, s) => {
                        var gearDisplay = UIManager.shared.GetUIComponent<GearDisplayer>();
                        gearDisplay.Show();
                        StepTapToContinue<TutorialCombo>.DefaultSetUp(t, s);
                    }
                ),
                new StepTapToContinue<TutorialCombo>(texts[5], tapCatcher)
            };
            
            base.OnLoaded(mng);
        }

        private void WrappedStepComplete(ITutorialControllable controllable){
            if (GameManager.shared.game.player.hitBalls.Count != 2){
                _attBall.CurrentState = BallView.State.Controlled;
                _defBall.CurrentState = BallView.State.Controlled;
                GameManager.shared.game.player.hitBalls.Clear();
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