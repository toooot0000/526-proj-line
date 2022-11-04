using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Model.Obstacles{
    
    
    public enum BlockLevel: int{
        [Description("small")]
        Small = 3,
        [Description("medium")]
        Medium = 4,
        [Description("large")]
        Large = 5
    }
    
    public class Block: GameModel, IPlayableObject{

        public BlockLevel level;
        public RectInt RectInt{ get; set; }

        public Block(GameModel parent, RectInt rectInt, BlockLevel level) : base(parent){
            RectInt = rectInt;
            this.level = level;
        }
        
        public static Vector2Int GenerateGridSize(BlockLevel level){
            var width = Random.Range(1, (int)level);
            return new Vector2Int(width, (int)level - width);
        }
    }
}