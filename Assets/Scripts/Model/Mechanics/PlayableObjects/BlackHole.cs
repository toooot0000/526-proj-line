using UnityEngine;

namespace Model.Mechanics.PlayableObjects{
    public class BlackHole:GameModel, IPlayableObject{
        protected BlackHole(GameModel parent) : base(parent){ }
        public RectInt InitGridPosition{ get; set; }
    }
}