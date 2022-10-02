using Model;
using UI;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Stage.Player{
    [RequireComponent(typeof(PlayerAnimationController))]
    public class Player: DamageableView{
        public override IDamageable Model {
            set {
                base.Model = value;
                armorDisplayer.Number = value.Armor;
            }
        }


        private void Start(){
            Model = GameManager.shared.game.player;
        }

        public override void Die(){
            base.Die();
            StartCoroutine(CoroutineUtility.Delayed(0.5f, () => {
                UIManager.shared.OpenUI("UIGameEnd");
            }));
        }

        public override void Attack(){
            // TODO Play Attack Animation/defends animation/special attack animation;
            animationController.Play(PlayerAnimation.Attack, 0.07f, wrappedActionInfo.target.ProcessDamage);
        }

        public override void ProcessDamage(){
            var point = ((StageActionInfoEnemyAttack)wrappedActionInfo.actionInfo).damage.point;
            damageNumberDisplay.Number = point;
            CurrentHp -= point;
            if (isDead){
                animationController.Play(PlayerAnimation.Die, () => wrappedActionInfo.resolvedCallback(wrappedActionInfo));
            } else{
                animationController.Play(PlayerAnimation.BeingAttacked, () => wrappedActionInfo.resolvedCallback(wrappedActionInfo));
            }
        }
    }
}