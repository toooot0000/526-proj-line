using System;
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
            model.OnIntentionChanged += UpdateIntention;
            UpdateIntention(model.currentGame, model);
            animationController.Play(PlayerAnimation.Appear, callback);
        }

        private void UpdateIntention(Game game, GameModel model){
            var enemy = (Model.Enemy)model;
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

        public override void Die(){
            base.Die();
            (Model as Model.Enemy)!.OnIntentionChanged -= UpdateIntention;
        }

        public void Defend(){
            
        }

        public void SpecialAttack(){
            
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