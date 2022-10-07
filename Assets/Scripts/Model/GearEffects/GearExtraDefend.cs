namespace Model.GearEffects{
    public class GearExtraDefend : GearEffectBase{
        public GearExtraDefend(string[] args) : base(args){ }

        public override void Execute(StageActionInfoPlayerAttack attackInfo){
            attackInfo.defend += int.Parse(args[0]);
        }
    }
}