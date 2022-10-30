using System;
using Model.Buff;
using Model.EnemySpecialAttacks;
using Model.GearEffects;

namespace Model{
    public abstract class StageActionInfoBase : GameModel, IBuffModifiable{
        public Damage damage = null;
        protected StageActionInfoBase(GameModel parent) : base(parent){ }
        public abstract void Execute();
    }

    public class StageActionInfoPlayerAction : StageActionInfoBase{
        public readonly GearEffectBase[] effects;
        public Ball[] circledBalls;
        public int defend;
        public Ball[] hitBalls;
        public readonly IBuffEffect<StageActionInfoPlayerAction>[] buffEffects;

        public StageActionInfoPlayerAction(GameModel parent, GearEffectBase[] effects, IBuffEffect<StageActionInfoPlayerAction>[] buffEffects) : base(parent){
            this.effects = effects;
            this.buffEffects = buffEffects;
        }

        public override void Execute(){
            ExecuteSpecials();
            currentGame.player.Armor += defend;
            damage?.target.TakeDamage(damage);
        }
        
        public void ExecuteSpecials(){
            foreach (var effect in effects) effect.Execute(this);
            foreach (var effect in buffEffects) effect.Execute(this);
        }
    }

    [Obsolete]
    public class StageActionInfoEnemyDefend : StageActionInfoEnemyAction{
        public StageActionInfoEnemyDefend(GameModel parent) : base(parent){ }
    }
    
    [Obsolete]
    public class StageActionInfoEnemyAttack : StageActionInfoEnemyAction{
        public StageActionInfoEnemyAttack(GameModel parent) : base(parent){ }
    }

    [Obsolete]
    public class StageActionInfoEnemySpecial : StageActionInfoEnemyAction{
        public StageActionInfoEnemySpecial(GameModel parent) : base(parent){ }
    }

    public class StageActionInfoEnemyAction : StageActionInfoBase{
        public int defend = 0;
        public SpecialAttackBase special = null;
        public StageActionInfoEnemyAction(GameModel parent) : base(parent){ }
        public override void Execute(){
            ExecuteSpecials();
            damage?.target.TakeDamage(damage);
            currentGame.CurrentEnemy.Armor += defend;
        }

        public void ExecuteSpecials(){
            special?.Execute(this);
        }
    }
}