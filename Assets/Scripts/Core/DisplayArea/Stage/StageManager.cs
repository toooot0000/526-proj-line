using System;
using System.Collections;
using Core.DisplayArea.Stage.Enemy;
using Core.DisplayArea.Stage.Player;
using Core.PlayArea.Balls;
using Model;
using Tutorial;
using Tutorial.Tutorials.BasicConcept;
using Tutorial.Tutorials.Charge;
using Tutorial.Tutorials.Combo;
using Tutorial.Tutorials.EnemyIntention;
using Tutorial.Tutorials.EnemyUpdate;
using Tutorial.Tutorials.Stage1Clear;
using Tutorial.Tutorials.Stage1Soft;
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

        public void ProcessStageAction(StageActionBase action){
            playerView.stageAction = action;
            enemyView.stageAction = action;
            var procedure = action switch{
                StageActionPlayerAction attack => ProcessPlayerAttack(attack),
                StageActionEnemyAttack attack => ProcessEnemyAttack(attack),
                StageActionEnemyDefend defend => ProcessEnemyDefend(defend),
                StageActionEnemySpecial special => ProcessEnemySpecialAttack(special),
                _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
            };
            StartCoroutine(procedure);
        }

        private IEnumerator ProcessPlayerAttack(StageActionPlayerAction action){
            yield return new WaitWhile(() => _pause);
            action.Execute();
            ballManager.FlyAllBalls(this, 0.5f);
            yield return new WaitForSeconds(0.4f);
            yield return new WaitWhile(() => _pause);
            playerView.Attack(null);
            yield return new WaitForSeconds(0.07f);
            yield return enemyView.TakeDamage();
            yield return new WaitWhile(() => _pause);
            yield return AfterPlayerAttacked();
        }

        private IEnumerator ProcessEnemyAttack(StageActionEnemyAttack action){
            yield return new WaitWhile(() => _pause);
            action.Execute();
            enemyView.Attack(null);
            yield return new WaitForSeconds(0.1f);
            yield return playerView.TakeDamage();
            yield return new WaitWhile(() => _pause);
            SwitchTurn();
            yield return new WaitWhile(() => _pause);
            if(playerView.isDead) GameManager.shared.GameEnd();
            yield return new WaitWhile(() => _pause);
        }

        private IEnumerator ProcessEnemySpecialAttack(StageActionEnemySpecial action){
            yield return new WaitWhile(() => _pause);
            action.Execute();
            enemyView.SpecialAttack(null);
            yield return new WaitForSeconds(0.1f);
            yield return playerView.TakeDamage();
            yield return new WaitWhile(() => _pause);
            SwitchTurn();
            yield return new WaitWhile(() => _pause);
            if(playerView.isDead) GameManager.shared.GameEnd();
            yield return new WaitWhile(() => _pause);
        }

        private IEnumerator ProcessEnemyDefend(StageActionEnemyDefend action){
            yield return new WaitWhile(() => _pause);
            action.Execute();
            yield return enemyView.Defend();
            yield return new WaitWhile(() => _pause);
            SwitchTurn();
            yield return new WaitWhile(() => _pause);
        }

        private IEnumerator AfterPlayerAttacked(){
            if (enemyView.isDead){
                OnEnemyDieTriggerTutorial();
                if (GameManager.shared.game.currentStage.NextEnemy != null){
                    yield return MoveToNextEnemy();
                } else{
                    yield return CollectRewards();
                }
            } else{
                yield return CoroutineUtility.Delayed(1f, SwitchTurn);
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

        private IEnumerator MoveToNextEnemy(){
            GameManager.shared.game.currentStage.ForwardCurrentEnemy();
            yield return new WaitWhile(() => _pause);
            enemyView.BindToCurrentEnemy();
            yield return enemyView.Appear(); 
            if(_isInTutorial) OnEnemyAppear?.Invoke(this);
            yield return new WaitWhile(() => _pause);
            yield return CoroutineUtility.Delayed(1f, SwitchTurn);
        }

        private IEnumerator CollectRewards(){
            GameManager.shared.game.currentStage.Beaten();
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

        /// <summary>
        ///     Kick off the switch turn procedure
        /// </summary>
        private void SwitchTurn(){
            GameManager.shared.game.SwitchTurn();
            StartCoroutine(GameManager.shared.game.turn == Game.Turn.Player ? StartPlayerTurn() : StartToEnemyTurn());
        }
        
        private IEnumerator SwitchTurnEnumerator(){
            GameManager.shared.game.SwitchTurn();
            yield return GameManager.shared.game.turn == Game.Turn.Player ? StartPlayerTurn() : StartToEnemyTurn();
        }

        private IEnumerator StartPlayerTurn(){
            yield return GameManager.shared.turnSignDisplayer.Show(Game.Turn.Player);
            yield return new WaitWhile(() => _pause);
            GameManager.shared.touchTracker.isAcceptingInput = true;
            GameManager.shared.ballManager.SpawnBalls();
            if (GameManager.shared.game.currentStage.id == 0){
                switch (GameManager.shared.CurrentTurnNum){
                    case 1:
                        yield return null;
                        GameManager.shared.tutorialManager.LoadTutorial<TutorialBasicConcept>();
                        break;
                    case 2:
                        GameManager.shared.tutorialManager.LoadTutorial<TutorialStage1Soft>();
                        break;
                }
            } else if (GameManager.shared.game.currentStage.id == 1){
                switch (GameManager.shared.CurrentTurnNum){
                    case 1:
                        GameManager.shared.tutorialManager.LoadTutorial<TutorialCombo>();
                        break;
                }

                if (GameManager.shared.game.currentStage.CurrentEnemy.id == -3){
                    GameManager.shared.tutorialManager.LoadTutorial<TutorialCharge>();
                }
            }
        }

        private IEnumerator StartToEnemyTurn(){
            var stageInfo = GameManager.shared.game.CurrentEnemy.GetCurrentStageAction();
            yield return GameManager.shared.turnSignDisplayer.Show(Game.Turn.Enemy);
            if (GameManager.shared.game.currentStage.id == 0){
                switch (GameManager.shared.CurrentTurnNum){
                    case 1:
                        GameManager.shared.tutorialManager.LoadTutorial<TutorialEnemyIntention>();
                        break;
                }
            }
            ProcessStageAction(stageInfo);
        }

        public IEnumerator StartBattleStage(){
            PresentStage(GameManager.shared.game.currentStage);
            yield return StartPlayerTurn();
        }
    }
}