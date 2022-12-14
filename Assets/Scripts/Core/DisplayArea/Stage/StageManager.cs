using System;
using System.Collections;
using System.Linq;
using Core.DisplayArea.Stage.Enemy;
using Core.DisplayArea.Stage.Player;
using Core.PlayArea;
using Core.PlayArea.Balls;
using Core.PlayArea.BlackHoles;
using Core.PlayArea.Blocks;
using Core.PlayArea.Crystals;
using Core.PlayArea.Mines;
using Model;
using Model.Buff;
using Model.Mechanics.PlayableObjects;
using Tutorial;
using Tutorial.Tutorials.BasicConcept;
using Tutorial.Tutorials.Charge;
using Tutorial.Tutorials.Combo;
using Tutorial.Tutorials.EnemyIntention;
using Tutorial.Tutorials.EnemyUpdate;
using Tutorial.Tutorials.Mechanics;
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
        public PlayAreaManager playAreaManager;
        private bool _isInTutorial;

        private Model.Stage _modelStage;
        private bool _pause;

        public void HandOverControlTo(TutorialBase tutorial){
            _isInTutorial = true;
        }

        public void GainBackControlFrom(TutorialBase tutorial){
            _isInTutorial = false;
        }

        public event TutorialControllableEvent OnEnemyAppear;

        private void PresentStage(Model.Stage currentStage){
            _modelStage = currentStage;
            enemyView.BindToCurrentEnemy();
            enemyView.Appear(null);
            if (_isInTutorial) OnEnemyAppear?.Invoke(this);
        }

        private IEnumerator ProcessEnemyAction(StageActionBase action){
            playerView.stageAction = action;
            enemyView.stageAction = action;
            var procedure = action switch{
                StageActionEnemyAttack attack => ProcessEnemyAttack(attack),
                StageActionEnemyDefend defend => ProcessEnemyDefend(defend),
                StageActionEnemySpecial special => ProcessEnemySpecialAttack(special),
                _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
            };
            yield return procedure;
        }

        private IEnumerator ProcessPlayerAttack(StageActionBase action){
            yield return new WaitWhile(() => _pause);
            ballManager.FlyAllBalls(this, 0.5f);
            yield return new WaitForSeconds(0.4f);
            if(_pause) yield return new WaitWhile(() => _pause);
            playerView.Attack(null);
            yield return new WaitForSeconds(0.07f);
            action.Execute();
            yield return enemyView.TakeDamage();
            if(_pause) yield return new WaitWhile(() => _pause);
        }

        private IEnumerator ProcessEnemyAttack(StageActionBase action){
            yield return new WaitWhile(() => _pause);
            enemyView.Attack(null);
            yield return new WaitForSeconds(0.1f);
            action.Execute();
            yield return playerView.TakeDamage();
            yield return new WaitWhile(() => _pause);
        }

        private IEnumerator ProcessEnemySpecialAttack(StageActionBase action){
            yield return new WaitWhile(() => _pause);
            enemyView.SpecialAttack(null);
            yield return new WaitForSeconds(0.1f);
            action.Execute();
            yield return playerView.TakeDamage();
            if(_pause) yield return new WaitWhile(() => _pause);
        }

        private IEnumerator ProcessEnemyDefend(StageActionBase action){
            if(_pause) yield return new WaitWhile(() => _pause);
            action.Execute();
            yield return enemyView.Defend();
            if(_pause) yield return new WaitWhile(() => _pause);
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
            if(_pause) yield return new WaitWhile(() => _pause);
            enemyView.BindToCurrentEnemy();
            yield return enemyView.Appear();
            if (_isInTutorial) OnEnemyAppear?.Invoke(this);
            if(_pause) yield return new WaitWhile(() => _pause);
        }

        private IEnumerator CollectRewards(){
            GameManager.shared.game.currentStage.Beaten();
            if(_pause) yield return new WaitWhile(() => _pause);
            if (!_modelStage.IsLast){
                if (_modelStage.bonusCoins == -1){
                    if(_pause) yield return new WaitWhile(() => _pause);
                    UIManager.shared.OpenUI("UISelectGear", _modelStage.bonusGears);
                } else{
                    if(_pause) yield return new WaitWhile(() => _pause);
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

        private IEnumerator StartPlayerTurn(){
            yield return GameManager.shared.turnSignDisplayer.Show(Game.Turn.Player);
            if(_pause) yield return new WaitWhile(() => _pause);
            GameManager.shared.touchTracker.isAcceptingInput = true;
            GameManager.shared.BallManager.SpawnBalls();
            yield return OnPlayerTurnStartTriggerTutorial();
            if(_pause) yield return new WaitWhile(() => _pause);
            playerView.Model.ExecuteOnTurnBeginBuffEffect();
        }

        private IEnumerator StartToEnemyTurn(){
            var enemyAction = GameManager.shared.game.CurrentEnemy.GetCurrentStageAction();
            yield return GameManager.shared.turnSignDisplayer.Show(Game.Turn.Enemy);
            enemyView.Model.ExecuteOnTurnBeginBuffEffect();
            if(_pause) yield return new WaitWhile(() => _pause);
            if (GameManager.shared.game.currentStage.id == 0)
                switch (GameManager.shared.CurrentTurnNum){
                    case 1:
                        GameManager.shared.tutorialManager.LoadTutorial<TutorialEnemyIntention>();
                        break;
                }
            yield return ProcessEnemyAction(enemyAction);
            if (playerView.isDead) GameManager.shared.GameEnd();
            if(_pause) yield return new WaitWhile(() => _pause);
            enemyView.Model.ExecuteOnTurnEndBuffEffect();
            if(_pause) yield return new WaitWhile(() => _pause);
            SwitchTurn();
            if(_pause) yield return new WaitWhile(() => _pause);
        }

        public IEnumerator StartBattleStage(){
            PresentStage(GameManager.shared.game.currentStage);
            yield return StartPlayerTurn();
        }

        public IEnumerator StartPlayerAction(StageActionPlayerAction playerAction = null){
            var currentAction = playerAction ?? GameManager.shared.game.player.GetAction();
            GameManager.shared.game.player.ClearAllBalls();
            yield return ProcessPlayerAttack(currentAction);
            if (enemyView.isDead){
                OnEnemyDieTriggerTutorial();
                if (_modelStage.NextEnemy == null){
                    playAreaManager.ClearAllObjects();
                    yield return CollectRewards();
                    yield break;
                }
                yield return MoveToNextEnemy();
            }
            playerView.Model.ExecuteOnTurnEndBuffEffect();
            foreach (var onPlayerTurnEnd in playAreaManager.GetAllViewsOfProperty<IOnPlayerTurnEnd>()){
                onPlayerTurnEnd.OnPlayerTurnEnd();
            }
            yield return new WaitForSeconds(1f);
            SwitchTurn();
        }

        private static IEnumerator OnPlayerTurnStartTriggerTutorial(){
            switch (GameManager.shared.game.currentStage.id){
                case 0:
                    switch (GameManager.shared.CurrentTurnNum){
                        case 1:
                            yield return null;
                            GameManager.shared.tutorialManager.LoadTutorial<TutorialBasicConcept>();
                            break;
                        case 2:
                            GameManager.shared.tutorialManager.LoadTutorial<TutorialStage1Soft>();
                            break;
                    }
                    break;
                case 1:{
                    switch (GameManager.shared.CurrentTurnNum){
                        case 1:
                            GameManager.shared.tutorialManager.LoadTutorial<TutorialCombo>();
                            break;
                    }
                    if (GameManager.shared.game.currentStage.CurrentEnemy.id == -3)
                        GameManager.shared.tutorialManager.LoadTutorial<TutorialCharge>();
                    break;
                }
            }

            if (!GameManager.shared.tutorialManager.TutorialHasTriggered<TutorialBlock>() && 
                GameManager.shared.playAreaManager.HasViewOf<BlockView>()){
                GameManager.shared.tutorialManager.LoadTutorial<TutorialBlock>();
            } else if (!GameManager.shared.tutorialManager.TutorialHasTriggered<TutorialBomb>() && 
                       GameManager.shared.playAreaManager.HasViewOf<MineView>()){
                GameManager.shared.tutorialManager.LoadTutorial<TutorialBomb>();
            } else if (!GameManager.shared.tutorialManager.TutorialHasTriggered<TutorialBlackHole>() &&
                       GameManager.shared.playAreaManager.HasViewOf<BlackHoleView>()){
                GameManager.shared.tutorialManager.LoadTutorial<TutorialBlackHole>();
            }  else if (!GameManager.shared.tutorialManager.TutorialHasTriggered<TutorialCrystalFreeze>() &&
                        GameManager.shared.playAreaManager.HasViewOf<CrystalView>() && GameManager.shared.playAreaManager.GetAllViews().OfType<CrystalView>().First().Model.type == CrystalType.FreezeMovable){
                GameManager.shared.tutorialManager.LoadTutorial<TutorialCrystalFreeze>();
            } else if (!GameManager.shared.tutorialManager.TutorialHasTriggered<TutorialCrystalStrengthen>() &&
                       GameManager.shared.playAreaManager.HasViewOf<CrystalView>() && GameManager.shared.playAreaManager.GetAllViews().OfType<CrystalView>().First().Model.type == CrystalType.LengthenLine){
                GameManager.shared.tutorialManager.LoadTutorial<TutorialCrystalStrengthen>();
            } 
            

            // if(GameManager.shared.playAreaManager.HasViewOf<>())
        }
    }
}