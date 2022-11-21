using Core.PlayArea.Mines;
using Model.Mechanics.PlayableObjects;
using Model.Mechanics.PlayableObjects.MineEffects;
using Utility;

namespace Model.EnemySpecialAttacks {
    /// <summary>
    /// Args: [speed: float];[size: int];[mine effect name: string];[mine effect args: string[]]
    /// </summary>
    public class SpAttAddBomb: SpecialAttackBase{
        public override string Desc => "Throw a bomb in the area. Watch out!";
        public SpAttAddBomb(string[] args) : base(args) { }

        public override void Execute(StageActionBase info) {
            var spd = args.Length > 0 ? float.Parse(args[0]) : 2f;
            var size = args.Length > 1 ? IntUtility.ParseString(args[1]) : 1;
            MineEffect eff = new MineEffectLoseLife();
            if (args.Length > 2) {
                var name = args[2];
                var effArgs = args[3..];
                eff = MineEffect.Make(name, effArgs);
            }
            var mine = GameManager.shared.game.playArea.MakeAndPlaceMine(spd, size, eff);
            GameManager.shared.playAreaManager.GetManager<MineManager>().Place(mine);
        }
    }
}