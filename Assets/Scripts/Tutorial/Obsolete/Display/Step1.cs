using Tutorial;

namespace Tutorials.Display{
    public partial class TutorialDisplay{
        private partial class Step1: IStepBase{
            public virtual void SetUp(TutorialBase tutorial){
                var ttr = (TutorialDisplay)tutorial;
                ttr.desc[0].enabled = true;
                ttr.LiftToFront(ttr.tutorialManager.stageManager.enemyView.gameObject);
                ttr.touchCatcher.OnTouched += ttr.StepComplete;
            }

            public virtual void Complete(TutorialBase tutorial){
                var ttr = (TutorialDisplay)tutorial;
                ttr.PutToBack(ttr.tutorialManager.stageManager.enemyView.gameObject);
                ttr.desc[0].enabled = false;
                ttr.touchCatcher.OnTouched -= ttr.StepComplete;
            }
        }
    }
}