using UnityEngine;

namespace Model.Obstacles{
    
    public abstract class MineEffect{
        public abstract void Execute(Player player);
    }
    
    public class Mine: GameModel, IPlayableObject{
        public RectInt RectInt{ get; set; }
        public MineEffect effect;
        public Mine(GameModel parent, RectInt rectInt) : base(parent){ }
    }
}