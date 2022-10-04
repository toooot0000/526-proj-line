namespace Model.EnemySpecialAttacks{
    public class SpAttStrike: SpecialAttackBase{
        public SpAttStrike(string[] args) : base(args){ }
        public override void Execute(StageActionInfoEnemySpecial info){
            info.damage = new Damage(info.currentGame.CurrentEnemy){
                target = info.currentGame.player,
                totalPoint = int.Parse(args[0]),
            };
        }
    }
}