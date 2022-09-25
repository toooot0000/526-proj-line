using System;
using Core.Model;
using UnityEngine;

namespace Core.DisplayArea.Stage{
    public class StageManager: MonoBehaviour{

        public struct DamageWrapper{
            public DamageableView source;
            public DamageableView target;
            public Damage raw;
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
            var dmgWrp = new DamageWrapper(){
                source = player.Model == dmg.source ? player : enemy,
                target = player.Model == dmg.target ? player : enemy,
                raw = dmg
            };
            player.damage = dmgWrp;
            enemy.damage = dmgWrp;
            dmgWrp.source.animationController.PlayAnimation(PlayerAnimationController.PlayerAnimation.Attack);
        }
    }
}