using System.Linq;
using Core.PlayArea.Balls;
using Model;
using TMPro;
using Tutorials.Common;
using UnityEngine;

namespace Tutorials.Charge{
    /// <summary>
    /// 1. "Enemy is going to do a special attack. Better to be prepared!"
    /// 2. "Circle the shield ball to trigger the charge effect"
    /// 3. "You can check your current action result here.\nNow you will gain 5 shield that will counter the enemy's heavy attack!"
    /// </summary>
    public partial class TutorialCharge: TutorialBase{
        protected override IStepBase[] Steps{ get; } = new IStepBase[]{
            new Step1(), new Step2(), new Step3(), new Step4(), new Step5()
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
        private partial class Step5: IStepBase{ }
    }
}