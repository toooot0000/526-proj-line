namespace Model.GearEffects{
    public class GearCmbExtraDamage: GearEffectBase{
        public GearCmbExtraDamage( string[] args) : base(args){ }
        public override void Execute(StageActionInfoPlayerAttack attackInfo){
            attackInfo.damage.totalPoint += int.Parse(args[0]);
        }
    }
}