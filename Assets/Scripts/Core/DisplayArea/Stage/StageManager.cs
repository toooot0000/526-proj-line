using System;
using BackendApi;
using Model;
using UI;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Stage{
    
    /// <summary>
    /// Be responsible for the stage process
    /// </summary>
    public class StageManager: MonoBehaviour{

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
        public Player.Player player;
        public Enemy.Enemy enemy;

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
            enemy.BindToCurrentEnemy(null);
            _stageStart = DateTime.Now;
            _currentLevel = _modelStage.id;
        }
        
        [Obsolete("Use OnProcessStageAction")]
        private void OnProcessDamage(Game game, GameModel model){
            var dmg = (model as Damage)!;
            DamageableView target = player.Model == dmg.target ? player : enemy;
            var dmgWrp = new DamageWrapper(){
                source = player.Model == dmg.source ? player : enemy,
                target = target,
                raw = dmg,
                resolvedCallback = OnDamageResolved
            };
            player.damage = dmgWrp;
            enemy.damage = dmgWrp;
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

        private void ProcessDamage(Damage damage){
            DamageableView target = player.Model == damage.target ? player : enemy;
            var dmgWrp = new DamageWrapper(){
                source = player.Model == damage.source ? player : enemy,
                target = target,
                raw = damage,
                resolvedCallback = OnDamageResolved
            };
            player.damage = dmgWrp;
            enemy.damage = dmgWrp;
            dmgWrp.source.Attack();
        }

        private void ProcessPlayerAttack(StageActionInfoPlayerAttack info){
            var dmgWrp = new StageActionInfoWrapper(){
                source = player,
                target = enemy,
                actionInfo = info,
                resolvedCallback = OnPlayerAttackResolved
            };
            player.wrappedActionInfo = dmgWrp;
            enemy.wrappedActionInfo = dmgWrp;
            player.Attack();
        }

        private void ProcessEnemyAttack(StageActionInfoEnemyAttack info){
            var infoWrp = new StageActionInfoWrapper(){
                source = enemy,
                target = player,
                actionInfo = info,
                resolvedCallback = wrapper => StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.game.SwitchTurn))
            };
            player.wrappedActionInfo = infoWrp;
            enemy.wrappedActionInfo = infoWrp;
            enemy.Attack();
        }

        private void ProcessEnemySpecialAttack(StageActionInfoEnemySpecial info){
            var infoWrp = new StageActionInfoWrapper(){
                source = enemy,
                target = player,
                actionInfo = info,
                resolvedCallback = wrapper => StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.game.SwitchTurn))
            };
            player.wrappedActionInfo = infoWrp;
            enemy.wrappedActionInfo = infoWrp;
            enemy.SpecialAttack();
        }

        private void ProcessEnemyDefend(StageActionInfoEnemyDefend info){
            var infoWrp = new StageActionInfoWrapper(){
                source = enemy,
                target = null,
                actionInfo = info,
                resolvedCallback = wrapper =>
                    StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.game.SwitchTurn))
            };
            player.wrappedActionInfo = infoWrp;
            enemy.wrappedActionInfo = infoWrp;
            enemy.Defend();
        }


        private void OnPlayerAttackResolved(StageActionInfoWrapper wrapped){
            var dmg = (wrapped.actionInfo as StageActionInfoPlayerAttack)!.damage;
            if(enemy.isDead) {
                if(dmg.currentGame.currentStage.NextEnemy != null){
                    dmg.currentGame.currentStage.ForwardCurrentEnemy();
                    enemy.BindToCurrentEnemy(
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
        

        private void OnDamageResolved(DamageWrapper dmg){
            if(dmg.target.isDead && dmg.target is Enemy.Enemy enemy1) {
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


