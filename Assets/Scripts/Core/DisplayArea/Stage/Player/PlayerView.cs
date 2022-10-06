using System;
using System.Collections;
using Model;
using UI;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Stage.Player{
    [RequireComponent(typeof(PlayerAnimationController))]
    public class PlayerView : DamageableView{
        public override IDamageable Model{
            set{
                base.Model = value;
                armorDisplayer.Number = value.Armor;
            }
        }


        private void Start(){
            Model = GameManager.shared.game.player;
        }

        public void Die(Action callback = null){
            StartCoroutine(CoroutineUtility.Delayed(0.5f, callback));
        }

        public void Attack(Action callback){
            animationController.Play(PlayerAnimation.Attack, 0.07f, () => {
                armorDisplayer.Number = Model.Armor;
                callback?.Invoke();
            });
        }

        public IEnumerator Attack(){
            yield return animationController.PlayUntilComplete(PlayerAnimation.Attack);
        }

        public void TakeDamage(Action callback){
            var point = stageActionInfo.damage.totalPoint;
            damageNumberDisplay.Number = CurrentHp - Model.CurrentHp;
            CurrentHp = Model.CurrentHp;
            armorDisplayer.Number = Model.Armor;
            if (isDead)
                animationController.Play(PlayerAnimation.Die, callback);
            else
                animationController.Play(PlayerAnimation.BeingAttacked, callback);
        }

        public IEnumerator TakeDamage(){
            var point = stageActionInfo.damage.totalPoint;
            damageNumberDisplay.Number = CurrentHp - Model.CurrentHp;
            CurrentHp = Model.CurrentHp;
            armorDisplayer.Number = Model.Armor;
            if (isDead)
                yield return animationController.PlayUntilComplete(PlayerAnimation.Die);
            else
                yield return animationController.PlayUntilComplete(PlayerAnimation.BeingAttacked);
        }
    }
}