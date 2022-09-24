using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using Utility.Loader;

namespace Core.Model{
    public class Stage: GameModel{

        public readonly Enemy[] enemies;

        public readonly Gear[] bonusGears;

        private int _enemyIndex = 0;

        public Enemy CurrentEnemy => enemies[_enemyIndex];

        public event ModelEvent OnStageBeaten;

        public Stage(GameModel parent, Enemy[] enemies) : base(parent)
        {
            this.enemies = enemies;
            foreach (var enemy in enemies) {
                enemy.OnDie += (game, model) => {
                    _enemyIndex++;
                    if (_enemyIndex == enemies.Length) {
                        GameManager.shared.Delayed(1, () => {
                            OnStageBeaten?.Invoke(currentGame, this);
                        });
                    }
                };
            }
        }

        public Stage(GameModel parent, int id) : base(parent) {
            var info = CsvLoader.TryToLoad("Config/stages", id);
            if (info == null) return;
            enemies = (info["enemies"] as string)!.Split(";").Select((s => new Enemy(parent, int.Parse(s)) )).ToArray();
            bonusGears = (info["bonus_gears"] as string)!.Split(";").Select((s => new Gear(parent, int.Parse(s)) )).ToArray();
        }
    }
}