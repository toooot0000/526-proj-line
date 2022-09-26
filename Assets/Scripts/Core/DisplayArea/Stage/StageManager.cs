using System;
using Core.Model;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Stage{
    public class StageManager: MonoBehaviour{

        public class DamageWrapper{
            public DamageableView source;
            public DamageableView target;
            public Damage raw;
            public Action resolvedCallback;
        }
        
        private Model.Stage _modelStage;
        public Player player;
        public Enemy enemy;

        private void Start(){
            GameManager.shared.game.OnStageLoaded += OnStageLoaded;
            OnStageLoaded(GameManager.shared.game);
        }


        private void OnStageLoaded(Game game){
            _modelStage = game.currentStage;
            _modelStage.OnProcessDamage += OnProcessDamage;
            enemy.Model = _modelStage.CurrentEnemy;
        }

        private void OnProcessDamage(Game game, GameModel model){
            var dmg = (model as Damage)!;
            DamageableView target = player.Model == dmg.target ? player : enemy;
            var dmgWrp = new DamageWrapper(){
                source = player.Model == dmg.source ? player : enemy,
                target = target,
                raw = dmg,
                resolvedCallback = () => {
                    if(target.isDead && target is Enemy enemy1) enemy1.BindToCurrentEnemy(
                            () => StartCoroutine(CoroutineUtility.Delayed(2f, GameManager.shared.game.SwitchTurn))
                        );
                    else StartCoroutine(CoroutineUtility.Delayed(1f, GameManager.shared.game.SwitchTurn));
                }
            };
            player.damage = dmgWrp;
            enemy.damage = dmgWrp;
            dmgWrp.source.Attack();
        }
    }
}