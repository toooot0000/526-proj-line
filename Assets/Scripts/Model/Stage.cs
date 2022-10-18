using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Utility;
using Utility.Loader;

namespace Model{

    public enum StageType{
        [Description("battle")]
        Battle,
        [Description("shop")]
        Shop,
        [Description("event")]
        Event
    }


    public class Stage : GameModel{

        public int id;

        public Enemy[] enemies = new Enemy[]{ };

        public Gear[] bonusGears = new Gear[]{ };

        private int _enemyIndex = 0;
        public Enemy CurrentEnemy => _enemyIndex < enemies.Length ? enemies[_enemyIndex] : null;
        public Event CurrentEvent{ get; set; } = null;
        public Enemy NextEnemy => _enemyIndex >= enemies.Length - 1 ? null : enemies[_enemyIndex + 1];
        public bool IsBeaten => _enemyIndex == enemies.Length;
        public int RemainingEnemyNumber => enemies.Length - _enemyIndex;
        public bool IsLast => nextStageChoice.Length == 0 || nextStageChoice[0] == -1;
        public StageType type = StageType.Battle;

        public event ModelEvent OnStageBeaten;
        public event ModelEvent OnEnemyChanged;
        [Obsolete("Use OnProcessStageAction")]
        public event ModelEvent OnProcessDamage;
        /// <summary>
        /// model = StageActionInfo
        /// </summary>
        public event ModelEvent OnProcessStageAction;

        public event ModelEvent OnNewConfigLoaded;
        
        [Obsolete("Use Next StageChoice")]
        public int nextStage = 0;
        public String desc;
        public int bonusCoins = -1;
        public int[] nextStageChoice;
        //public int[] nextStageChoiceInt;

        public Stage(GameModel parent) : base(parent){ } 

        public Stage(GameModel parent, Enemy[] enemies) : base(parent){
            this.enemies = enemies;
        }

        public Stage(GameModel parent, int id) : base(parent) {
            LoadFromConfig(id);
        }

        public void ForwardCurrentEnemy(){
            _enemyIndex++;
            if (_enemyIndex == enemies.Length){
                _enemyIndex = 0;
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

        public void LoadFromConfig(int stageId){
            var info = CsvLoader.TryToLoad("Configs/stages", stageId);
            if (info == null) return;
            id = stageId;
            desc = info["desc"] as string;

            var nextStageStr = (string)info["next_stage_choices"];
            if (!string.IsNullOrEmpty(nextStageStr)){
                nextStageChoice = nextStageStr.Split(";").Select(IntUtility.ParseString).ToArray();
            }
            type = EnumUtility.GetValue<StageType>(info["type"] as string);

            switch (type){
                case StageType.Battle:
                    enemies = (info["enemies"] as string)!.Split(";").Select((s => new Enemy(parent, IntUtility.ParseString(s)) )).ToArray();
                    if (((string)info["bonus_gears"]).Length != 0){
                        bonusGears = ((string)info["bonus_gears"])!.Split(";").Select((s => new Gear(parent, IntUtility.ParseString(s)) )).ToArray();
                    }
                    bonusCoins = (int)info["bonus_coins"];
                    CurrentEnemy.BecomeCurrent();
                    OnEnemyChanged?.Invoke(currentGame, this);
                    break;
                case StageType.Shop:
                    break;
                case StageType.Event:
                    var eventId = (int)info["event_id"]; 
                    CurrentEvent = new Event(this, eventId);
                    break;
            }
            
            
            OnNewConfigLoaded?.Invoke(currentGame, this);
        }
    }
}