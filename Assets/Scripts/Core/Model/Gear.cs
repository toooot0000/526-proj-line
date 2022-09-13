using System;

namespace Core.Model{
    [Serializable]
    public class Gear: GameModel{
        public enum Type{
            Weapon,
            Shield,
        }
        public int id;
        public String name;
        public String desc;
        public int rarity;
        public Type type;
        public int ballId;
        public int ballNum;
        public String chargeEffect;
        public String combEffect;
        public int cooldown;
        public Gear(GameModel parent) : base(parent){ }
    }
}