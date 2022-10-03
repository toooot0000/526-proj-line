using Model;
using UI;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Stage.Player{
    [RequireComponent(typeof(PlayerAnimationController))]
    public class PlayerView: DamageableView{
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
            animationController.Play(PlayerAnimation.Attack, 0.07f, ()=> {
                wrappedActionInfo.target.TakeDamage();
                armorDisplayer.Number = Model.Armor;
            });
        }

        public override void TakeDamage(){
            var point = ((StageActionInfoEnemyAttack)wrappedActionInfo.actionInfo).damage.totalPoint;
            damageNumberDisplay.Number = CurrentHp - Model.CurrentHp;
            CurrentHp = Model.CurrentHp;
            armorDisplayer.Number = Model.Armor;
            if (isDead){
                animationController.Play(PlayerAnimation.Die, () => wrappedActionInfo.resolvedCallback(wrappedActionInfo));
            } else{
                animationController.Play(PlayerAnimation.BeingAttacked, () => wrappedActionInfo.resolvedCallback(wrappedActionInfo));
            }
        }
    }
}