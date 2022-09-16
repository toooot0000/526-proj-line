using System.Collections.Generic;

namespace Core.Model{
    public delegate void ModelEvent(Game game, GameModel model);

    public class Game: GameModel{

        public enum Turn{
            Player,
            Enemy,
        }

        public Turn turn = Turn.Player;
        
        public Player player;
        public List<Enemy> enemies;

        public Game(GameModel parent = null) : base(parent){
            currentGame = this;
            player = new(this);
            enemies = new();
            enemies.Add(new(this));
            enemies.Add(new(this));
            enemies.Add(new(this));
        }

        public List<Ball> GetAllSkillBalls(){
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