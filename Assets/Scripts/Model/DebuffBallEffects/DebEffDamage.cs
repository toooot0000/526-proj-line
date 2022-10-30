namespace Model.DebuffBallEffects{
    public class DebEffDamage: DebuffBallEffectBase{
        public DebEffDamage(string[] args) : base(args){ }
        public override void Execute(StageActionInfoPlayerAction actionInfo){
            // attackInfo.lifeDeduction += int.Parse(args[0]);
        }
    }
}