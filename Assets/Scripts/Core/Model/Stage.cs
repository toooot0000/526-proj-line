namespace Core.Model{
    public class Stage: GameModel{

        public Enemy[] enemies;

        public Stage(GameModel parent, Enemy[] enemies) : base(parent){
            this.enemies = enemies;
        }
    }
}