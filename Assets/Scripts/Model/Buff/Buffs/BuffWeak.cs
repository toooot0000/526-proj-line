namespace Model.Buff.Buffs{

    public class BuffWeak: Buff, IBuffTriggerOnTurnEnd, IBuffTriggerOnGetPlayerActionInfo, IBuffTriggerOnGetEnemyActionInfo{
        class TurnEndEffect: IBuffEffect<IDamageable>{
            public void Execute(IDamageable damageable){
                var buff = Buff.GetBuffOfTypeFrom<BuffWeak>(damageable);
                buff?.RemoveLayer(1);
            }
        }
        class PlayerActionEffect : IBuffEffect<StageActionPlayerAction>{
            public void Execute(StageActionPlayerAction buffable){
                buffable.damage.initPoint /= 2;
            }
        }

        class EnemyActionEffect : IBuffEffect<StageActionEnemyAction>{
            public void Execute(StageActionEnemyAction buffable){
                buffable.damage.initPoint /= 2;
            }
        }

        public BuffWeak(GameModel parent, int layer) : base(parent, layer){ SetUp(); }

        public IBuffEffect<IDamageable> OnTurnEnd(){
            return new TurnEndEffect();
        }

        public IBuffEffect<StageActionPlayerAction> OnGetPlayerActionInfo(){
            return new PlayerActionEffect();
        }

        public IBuffEffect<StageActionEnemyAction> OnGetEnemyActionInfo(){
            return new EnemyActionEffect();
        }

        protected override string GetBuffName() => "weak";
    }
}