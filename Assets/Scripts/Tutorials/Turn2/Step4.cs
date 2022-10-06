namespace Tutorials.Turn2{
    public partial class TutorialTurn2{
        private partial class Step4{
            public override void SetUp(TutorialBase tutorial){
                var ttr = (TutorialTurn2)tutorial;
                ttr.meshes[3].enabled = true;
                ttr.touchCatcher.Enabled = true;
                ttr.touchCatcher.OnTouched += ttr.StepComplete;
            }

            public override void Complete(TutorialBase tutorial){
                var ttr = (TutorialTurn2)tutorial;
                ttr.meshes[3].enabled = false;
                ttr.tutorialManager.tracker.tutorKeepLine = false;
                ttr.touchCatcher.OnTouched -= ttr.StepComplete;
            }
        }
    }
}