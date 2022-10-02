namespace Model.EnemySpecialAttacks{
    public abstract class SpecialAttackBase{
        public string[] args;

        public SpecialAttackBase(string[] args){
            this.args = args;
        }
        public abstract void Execute(Game game);
    }
}