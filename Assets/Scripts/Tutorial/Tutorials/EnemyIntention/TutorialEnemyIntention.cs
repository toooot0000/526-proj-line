using Tutorial.Common;
using Tutorial.Utility;

namespace Tutorial.Tutorials.EnemyIntention{
    public class TutorialEnemyIntention: TutorialBase{
        public new const string PrefabName = "TutorialEnemyIntention";
        
        private StepBase[] _step;
        public TutorialText[] texts;
        public TouchCatcher touchCatcher;
        protected override StepBase[] Steps => _step;
        
        public override void OnLoaded(TutorialManager mng){
            foreach (var text in texts){
                text.Enable = false;
            }

            var obj = mng.stageManager.enemyView.intentionDisplayer.gameObject;
            _step = new StepBase[]{
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