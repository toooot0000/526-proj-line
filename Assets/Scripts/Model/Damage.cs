using System;

namespace Model{
    public interface IDamageable{
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

        public int totalPoint;

        // public int hpDeduction;
        // public int armorDeduction;
        public Type type;
        public IDamageable source;
        public IDamageable target;
        public int finalDamagePoint = 0;
        public Damage(GameModel parent) : base(parent){ }
    }
}