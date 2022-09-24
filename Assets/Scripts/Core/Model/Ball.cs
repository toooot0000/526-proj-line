using System;
using System.Collections.Generic;

namespace Core.Model{
    [Serializable]
    public class Ball: GameModel{
        public enum Type{
            Physics,
            Magic,
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
    }
}