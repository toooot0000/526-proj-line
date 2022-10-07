namespace Model.GearEffects{
    public class GearExtraDamage : GearEffectBase{
        public GearExtraDamage(string[] args) : base(args){ }

        public override void Execute(StageActionInfoPlayerAttack attackInfo){
            attackInfo.damage.totalPoint += int.Parse(args[0]);
        }
    }
}