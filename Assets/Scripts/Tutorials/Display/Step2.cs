namespace Tutorials.Display{
    public partial class TutorialDisplay{
        private partial class Step2: StepBase{
            public override void SetUp(TutorialBase tutorial){
                var ttr = (TutorialDisplay)tutorial;
                ttr.desc[1].enabled = true;
                ttr.LiftToFront(ttr.tutorialManager.stageManager.playerView.gameObject);
                ttr.touchCatcher.OnTouched += ttr.StepComplete;
            }

            public override void Complete(TutorialBase tutorial){
                var ttr = (TutorialDisplay)tutorial;
                ttr.PutToBack(ttr.tutorialManager.stageManager.playerView.gameObject);
                ttr.desc[1].enabled = false;
                ttr.touchCatcher.OnTouched -= ttr.StepComplete;
            }
        }
    }
}