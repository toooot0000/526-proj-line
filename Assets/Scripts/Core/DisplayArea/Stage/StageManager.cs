using System;
using BackendApi;
using Core.PlayArea.Balls;
using Model;
using UI;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Stage{
    
    /// <summary>
    /// Be responsible for the stage process
    /// </summary>
    public class StageManager: MonoBehaviour{
        
        [Obsolete("Use StageActionInfoWrapper")]
        public class DamageWrapper{
            public DamageableView source;
            public DamageableView target;
            public Damage raw;
            public Action<DamageWrapper> resolvedCallback;
        }

        public class StageActionInfoWrapper{
            public DamageableView source;
            public DamageableView target;
            public StageActionInfoBase actionInfo;
            public Action<StageActionInfoWrapper> resolvedCallback;
        }

        private Model.Stage _modelStage;
        public Player.PlayerView playerView;
        public Enemy.EnemyView enemyView;
        public BallManager ballManager;

        private DateTime _stageStart;
        private int _currentLevel = -1;

        private void Start(){
            GameManager.shared.game.OnStageLoaded += OnStageLoaded;
            OnStageLoaded(GameManager.shared.game);
        }


        private void OnStageLoaded(Game game){
            _modelStage = game.currentStage;
            // _modelStage.OnProcessDamage += OnProcessDamage;
            _modelStage.OnProcessStageAction += OnProcessStageAction;
            enemyView.BindToCurrentEnemy(null);
            _stageStart = DateTime.Now;
            _currentLevel = _modelStage.id;
        }
        
        [Obsolete("Use OnProcessStageAction")]
        private void OnProcessDamage(Game game, GameModel model){
            var dmg = (model as Damage)!;
            DamageableView target = playerView.Model == dmg.target ? playerView : enemyView;
            var dmgWrp = new DamageWrapper(){
                source = playerView.Model == dmg.source ? playerView : enemyView,
                target = target,
                raw = dmg,
                resolvedCallback = OnDamageResolved
            };
            playerView.damage = dmgWrp;
            enemyView.damage = dmgWrp;
            dmgWrp.source.Attack();
        }

        private void OnProcessStageAction(Game game, GameModel model){
            switch (model){
                case StageActionInfoPlayerAttack attack:
                    ProcessPlayerAttack(attack);
                    break;
                case StageActionInfoEnemyAttack attack :
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
        
        [Obsolete("Use Process(StageAction)")]
        private void ProcessDamage(Damage damage){
            DamageableView target = playerView.Model == damage.target ? playerView : enemyView;
            var dmgWrp = new DamageWrapper(){
                source = playerView.Model == damage.source ? playerView : enemyView,
                target = target,
                raw = damage,
                resolvedCallback = OnDamageResolved
            };
            playerView.damage = dmgWrp;
            enemyView.damage = dmgWrp;
            dmgWrp.source.Attack();
        }

        private void ProcessPlayerAttack(StageActionInfoPlayerAttack info){
            var dmgWrp = new StageActionInfoWrapper(){
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
            var infoWrp = new StageActionInfoWrapper(){
                source = enemyView,
                target = playerView,
                actionInfo = info,
                resolvedCallback = wrapper => StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.game.SwitchTurn))
            };
            playerView.wrappedActionInfo = infoWrp;
            enemyView.wrappedActionInfo = infoWrp;
            enemyView.Attack();
        }

        private void ProcessEnemySpecialAttack(StageActionInfoEnemySpecial info){
            var infoWrp = new StageActionInfoWrapper(){
                source = enemyView,
                target = playerView,
                actionInfo = info,
                resolvedCallback = wrapper => StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.game.SwitchTurn))
            };
            playerView.wrappedActionInfo = infoWrp;
            enemyView.wrappedActionInfo = infoWrp;
            enemyView.SpecialAttack();
        }

        private void ProcessEnemyDefend(StageActionInfoEnemyDefend info){
            var infoWrp = new StageActionInfoWrapper(){
                source = enemyView,
                target = null,
                actionInfo = info,
                resolvedCallback = wrapper =>
                    StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.game.SwitchTurn))
            };
            playerView.wrappedActionInfo = infoWrp;
            enemyView.wrappedActionInfo = infoWrp;
            enemyView.Defend();
        }


        private void OnPlayerAttackResolved(StageActionInfoWrapper wrapped){
            var dmg = (wrapped.actionInfo as StageActionInfoPlayerAttack)!.damage;
            if(enemyView.isDead) {
                if(dmg.currentGame.currentStage.NextEnemy != null){
                    dmg.currentGame.currentStage.ForwardCurrentEnemy();
                    enemyView.BindToCurrentEnemy(
                        () => StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.game.SwitchTurn))
                    );
                }
                else{
                    var currentTime = DateTime.Now;
                    var clearEvent = new EventClearanceRecord(){
                        time = (int)((DateTime.Now - _stageStart).TotalMilliseconds),
                        status = "success",
                        level = _currentLevel
                    };
                    EventLogger.Shared.Log(loggableEvent: clearEvent);
                    if (_modelStage.nextStage != -1){
                        if (_modelStage.bonusCoins == -1){
                            UIManager.shared.OpenUI("UISelectGear", (object)_modelStage.bonusGears);
                        } else{
                            UIManager.shared.OpenUI("UIGetCoins", _modelStage.bonusCoins);
                        }
                    } else{
                        UIManager.shared.OpenUI("UIGameComplete");
                    }
                }
            }
            else StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.game.SwitchTurn));
        }
        
        
        [Obsolete("Use On(StageAction)Resolved instead!")]
        private void OnDamageResolved(DamageWrapper dmg){
            if(dmg.target.isDead && dmg.target is Enemy.EnemyView enemy1) {
                if(dmg.raw.currentGame.CurrentEnemy != null){
                    enemy1.BindToCurrentEnemy(
                        () => StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.game.SwitchTurn))
                    );
                }
                else{
                    var currentTime = DateTime.Now;
                    var clearEvent = new EventClearanceRecord(){
                        time = (int)((DateTime.Now - _stageStart).TotalMilliseconds),
                        status = "success",
                        level = _currentLevel
                    };
                    EventLogger.Shared.Log(loggableEvent: clearEvent);
                    if (_modelStage.nextStage != -1){
                        if (_modelStage.bonusCoins == -1){
                            UIManager.shared.OpenUI("UISelectGear", (object)_modelStage.bonusGears);
                        } else{
                            UIManager.shared.OpenUI("UIGetCoins", _modelStage.bonusCoins);
                        }
                    } else{
                        UIManager.shared.OpenUI("UIGameComplete");
                    }
                }
            }
            else StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.game.SwitchTurn));
        }
    }
}


