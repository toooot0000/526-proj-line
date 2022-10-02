using System;
using System.ComponentModel;
using System.Linq;
using Utility;
using Utility.Loader;

namespace Model{
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
        public string imgPath;
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
            ball = new Ball(this, (int)gear["ball_id"]);
            ballNum = (int)gear["ball_num"];
            chargeEffect = gear["charge_effect"] as string;
            comboEffect = gear["combo_effect"] as string;
            cooldown = (int)gear["cooldown"];
            imgPath = gear["img_path"] as string;
        }

        public bool isCharge()
        {
            Player player = (Player)base.parent;
            int n = player.circledBalls.Count;
            for (int i = 0; i < n; i++)
            {
                Ball curBall = player.circledBalls.ElementAt(i);
                if (((Gear)curBall.parent).id == id)
                    return true;
            }
            return false;
        }
        
        public bool isCombo()
        {
            Player player = (Player)base.parent;
            int n = player.hitBalls.Count;
            for (int i = 0; i < n; i++)
            {
                Ball curBall = player.circledBalls.ElementAt(i);
                if (((Gear)curBall.parent).id == id)
                    return true;
            }
            return false;
        }
    }
}