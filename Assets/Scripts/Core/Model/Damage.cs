using System;

namespace Core.Model{
    
    public interface IDamageable{
        public void TakeDamage(Damage damage);
    }
    
    [Serializable]
    public struct Damage{

        public enum Type{
            Physics,
            Magic,
        }
        
        public int point;
        public Type type;
        public IDamageable target;
    }
}