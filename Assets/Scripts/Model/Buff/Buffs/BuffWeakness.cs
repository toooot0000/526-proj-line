namespace Model.Buff.Buffs{

    public class BuffWeakness: Buff, IBuffTriggerOnTurnEnd<IDamageable>, IBuffTriggerOnGetPlayerActionInfo, IBuffTriggerOnGetEnemyActionInfo{
        class TurnEndEffect: IBuffEffect<IDamageable>{
            private readonly BuffWeakness _buff;
            public TurnEndEffect(BuffWeakness buff){
                _buff = buff;
            }
            public void Execute(IDamageable damageable){
                _buff.RemoveLayer(1);
            }
        }
        class PlayerActionEffect : IBuffEffect<StageActionInfoPlayerAction>{
            public void Execute(StageActionInfoPlayerAction buffable){
                buffable.damage.totalPoint /= 2;
            }
        }

        class EnemyActionEffect : IBuffEffect<StageActionInfoEnemyAction>{
            public void Execute(StageActionInfoEnemyAction buffable){
                buffable.damage.totalPoint /= 2;
            }
        }

        public BuffWeakness(GameModel parent, int layer) : base(parent, layer){ SetUp(); }

        public IBuffEffect<IDamageable> OnTurnEnd(){
            return new TurnEndEffect(this);
        }

        public IBuffEffect<StageActionInfoPlayerAction> OnGetPlayerActionInfo(){
            return new PlayerActionEffect();
        }

        public IBuffEffect<StageActionInfoEnemyAction> OnGetEnemyActionInfo(){
            return new EnemyActionEffect();
        }

        protected override string GetBuffName() => "weakness";
    }
}