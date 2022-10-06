using Core.DisplayArea.Stage;
using TMPro;

namespace Tutorials.Display{

    public partial class TutorialDisplay: TutorialBase{
        protected override StepBase[] Steps{ get; } = new StepBase[] {
            new Step1(), new Step2(), new Step3()
        };

        public TextMeshProUGUI[] desc;
        public TouchCatcher touchCatcher;

        public override void Load(TutorialManager mng){
            mng.stageManager.HandOverControlTo(this);
            mng.stageManager.TutorSetPause(true);
            foreach (var mesh in desc){
                mesh.enabled = false;
            }
            base.Load(mng);
        }

        protected override void Complete(){
            tutorialManager.stageManager.TutorSetPause(false);
            tutorialManager.stageManager.GainBackControlFrom(this);
            base.Complete();
        }

        private partial class Step1 : StepBase{ }
        private partial class Step2 : StepBase{ }
        private partial class Step3 : StepBase{ }
    }
}