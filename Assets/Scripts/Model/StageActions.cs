using System;
using System.Collections.Generic;
using Model.Buff;
using Model.EnemySpecialAttacks;
using Model.GearEffects;

namespace Model{
    public abstract class StageActionBase : GameModel, IBuffModifiable{
        public Damage damage = null;
        public readonly List<Damage> extraDamages = new();
        protected StageActionBase(GameModel parent) : base(parent){ }
        public abstract void Execute();

        public void AddExtraDamage(Damage extraDamage){
            extraDamages.Add(extraDamage);
        }

        public void ComputeAllDamages(){
            damage?.Execute();
            foreach (var extraDamage in extraDamages){
                extraDamage.Execute();
            }
        }
    }

    public class StageActionPlayerAction : StageActionBase{
        public readonly GearEffectBase[] effects;
        public Ball[] circledBalls;
        public int defend;
        public Ball[] hitBalls;
        public readonly IBuffEffect<StageActionPlayerAction>[] buffEffects;

        public StageActionPlayerAction(GameModel parent, GearEffectBase[] effects, IBuffEffect<StageActionPlayerAction>[] buffEffects) : base(parent){
            this.effects = effects;
            this.buffEffects = buffEffects;
        }

        public override void Execute(){
            ExecuteSpecials();
            currentGame.player.Armor += defend;
            ComputeAllDamages();
        }
        
        public void ExecuteSpecials(){
            foreach (var effect in effects) effect.Execute(this);
            foreach (var effect in buffEffects) effect.Execute(this);
        }
    }

    public class StageActionEnemyDefend : StageActionEnemyAction{
        public StageActionEnemyDefend(GameModel parent) : base(parent){ }
    }
    
    public class StageActionEnemyAttack : StageActionEnemyAction{
        public StageActionEnemyAttack(GameModel parent) : base(parent){ }
    }

    public class StageActionEnemySpecial : StageActionEnemyAction{
        public StageActionEnemySpecial(GameModel parent) : base(parent){ }
    }

    public class StageActionEnemyAction : StageActionBase{
        public int defend = 0;
        public SpecialAttackBase special = null;
        public StageActionEnemyAction(GameModel parent) : base(parent){ }
        public override void Execute(){
            ExecuteSpecials();
            currentGame.CurrentEnemy.Armor += defend;
            ComputeAllDamages();
        }

        public void ExecuteSpecials(){
            special?.Execute(this);
        }
    }
}