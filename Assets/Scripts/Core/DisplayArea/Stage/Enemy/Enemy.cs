using System;
using Core.DisplayArea.Stage.Player;
using Model;
using UnityEngine;

namespace Core.DisplayArea.Stage.Enemy{

    [RequireComponent(typeof(PlayerAnimationController))]
    public class Enemy : DamageableView{
        public RemainingEnemy remaining;
        public IntentionDisplayer intentionDisplayer;

        public override IDamageable Model{
            set{
                base.Model = value;
                remaining.Number = GameManager.shared.game.currentStage.RemainingEnemyNumber;
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
                wrappedActionInfo.target.ProcessDamage();
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

        public override void ProcessDamage(){
            var point = ((StageActionInfoPlayerAttack)wrappedActionInfo.actionInfo).damage.point;
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