using System;
using Model;
using UnityEngine;

namespace Core.DisplayArea.Stage.Enemy{

    [RequireComponent(typeof(PlayerAnimationController))]
    public class Enemy : DamageableView{
        public RemainingEnemy remaining;

        public override IDamageable Model{
            set{
                base.Model = value;
                remaining.Number = GameManager.shared.game.currentStage.RemainingEnemyNumber;
            }
        }
        public void BindToCurrentEnemy(Action callback){
            isDead = false;
            Model = GameManager.shared.game.CurrentEnemy;
            animationController.Play(PlayerAnimation.Appear, callback);
        }
    }
}