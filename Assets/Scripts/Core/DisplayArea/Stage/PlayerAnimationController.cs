using System.ComponentModel;
using UnityEngine;
using Utility.Animation;
using Utility.Extensions;

namespace Core.DisplayArea.Stage{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController: AnimationController<PlayerAnimation>{
    }

    public enum PlayerAnimation{
        [Description("TrgAttack")]
        Attack,
        [Description("TrgBeingAttacked")]
        BeingAttacked,
        [Description("TrgDie")]
        Die,
        [Description("TrgAppear")]
        Appear,
    }
}