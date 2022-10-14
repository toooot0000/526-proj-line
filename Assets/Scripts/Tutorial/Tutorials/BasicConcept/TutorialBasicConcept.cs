using Tutorial.Common;
using Tutorials;
using Tutorials.Utility;
using UnityEngine;

namespace Tutorial.Tutorials.BasicConcept{
    public class TutorialBasicConcept: TutorialBase{ 
        public new const string PrefabName = "TutorialBasicConcept";
        
        public TutorialText[] desc;
        public TouchCatcher touchCatcher;
        public RectTransform ballPosition;
        public TutorialMovingPointer movingPointer;

        private IStepBase[] _steps;
        protected override IStepBase[] Steps => _steps;

        public override void OnLoaded(TutorialManager mng){
            _steps = new IStepBase[]{
                new StepTouchToContinue(desc[0], touchCatcher, mng.stageManager.playerView.gameObject),
                new StepTouchToContinue(desc[1], touchCatcher, mng.stageManager.enemyView.gameObject),
                new StepTouchToContinue(desc[2], touchCatcher, mng.ballManager.balls[0].gameObject),
                new Step4(),
            };
            mng.stageManager.HandOverControlTo(this);
            mng.stageManager.TutorSetPause(true);
            foreach (var tutorialText in desc){
                tutorialText.Enable = false;
            }
            movingPointer.Enabled = false;
            base.OnLoaded(mng);
        }

        protected override void Complete(){
            tutorialManager.stageManager.TutorSetPause(false);
            tutorialManager.stageManager.GainBackControlFrom(this);
            base.Complete();
        }
    }
}