using System.Linq;
using Model;
using Tutorial.Common;
using Tutorial.Utility;
using UI;
using UI.GearDisplayer;
using UnityEngine;

namespace Tutorial.Tutorials.Charge{
    public class TutorialCharge: TutorialBase{
        public new const string PrefabName = "TutorialCharge";
        
        public TutorialText[] texts;
        public TutorialTapCatcher tapCatcher;
        public TutorialMovingPointer movingPointer;
        public Transform ballPosition;
        public float circlingRadius = 0.5f;
        
        private StepBase[] _steps;
        protected override StepBase[] Steps => _steps;


        public override void OnLoaded(TutorialManager mng){
            mng.stageManager.HandOverControlTo(this);
            mng.stageManager.TutorSetPause(true);
            mng.ballManager.HandOverControlTo(this);
            mng.tracker.HandOverControlTo(this);
            
            _steps = new StepBase[]{
                new StepTapToContinue<TutorialCharge>(texts[0], tapCatcher, mng.stageManager.enemyView.intentionDisplayer.gameObject),
                new StepTapToContinue<TutorialCharge>(texts[1], null, 
                    setUp: (t, s) => {
                        var position = t.ballPosition.position;
                        
                        // set up moving pointer
                        t.movingPointer.Enabled = true;
                        var positions = new Vector3[11];
                        var curDeg = -90f;
                        for (var i = 0; i < 11; i++){
                            var curPosition = position + new Vector3(){
                                x = Mathf.Cos(curDeg * Mathf.Deg2Rad) * t.circlingRadius,
                                y = Mathf.Sin(curDeg* Mathf.Deg2Rad) * t.circlingRadius
                            };
                            positions[i] = curPosition;
                            curDeg += 36;
                        }
                        t.movingPointer.Positions = positions;
                        t.movingPointer.StartMoving();
                        
                        // set ball
                        var defBall = t.tutorialManager.ballManager.balls.First(b => b.Model.type == BallType.Defend);
                        var attBall = t.tutorialManager.ballManager.balls.First(b => b.Model.type == BallType.Physics);
                        t.LiftToFront(defBall.gameObject);
                        defBall.TutorialSetPosition(position);
                        attBall.TutorialSetPosition(position + new Vector3(0, -1, 0));
                        defBall.tutorCanBeCircled = true;
                        defBall.tutorCanBeHit = false;
                        attBall.tutorCanBeCircled = false;
                        attBall.tutorCanBeHit = false;
                        
                        // set tracker
                        t.LiftToFront(t.tutorialManager.tracker.gameObject);
                        t.tutorialManager.tracker.tutorKeepLine = true;
                        
                        // show texts
                        t.texts[1].Enabled = true;
                    }, 
                    bind: (t, s) => {
                        t.tutorialManager.tracker.OnTouchEnd += t.WrappedStepComplete;
                    },
                    cleanUp: (t, s) => {
                        t.movingPointer.Enabled = false;
                        t.texts[1].Enabled = false;
                        t.PutToBack(t.tutorialManager.tracker.gameObject);
                    },
                    unbind: (t, s) => {
                        t.tutorialManager.tracker.OnTouchEnd -= WrappedStepComplete;
                    }
                ),
                new StepTapToContinue<TutorialCharge>(texts[2], tapCatcher, 
                    setUp: StepTapToContinue<TutorialCharge>.DefaultSetUp,
                    cleanUp: (t, s) => {
                        t.PutToBack(t.tutorialManager.tracker.gameObject);
                        StepTapToContinue<TutorialCharge>.DefaultCleanUp(t, s);
                    }
                ),
                new StepTapToContinue<TutorialCharge>(texts[3], tapCatcher),
                new StepTapToContinue<TutorialCharge>(texts[4], tapCatcher, 
                    setUp: (t, s) => {
                        UIManager.shared.GetUIComponent<GearDisplayer>().Show();
                        StepTapToContinue<TutorialCharge>.DefaultSetUp(t, s);
                    },
                    cleanUp: (t, s) => {
                        t.tutorialManager.tracker.tutorKeepLine = false;
                        StepTapToContinue<TutorialCharge>.DefaultCleanUp(t, s);
                    }
                )
                
            };
            
            base.OnLoaded(mng);
        }

        private void WrappedStepComplete(ITutorialControllable controllable){
            if (GameManager.shared.game.player.circledBalls.Count == 1 &&
                GameManager.shared.game.player.circledBalls[0].type == BallType.Defend){
                StepComplete(controllable);
            } else{
                tutorialManager.ballManager.HandOverControlTo(this);
            }
        }


        protected override void Complete(){
            tutorialManager.tracker.GainBackControlFrom(this);
            tutorialManager.ballManager.GainBackControlFrom(this);
            tutorialManager.stageManager.TutorSetPause(false);
            tutorialManager.stageManager.GainBackControlFrom(this);
            base.Complete();
        }
    }
}