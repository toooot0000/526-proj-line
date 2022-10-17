using Tutorial.Common;
using Tutorial.Utility;

namespace Tutorial.Tutorials.TurnSign{
    public class TutorialTurnSign: TutorialBase{
        public new const string PrefabName = "TutorialTurnSign";
        public TutorialText[] texts;
        public TouchCatcher touchCatcher;
        
        private StepBase[] _step;
        protected override StepBase[] Steps => _step;

        public override void OnLoaded(TutorialManager mng){
            foreach (var text in texts){
                text.Enable = false;
            }
            _step = new StepBase[]{
                new StepTouchToContinue(texts[0], touchCatcher),
                new StepTouchToContinue(texts[1], touchCatcher)
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