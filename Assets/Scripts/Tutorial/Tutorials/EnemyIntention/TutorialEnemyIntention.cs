using Tutorial.Common;
using Tutorial.Utility;

namespace Tutorial.Tutorials.EnemyIntention{
    public class TutorialEnemyIntention: TutorialBase{
        public new const string PrefabName = "TutorialEnemyIntention";
        
        private StepBase[] _step;
        public TutorialText[] texts;
        public TutorialTapCatcher tutorialTapCatcher;
        protected override StepBase[] Steps => _step;
        
        public override void OnLoaded(TutorialManager mng){
            foreach (var text in texts){
                text.Enabled = false;
            }

            var obj = mng.stageManager.enemyView.intentionDisplayer.gameObject;
            _step = new StepBase[]{
                new StepTapToContinue<TutorialEnemyIntention>(texts[0], tutorialTapCatcher, obj),
                new StepTapToContinue<TutorialEnemyIntention>(texts[1], tutorialTapCatcher, obj)
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