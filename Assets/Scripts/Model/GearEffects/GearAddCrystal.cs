using Core.PlayArea.Crystals;
using Model.Mechanics.PlayableObjects;
using Utility;

namespace Model.GearEffects{
    public class GearAddCrystal:GearEffectBase{
        public GearAddCrystal(string[] args) : base(args){ }
        public override void Execute(StageActionPlayerAction action){
            var type = EnumUtility.GetValue<CrystalType>(args[0]);
            var ct = GameManager.SharedGame.playArea.MakeAndPlaceCrystal(type, float.Parse(args[1]));
            GameManager.shared.playAreaManager.GetManager<CrystalManager>().Place(ct);
        }
    }
}