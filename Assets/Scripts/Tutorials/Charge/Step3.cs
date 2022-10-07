namespace Tutorials.Charge{
    public partial class TutorialCharge{
        private partial class Step3{
            public override void SetUp(TutorialBase tutorial){
                var ttr = (TutorialCharge)tutorial;
                ttr.meshes[2].enabled = true;
                ttr.touchCatcher.Enabled = true;
                ttr.touchCatcher.OnTouched += ttr.StepComplete;
            }

            public override void Complete(TutorialBase tutorial){
                var ttr = (TutorialCharge)tutorial;
                ttr.meshes[2].enabled = false;
                ttr.touchCatcher.Enabled = false;
                ttr.touchCatcher.OnTouched -= ttr.StepComplete;
                ttr.PutToBack(ttr._defBallView.gameObject);
            }
        }
    }
}