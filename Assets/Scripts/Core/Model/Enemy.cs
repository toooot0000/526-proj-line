using System;
using UnityEngine;
using Utility.Loader;

namespace Core.Model{
    [Serializable]
    public class Enemy: GameModel, IDamageable{
        public int hpUpLimit = 1000;
        public int currentHp;
        public int attack = 1;

        public int id;

        public String desc;

        public String special;

        public int cooldown;


        public event ModelEvent OnAttack;
        public event ModelEvent OnBeingAttacked;
        public event ModelEvent OnDie;

        public Enemy(GameModel parent) : base(parent){
            currentHp = hpUpLimit;
        }

        public Enemy(GameModel parent, int id) : base(parent) {
            this.id = id;
            var enemy = CsvLoader.TryToLoad("Configs/enemies", id);
            if (enemy == null) return;
            try {
                type = EnumUtility.GetValue<Type>(ball["type"] as string);
                desc = ball["desc"] as string;
                special = ball["special"] as string;
                cooldown = (int)ball["cooldown"];
                hpUpLimit = (int)ball["hp"];
                attack = (int)ball["attack"];
            }
            catch (Exception e) {
            }
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

        public void Die()
        {
            OnDie?.Invoke(currentGame, this);
        }
    }
}