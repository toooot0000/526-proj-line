using System.Linq;
using Tutorial;
using UI;
using UI.GearDisplayer;

namespace Tutorials.Charge{
    public partial class TutorialCharge{
        private partial class Step5{
            private GearDisplayer _displayer;
            public virtual void SetUp(TutorialBase tutorial){
                var ttr = (TutorialCharge)tutorial;
                ttr.meshes[4].enabled = true;
                ttr.touchCatcher.Enabled = true;
                _displayer = (GearDisplayer)UIManager.shared.uiComponents.First(comp => comp is GearDisplayer);
                _displayer.Show();
                ttr.touchCatcher.OnTouched += ttr.StepComplete;
            }

            public virtual void Complete(TutorialBase tutorial){
                var ttr = (TutorialCharge)tutorial;
                ttr.meshes[4].enabled = false;
                ttr.tutorialManager.tracker.tutorKeepLine = false;
                ttr.touchCatcher.OnTouched -= ttr.StepComplete;
            }
        }
    }
}