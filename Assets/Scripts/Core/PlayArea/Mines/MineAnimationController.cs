using System.ComponentModel;
using Utility.Animation;

namespace Core.PlayArea.Mines{

    public enum MineAnimation{
        [Description("Idle")]
        Idle,
        [Description("Explosion")]
        Explosion,
        [Description("Disappear")]
        Disappear,
    }
    
    public class MineAnimationController: AnimationController<MineAnimation>{ }
}