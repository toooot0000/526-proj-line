using System;
using System.Data;

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
        public Ball ball;
        public int ballNum;
        public String chargeEffect;
        public String combEffect;
        public int cooldown;
        public Gear(GameModel parent) : base(parent){ }
        
        public Gear(GameModel parent, int id, String name, String desc, int rarity, Type type, int ballId, int ballNum, String chargeEffect, String combEffect, int cooldown) : base(parent){
            this.id = id;
            this.name = name;
            this.desc = desc;
            this.rarity = rarity;
            this.type = type;
            this.ballId = ballId;
            this.ballNum = ballNum;
            this.chargeEffect = chargeEffect;
            this.combEffect = combEffect;
            this.cooldown = cooldown;
        }

        public Gear(GameModel parent, DataRow row) : base(parent)
        {
            id = Convert.ToInt32(row["id"]);
            name = Convert.ToString(row["name"]);
            desc = Convert.ToString(row["desc"]);
            rarity = Convert.ToInt32(row["rarity"]);
            type = ParseType(Convert.ToString(row["type"]));
            ballId = Convert.ToInt32(row["ballId"]);
            ballNum = Convert.ToInt32(row["ballNum"]);
            chargeEffect = Convert.ToString(row["chargeEffect"]);
            combEffect = Convert.ToString(row["combEffect"]);
            cooldown = Convert.ToInt32(row["cooldown"]);
        }
        
        public static Type ParseType(String type){
            switch(type){
                case "Weapon":
                    return Type.Weapon;
                case "Shield":
                    return Type.Shield;
                default:
                    return Type.Weapon;
            }
        }
        
    }
}