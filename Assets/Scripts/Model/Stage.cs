using System.Linq;
using Utility;
using Utility.Loader;

namespace Model{
    public class Stage: GameModel{

        public int id;

        public readonly Enemy[] enemies;

        public readonly Gear[] bonusGears;

        private int _enemyIndex = 0;

        public Enemy CurrentEnemy => _enemyIndex < enemies.Length ? enemies[_enemyIndex] : null;

        public int RemainingEnemyNumber => enemies.Length - _enemyIndex;

        public event ModelEvent OnStageBeaten;
        public event ModelEvent OnEnemyChanged;
        public event ModelEvent OnProcessDamage;

        public int nextStage = 0;

        public Stage(GameModel parent, Enemy[] enemies) : base(parent)
        {
            this.enemies = enemies;
            BindEvents();
        }

        public Stage(GameModel parent, int id) : base(parent) {
            var info = CsvLoader.TryToLoad("Configs/stages", id);
            if (info == null) return;
            id = (int)info["id"];
            nextStage = (int)info["next_stage"];
            enemies = (info["enemies"] as string)!.Split(";").Select((s => new Enemy(parent, IntUtility.ParseString(s)) )).ToArray();
            bonusGears = (info["bonus_gears"] as string)!.Split(";").Select((s => new Gear(parent, IntUtility.ParseString(s)) )).ToArray();
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

        private void BindEvents(){
            foreach (var enemy in enemies) {
                enemy.OnDie += ForwardCurrentEnemy;
            }
            CurrentEnemy.BecomeCurrent();
        }

        public void ProcessDamage(Damage dmg){
            dmg.target.TakeDamage(dmg);
            OnProcessDamage?.Invoke(currentGame, dmg);
        }
    }
}