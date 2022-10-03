using System;
using System.ComponentModel;
using Utility;
using Utility.Loader;

namespace Model{
    [Serializable]
    public class Ball: GameModel{
        public int id;
        public String desc;
        public BallType type;
        public int point;
        public float size;
        public float speed;
        public float charge;
        public float combo;
        
        public Ball(GameModel parent) : base(parent){ }

        public Ball(Gear parent, int id) : base(parent) {
            this.id = id;
            var ball = CsvLoader.TryToLoad("Configs/balls", id);
            if (ball == null) return;
            try {
                type = EnumUtility.GetValue<BallType>(ball["type"] as string);
            }
            catch (Exception e) {
                type = BallType.Physics;
            }
            
            desc = ball["desc"] as string;
            point = (int)ball["point"];
            speed = (float)ball["speed"];
            size = (float)ball["size"];
        }
    }

    public enum BallType{
        [Description("physics")]
        Physics,
        [Description("magic")]
        Magic,
        [Description("defend")]
        Defend,
        [Description("debuff")]
        Debuff,
    }
}