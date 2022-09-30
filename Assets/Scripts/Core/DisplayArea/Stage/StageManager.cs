using System;
using BackendApi;
using Model;
using UI;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Stage{
    public class StageManager: MonoBehaviour{

        public class DamageWrapper{
            public DamageableView source;
            public DamageableView target;
            public Damage raw;
            public Action<DamageWrapper> resolvedCallback;
        }
        
        private Model.Stage _modelStage;
        public Player player;
        public Enemy enemy;

        private DateTime _stageStart;
        private int _currentLevel = -1;

        private void Start(){
            GameManager.shared.game.OnStageLoaded += OnStageLoaded;
            OnStageLoaded(GameManager.shared.game);
        }


        private void OnStageLoaded(Game game){
            _modelStage = game.currentStage;
            _modelStage.OnProcessDamage += OnProcessDamage;
            enemy.BindToCurrentEnemy(null);
            _stageStart = DateTime.Now;
            _currentLevel = _modelStage.id;
        }

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

        private void OnDamageResolved(DamageWrapper dmg){
            if(dmg.target.isDead && dmg.target is Enemy enemy1) {
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


