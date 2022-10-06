using System.Linq;
using Core.DisplayArea.Stage;
using Core.DisplayArea.Stage.Enemy;
using Core.DisplayArea.Stage.Player;
using Core.PlayArea.Balls;
using Core.PlayArea.TouchTracking;
using Model;
using UnityEngine;

namespace Tutorials.SliceBall{
    public class TutorialContextSliceBall : TutorialContextBase{
        public BallManager ballManager;
        public StageManager stageManager;
        public TouchTracker tracker;
    }


    public partial class TutorialSliceBall : TutorialBase{
        public MovingPointer movingPointer;

        public Transform start;
        public Transform end;
        public Transform ballPoint;
        
        private BallView _attackBallView;
        private int _currentStepIndex;
        private BallView _defendBallView;
        private EnemyView _enemy;
        private PlayerView _player;
        private TouchTracker _tracker;

        protected override StepBase[] Steps{ get; } = new StepBase[]{
            new Step1(),
            new Step2()
        };
        public override void Load(TutorialContextBase context){
            // 1. 重置球的位置; attack: local(0, 0); defend: local(1, 1)
            // 2. 
            var balls = ((TutorialContextSliceBall)context).ballManager.balls;
            _attackBallView = balls.First(b => b.Model.type == BallType.Physics);
            _defendBallView = balls.First(b => b.Model.type == BallType.Defend);
            _tracker = ((TutorialContextSliceBall)context).tracker;
            base.Load(context);
        }

        /// <summary>
        ///     Show how to slice ball
        /// </summary>
        private partial class Step1 : StepBase{ }
        private partial class Step2 : StepBase{ }
    }
}