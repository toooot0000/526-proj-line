using Model.EnemySpecialAttacks;
using Model.GearEffects;

namespace Model{
    
    public abstract class StageActionInfoBase: GameModel{
        protected StageActionInfoBase(GameModel parent) : base(parent){ }
        public abstract void Execute();
    }

    public class StageActionInfoEnemyAttack : StageActionInfoBase{
        public Damage damage;
        public StageActionInfoEnemyAttack(GameModel parent) : base(parent){ }

        public override void Execute(){
            damage.target.TakeDamage(damage);
        }
    }
    
    public class StageActionInfoPlayerAttack : StageActionInfoBase{
        public Damage damage;
        public int defend;
        public readonly GearEffectBase[] effects;
        public StageActionInfoPlayerAttack(GameModel parent, GearEffectBase[] effects) : base(parent){
            this.effects = effects;
        }
        public override void Execute(){
            foreach (var effect in effects){
                effect.Execute(this);
            }
            damage.target.TakeDamage(damage);
            currentGame.player.Armor += defend;
        }
    }

    public class StageActionInfoEnemyDefend : StageActionInfoBase{
        public int defend;
        public StageActionInfoEnemyDefend(GameModel parent) : base(parent){ }
        public override void Execute(){
            currentGame.CurrentEnemy.Armor += defend;
        }
    }

    public class StageActionInfoEnemySpecial : StageActionInfoBase{
        public SpecialAttackBase special;
        public StageActionInfoEnemySpecial(GameModel parent) : base(parent){ }

        public override void Execute(){
            special.Execute(currentGame);
        }
    }
}