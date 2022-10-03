namespace Model.GearEffects{
    public abstract class GearEffectBase: GameModel{
        protected GearEffectBase(GameModel parent) : base(parent){ }

        public GearEffectBase(Gear parent) : base(parent){ }
        public abstract void Execute();
    }
}