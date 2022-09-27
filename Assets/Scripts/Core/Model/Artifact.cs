using System;

namespace Core.Model{
    [Serializable]
    public class Artifact: GameModel{
        public int id;
        public String desc;
        public int rarity;

        public Artifact(int id, String desc, int rarity, GameModel parent) : base(parent){
            this.id = id;
            this.desc = desc;
            this.rarity = rarity;
        }
    }
}