using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Animations;
using Utility.Loader;

namespace Core.Model{
    public class Stage: GameModel{

        public readonly Enemy[] enemies;

        public readonly Gear[] bonusGears;

        private int _enemyIndex = 0;

        public Enemy CurrentEnemy => _enemyIndex < enemies.Length ? enemies[_enemyIndex] : null;

        public event ModelEvent OnStageBeaten;
        public event ModelEvent OnEnemyChanged;
        public event ModelEvent OnProcessDamage;

        public Stage(GameModel parent, Enemy[] enemies) : base(parent)
        {
            this.enemies = enemies;
            BindEvents();
        }

        public Stage(GameModel parent, int id) : base(parent) {
            var info = CsvLoader.TryToLoad("Configs/stages", id);
            if (info == null) return;
            enemies = (info["enemies"] as string)!.Split(";").Select((s => new Enemy(parent, int.Parse(s)) )).ToArray();
            bonusGears = (info["bonus_gears"] as string)!.Split(";").Select((s => new Gear(parent, int.Parse(s)) )).ToArray();
            BindEvents();
        }

        private void ForwardCurrentEnemy(Game game, GameModel deadEnemy) {
            _enemyIndex++;
            if (_enemyIndex == enemies.Length) {
                GameManager.shared.Delayed(1, () => {
                    OnStageBeaten?.Invoke(currentGame, this);
                });
            }
            else {
                CurrentEnemy.BecomeCurrent();
                OnEnemyChanged?.Invoke(currentGame, this);
            }
        }

        private void BindEvents() {
            foreach (var enemy in enemies) {
                enemy.OnDie += ForwardCurrentEnemy;
            }
            CurrentEnemy.BecomeCurrent();
        }

        public void ProcessDamage(Damage dmg){
            dmg.target.TakeDamage(dmg);
            OnProcessDamage?.Invoke(currentGame, dmg);
            currentGame.SwitchTurn();
        }
    }
}