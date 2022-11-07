using UnityEngine;

namespace Model.Mechanics.PlayableObjects{
    
    public abstract class MineEffect{
        public abstract void Execute();
    }
    
    public class Mine: GameModel, IPlayableObject{
        public RectInt InitGridPosition{ get; set; }
        public MineEffect effect;

        public Mine(GameModel parent, RectInt rectInt) : base(parent) {
            InitGridPosition = rectInt;
        }
    }
}