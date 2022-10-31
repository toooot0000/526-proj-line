namespace Model.GearEffects{
    public class GearMultiDamage: GearEffectBase{
        public GearMultiDamage(string[] args) : base(args){ }
        public override void Execute(StageActionPlayerAction action){
            action.damage.initPoint *= int.Parse(args[0]);
        }
    }
}