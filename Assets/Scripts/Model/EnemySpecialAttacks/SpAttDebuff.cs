using Utility;
using Utility.Loader;

namespace Model.EnemySpecialAttacks{
    public class SpAttDebuff: SpecialAttackBase{
        public override string Desc => $"Cast {_layer.ToString()} layers of {_displayName}";
        private readonly string _displayName;
        private readonly int _layer;
        private readonly string _buffName;
        public SpAttDebuff(string[] args) : base(args){
            _buffName = args[0];
            var buffId = Buff.Buff.NameToId[_buffName];
            var info = CsvLoader.TryToLoad("Configs/buffs", buffId);
            _displayName = info["display_name"] as string;
            _layer = IntUtility.ParseString(args[1]);
        }
        
        public override void Execute(StageActionBase info){
            var buff = Buff.Buff.MakeBuffByBuffName(_buffName, GameManager.SharedGame.player, _layer);
            GameManager.SharedGame.player.AddBuff(buff);
        }
    }
}