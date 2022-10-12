using System.Linq;
using Core.DisplayArea.Stage;
using Core.PlayArea.Balls;
using Model;
using TMPro;
using Tutorials.Common;
using UnityEngine;

namespace Tutorials.Turn2{
    public partial class TutorialTurn2: TutorialBase{
        protected override IStepBase[] Steps{ get; } = new IStepBase[]{
            new Step1(), new Step2(), new Step3(), new Step4()
        };

        public TextMeshProUGUI[] meshes;
        public TouchCatcher touchCatcher;
        public TutorialMovingPointer tutorialMovingPointer;
        public Transform startPoint;
        public Transform endPoint;

        private BallView _attBallView;
        private BallView _defBallView;

        public override void OnLoaded(TutorialManager mng){

            mng.stageManager.HandOverControlTo(this);
            mng.stageManager.TutorSetPause(true);
            
            mng.ballManager.HandOverControlTo(this);
            _attBallView = mng.ballManager.balls.First(b => b.Model.type == BallType.Physics);
            _defBallView = mng.ballManager.balls.First(b => b.Model.type == BallType.Defend);

            
            mng.tracker.HandOverControlTo(this);
            
            tutorialMovingPointer.Enabled = false;
            foreach (var mesh in meshes) mesh.enabled = false;
            base.OnLoaded(mng);
        }

        protected override void Complete(){
            tutorialManager.stageManager.TutorSetPause(false);
            tutorialManager.stageManager.GainBackControlFrom(this);
            tutorialManager.ballManager.GainBackControlFrom(this);
            tutorialManager.tracker.GainBackControlFrom(this);
            base.Complete();
        }
        
        private partial class Step1: IStepBase{ }
        private partial class Step2: IStepBase{ }
        private partial class Step3: IStepBase{ }
        private partial class Step4: IStepBase{ }
    }
}