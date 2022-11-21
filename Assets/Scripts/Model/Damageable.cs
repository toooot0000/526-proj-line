using System;
using System.Collections.Generic;
using System.Linq;
using Model.Buff;
using UnityEditor;

namespace Model{
    public abstract class Damageable: GameModel, IBuffHolder{
        protected Damageable(GameModel parent) : base(parent){ }
        private int _currentHp;
        public event ModelEvent<Damageable> OnHpChanged;
        public event ModelEvent<Damageable> OnDie;
        public virtual int CurrentHp{
            get => _currentHp;
            set{
                if (value == _currentHp) return;
                _currentHp = Math.Clamp(value, 0, HpUpLimit);
                OnHpChanged?.Invoke(currentGame, this);
                if(_currentHp <= 0) Die();
            }
        }

        public virtual int HpUpLimit{ get; set; }

        public event ModelEvent<Damageable> OnArmorChanged;
        private int _armor = 0;
        public virtual int Armor{
            set{
                _armor = Math.Max(value, 0);
                OnArmorChanged?.Invoke(currentGame, this);
            }
            get => _armor;
        }

        public event ModelEvent<Damage> OnTakeDamage;
        public virtual void TakeDamage(Damage damage){
            var finalPoint = damage.FinalDamagePoint;
            damage.lifeDeductionPoint = Math.Max(finalPoint - Armor, 0);
            CurrentHp -= Math.Max(finalPoint - Armor, 0);
            Armor -= finalPoint;
            OnTakeDamage?.Invoke(currentGame, damage);
        }
        
        private readonly List<Buff.Buff> _buffs = new();
        public event ModelEvent<Buff.Buff> OnBuffLayerAdded;
        public event ModelEvent<Buff.Buff> OnBuffLayerRemoved;

        public void AddBuffLayer<TBuff>(int layer) where TBuff : Buff.Buff{
            var buff = Buff.Buff.GetBuffOfTypeFrom<TBuff>(this);
            if (buff == null){
                buff = Buff.Buff.MakeBuff<TBuff>(this, layer);
                _buffs.Add(buff);
            } else{
                buff.AddLayer(layer);
            }
            OnBuffLayerAdded?.Invoke(currentGame, buff);
        }

        public void AddBuff(Buff.Buff buff){
            foreach (var curBuff in _buffs){
                if (curBuff.GetType() == buff.GetType()){
                    curBuff.layer += buff.layer;
                    OnBuffLayerAdded?.Invoke(currentGame, curBuff);
                    return;
                }
            }
            _buffs.Add(buff);
            buff.holder = this;
            OnBuffLayerAdded?.Invoke(currentGame, buff);
        }

        public void RemoveBuffLayer<TBuff>(int layer) where TBuff : Buff.Buff{
            var buff = Buff.Buff.GetBuffOfTypeFrom<TBuff>(this);
            if (buff == null) return;
            buff.RemoveLayer(layer);
            if (buff.layer == 0){
                _buffs.Remove(buff);
            }
            OnBuffLayerRemoved?.Invoke(currentGame, buff);
        }

        public IEnumerable<Buff.Buff> GetAllBuffs() => _buffs.ToArray();

        private IEnumerable<IBuffEffect<Damageable>> GetOnTurnBeginBuffEffect(){
            var triggers = Buff.Buff.GetBuffOfTriggerFrom<IBuffTriggerOnTurnBegin>(this);
            return triggers?.Select(t => t.OnTurnBegin());
        }

        private IEnumerable<IBuffEffect<Damageable>> GetOnTurnEndBuffEffect(){
            var triggers = Buff.Buff.GetBuffOfTriggerFrom<IBuffTriggerOnTurnEnd>(this);
            return triggers?.Select(t => t.OnTurnEnd());
        }

        public void ExecuteOnTurnEndBuffEffect(){
            var effects = GetOnTurnEndBuffEffect();
            if (effects == null) return;
            foreach (var buffEffect in effects){
                buffEffect.Execute(this);
            }
        }

        public void ExecuteOnTurnBeginBuffEffect(){
            var effects = GetOnTurnBeginBuffEffect();
            if (effects == null) return;
            foreach (var buffEffect in effects){
                buffEffect.Execute(this);
            }
        }

        public virtual void Die(){
            OnDie?.Invoke(currentGame, this);
        }
    }
}