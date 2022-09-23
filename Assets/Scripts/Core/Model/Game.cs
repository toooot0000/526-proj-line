using System.Collections.Generic;

namespace Core.Model{
    public delegate void ModelEvent(Game game, GameModel model);

    public delegate void SimpleModelEvent(Game game);

    public class Game: GameModel{

        public enum Turn {
            Player,
            Enemy,
        }

        public Turn turn = Turn.Player;
        
        public Player player;
        // public List<Enemy> enemies;

        public Stage curStage;
        
        public event SimpleModelEvent OnTurnChanged;
        

        public Game(GameModel parent = null) : base(parent){
            currentGame = this;
            player = new(this);
            curStage = new(this, new[]{ new Enemy(this), new Enemy(this) });
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

        public void SwitchTurn(){
            if (turn == Turn.Player){
                turn = Turn.Enemy;
            } else{
                turn = Turn.Player;
            }
            OnTurnChanged?.Invoke(this);
        }
    }
}