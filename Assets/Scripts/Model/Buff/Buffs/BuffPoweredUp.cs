namespace Model.Buff.Buffs{
    public class BuffPoweredUp: Buff{
        public BuffPoweredUp(GameModel parent, int layer) : base(parent, layer){ SetUp(this); }
        protected override string GetBuffName() => "power_up";
    }
}