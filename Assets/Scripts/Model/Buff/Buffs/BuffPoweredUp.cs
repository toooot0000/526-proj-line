namespace Model.Buff.Buffs{
    public class BuffPoweredUp: Buff{
        public  BuffPoweredUp(GameModel parent, int layer) : base(parent, layer){ }
        protected override string GetBuffName() => "power_up";
    }
}