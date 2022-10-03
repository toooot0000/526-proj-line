using System;
using Core.DisplayArea.Stage.Player;
using Model;
using UnityEngine;

namespace Core.DisplayArea.Stage.Enemy{

    [RequireComponent(typeof(PlayerAnimationController))]
    public class EnemyView : DamageableView{
        public RemainingEnemy remaining;
        public IntentionDisplayer intentionDisplayer;

        public override IDamageable Model{
            set{
                base.Model = value;
                remaining.Number = GameManager.shared.game.currentStage.RemainingEnemyNumber;
                armorDisplayer.Number = value.Armor;
            }
        }

        public void BindToCurrentEnemy(Action callback){
            isDead = false;
            Model = GameManager.shared.game.CurrentEnemy;
            var model = ((Model.Enemy)Model);
            UpdateIntention();
            animationController.Play(PlayerAnimation.Appear, callback);
        }

        private void UpdateIntention(){
            var enemy = (Model.Enemy)Model;
            intentionDisplayer.UpdateIntention(new IntentionDisplayer.IntentionInfo(){
                intention = enemy.CurrentIntention,
                number = ((Model.Enemy)Model).CurrentIntention switch {
                    EnemyIntention.Attack => enemy.attack,
                    EnemyIntention.Defend => enemy.defend,
                    EnemyIntention.SpecialAttack => 0,
                    _ => throw new ArgumentOutOfRangeException()
                }
            });
        }

        public override void Attack(){
            animationController.Play(PlayerAnimation.Attack, 0.07f, 
            ()=> {
                UpdateIntention();
                wrappedActionInfo.target.TakeDamage();
            });
        }

        public void Defend(){
            animationController.Play(PlayerAnimation.Defend, ()=> {
                armorDisplayer.Number = Model.Armor;
                UpdateIntention();
                wrappedActionInfo.resolvedCallback(wrappedActionInfo);
            });
        }

        public void SpecialAttack(){
            animationController.Play(PlayerAnimation.Defend, ()=> {
                UpdateIntention();
                wrappedActionInfo.resolvedCallback(wrappedActionInfo);
            });
        }

        public override void TakeDamage(){
            var point = ((StageActionInfoPlayerAttack)wrappedActionInfo.actionInfo).damage.totalPoint;
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