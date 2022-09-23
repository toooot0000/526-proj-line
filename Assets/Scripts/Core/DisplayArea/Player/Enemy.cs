using Core.Common;
using UnityEngine;

namespace Core.DisplayArea.Player{
    [RequireComponent(typeof(PlayerAnimationController))]
    public class Enemy: MonoBehaviour{

        public ProgressBar bar;
        
        private Model.Enemy _model = null;
        private PlayerAnimationController _controller;

        private void Start(){
            _controller = GetComponent<PlayerAnimationController>();
            _model = GameManager.shared.game.curStage.enemies[0];
            if (_model != null){
                _model.OnAttack += (game, model) => 
                    _controller.PlayAnimation(PlayerAnimationController.PlayerAnimation.Attack);
                _model.OnBeingAttacked += (game, model) => {
                    _controller.PlayAnimation(PlayerAnimationController.PlayerAnimation.BeingAttacked);
                    bar.Percentage = (float)_model.currentHp / _model.hpUpLimit;
                };
                _model.OnDie += (game, model) =>
                    _controller.PlayAnimation(PlayerAnimationController.PlayerAnimation.Die);
            }
        }
    }
}