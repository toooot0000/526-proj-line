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
                new StepTapToContinue<UITutorialStage1Clear>(texts[0], mng.uiController.tutorialTapCatcher),
                new StepTapToContinue<UITutorialStage1Clear>(texts[1], mng.uiController.tutorialTapCatcher, (t, s) => {
                    t.tutorialManager.stageManager.TutorSetPause(false);
                    UIManager.shared.OnOpenUI += ui => {
                        if (ui is not UISelectGear selectGear) return;
                        t.tutorialManager.stageManager.TutorSetPause(true);
                        s.AddHighlightObject(selectGear.GetFirstPanel().gameObject);
                        StepTapToContinue<UITutorialStage1Clear>.DefaultSetUp(t, s);
                    };
                }),
                new StepTapToContinue<UITutorialStage1Clear>(texts[2], null, (t, s) => {
                    var selectGear = UIManager.shared.GetUI<UISelectGear>();
                    if (selectGear == null) return;
                    s.AddHighlightObject(selectGear.confirmButton.gameObject);
                    s.HighlightAll(t);
                    s.SetTextEnabled(true);
                }, (t, s) => {
                    s.LowlightAll(t);
                    s.SetTextEnabled(false);
                }, bind: (tutorial, step) => {
                    var selectGear = UIManager.shared.GetUI<UISelectGear>();
                    if (selectGear == null) return;
                    selectGear.OnConfirmClicked += tutorial.StepComplete;
                }, unbind: (tutorial, step) => {
                    var selectGear = UIManager.shared.GetUI<UISelectGear>();
                    if (selectGear == null) return;
                    selectGear.OnConfirmClicked -= tutorial.StepComplete;
                }),
                new StepTapToContinue<UITutorialStage1Clear>(texts[3], mng.uiController.tutorialTapCatcher, setUp: (t, s) => {
                    UIManager.shared.OnOpenUI += ui => {
                        if (ui is not UISelectStage selectStage) return;
                        s.AddHighlightObject(selectStage.GetFirstPanel().gameObject);
                        s.HighlightAll(t);
                        s.SetTextEnabled(true);
                    };
                }),
                new StepTapToContinue<UITutorialStage1Clear>(texts[4], null, (t, step) => {
                    var selectStage = UIManager.shared.GetUI<UISelectStage>();
                    step.AddHighlightObject(selectStage.confirmButton.gameObject);
                    step.HighlightAll(t);
                    step.SetTextEnabled(true);
                }, (t, step) => {
                    step.LowlightAll(t);
                    step.SetTextEnabled(false);
                }, bind: (tutorial, step) => {
                    var selectStage = UIManager.shared.GetUI<UISelectStage>();
                    if (selectStage == null) return;
                    selectStage.OnConfirmClicked += tutorial.StepComplete;
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