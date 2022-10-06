namespace Model.GearEffects{
    public class GearChgMulti: GearEffectBase{
        public GearChgMulti(string[] args) : base(args){ }
        public override void Execute(StageActionInfoPlayerAttack attackInfo){
            attackInfo.damage.totalPoint *= int.Parse(args[0]);
        }
    }
}