namespace Model.GearChargeEffects{
    public abstract class GearChargeEffectBase: GameModel{
        protected GearChargeEffectBase(GameModel parent) : base(parent){ }
        public abstract void Execute();
    }
}