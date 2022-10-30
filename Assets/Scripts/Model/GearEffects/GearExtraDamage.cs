namespace Model.GearEffects{
    public class GearExtraDamage : GearEffectBase{
        public GearExtraDamage(string[] args) : base(args){ }

        public override void Execute(StageActionInfoPlayerAction actionInfo){
            actionInfo.damage.totalPoint += int.Parse(args[0]);
        }
    }
}