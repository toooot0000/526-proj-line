using System;
using System.Collections.Generic;
using System.Linq;
using Model.Buff;
using UnityEngine;

namespace Model{
    // public abstract class Damageable : IBuffHolder{
    //     public abstract int CurrentHp{ get; set; }
    //     public abstract int HpUpLimit{ get; set; }
    //     public abstract int Armor{ get; set; }
    //     public abstract void TakeDamage(Damage damage);
    //     public abstract void AddBuffLayer<TBuff>(int layer) where TBuff : Buff.Buff;
    //     public abstract Buff.Buff[] GetAllBuffs();
    //     public abstract void RemoveBUffLayer<TBuff>(int layer) where TBuff : Buff.Buff;
    // }


    [Serializable]
    public class Damage : GameModel{
        public enum Type{
            Physics,
            Magic
        }

        public int initPoint;
        public float multipleParam;
        public int addSubParam;
        public bool isPenetrate = false; 
        
        public Type type;
        public Damageable source;
        public Damageable target;
        public int finalDamagePoint = 0;

        public static Damage Default<T>(T target) where T: Damageable{
            return new(null, Type.Physics, 0, target);
        }

        public Damage(GameModel parent, Type type, int point, Damageable target): base(parent){
            this.type = type;
            this.initPoint = point;
            this.target = target;
            multipleParam = 1;
            addSubParam = 0;
        }

        public int GetFinalPoint(){
            return Mathf.RoundToInt(Math.Max(initPoint + addSubParam, 0) * multipleParam);
        }

        public void ApplyMultipleParam(float param){
            multipleParam = Mathf.Max(multipleParam + param, 0);
        }

        public void ApplyAddSubParam(int point){
            addSubParam += point;
        }

        public void Execute(){
            target.TakeDamage(this);
        }
    }
}