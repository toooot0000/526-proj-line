namespace Model.Debuff{
    public class DebuffBall: Ball{

        public int debuffBallId = 0;
        
        public DebuffBall(GameModel parent) : base(parent){ }

        public DebuffBall(Gear parent, int id) : base(parent, id){
            
        }
        
        
        
    }
}