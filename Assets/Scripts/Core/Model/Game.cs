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

        public Enemy CurEnemy => curStage.enemies[0];
        
        public event SimpleModelEvent OnTurnChanged;
        public event SimpleModelEvent OnStageLoaded;


        public Game(GameModel parent = null) : base(parent){
            currentGame = this;
            InitPlayer();
            LoadStage(0);
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

        private void SwitchTurn(){
            if (turn == Turn.Player){
                turn = Turn.Enemy;
            } else{
                turn = Turn.Player;
            }
            OnTurnChanged?.Invoke(this);
        }

        private void LoadStage(int id){
            curStage = new Stage(this, id);
            OnStageLoaded?.Invoke(this);
        }

        private void InitPlayer(){
            player = new(this);
            player.OnAttack += (game, model) => SwitchTurn();
        }
    }
}