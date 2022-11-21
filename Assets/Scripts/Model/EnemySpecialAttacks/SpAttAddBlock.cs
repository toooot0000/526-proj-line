using Model.Mechanics.PlayableObjects;
using Utility;

namespace Model.EnemySpecialAttacks{
    public class SpAttAddBlock: SpecialAttackBase{
        public override string Desc => $"Add a {args[0]} size block.";
        public SpAttAddBlock(string[] args) : base(args){ }
        public override void Execute(StageActionBase info){
            var level = EnumUtility.GetValue<BlockLevel>(args[0]);
            GameManager.shared.playAreaManager.AddBlock(level);
        }
    }
}