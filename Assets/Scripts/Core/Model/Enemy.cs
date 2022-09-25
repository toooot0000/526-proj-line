using System;
using UnityEngine;
using Utility.Loader;

namespace Core.Model{
    [Serializable]
    public class Enemy: GameModel, IDamageable{
        public int hpUpLimit = 1000;
        private int _currentHp;

        public int CurrentHp {
            set {
                _currentHp = value;
                if (value == 0) Die();
            }
            get => _currentHp;
        }
        public int attack = 1;

        public int id;

        public String desc;

        public String special;

        public int cooldown;


        public event ModelEvent OnAttack;
        public event ModelEvent OnBeingAttacked;
        public event ModelEvent OnDie;

        public Enemy(GameModel parent) : base(parent){
            CurrentHp = hpUpLimit;
        }

        public Enemy(GameModel parent, int id) : base(parent) {
            this.id = id;
            var enemy = CsvLoader.TryToLoad("Configs/enemies", id);
            if (enemy == null) return;
            desc = enemy["desc"] as string;
            special = enemy["special"] as string;
            cooldown = (int)enemy["cooldown"];
            hpUpLimit = (int)enemy["hp"];
            attack = (int)enemy["attack"];
            CurrentHp = hpUpLimit;
        }
        
        public void TakeDamage(Damage damage){
            CurrentHp -= damage.point;
            OnBeingAttacked?.Invoke(currentGame, this);
        }

        public void Attack(){
            OnAttack?.Invoke(currentGame, this);
            currentGame.player.TakeDamage(new Damage(){
                point = attack,
                type = Damage.Type.Physics,
                target = currentGame.player
            });
            currentGame.SwitchTurn();
        }

        private void DelayedAttack(Game game) {
            if (game.turn != Game.Turn.Enemy) return;
            GameManager.shared.Delayed(2.0f, Attack);
            
        }

        public void Die() {
            currentGame.OnTurnChanged -= DelayedAttack;
            OnDie?.Invoke(currentGame, this);
        }

        public void BecomeCurrent() {
            currentGame.OnTurnChanged += DelayedAttack;
        }
    }
}