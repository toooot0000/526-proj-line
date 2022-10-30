namespace Model.GearEffects{
    public class GearMultiDamage: GearEffectBase{
        public GearMultiDamage(string[] args) : base(args){ }
        public override void Execute(StageActionInfoPlayerAction actionInfo){
            actionInfo.damage.totalPoint *= int.Parse(args[0]);
        }
    }
}