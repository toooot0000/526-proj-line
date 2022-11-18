namespace Model.EnemySpecialAttacks{
    public class SpAttAddBlackHole: SpecialAttackBase{
        public SpAttAddBlackHole(string[] args) : base(args){ }
        public override void Execute(StageActionBase info){
            var bh = GameManager.SharedGame.playArea.MakeAndPlaceBlackHole(0.1f, 3);
            GameManager.shared.playAreaManager.blackHoleManager.Place(bh);
        }
    }
}