namespace Model.DebuffBall{
    public abstract class DebuffBallEffectBase{
        protected string[] args;

        protected DebuffBallEffectBase(string[] args){
            this.args = args;
        }
        public abstract void Execute(StageActionPlayerAction action);
    }
}