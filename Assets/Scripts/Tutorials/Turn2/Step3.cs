namespace Tutorials.Turn2{
    public partial class TutorialTurn2{
        private partial class Step3{
            public override void SetUp(TutorialBase tutorial){
                var ttr = (TutorialTurn2)tutorial;
                ttr.meshes[2].enabled = true;
                ttr.touchCatcher.Enabled = true;
                ttr.touchCatcher.OnTouched += ttr.StepComplete;
            }

            public override void Complete(TutorialBase tutorial){
                var ttr = (TutorialTurn2)tutorial;
                ttr.meshes[2].enabled = false;
                ttr.touchCatcher.Enabled = false;
                ttr.touchCatcher.OnTouched -= ttr.StepComplete;
                ttr.tutorialManager.tracker.tutorKeepLine = false;
                ttr.PutToBack(ttr._attBallView.gameObject);
                ttr.PutToBack(ttr._defBallView.gameObject);
            }
        }
    }
}