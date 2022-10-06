using System;
using System.Collections;
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

        public void BindToCurrentEnemy(){
            isDead = false;
            Model = GameManager.shared.game.CurrentEnemy;
            UpdateIntention();
        }

        public void Appear(Action callback){
            animationController.Play(PlayerAnimation.Appear, callback);
        }

        public IEnumerator Appear(){
            yield return animationController.PlayUntilComplete(PlayerAnimation.Appear);
        }

        private void UpdateIntention(){
            var enemy = (Model.Enemy)Model;
            intentionDisplayer.UpdateIntention(new IntentionDisplayer.IntentionInfo{
                intention = enemy.CurrentIntention,
                number = ((Model.Enemy)Model).CurrentIntention switch{
                    EnemyIntention.Attack => enemy.attack,
                    EnemyIntention.Defend => enemy.defend,
                    EnemyIntention.SpecialAttack => 0,
                    _ => throw new ArgumentOutOfRangeException()
                }
            });
        }
        public void Attack(Action callback){
            animationController.Play(PlayerAnimation.Attack, 0.07f, ()=> {
                UpdateIntention();
                callback?.Invoke();
            });
        }
        public IEnumerator Attack(){
            yield return animationController.PlayUntilComplete(PlayerAnimation.Attack);
        }
        public void Defend(Action callback){
            animationController.Play(PlayerAnimation.Defend, () => {
                armorDisplayer.Number = Model.Armor;
                UpdateIntention();
                callback?.Invoke();
            });
        }
        public IEnumerator Defend(){
            animationController.Play(PlayerAnimation.Defend);
            yield return new WaitForSeconds(0.4f);
            armorDisplayer.Number = Model.Armor;
            UpdateIntention();
        }

        public void SpecialAttack(Action callback){
            var info = (stageActionInfo as StageActionInfoEnemySpecial)!;
            if (info.damage != null){
                Attack(null);
                armorDisplayer.Number = Model.Armor;
            } else{
                Defend(null);
            }
        }
        public IEnumerator SpecialAttack(){
            var info = (stageActionInfo as StageActionInfoEnemySpecial)!;
            if (info.damage != null){
                yield return Attack();
            } else{
                yield return Defend();
            }
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