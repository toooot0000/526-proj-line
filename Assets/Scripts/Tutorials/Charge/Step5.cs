namespace Tutorials.Charge{
    public partial class TutorialCharge{
        private partial class Step5{
            public override void SetUp(TutorialBase tutorial){
                var ttr = (TutorialCharge)tutorial;
                ttr.meshes[4].enabled = true;
                ttr.touchCatcher.Enabled = true;
                ttr.touchCatcher.OnTouched += ttr.StepComplete;
            }

            public override void Complete(TutorialBase tutorial){
                var ttr = (TutorialCharge)tutorial;
                ttr.meshes[4].enabled = false;
                ttr.tutorialManager.tracker.tutorKeepLine = false;
                ttr.touchCatcher.OnTouched -= ttr.StepComplete;
            }
        }
    }
}