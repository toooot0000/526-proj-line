using Model.EnemySpecialAttacks;
using Model.GearEffects;

namespace Model{
    public abstract class StageActionInfoBase : GameModel{
        public Damage damage = null;
        protected StageActionInfoBase(GameModel parent) : base(parent){ }
        public abstract void Execute();
    }

    public class StageActionInfoEnemyAttack : StageActionInfoBase{
        public StageActionInfoEnemyAttack(GameModel parent) : base(parent){ }

        public override void Execute(){
            damage.target.TakeDamage(damage);
        }
    }

    public class StageActionInfoPlayerAttack : StageActionInfoBase{
        public readonly GearEffectBase[] effects;
        public Ball[] circledBalls;
        public int defend;
        public Ball[] hitBalls;

        public StageActionInfoPlayerAttack(GameModel parent, GearEffectBase[] effects) : base(parent){
            this.effects = effects;
        }

        public override void Execute(){
            foreach (var effect in effects) effect.Execute(this);
            damage?.target.TakeDamage(damage);
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
        public int defend = 0;
        public SpecialAttackBase special;
        public StageActionInfoEnemySpecial(GameModel parent) : base(parent){ }

        public override void Execute(){
            special.Execute(this);
            damage?.target.TakeDamage(damage);
            currentGame.CurrentEnemy.Armor += defend;
        }
    }
}