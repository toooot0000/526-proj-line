using Tutorial.Common;
using Tutorials;
using Tutorials.Utility;

namespace Tutorial.Tutorials.EnemyIntention{
    public class TutorialEnemyIntention: TutorialBase{
        public new const string PrefabName = "TutorialEnemyIntention";
        
        private IStepBase[] _step;
        public TutorialText[] texts;
        public TouchCatcher touchCatcher;
        protected override IStepBase[] Steps => _step;
        
        public override void OnLoaded(TutorialManager mng){
            foreach (var text in texts){
                text.Enable = false;
            }

            var obj = mng.stageManager.enemyView.intentionDisplayer.gameObject;
            _step = new IStepBase[]{
                new StepTouchToContinue(texts[0], touchCatcher, obj),
                new StepTouchToContinue(texts[1], touchCatcher, obj)
            };
            
            mng.stageManager.HandOverControlTo(this);
            mng.stageManager.TutorSetPause(true);
            base.OnLoaded(mng);
        }

        protected override void Complete(){
            tutorialManager.stageManager.TutorSetPause(false);
            tutorialManager.stageManager.GainBackControlFrom(this);
            base.Complete();
        }
    }
}