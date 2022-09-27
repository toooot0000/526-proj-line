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
        
        public Ball(GameModel parent, int id, String desc, Type type, int point, float size, float speed, float charge, float combo): base(parent){
            this.id = id;
            this.desc = desc;
            this.type = type;
            this.point = point;
            this.size = size;
            this.speed = speed;
            this.charge = charge;
            this.combo = combo;
        }
    }
}