using System;

namespace Core.Model{
    [Serializable]
    public class Enemy: GameModel, IDamageable
    {
        public int id;
        public string desc;
        public int hp;
        public int attack;
        public string special;
        public int cooldown;
        public int currentHp;

        public event ModelEvent OnAttack;
        public event ModelEvent OnBeingAttacked;
        public event ModelEvent OnDie;

        public Enemy(GameModel parent) : base(parent){
            hp = 1000;
            attack = 1;
            currentHp = hp;
        }
        
        public Enemy(GameModel parent, int id, string desc, int hp, int attack, string special, int cooldown, int currentHp) : base(parent){
            this.id = id;
            this.desc = desc;
            this.hp = hp;
            this.attack = attack;
            this.special = special;
            this.cooldown = cooldown;
            this.currentHp = hp;
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