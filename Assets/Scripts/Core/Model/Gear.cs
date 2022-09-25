using System;
using System.ComponentModel;
using Utility;
using Utility.Loader;

namespace Core.Model{
    [Serializable]
    public class Gear: GameModel{
        public enum Type{
            [Description("weapon")]
            Weapon,
            [Description("shield")]
            Shield,
        }
        public int id;
        public string name;
        public string desc;
        public int rarity;
        public Type type;
        public int ballId;
        public Ball ball;
        public int ballNum;
        public string chargeEffect;
        public string comboEffect;
        public int cooldown;
        public Gear(GameModel parent) : base(parent){ }

        public Gear(GameModel parent, int id) : base(parent) {
            var gear = CsvLoader.TryToLoad("Configs/gears", id);
            if (gear == null) return;
            this.id = id;
            name = gear["name"] as string;
            desc = gear["desc"] as string;
            rarity = (int)gear["rarity"];
            try {
                type = EnumUtility.GetValue<Type>(gear["type"] as string);
            }
            catch (Exception e) {
                type = Type.Weapon;
            }
            ball = new Ball(parent, (int)gear["ball_id"]);
            ballNum = (int)gear["ball_num"];
            chargeEffect = gear["charge_effect"] as string;
            comboEffect = gear["combo_effect"] as string;
            cooldown = (int)gear["cooldown"];
        }
        
    }
}