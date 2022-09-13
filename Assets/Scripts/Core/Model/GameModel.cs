namespace Core.Model{
    public class GameModel{
        public Game currentGame;
        protected GameModel(GameModel parent){
            if (parent == null) return;
            currentGame = parent.currentGame;
        }
    }
}