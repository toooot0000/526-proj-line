using System;
using UnityEngine;

namespace Core.Model{
    [Serializable]
    public class Enemy: GameModel, IDamageable{
        public int hpUpLimit = 10;
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
            Debug.Log($"Enemy take damage(point {damage.point}, current HP: {currentHp})");
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