namespace Model.DebuffBall.Effects{
    public class DebEffDamage: DebuffBallEffectBase{
        public DebEffDamage(string[] args) : base(args){ }
        public override void Execute(StageActionPlayerAction action){
            // attackInfo.lifeDeduction += int.Parse(args[0]);
        }
    }
}