using Utility;

namespace Model.EnemySpecialAttacks{
    public class SpAttStrike : SpecialAttackBase{
        public SpAttStrike(string[] args) : base(args){ }

        public override void Execute(StageActionBase info){
            info.AddExtraDamage(new Damage(info.currentGame.CurrentEnemy, Damage.Type.Physics, IntUtility.ParseString(args[0]), GameManager.shared.game.player));
        }
    }
}