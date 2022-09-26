using System;
using Core.Model;
using TMPro;
using UI;
using UnityEngine;
using Utility;
using ProgressBar = Core.Common.ProgressBar;

namespace Core.DisplayArea.Stage{
    [RequireComponent(typeof(PlayerAnimationController))]
    public class Player: DamageableView{

        private void Start(){
            Model = GameManager.shared.game.player;
        }

        public override void Die(){
            base.Die();
            StartCoroutine(CoroutineUtility.Delayed(0.5f, () => {
                UIManager.shared.OpenUI("UIGameEnd");
            }));
        }

    }
}