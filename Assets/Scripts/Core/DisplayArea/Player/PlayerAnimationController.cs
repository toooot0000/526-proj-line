using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Utility;
using Utility.Extensions;

namespace Core.DisplayArea.Player{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour{
        
        public enum PlayerAnimation{
            [Description("TrgAttack")]
            Attack,
            [Description("TrgBeingAttacked")]
            BeingAttacked,
            [Description("TrgDie")]
            Die,
        }
        private Animator _animator;

        private void Start(){
            _animator = GetComponent<Animator>();
        }
        
        public void PlayAnimation(PlayerAnimation anim){
            _animator.SetTrigger(anim.GetDescription());
        }
    }
}