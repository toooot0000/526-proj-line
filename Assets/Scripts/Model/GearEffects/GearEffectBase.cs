namespace Model.GearEffects{
    public abstract class GearEffectBase{
        protected string[] args;

        public GearEffectBase(string[] args){
            this.args = args;
        }

        public abstract void Execute(StageActionInfoPlayerAction actionInfo);
    }
}