using Tutorial.Common;
using Tutorial.UI;
using Tutorial.Utility;
using UI;
using UI.Interfaces.SelectStage;

namespace Tutorial.Tutorials.StageTypes{
    public class UITutorialStageTypes: UITutorialBase{
        public new const string PrefabName = "UITutorialStageTypes";
        public TutorialText[] texts;
        private StepBase[] _steps;
        protected override StepBase[] Steps => _steps;

        public override void OnLoaded(TutorialManager mng){

            _steps = new StepBase[]{
                new StepTapToContinue<UITutorialStageTypes>(texts[0], mng.uiController.tutorialTapCatcher),
                new StepTapToContinue<UITutorialStageTypes>(texts[1], mng.uiController.tutorialTapCatcher, 
                    setUp: (t, s) => {
                        var selectStage = UIManager.shared.GetUI<UISelectStage>();
                        selectStage.HandOverControlTo(t);
                        s.AddHighlightObject(selectStage.GetPanelOfId(2).gameObject);
                        StepTapToContinue<UITutorialStageTypes>.DefaultSetUp(t, s);
                    }
                ),
                new StepTapToContinue<UITutorialStageTypes>(texts[2], mng.uiController.tutorialTapCatcher, 
                    setUp: (t, s) => {
                        s.AddHighlightObject(UIManager.shared.GetUI<UISelectStage>().GetPanelOfId(3).gameObject);
                        StepTapToContinue<UITutorialStageTypes>.DefaultSetUp(t, s);
                    },
                    cleanUp: (t, s) => {
                        var selectStage = UIManager.shared.GetUI<UISelectStage>();
                        selectStage.GainBackControlFrom(t);
                        StepTapToContinue<UITutorialStageTypes>.DefaultCleanUp(t, s);
                    }
                ),
                new StepTapToContinue<UITutorialStageTypes>(texts[3], mng.uiController.tutorialTapCatcher)
            };
            
            base.OnLoaded(mng);
        }
    }
}