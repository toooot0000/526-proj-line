using System;
using Core.Common;
using Core.Model;
using TMPro;
using UnityEngine;
using Utility;

namespace Core.DisplayArea.Stage{

    [RequireComponent(typeof(PlayerAnimationController))]
    public class Enemy : DamageableView{
        public RemainingEnemy remaining;

        public override Model.IDamageable Model{
            set{
                base.Model = value;
                remaining.Number = GameManager.shared.game.currentStage.RemainingEnemyNumber;
            }
        }

        public void BindToCurrentEnemy(){
            if (GameManager.shared.game.CurrentEnemy != null){
                Model = GameManager.shared.game.CurrentEnemy;
                animationController.Play(PlayerAnimation.Appear);
            }
        }
        
        public void BindToCurrentEnemy(Action callback){
            isDead = false;
            Model = GameManager.shared.game.CurrentEnemy;
            animationController.Play(PlayerAnimation.Appear, callback);
        }
    }
}