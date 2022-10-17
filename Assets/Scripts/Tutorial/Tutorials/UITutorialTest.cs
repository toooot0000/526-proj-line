using System;
using Tutorial.Common;
using Tutorial.UI;
using Tutorial.Utility;
using UI;
using UI.GearDisplayer;
using UI.Interfaces;

namespace Tutorial.Tutorials{
    [Obsolete]
    public class UITutorialTest: UITutorialBase{
        public new const string PrefabName = "UITutorialTest";
        public TutorialText[] texts;
        private StepBase[] _step;
        protected override StepBase[] Steps => _step;

        public override void OnLoaded(TutorialManager mng){

            var gearDisplayer = UIManager.shared.GetUIComponent<GearDisplayer>();
            
            _step = new StepBase[]{
                new StepTapToContinue<UITutorialTest>(texts[0], mng.uiController.tutorialTapCatcher, gearDisplayer.gameObject),
                new StepTapToContinue<UITutorialTest>(texts[0], mng.uiController.tutorialTapCatcher, gearDisplayer.gameObject, (tutorial, step) => {
                    tutorial.tutorialManager.uiController.shade.SetActive(false);
                    var gameStart = UIManager.shared.GetUI<UIGameStart>();
                    if (gameStart == null) return;
                    gameStart.OnClose += ui => {
                        tutorial.tutorialManager.uiController.shade.SetActive(true);
                        step.SetUp(tutorial);
                    };
                })
            };
            mng.uiController.tutorialTapCatcher.Enabled = true;
            base.OnLoaded(mng);
        }

        protected override void Complete(){
            tutorialManager.uiController.tutorialTapCatcher.Enabled = false;
            base.Complete();
        }
    }
}