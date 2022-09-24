using System;
using System.Collections.Generic;
using System.ComponentModel;
using Utility;
using Utility.Loader;

namespace Core.Model{
    [Serializable]
    public class Ball: GameModel{
        public enum Type{
            [Description("physics")]
            Physics,
            [Description("magic")]
            Magic,
            [Description("defend")]
            Defend,
        }
        
        public int id;
        public String desc;
        public Type type;
        public int point;
        public float size;
        public float speed;
        public float charge;
        public float combo;
        
        public Ball(GameModel parent) : base(parent){ }

        public Ball(GameModel parent, int id) : base(parent) {
            this.id = id;
            var ball = CsvLoader.TryToLoad("Configs/balls", id);
            if (ball == null) return;
            try {
                type = EnumUtility.GetValue<Type>(ball["type"] as string);
            }
            catch (Exception e) {
            }
        }
    }
}