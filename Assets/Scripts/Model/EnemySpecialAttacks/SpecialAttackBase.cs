namespace Model.EnemySpecialAttacks{
    public abstract class SpecialAttackBase{
        protected readonly string[] args;

        public SpecialAttackBase(string[] args){
            this.args = args;
        }

        public abstract void Execute(StageActionInfoBase info);
    }
}