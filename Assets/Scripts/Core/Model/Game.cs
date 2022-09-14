using System.Collections.Generic;

namespace Core.Model{
    public class Game: GameModel{

        public enum Turn{
            Player,
            Enemy,
        }

        public Turn turn = Turn.Player;
        public List<Ball> currentSelectBalls = new List<Ball>();
        
        public Player player;
        public Enemy[] enemies;

        public Game(GameModel parent = null) : base(parent){
            currentGame = this;
            player = new Player(this);
            enemies = new Enemy[3]{ new Enemy(this), new Enemy(this), new Enemy(this) };
        }

        public List<Ball> GetAllBalls(){
            var ret = new List<Ball>();
            foreach (var item in player.gears){
                for (var i = 0; i < item.ballNum; i++){
                    ret.Add(item.ball);
                }
            }
            return ret;
        }
        
    }
}