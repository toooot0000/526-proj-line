using System.Linq;
using Tutorial.Common;
using Tutorial.Utility;
using UnityEngine;

namespace Tutorial.Tutorials.BasicConcept{
    public class TutorialBasicConcept: TutorialBase{ 
        public new const string PrefabName = "TutorialBasicConcept";
        
        public TutorialText[] desc;
        public TutorialTapCatcher tutorialTapCatcher;
        public RectTransform ballPosition;
        public TutorialMovingPointer movingPointer;

        private StepBase[] _steps;
        protected override StepBase[] Steps => _steps;

        public override void OnLoaded(TutorialManager mng){
            _steps = new StepBase[]{
                new StepTapToContinue<TutorialBasicConcept>(desc[0], tutorialTapCatcher, mng.stageManager.playerView.gameObject),
                new StepTapToContinue<TutorialBasicConcept>(desc[1], tutorialTapCatcher, mng.stageManager.enemyView.gameObject),
                new StepTapToContinue<TutorialBasicConcept>(desc[2], tutorialTapCatcher, mng.ballManager.balls.First().gameObject),
                new Step4(),
            };
            mng.stageManager.HandOverControlTo(this);
            mng.stageManager.TutorSetPause(true);
            foreach (var tutorialText in desc){
                tutorialText.Enabled = false;
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