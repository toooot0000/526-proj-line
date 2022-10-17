using Tutorial.Common;
using Tutorial.UI;
using Tutorial.Utility;
using UI;
using UI.Interfaces.SelectGear;

namespace Tutorial.Tutorials.Stage1Clear{
    public class TutorialStage1Clear: UITutorialBase{

        private StepBase[] _step;
        protected override StepBase[] Steps => _step;

        public TutorialText[] texts;

        public override void OnLoaded(TutorialManager mng){
            
            mng.stageManager.HandOverControlTo(this);
            mng.stageManager.TutorSetPause(true);            
            
            _step = new StepBase[]{
                new StepTapToContinue(texts[0], mng.uiController.touchCatcher),
                new StepTapToContinue(texts[1], mng.uiController.touchCatcher, (t, s) => {
                    t.tutorialManager.stageManager.TutorSetPause(false);
                    UIManager.shared.OnOpenUI += ui => {
                        if (ui is not UISelectGear selectGear) return;
                        var step = (StepTapToContinue)s;
                        t.tutorialManager.stageManager.TutorSetPause(true);
                        step.AddHighlightObject(selectGear.GetFirstPanel().gameObject);
                    };
                }),
                new StepTapToContinue(texts[2], mng.uiController.touchCatcher, (t, s) => {
                    var selectGear = UIManager.shared.GetUI<UISelectGear>();
                    if (selectGear == null) return;
                    var step = (StepTapToContinue)s;
                    step.AddHighlightObject(selectGear.confirmButton.gameObject);
                })
                                
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