using Tutorial.Common;
using Tutorial.Utility;
using UI;
using UI.NextEnemyDisplayer;

namespace Tutorial.Tutorials.EnemyUpdate{
    /// <summary>
    /// 当第二关第一只怪物死掉的时候触发，告诉玩家一关会有多个敌人
    /// </summary>
    public class TutorialEnemyUpdate : TutorialBase{

        public new const string PrefabName = "TutorialEnemyUpdate";
        private StepBase[] _step;
        public TutorialText[] texts;
        public TutorialTapCatcher tutorialTapCatcher;
        protected override StepBase[] Steps => _step;
        
        public override void OnLoaded(TutorialManager mng){
            
            mng.stageManager.HandOverControlTo(this);
            mng.stageManager.TutorSetPause(true);
            
            _step = new StepBase[]{
                new StepTapToContinue<TutorialEnemyUpdate>(texts[0], tutorialTapCatcher),
                new StepTapToContinue<TutorialEnemyUpdate>(texts[1], tutorialTapCatcher, 
                    setUp: (t, s) => {
                        // Triggered on stage new enemy update
                        t.tutorialManager.stageManager.TutorSetPause(false);
                        t.tutorialManager.stageManager.OnEnemyAppear += (stage) => {
                            t.tutorialManager.stageManager.TutorSetPause(true);
                            s.AddHighlightObject(t.tutorialManager.stageManager.enemyView.gameObject);
                            StepTapToContinue<TutorialEnemyUpdate>.DefaultSetUp(t, s);
                        };
                    }
                ),
                new StepTapToContinue<TutorialEnemyUpdate>(texts[2], tutorialTapCatcher,
                    setUp: (t, s) => {
                        UIManager.shared.GetUIComponent<NextEnemyDisplayer>().Show();
                        StepTapToContinue<TutorialEnemyUpdate>.DefaultSetUp(t, s);
                    }
                )
            };
            
            base.OnLoaded(mng);
        }

        protected override void Complete(){
            tutorialManager.stageManager.TutorSetPause(false);
            tutorialManager.stageManager.GainBackControlFrom(this);
            base.Complete();
        }
    }
}