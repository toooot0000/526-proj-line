namespace Model.GearEffects{
    public class GearExtraDamage : GearEffectBase{
        public GearExtraDamage(string[] args) : base(args){ }

        public override void Execute(StageActionPlayerAction action){
            action.damage.initPoint += int.Parse(args[0]);
        }
    }
}