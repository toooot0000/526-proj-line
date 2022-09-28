using System;

namespace Model{
    [Serializable]
    public class Artifact: GameModel{
        public int id;
        public String desc;
        public int rarity;
        public Artifact(GameModel parent) : base(parent){ }
    }
}