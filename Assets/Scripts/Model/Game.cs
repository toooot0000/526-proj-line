using System.Collections.Generic;
using JetBrains.Annotations;

namespace Model{
    public delegate void ModelEvent(Game game, GameModel model);

    public delegate void SimpleModelEvent(Game game);

    public class Game: GameModel{

        public enum Turn {
            Player,
            Enemy,
        }

        public Turn turn = Turn.Player;
        
        public Player player;

        public Stage currentStage;

        public Enemy CurrentEnemy => currentStage.CurrentEnemy;
        
        public event SimpleModelEvent OnTurnChanged;
        public event SimpleModelEvent OnGameEnd;

        [NotNull] public event SimpleModelEvent OnGameComplete;
        
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

        public void SwitchTurn(){
            turn = turn == Turn.Player ? Turn.Enemy : Turn.Player;
            OnTurnChanged?.Invoke(this);
        }

        private void LoadStage(int id) {
            if (id == -1) {
                Complete();
                return;
            }
            currentStage = new Stage(this, id);
            OnStageLoaded?.Invoke(this);
            turn =  Turn.Player;
            OnTurnChanged?.Invoke(this);
        }

        private void InitPlayer(){
            player = new(this);
        }

        public void End()
        {
            // blabla
            
            OnGameEnd?.Invoke(this);
        }

        public void Complete() {
            // blabla
            
            OnGameComplete?.Invoke(this);
        }

        public void GoToNextStage() {
            LoadStage(currentStage.nextStage);
        }
    }
}