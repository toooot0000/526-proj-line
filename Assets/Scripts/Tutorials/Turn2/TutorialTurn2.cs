using System.Linq;
using Core.DisplayArea.Stage;
using Core.PlayArea.Balls;
using Model;
using TMPro;
using UnityEngine;

namespace Tutorials.Turn2{
    public partial class TutorialTurn2: TutorialBase{
        protected override StepBase[] Steps{ get; } = new StepBase[]{
            new Step1(), new Step2(), new Step3(), new Step4()
        };

        public TextMeshProUGUI[] meshes;
        public TouchCatcher touchCatcher;
        public MovingPointer movingPointer;
        public Transform startPoint;
        public Transform endPoint;

        private BallView _attBallView;
        private BallView _defBallView;

        public override void Load(TutorialManager mng){

            mng.stageManager.HandOverControlTo(this);
            mng.stageManager.TutorSetPause(true);
            
            mng.ballManager.HandOverControlTo(this);
            _attBallView = mng.ballManager.balls.First(b => b.Model.type == BallType.Physics);
            _defBallView = mng.ballManager.balls.First(b => b.Model.type == BallType.Defend);

            
            mng.tracker.HandOverControlTo(this);
            
            movingPointer.Enabled = false;
            foreach (var mesh in meshes) mesh.enabled = false;
            base.Load(mng);
        }

        protected override void Complete(){
            tutorialManager.stageManager.TutorSetPause(false);
            tutorialManager.stageManager.GainBackControlFrom(this);
            tutorialManager.ballManager.GainBackControlFrom(this);
            tutorialManager.tracker.GainBackControlFrom(this);
            base.Complete();
        }
        
        private partial class Step1: StepBase{ }
        private partial class Step2: StepBase{ }
        private partial class Step3: StepBase{ }
        private partial class Step4: StepBase{ }
    }
}