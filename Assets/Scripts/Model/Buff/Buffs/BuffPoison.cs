namespace Model.Buff.Buffs{
    public class BuffPoison: Buff, IBuffTriggerOnTurnEnd{
        public BuffPoison(GameModel parent, int layer) : base(parent, layer){ }
        protected override string GetBuffName() => "poison";

        private class TurnEndEffect: IBuffEffect<Damageable>{
            public void Execute(Damageable buffable){
                var poison = Buff.GetBuffOfTypeFrom<BuffPoison>(buffable);
                if (poison == null) return;
                buffable.TakeDamage(poison.ToDamage());
                buffable.RemoveBuffLayer<BuffPoison>(1);
            }
        }
        
        public IBuffEffect<Damageable> OnTurnEnd(){
            return new TurnEndEffect();
        }

        public Damage ToDamage(){
            return new Damage(parent, Damage.Type.Magic, layer, holder as Damageable);
        }
    }
}