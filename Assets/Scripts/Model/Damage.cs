using System;
using Model.Buff;
using UnityEngine;

namespace Model{
    public interface IDamageable: IBuffHolder{
        public int CurrentHp{ get; set; }
        public int HpUpLimit{ get; set; }
        public int Armor{ get; set; }
        public void TakeDamage(Damage damage);
    }

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
        public IDamageable source;
        public IDamageable target;
        public int finalDamagePoint = 0;

        public Damage(GameModel parent, Type type, int point, IDamageable target): base(parent){
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