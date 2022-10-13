using Tutorials.Common;
using Tutorials.Utility;

namespace Tutorials.TurnSign{
    public class TutorialTurnSign: TutorialBase{
        public new const string PrefabName = "TutorialTurnSign";
        public TutorialText[] texts;
        public TouchCatcher touchCatcher;
        
        private IStepBase[] _step;
        protected override IStepBase[] Steps => _step;

        public override void OnLoaded(TutorialManager mng){
            foreach (var text in texts){
                text.Enable = false;
            }
            _step = new IStepBase[]{
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