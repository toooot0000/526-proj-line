using System;
using System.Collections;
using Model;
using UI;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Stage.Player{
    [RequireComponent(typeof(PlayerAnimationController))]
    public class PlayerView : DamageableView{
        
        private void Start(){
            Model = GameManager.shared.game.player;
        }

        public void Die(Action callback = null){
            StartCoroutine(CoroutineUtility.Delayed(0.5f, callback));
        }

        public void Attack(Action callback){
            animationController.Play(PlayerAnimation.Attack, 0.07f, () => {
                // armorDisplayer.Number = Model.Armor;
                callback?.Invoke();
            });
        }

        public IEnumerator Attack(){
            yield return animationController.PlayUntilComplete(PlayerAnimation.Attack);
        }

        public void TakeDamage(Action callback){
            if (isDead)
                animationController.Play(PlayerAnimation.Die, callback);
            else
                animationController.Play(PlayerAnimation.BeingAttacked, callback);
        }

        public IEnumerator TakeDamage(){
            if (isDead)
                yield return animationController.PlayUntilComplete(PlayerAnimation.Die);
            else
                yield return animationController.PlayUntilComplete(PlayerAnimation.BeingAttacked);
        }
    }
}