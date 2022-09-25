using Core.Common;
using Core.Model;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Player{
    [RequireComponent(typeof(PlayerAnimationController))]
    public class Enemy: MonoBehaviour{

        public ProgressBar bar;
        
        private Model.Enemy _model = null;
        private Model.Game _game = null;
        private PlayerAnimationController _controller;

        private void Start(){
            _controller = GetComponent<PlayerAnimationController>();
            _model = GameManager.shared.game.curStage.CurrentEnemy;
            _game = GameManager.shared.game;
            BindEvents();
        }

        private void BindEvents() {
            _model.OnAttack += OnAttack;
            _model.OnBeingAttacked += OnBeingAttacked;
            _model.OnDie += OnDie;
        }

        private void UnbindEvents() {
            _model.OnAttack -= OnAttack;
            _model.OnBeingAttacked -= OnBeingAttacked;
            _model.OnDie -= OnDie;
        }

        private void OnAttack(Game game, GameModel model) {
            _controller.PlayAnimation(PlayerAnimationController.PlayerAnimation.Attack);
        }

        private void OnBeingAttacked(Game game, GameModel model) {
            _controller.PlayAnimation(PlayerAnimationController.PlayerAnimation.BeingAttacked);
            bar.Percentage = (float)_model.CurrentHp / _model.hpUpLimit*100f;
        }

        private void OnDie(Game game, GameModel model) {
            _controller.PlayAnimation(PlayerAnimationController.PlayerAnimation.Die);
            StartCoroutine(CoroutineUtility.Delayed(1, UnbindEvents));
        }
    }
}