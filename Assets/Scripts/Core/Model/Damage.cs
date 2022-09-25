using System;

namespace Core.Model{
    
    public interface IDamageable{
        public void TakeDamage(Damage damage);
        public int CurrentHp{ get; set; }
        public int HpUpLimit{ get; set; }
    }
    
    [Serializable]
    public class Damage: GameModel{

        public enum Type{
            Physics,
            Magic,
        }
        
        public int point;
        public Type type;
        public IDamageable target;
        public IDamageable source;
        public Damage(GameModel parent) : base(parent){ }
    }
}