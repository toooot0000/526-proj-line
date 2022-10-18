using System;
using System.Collections;
using Core.DisplayArea.Stage.Enemy;
using Core.DisplayArea.Stage.Player;
using Core.PlayArea.Balls;
using Model;
using Tutorial;
using Tutorial.Tutorials.EnemyUpdate;
using Tutorial.Tutorials.Stage1Clear;
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

        private Model.Stage _modelStage;
        private bool _isInTutorial;
        private bool _pause = false;
        public event TutorialControllableEvent OnEnemyAppear;

        public void HandOverControlTo(TutorialBase tutorial){
            _isInTutorial = true;
        }

        public void GainBackControlFrom(TutorialBase tutorial){
            _isInTutorial = false;
        }

        public void PresentStage(Model.Stage currentStage){
            _modelStage = currentStage;
            enemyView.BindToCurrentEnemy();
            enemyView.Appear(null);
            if(_isInTutorial) OnEnemyAppear?.Invoke(this);
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
            // yield return CoroutineUtility.Delayed(0.07f, () => enemyView.TakeDamage(null));
            yield return new WaitForSeconds(0.07f);
            yield return enemyView.TakeDamage();
            // GameManager.shared.tutorialManager.LoadTutorial("TutorialDisplay");
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
            if(playerView.isDead) UIManager.shared.OpenUI("UIGameEnd");
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
            if(playerView.isDead) UIManager.shared.OpenUI("UIGameEnd");
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
                OnEnemyDieTriggerTutorial();
                if (dmg.currentGame.currentStage.NextEnemy != null){
                    dmg.currentGame.currentStage.ForwardCurrentEnemy();
                    yield return new WaitWhile(() => _pause);
                    enemyView.BindToCurrentEnemy();
                    yield return enemyView.Appear(); 
                    if(_isInTutorial) OnEnemyAppear?.Invoke(this);
                    yield return new WaitWhile(() => _pause);
                    yield return CoroutineUtility.Delayed(1f, GameManager.shared.SwitchTurn);
                } else{
                    yield return new WaitWhile(() => _pause);
                    if (!_modelStage.IsLast){
                        if (_modelStage.bonusCoins == -1){
                            yield return new WaitWhile(() => _pause);
                            UIManager.shared.OpenUI("UISelectGear", _modelStage.bonusGears);
                        } else{
                            yield return new WaitWhile(() => _pause);
                            UIManager.shared.OpenUI("UIGetCoins", _modelStage.bonusCoins);
                        }
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

        private static void OnEnemyDieTriggerTutorial(){
            switch (GameManager.shared.game.currentStage.id){
                case 0:
                    GameManager.shared.tutorialManager.LoadTutorial<UITutorialStage1Clear>();
                    break;
                case 1:
                    GameManager.shared.tutorialManager.LoadTutorial<TutorialEnemyUpdate>();
                    break;
            }
        }
    }
}