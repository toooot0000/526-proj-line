namespace Tutorials.Charge{
    public partial class TutorialCharge{
        private partial class Step4{
            public virtual void SetUp(TutorialBase tutorial){
                var ttr = (TutorialCharge)tutorial;
                ttr.meshes[3].enabled = true;
                ttr.touchCatcher.Enabled = true;
                ttr.touchCatcher.OnTouched += ttr.StepComplete;
                ttr.LiftToFront(ttr.tutorialManager.actionDetailDisplayer.gameObject);
            }

            public virtual void Complete(TutorialBase tutorial){
                var ttr = (TutorialCharge)tutorial;
                ttr.meshes[3].enabled = false;
                ttr.PutToBack(ttr.tutorialManager.actionDetailDisplayer.gameObject);
                ttr.touchCatcher.OnTouched -= ttr.StepComplete;
            }
        }
    }
}