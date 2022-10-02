using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Model.EnemySpecialAttacks;
using UnityEngine;
using Utility;
using Utility.Loader;
using Object = UnityEngine.Object;

namespace Model{

    public enum EnemyIntention{
        [Description("att")]
        Attack,
        [Description("def")]
        Defend,
        [Description("sp")]
        SpecialAttack
    }
    
    [Serializable]
    public class Enemy: GameModel, IDamageable{
        public int HpUpLimit{ set; get; }
        
        private int _currentHp;
        public int CurrentHp {
            set {
                _currentHp = value;
                if (value <= 0) Die();
            }
            get => _currentHp;
        }
        public int attack = 1;

        public int id;

        public String desc;

        public SpecialAttackBase special;

        public int cooldown;

        private int _nextActionInd = 0;
        public EnemyIntention[] intentions;
        public EnemyIntention CurrentIntention => intentions[_nextActionInd];

        private int _armor;

        public int Armor{
            set{
                _armor = value;
                OnArmorChanged?.Invoke(currentGame, this);
            }
            get => _armor;
        }

        public int defend;
        
        public event ModelEvent OnAttack;
        public event ModelEvent OnDefend;
        public event ModelEvent OnSpecial;
        public event ModelEvent OnBeingAttacked;
        public event ModelEvent OnDie;
        public event ModelEvent OnArmorChanged;
        public event ModelEvent OnIntentionChanged;

        public Enemy(GameModel parent) : base(parent){
            CurrentHp = HpUpLimit;
        }

        public Enemy(GameModel parent, int id) : base(parent) {
            this.id = id;
            var enemy = CsvLoader.TryToLoad("Configs/enemies", id);
            if (enemy == null) return;
            desc = enemy["desc"] as string;
            cooldown = (int)enemy["cooldown"];
            HpUpLimit = (int)enemy["hp"];
            attack = (int)enemy["attack"];
            intentions = (enemy["action_pattern"] as string)!.Split(";").Select(s => EnumUtility.GetValue<EnemyIntention>(s)).ToArray();
            Debug.Log(intentions.Length.ToString());
            
            var spStr = (enemy["special"] as string)!.Split(";");
            var className = spStr.First();
            special = Activator.CreateInstance(Type.GetType($"Model.EnemySpecialAttacks.{className}", true), new object[]{spStr[1..]}) as
                    SpecialAttackBase;
            defend = (int)enemy["defend"];
            CurrentHp = HpUpLimit;
        }
        
        public void TakeDamage(Damage damage){
            CurrentHp -= damage.point;
            OnBeingAttacked?.Invoke(currentGame, this);
        }
        
        [Obsolete("Use *WithInfo")]
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

        public void AttackWithInfo(){
            var dmg = new Damage(currentGame){
                point = attack,
                type = Damage.Type.Physics,
                target = currentGame.player,
                source = this
            };
            var stageInfo = new StageActionInfoEnemyAttack(this){
                damage = dmg
            };
            OnAttack?.Invoke(currentGame, this);
            currentGame.currentStage.ProcessStageAction(stageInfo);
        }

        public void SpecialAttack(){
            var info = new StageActionInfoEnemySpecial(this){
                special = special
            };
            OnSpecial?.Invoke(currentGame, this);
            currentGame.currentStage.ProcessStageAction(info);
        }

        public void Defend(){
            Armor += defend;
            var info = new StageActionInfoEnemyDefend(this){
                defend = defend
            };
            OnDefend?.Invoke(currentGame, this);
            currentGame.currentStage.ProcessStageAction(info);
        }

        private void DoAction(Game game) {
            if (game.turn != Game.Turn.Enemy) return;
            GameManager.shared.Delayed(0.1f, IntentionToAction());
            ForwardIntention();
        }

        private void ForwardIntention(){
            _nextActionInd = (_nextActionInd + 1) % intentions.Length;
            OnIntentionChanged?.Invoke(currentGame, this);
        }

        private Action IntentionToAction(){
            Action nextAction = null;
            switch (intentions[_nextActionInd]){
                case EnemyIntention.Attack:
                    nextAction = AttackWithInfo;
                    break;
                case EnemyIntention.Defend:
                    nextAction = Defend;
                    break;
                case EnemyIntention.SpecialAttack:
                    nextAction = SpecialAttack;
                    break;
            }
            return nextAction;
        }

        public void Die() {
            currentGame.OnTurnChanged -= DoAction;
            OnDie?.Invoke(currentGame, this);
        }

        public void BecomeCurrent() {
            currentGame.OnTurnChanged += DoAction;
        }
    }
}