namespace Core.Model{
    public class Game: GameModel{
        public Player player;
        public Enemy[] enemies;

        public Game(GameModel parent = null) : base(parent){
            currentGame = this;
            player = new Player(this);
            enemies = new Enemy[3]{ new Enemy(this), new Enemy(this), new Enemy(this) };
        }
    }
}