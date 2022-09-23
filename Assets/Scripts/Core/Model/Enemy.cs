using System;

namespace Core.Model{
    [Serializable]
    public class Enemy: GameModel, IDamageable{
        public int hpUpLimit = 1000;
        public int currentHp;
        public int attack = 1;

        public event ModelEvent OnAttack;
        public event ModelEvent OnBeingAttacked;
        public event ModelEvent OnDie;

        public Enemy(GameModel parent) : base(parent){
            currentHp = hpUpLimit;
        }
        public void TakeDamage(Damage damage){
            currentHp -= damage.point;
            OnBeingAttacked?.Invoke(currentGame, this);
        }

        public void Attack(){
            currentGame.player.TakeDamage(new Damage(){
                point = attack,
                type = Damage.Type.Physics,
                target = currentGame.player
            });
            OnAttack?.Invoke(currentGame, this);
        }
    }
}