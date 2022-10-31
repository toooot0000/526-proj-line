using System.Collections.Generic;
using System.Linq;
using Model.Buff;

namespace Model{
    public abstract class Damageable: GameModel, IBuffHolder{
        protected Damageable(GameModel parent) : base(parent){ }
        public abstract int CurrentHp{ get; set; }
        public abstract int HpUpLimit{ get; set; }
        public abstract int Armor{ get; set; }
        public abstract void TakeDamage(Damage damage);
        private readonly List<Buff.Buff> _buffs = new();
        
        public void AddBuffLayer<TBuff>(int layer) where TBuff : Buff.Buff{
            var buff = Buff.Buff.GetBuffOfTypeFrom<TBuff>(this);
            if (buff == null){
                _buffs.Add(Buff.Buff.MakeBuff<TBuff>(this, layer));
            } else{
                buff.AddLayer(layer);
            }
        }

        public void RemoveBUffLayer<TBuff>(int layer) where TBuff : Buff.Buff{
            var buff = Buff.Buff.GetBuffOfTypeFrom<TBuff>(this);
            buff?.RemoveLayer(layer);
        }

        public IEnumerable<Buff.Buff> GetAllBuffs() => _buffs;

        private IEnumerable<IBuffEffect<Damageable>> GetOnTurnBeginBuffEffect(){
            var triggers = Buff.Buff.GetBuffOfTriggerFrom<IBuffTriggerOnTurnBegin>(this);
            return triggers.Select(t => (IBuffEffect<Damageable>)t.OnTurnBegin());
        }

        private IEnumerable<IBuffEffect<Damageable>> GetOnTurnEndBuffEffect(){
            var triggers = Buff.Buff.GetBuffOfTriggerFrom<IBuffTriggerOnTurnEnd>(this);
            return triggers.Select(t => (IBuffEffect<Damageable>)t.OnTurnEnd());
        }

        public void ExecuteOnTurnEndBuffEffect(){
            foreach (var buffEffect in GetOnTurnEndBuffEffect()){
                buffEffect.Execute(this);
            }
        }

        public void ExecuteOnTurnBeginBuffEffect(){
            foreach (var buffEffect in GetOnTurnBeginBuffEffect()){
                buffEffect.Execute(this);
            }
        }
    }
}