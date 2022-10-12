using System.Linq;
using Core.DisplayArea.Stage;
using Core.DisplayArea.Stage.Enemy;
using Core.DisplayArea.Stage.Player;
using Core.PlayArea.Balls;
using Core.PlayArea.TouchTracking;
using Model;
using Tutorials.Common;
using UnityEngine;

namespace Tutorials.SliceBall{
    public class TutorialContextSliceBall : TutorialContextBase{
        public BallManager ballManager;
        public StageManager stageManager;
        public TouchTracker tracker;
    }


    public partial class TutorialSliceBall : TutorialBase{
        public TutorialMovingPointer tutorialMovingPointer;

        public Transform start;
        public Transform end;
        public Transform ballPoint;
        
        private BallView _attackBallView;
        private int _currentStepIndex;
        private BallView _defendBallView;
        private EnemyView _enemy;
        private PlayerView _player;
        private TouchTracker _tracker;

        protected override IStepBase[] Steps{ get; } = new IStepBase[]{
            new Step1()
        };
        public override void OnLoaded(TutorialManager mng){
            var balls = mng.ballManager.balls;
            _attackBallView = balls.First(b => b.Model.type == BallType.Physics);
            _defendBallView = balls.First(b => b.Model.type == BallType.Defend);
            _tracker = mng.tracker;
            base.OnLoaded(mng);
        }

        /// <summary>
        ///     Show how to slice ball
        /// </summary>
        private partial class Step1 : IStepBase{ }
    }
}