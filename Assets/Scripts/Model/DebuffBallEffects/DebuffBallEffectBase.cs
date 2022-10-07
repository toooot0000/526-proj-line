namespace Model.DebuffBallEffects{
    public abstract class DebuffBallEffectBase{
        protected string[] args;
        public DebuffBallEffectBase(string[] args){
            this.args = args;
        }
        public abstract void Execute(StageActionInfoPlayerAttack attackInfo);
    }
}