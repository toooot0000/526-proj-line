using System;
using UnityEngine;
using Utility.Loader;

namespace Core.Model{
    [Serializable]
    public class Enemy: GameModel, IDamageable{
        public int HpUpLimit{ set; get; }
        
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
            CurrentHp = HpUpLimit;
        }

        public Enemy(GameModel parent, int id) : base(parent) {
            this.id = id;
            var enemy = CsvLoader.TryToLoad("Configs/enemies", id);
            if (enemy == null) return;
            desc = enemy["desc"] as string;
            special = enemy["special"] as string;
            cooldown = (int)enemy["cooldown"];
            HpUpLimit = (int)enemy["hp"];
            attack = (int)enemy["attack"];
            CurrentHp = HpUpLimit;
        }
        
        public void TakeDamage(Damage damage){
            CurrentHp -= damage.point;
            OnBeingAttacked?.Invoke(currentGame, this);
        }

        public void Attack(){
            var dmg = new Damage(currentGame){
                point = attack,
                type = Damage.Type.Physics,
                target = currentGame.player,
                source = this
            };
            OnAttack?.Invoke(currentGame, this);
            currentGame.currentStage.ProcessDamage(dmg);
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