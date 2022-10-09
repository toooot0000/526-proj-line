using System;
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
        public Enemy NextEnemy => _enemyIndex >= enemies.Length - 1 ? null : enemies[_enemyIndex + 1];
        public bool IsBeaten => _enemyIndex == enemies.Length;
        public int RemainingEnemyNumber => enemies.Length - _enemyIndex;
        public bool IsLast => nextStageChoice.Length == 0 || nextStageChoice[0] == -1;

        public event ModelEvent OnStageBeaten;
        public event ModelEvent OnEnemyChanged;
        [Obsolete("Use OnProcessStageAction")]
        public event ModelEvent OnProcessDamage;
        /// <summary>
        /// model = StageActionInfo
        /// </summary>
        public event ModelEvent OnProcessStageAction;
        
        [Obsolete("Use Next StageChoice")]
        public readonly int nextStage = 0;
        public String desc;
        public readonly int bonusCoins = -1;
        public readonly int[] nextStageChoice;
        //public int[] nextStageChoiceInt;
        public Stage(GameModel parent, Enemy[] enemies) : base(parent)
        {
            this.enemies = enemies;
            // BindEvents();
        }

        public Stage(GameModel parent, int id) : base(parent) {
            var info = CsvLoader.TryToLoad("Configs/stages", id);
            if (info == null) return;
            id = (int)info["id"];
            desc = info["desc"] as string;
            nextStage = (int)info["next_stage"];

            var nextStageStr = (string)info["next_stage_choices"];
            if (!string.IsNullOrEmpty(nextStageStr)){
                nextStageChoice = nextStageStr.Split(";").Select(IntUtility.ParseString).ToArray();
            }
            enemies = (info["enemies"] as string)!.Split(";").Select((s => new Enemy(parent, IntUtility.ParseString(s)) )).ToArray();
            if (((string)info["bonus_gears"]).Length != 0){
                bonusGears = ((string)info["bonus_gears"])!.Split(";").Select((s => new Gear(parent, IntUtility.ParseString(s)) )).ToArray();
            }
            bonusCoins = (int)info["bonus_coins"];
            CurrentEnemy.BecomeCurrent();
        }

        private void ForwardCurrentEnemy(Game game, GameModel deadEnemy) {
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

        public void TestOnStageBeaten()
        {
            OnStageBeaten?.Invoke(currentGame,this);
        }
    }
}