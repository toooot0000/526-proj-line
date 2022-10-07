using System;
using System.Collections;
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
        public bool _pause = false;

        private Model.Stage _modelStage;

        public void HandOverControlTo(TutorialBase tutorial){
            _isInTutorial = true;
        }

        public void GainBackControlFrom(TutorialBase tutorial){
            _isInTutorial = false;
        }

        public void OnStageLoaded(Model.Stage currentStage){
            _modelStage = currentStage;
            enemyView.BindToCurrentEnemy();
        }

        public void ProcessStageActionInfo(StageActionInfoBase info){
            playerView.stageActionInfo = info;
            enemyView.stageActionInfo = info;
            var procedure = info switch{
                StageActionInfoPlayerAttack attack => ProcessPlayerAttack(attack),
                StageActionInfoEnemyAttack attack => ProcessEnemyAttack(attack),
                StageActionInfoEnemyDefend defend => ProcessEnemyDefend(defend),
                StageActionInfoEnemySpecial special => ProcessEnemySpecialAttack(special),
                _ => throw new ArgumentOutOfRangeException(nameof(info), info, null)
            };
            StartCoroutine(procedure);
        }

        private IEnumerator ProcessPlayerAttack(StageActionInfoPlayerAttack info){
            yield return new WaitWhile(() => _pause);
            info.Execute();
            ballManager.FlyAllBalls(this, 0.5f);
            yield return new WaitForSeconds(0.4f);
            yield return new WaitWhile(() => _pause);
            playerView.Attack(null);
            yield return CoroutineUtility.Delayed(0.07f, () => enemyView.TakeDamage(null));
            GameManager.shared.tutorialManager.LoadTutorial("TutorialDisplay");
            yield return new WaitWhile(() => _pause);
            yield return OnPlayerAttackResolved(info);
        }

        private IEnumerator ProcessEnemyAttack(StageActionInfoEnemyAttack info){
            yield return new WaitWhile(() => _pause);
            info.Execute();
            enemyView.Attack(null);
            yield return new WaitForSeconds(0.1f);
            yield return playerView.TakeDamage();
            yield return new WaitWhile(() => _pause);
            GameManager.shared.SwitchTurn();
            yield return new WaitWhile(() => _pause);
        }

        private IEnumerator ProcessEnemySpecialAttack(StageActionInfoEnemySpecial info){
            yield return new WaitWhile(() => _pause);
            info.Execute();
            enemyView.SpecialAttack(null);
            yield return new WaitForSeconds(0.1f);
            yield return playerView.TakeDamage();
            yield return new WaitWhile(() => _pause);
            GameManager.shared.SwitchTurn();
            yield return new WaitWhile(() => _pause);
        }

        private IEnumerator ProcessEnemyDefend(StageActionInfoEnemyDefend info){
            yield return new WaitWhile(() => _pause);
            info.Execute();
            yield return enemyView.Defend();
            yield return new WaitWhile(() => _pause);
            GameManager.shared.SwitchTurn();
            yield return new WaitWhile(() => _pause);
        }

        private IEnumerator OnPlayerAttackResolved(StageActionInfoBase info){
            var dmg = info.damage;
            if (enemyView.isDead){
                if (dmg.currentGame.currentStage.NextEnemy != null){
                    dmg.currentGame.currentStage.ForwardCurrentEnemy();
                    enemyView.BindToCurrentEnemy();
                    yield return enemyView.Appear();
                    yield return new WaitWhile(() => _pause);
                    yield return CoroutineUtility.Delayed(1f, GameManager.shared.SwitchTurn);
                } else{
                    if (!_modelStage.IsLast){
                        if (_modelStage.bonusCoins == -1)
                            UIManager.shared.OpenUI("UISelectGear", _modelStage.bonusGears);
                        else
                            UIManager.shared.OpenUI("UIGetCoins", _modelStage.bonusCoins);
                    } else{
                        UIManager.shared.OpenUI("UIGameComplete");
                    }
                }
            } else{
                yield return CoroutineUtility.Delayed(1f, GameManager.shared.SwitchTurn);
            }
        }

        public void TutorSetPause(bool value){
            if (!_isInTutorial) return;
            _pause = value;
        }
    }
}