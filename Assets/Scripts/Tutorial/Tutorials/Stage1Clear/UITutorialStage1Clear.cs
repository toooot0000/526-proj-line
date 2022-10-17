using Tutorial.Common;
using Tutorial.UI;
using Tutorial.Utility;
using UI;
using UI.Interfaces.SelectGear;
using UI.Interfaces.SelectStage;

namespace Tutorial.Tutorials.Stage1Clear{
    public class UITutorialStage1Clear: UITutorialBase{
        
        public new const string PrefabName = "UITutorialStage1Clear";
        
        private StepBase[] _step;
        protected override StepBase[] Steps => _step;

        public TutorialText[] texts;

        public override void OnLoaded(TutorialManager mng){
            
            mng.stageManager.HandOverControlTo(this);
            mng.stageManager.TutorSetPause(true);
            
            foreach (var tutorialText in texts){
                tutorialText.Enabled = false;
            }

            _step = new StepBase[]{
                new StepTapToContinue(texts[0], mng.uiController.touchCatcher),
                new StepTapToContinue(texts[1], mng.uiController.touchCatcher, (t, s) => {
                    t.tutorialManager.stageManager.TutorSetPause(false);
                    UIManager.shared.OnOpenUI += ui => {
                        if (ui is not UISelectGear selectGear) return;
                        var step = (StepTapToContinue)s;
                        t.tutorialManager.stageManager.TutorSetPause(true);
                        step.AddHighlightObject(selectGear.GetFirstPanel().gameObject);
                        StepTapToContinue.DefaultSetUp(t, s);
                    };
                }),
                new StepTapToContinue(texts[2], null, (t, s) => {
                    var selectGear = UIManager.shared.GetUI<UISelectGear>();
                    if (selectGear == null) return;
                    var step = (StepTapToContinue)s;
                    step.AddHighlightObject(selectGear.confirmButton.gameObject);
                    step.HighlightAll(t);
                    step.SetTextEnabled(true);
                }, (t, s) => {
                    var step = (StepTapToContinue)s;
                    step.LowlightAll(t);
                    step.SetTextEnabled(false);
                }, bind: (tutorial, step) => {
                    var selectGear = UIManager.shared.GetUI<UISelectGear>();
                    if (selectGear == null) return;
                    selectGear.OnConfirmClicked += tutorial.StepComplete;
                }, unbind: (tutorial, step) => {
                    var selectGear = UIManager.shared.GetUI<UISelectGear>();
                    if (selectGear == null) return;
                    selectGear.OnConfirmClicked -= tutorial.StepComplete;
                }),
                new StepTapToContinue(texts[3], null, (t, s) => {
                    UIManager.shared.OnOpenUI += ui => {
                        if (ui is not UISelectStage selectStage) return;
                        var step = (StepTapToContinue)s;
                        step.AddHighlightObject(selectStage.confirmButton.gameObject);
                        step.HighlightAll(t);
                        step.SetTextEnabled(true);
                    };
                }, (t, s) => {
                    var step = (StepTapToContinue)s;
                    step.LowlightAll(t);
                    step.SetTextEnabled(false);
                }, bind: (tutorial, step) => {
                    UIManager.shared.OnOpenUI += ui => {
                        if (ui is not UISelectStage selectStage) return;
                        selectStage.OnConfirmClicked += tutorial.StepComplete;
                    };
                }, unbind: (tutorial, step) => {
                    var selectStage = UIManager.shared.GetUI<UISelectStage>();
                    if (selectStage == null) return;
                    selectStage.OnConfirmClicked -= tutorial.StepComplete;
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