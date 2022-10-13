using System;
using Core.DisplayArea.Stage;
using TMPro;

namespace Tutorials.Display{

    [Obsolete]
    public partial class TutorialDisplay: TutorialBase{
        protected override IStepBase[] Steps{ get; } = new IStepBase[] {
            new Step1(), new Step2(), new Step3()
        };

        public TextMeshProUGUI[] desc;
        public TouchCatcher touchCatcher;

        public override void OnLoaded(TutorialManager mng){
            mng.stageManager.HandOverControlTo(this);
            mng.stageManager.TutorSetPause(true);
            foreach (var mesh in desc){
                mesh.enabled = false;
            }
            base.OnLoaded(mng);
        }

        protected override void Complete(){
            tutorialManager.stageManager.TutorSetPause(false);
            tutorialManager.stageManager.GainBackControlFrom(this);
            base.Complete();
        }

        private partial class Step1 : IStepBase{ }
        private partial class Step2 : IStepBase{ }
        private partial class Step3 : IStepBase{ }
    }
}