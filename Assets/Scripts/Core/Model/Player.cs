using System;

namespace Core.Model{
    [Serializable]
    public class Player: GameModel, IDamageable{

        public int hpUpLimit;
        public int currentHp;
        public Gear[] gears;
        public int gearUpLimit;
        public int energy;
        
        
        public void TakeDamage(Damage damage){
            currentHp -= damage.point;
        }

        public Player(GameModel parent) : base(parent){ }
    }
}