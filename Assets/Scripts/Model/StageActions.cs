using System;
using System.Collections;
using System.Collections.Generic;
using Model.Buff;
using Model.EnemySpecialAttacks;
using Model.GearEffects;
using Model.Mechanics.PlayableObjects;

namespace Model{
    public abstract class StageActionBase : GameModel, IBuffModifiable{
        public Damage damage = null;
        private readonly List<Damage> _extraDamages = new();
        protected StageActionBase(GameModel parent) : base(parent){ }
        public abstract void Execute();

        public void AddExtraDamage(Damage extraDamage){
            _extraDamages.Add(extraDamage);
        }

        public void ResolveAllDamages(){
            damage?.Resolve();
            foreach (var extraDamage in _extraDamages){
                extraDamage.Resolve();
            }
        }

        public IEnumerator ResolveAllDamagesEnumerator(){
            damage?.Resolve();
            foreach (var extraDamage in _extraDamages){
                yield return null;
                extraDamage.Resolve();
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
            ResolveAllDamages();
        }
        
        public void ExecuteSpecials(){
            foreach (var effect in effects ?? Array.Empty<GearEffectBase>()) effect.Execute(this);
            foreach (var effect in buffEffects ?? Array.Empty<IBuffEffect<StageActionPlayerAction>>()) effect.Execute(this);
        }
        
        /// <summary>
        /// Return false if all Player invoke action is empty!
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool IsEmpty(StageActionPlayerAction action){
            return action.damage.initPoint == 0 
                   && action.effects.Length == 0 
                   && action.defend == 0;
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
            ResolveAllDamages();
        }

        public void ExecuteSpecials(){
            special?.Execute(this);
        }
    }
}