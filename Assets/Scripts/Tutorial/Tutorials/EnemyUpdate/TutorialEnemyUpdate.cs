using Tutorial.Common;
using Tutorial.Utility;

namespace Tutorial.Tutorials.EnemyUpdate{
    /// <summary>
    /// 当第一只怪物死掉的时候触发，告诉玩家需要
    /// </summary>
    public class TutorialEnemyUpdate : TutorialBase{

        // TODO
        
        public new const string PrefabName = "TutorialEnemyUpdate";
        private StepBase[] _step;
        public TutorialText[] texts;
        public TouchCatcher touchCatcher;
        protected override StepBase[] Steps => _step;
        
        public override void OnLoaded(TutorialManager mng){
            foreach (var text in texts){
                text.Enable = false;
            }
            _step = new StepBase[]{
                new StepTouchToContinue(texts[0], touchCatcher)
            };
            
            mng.turnSignDisplayer.HandOverControlTo(this);
            mng.turnSignDisplayer.TutorSetPause(true);
            base.OnLoaded(mng);
        }

        protected override void Complete(){
            tutorialManager.turnSignDisplayer.TutorSetPause(false);
            tutorialManager.turnSignDisplayer.GainBackControlFrom(this);
            base.Complete();
        }
    }
}