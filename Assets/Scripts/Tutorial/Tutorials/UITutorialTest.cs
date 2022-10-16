using Tutorial.Common;
using Tutorial.UI;
using Tutorials;
using Tutorials.Utility;
using UI;
using UI.GearDisplayer;

namespace Tutorial.Tutorials{
    public class UITutorialTest: UITutorialBase{
        public new const string PrefabName = "UITutorialTest";
        public TutorialText[] texts;
        private IStepBase[] _step;
        protected override IStepBase[] Steps => _step;

        public override void OnLoaded(TutorialManager mng){

            var gearDisplayer = UIManager.shared.GetUIComponent<GearDisplayer>();
            
            _step = new IStepBase[]{
                new StepTouchToContinue(texts[0], mng.uiController.touchCatcher, gearDisplayer.gameObject)
            };
            mng.uiController.touchCatcher.Enabled = true;
            base.OnLoaded(mng);
        }

        protected override void Complete(){
            tutorialManager.uiController.touchCatcher.Enabled = false;
            base.Complete();
        }
    }
}