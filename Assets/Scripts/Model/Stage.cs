using System;
using System.Linq;
using Utility;
using Utility.Loader;

namespace Model{
    public class Stage : GameModel{
        public readonly Gear[] bonusGears;

        public readonly Enemy[] enemies;

        private int _enemyIndex;

        public int bonusCoins = -1;

        public int id;


        public int nextStage;

        public Stage(GameModel parent, Enemy[] enemies) : base(parent){
            this.enemies = enemies;
            // BindEvents();
        }

        public Stage(GameModel parent, int id) : base(parent){
            var info = CsvLoader.TryToLoad("Configs/stages", id);
            if (info == null) return;
            id = (int)info["id"];
            nextStage = (int)info["next_stage"];
            enemies = (info["enemies"] as string)!.Split(";").Select(s => new Enemy(parent, IntUtility.ParseString(s)))
                .ToArray();
            if (((string)info["bonus_gears"]).Length != 0)
                bonusGears = ((string)info["bonus_gears"])!.Split(";")
                    .Select(s => new Gear(parent, IntUtility.ParseString(s))).ToArray();
            bonusCoins = (int)info["bonus_coins"];
            CurrentEnemy.BecomeCurrent();
        }

        public Enemy CurrentEnemy => _enemyIndex < enemies.Length ? enemies[_enemyIndex] : null;

        public Enemy NextEnemy => _enemyIndex >= enemies.Length - 1 ? null : enemies[_enemyIndex + 1];

        public bool IsBeaten => _enemyIndex == enemies.Length;

        public int RemainingEnemyNumber => enemies.Length - _enemyIndex;

        public event ModelEvent OnStageBeaten;
        public event ModelEvent OnEnemyChanged;

        [Obsolete("Use OnProcessStageAction")]
        public event ModelEvent OnProcessDamage;

        /// <summary>
        ///     model = StageActionInfo
        /// </summary>
        public event ModelEvent OnProcessStageAction;

        private void ForwardCurrentEnemy(Game game, GameModel deadEnemy){
            _enemyIndex++;
            if (_enemyIndex == enemies.Length){
                GameManager.shared.Delayed(1, () => { OnStageBeaten?.Invoke(currentGame, this); });
            } else{
                CurrentEnemy.BecomeCurrent();
                OnEnemyChanged?.Invoke(currentGame, this);
            }
        }

        public void ForwardCurrentEnemy(){
            _enemyIndex++;
            if (_enemyIndex == enemies.Length){
                GameManager.shared.Delayed(1, () => { OnStageBeaten?.Invoke(currentGame, this); });
            } else{
                CurrentEnemy.BecomeCurrent();
                OnEnemyChanged?.Invoke(currentGame, this);
            }
        }


        public void ProcessStageAction(StageActionInfoBase info){
            info.Execute();
            OnProcessStageAction?.Invoke(currentGame, info);
        }
    }
}