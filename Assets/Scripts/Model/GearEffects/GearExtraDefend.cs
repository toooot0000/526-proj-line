namespace Model.GearEffects{
    public class GearExtraDefend : GearEffectBase{
        public GearExtraDefend(string[] args) : base(args){ }

        public override void Execute(StageActionInfoPlayerAction actionInfo){
            actionInfo.defend += int.Parse(args[0]);
        }
    }
}