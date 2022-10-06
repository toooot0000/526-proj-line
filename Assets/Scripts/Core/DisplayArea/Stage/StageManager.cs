using System;
using Core.DisplayArea.Stage.Enemy;
using Core.DisplayArea.Stage.Player;
using Core.PlayArea.Balls;
using Model;
using Tutorials;
using UI;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Stage{
    /// <summary>
    ///     Be responsible for the stage process
    /// </summary>
    public class StageManager : MonoBehaviour, ITutorialControllable{
        public PlayerView playerView;
        public EnemyView enemyView;
        public BallManager ballManager;
        private bool _isInTutorial;

        private Model.Stage _modelStage;

        public void ControlledByTutorial(TutorialBase tutorial){
            _isInTutorial = true;
        }

        public void GainBackControl(TutorialBase tutorial){
            _isInTutorial = false;
        }

        public void OnStageLoaded(Model.Stage currentStage){
            _modelStage = currentStage;
            enemyView.BindToCurrentEnemy();
        }

        public void ProcessStageActionInfo(StageActionInfoBase info){
            info.Execute();
            switch (info){
                case StageActionInfoPlayerAttack attack:
                    ProcessPlayerAttack(attack);
                    break;
                case StageActionInfoEnemyAttack attack:
                    ProcessEnemyAttack(attack);
                    break;
                case StageActionInfoEnemyDefend defend:
                    ProcessEnemyDefend(defend);
                    break;
                case StageActionInfoEnemySpecial special:
                    ProcessEnemySpecialAttack(special);
                    break;
            }
        }

        private void ProcessPlayerAttack(StageActionInfoPlayerAttack info){
            var dmgWrp = new StageActionInfoWrapper{
                source = playerView,
                target = enemyView,
                actionInfo = info,
                resolvedCallback = OnPlayerAttackResolved
            };
            playerView.wrappedActionInfo = dmgWrp;
            enemyView.wrappedActionInfo = dmgWrp;
            ballManager.FlyAllBalls(this, 0.5f);
            StartCoroutine(CoroutineUtility.Delayed(0.4f, playerView.Attack));
        }

        private void ProcessEnemyAttack(StageActionInfoEnemyAttack info){
            var infoWrp = new StageActionInfoWrapper{
                source = enemyView,
                target = playerView,
                actionInfo = info,
                resolvedCallback = wrapper =>
                    StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.SwitchTurn))
            };
            playerView.wrappedActionInfo = infoWrp;
            enemyView.wrappedActionInfo = infoWrp;
            enemyView.Attack();
        }

        private void ProcessEnemySpecialAttack(StageActionInfoEnemySpecial info){
            var infoWrp = new StageActionInfoWrapper{
                source = enemyView,
                target = playerView,
                actionInfo = info,
                resolvedCallback = wrapper =>
                    StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.SwitchTurn))
            };
            playerView.wrappedActionInfo = infoWrp;
            enemyView.wrappedActionInfo = infoWrp;
            enemyView.SpecialAttack();
        }

        private void ProcessEnemyDefend(StageActionInfoEnemyDefend info){
            var infoWrp = new StageActionInfoWrapper{
                source = enemyView,
                target = null,
                actionInfo = info,
                resolvedCallback = wrapper =>
                    StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.SwitchTurn))
            };
            playerView.wrappedActionInfo = infoWrp;
            enemyView.wrappedActionInfo = infoWrp;
            enemyView.Defend();
        }

        private void OnPlayerAttackResolved(StageActionInfoWrapper wrapped){
            var dmg = (wrapped.actionInfo as StageActionInfoPlayerAttack)!.damage;
            if (enemyView.isDead){
                if (dmg.currentGame.currentStage.NextEnemy != null){
                    dmg.currentGame.currentStage.ForwardCurrentEnemy();
                    enemyView.BindToCurrentEnemy();
                    enemyView.Appear(
                        () => StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.SwitchTurn))
                    );
                } else{
                    if (_modelStage.nextStage != -1){
                        if (_modelStage.bonusCoins == -1)
                            UIManager.shared.OpenUI("UISelectGear", _modelStage.bonusGears);
                        else
                            UIManager.shared.OpenUI("UIGetCoins", _modelStage.bonusCoins);
                    } else{
                        UIManager.shared.OpenUI("UIGameComplete");
                    }
                }
            } else{
                StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.SwitchTurn));
            }
        }

        public class StageActionInfoWrapper{
            public StageActionInfoBase actionInfo;
            public Action<StageActionInfoWrapper> resolvedCallback;
            public DamageableView source;
            public DamageableView target;
        }
    }
}