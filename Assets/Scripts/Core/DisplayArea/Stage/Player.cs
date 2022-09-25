using System;
using Core.Model;
using TMPro;
using UnityEngine;
using ProgressBar = Core.Common.ProgressBar;

namespace Core.DisplayArea.Stage{
    [RequireComponent(typeof(PlayerAnimationController))]
    public class Player: DamageableView{

        private void Start(){
            Model = GameManager.shared.game.player;
        }

    }
}