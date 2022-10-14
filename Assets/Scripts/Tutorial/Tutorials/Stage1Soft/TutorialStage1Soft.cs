using Tutorial.Common;
using Tutorials;
using Tutorials.Utility;

namespace Tutorial.Tutorials.Stage1Soft{
    public class TutorialStage1Soft: TutorialBase{

        public new const string PrefabName = "TutorialStage1Soft";
        
        private IStepBase[] _step;
        public TutorialText[] texts;
        public TouchCatcher touchCatcher;
        protected override IStepBase[] Steps => _step;
        
        public override void OnLoaded(TutorialManager mng){
            foreach (var text in texts){
                text.Enable = false;
            }
            _step = new IStepBase[]{
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