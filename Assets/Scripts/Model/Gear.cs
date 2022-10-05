﻿using System;
using System.ComponentModel;
using System.Linq;
using Model.GearEffects;
using Utility;
using Utility.Loader;

namespace Model{
    [Serializable]
    public class Gear: GameModel{
        public int id;
        public string name;
        public string desc;
        public int rarity;
        public GearType type;
        public int ballId;
        public Ball ball;
        public int ballNum;
        // public string chargeEffect;
        // public string comboEffect;
        public int cooldown;
        public string imgPath;

        public int chargeNum;
        public int comboNum;
        public GearEffectBase chargeEffect;
        public GearEffectBase comboEffect;
        public Gear(GameModel parent) : base(parent){ }

        public Gear(GameModel parent, int id) : base(parent) {
            var gear = CsvLoader.TryToLoad("Configs/gears", id);
            if (gear == null) return;
            this.id = id;
            name = gear["name"] as string;
            desc = gear["desc"] as string;
            rarity = (int)gear["rarity"];
            try {
                type = EnumUtility.GetValue<GearType>(gear["type"] as string);
            }
            catch (Exception e) {
                type = GearType.Weapon;
            }
            ball = new Ball(this, (int)gear["ball_id"]);
            ballNum = (int)gear["ball_num"];
            cooldown = (int)gear["cooldown"];
            imgPath = gear["img_path"] as string;

            chargeNum = (int)gear["charge_num"];
            comboNum = (int)gear["combo_num"];
            
            var spStr = (gear["charge_effect"] as string)!.Split(";");
            var className = spStr.First();
            if (className != null && !className.Equals("")){
                chargeEffect = Activator.CreateInstance(Type.GetType($"Model.GearEffects.{className}", true), new object[]{spStr[1..]}) as
                    GearEffectBase;
            }
            
            spStr = (gear["combo_effect"] as string)!.Split(";");
            className = spStr.First();
            if (className != null && !className.Equals("")){
                comboEffect = Activator.CreateInstance(Type.GetType($"Model.GearEffects.{className}", true), new object[]{spStr[1..]}) as
                    GearEffectBase;
            }
        }

        public bool IsCharged(){
            if (chargeNum == -1) return false;
            if (chargeEffect == null) return false;
            var player = (parent as Player)!;
            return player.circledBalls.Count >= chargeNum && player.circledBalls.Any(b => b.parent == this);
        }
        
        public bool IsComboIng(){
            if (comboNum == -1) return false;
            if (comboEffect == null) return false;
            var player = (parent as Player)!;
            return player.hitBalls.Count >= comboNum && player.hitBalls.Any(b => b.parent == this) ;
        }
    }

    public enum GearType{
        [Description("weapon")]
        Weapon,
        [Description("shield")]
        Shield,
    }
}