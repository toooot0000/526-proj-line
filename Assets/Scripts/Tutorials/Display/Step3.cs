namespace Tutorials.Display{
    public partial class TutorialDisplay{
        private partial class Step3: IStepBase{
            public virtual void SetUp(TutorialBase tutorial){
                var ttr = (TutorialDisplay)tutorial;
                ttr.desc[2].enabled = true;
                ttr.LiftToFront(ttr.tutorialManager.stageManager.enemyView.intentionDisplayer.gameObject);
                ttr.touchCatcher.OnTouched += ttr.StepComplete;
            }

            public virtual void Complete(TutorialBase tutorial){
                var ttr = (TutorialDisplay)tutorial;
                ttr.PutToBack(ttr.tutorialManager.stageManager.enemyView.intentionDisplayer.gameObject);
                ttr.desc[2].enabled = false;
                ttr.touchCatcher.OnTouched -= ttr.StepComplete;
            }
        }
    }
}