using System.ComponentModel;
using UnityEngine;
using Utility.Animation;

namespace Core.DisplayArea.Stage{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : AnimationController<PlayerAnimation>{ }

    public enum PlayerAnimation{
        [Description("Attack")] Attack,
        [Description("BeingAttacked")] BeingAttacked,
        [Description("Die")] Die,
        [Description("Appear")] Appear,
        [Description("Special")] Special,
        [Description("Defend")] Defend
    }
}