namespace Model.GearEffects{
    public class GearChgExtraDefend : GearEffectBase{
        public GearChgExtraDefend(string[] args) : base(args){ }

        public override void Execute(StageActionInfoPlayerAttack attackInfo){
            attackInfo.defend += int.Parse(args[0]);
        }
    }
}