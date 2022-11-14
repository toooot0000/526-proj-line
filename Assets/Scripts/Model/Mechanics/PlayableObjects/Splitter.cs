using UnityEngine;

namespace Model.Mechanics.PlayableObjects{
    public class Splitter: GameModel, IPlayableObject{
        public float sizeLimit = 0.5f;
        public Splitter(GameModel parent) : base(parent){ }
        public RectInt InitGridRectInt{ get; set; }
    }
}