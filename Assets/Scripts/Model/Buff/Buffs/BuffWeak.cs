namespace Model.Buff.Buffs{

    public class BuffWeak: Buff, IBuffTriggerOnTurnEnd, IBuffTriggerOnGetPlayerActionInfo, IBuffTriggerOnGetEnemyActionInfo{
        private class TurnEndEffect: IBuffEffect<Damageable>{
            public void Execute(Damageable damageable){
                var buff = Buff.GetBuffOfTypeFrom<BuffWeak>(damageable);
                buff?.RemoveLayer(1);
            }
        }

        private class PlayerActionEffect : IBuffEffect<StageActionPlayerAction>{
            public void Execute(StageActionPlayerAction buffable){
                buffable.damage.initPoint /= 2;
            }
        }

        private class EnemyActionEffect : IBuffEffect<StageActionEnemyAction>{
            public void Execute(StageActionEnemyAction buffable){
                buffable.damage.initPoint /= 2;
            }
        }

        public BuffWeak(GameModel parent, int layer) : base(parent, layer){ SetUp(this); }

        public IBuffEffect<Damageable> OnTurnEnd(){
            return new TurnEndEffect();
        }

        public IBuffEffect<StageActionPlayerAction> OnGetPlayerActionInfo(){
            return new PlayerActionEffect();
        }

        public IBuffEffect<StageActionEnemyAction> OnGetEnemyActionInfo(){
            return new EnemyActionEffect();
        }

        protected sealed override string GetBuffName() => "weak";
    }
}