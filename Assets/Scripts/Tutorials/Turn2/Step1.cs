namespace Tutorials.Turn2{
    public partial class TutorialTurn2{
        private partial class Step1: IStepBase{
            public virtual void SetUp(TutorialBase tutorial){
                var ttr = (TutorialTurn2)tutorial;
                ttr.meshes[0].enabled = true;
                ttr.LiftToFront(ttr.tutorialManager.stageManager.enemyView.intentionDisplayer.gameObject);
                ttr.touchCatcher.Enabled = true;
                ttr.touchCatcher.OnTouched += ttr.StepComplete;
            }

            public virtual void Complete(TutorialBase tutorial){
                var ttr = (TutorialTurn2)tutorial;
                ttr.PutToBack(ttr.tutorialManager.stageManager.enemyView.intentionDisplayer.gameObject);
                ttr.meshes[0].enabled = false;
                ttr.touchCatcher.OnTouched -= ttr.StepComplete;
            }
        }
    }
}