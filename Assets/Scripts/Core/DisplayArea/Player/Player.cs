using System;
using UnityEngine;
using UnityEngine.UIElements;
using ProgressBar = Core.Common.ProgressBar;

namespace Core.DisplayArea.Player{
    [RequireComponent(typeof(PlayerAnimationController))]
    public class Player: MonoBehaviour{

        public ProgressBar bar;
        
        private Model.Player _model = null;
        private PlayerAnimationController _controller;

        private void Start(){
            _controller = GetComponent<PlayerAnimationController>();
            _model = GameManager.shared.game.player;
            if (_model != null){
                _model.OnAttack += (game, model) => 
                    _controller.PlayAnimation(PlayerAnimationController.PlayerAnimation.Attack);
                _model.OnBeingAttacked += (game, model) => {
                    _controller.PlayAnimation(PlayerAnimationController.PlayerAnimation.BeingAttacked);
                    bar.Percentage = (float)_model.currentHp / _model.hpUpLimit*100f;
                };
                _model.OnDie += (game, model) =>
                    _controller.PlayAnimation(PlayerAnimationController.PlayerAnimation.Die);
            }
        }
    }
}