using System;
using System.ComponentModel;
using System.Linq;
using Model.EnemySpecialAttacks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Utility;
using Utility.Loader;

namespace Model{
    public enum EnemyIntention{
        [Description("att")] Attack,
        [Description("def")] Defend,
        [Description("sp")] SpecialAttack
    }

    [Serializable]
    public class Enemy : GameModel, IDamageable{
        public int attack = 1;
        public int id;
        public string name;
        public string desc;
        public EnemyIntention[] intentions;
        public int defend;
        public string imgPath;
        private int _armor;
        private int _currentHp;
        private int _nextActionInd;
        public SpecialAttackBase special;

        public Enemy(GameModel parent) : base(parent){
            CurrentHp = HpUpLimit;
        }

        public Enemy(GameModel parent, int id) : base(parent){
            this.id = id;
            var enemy = CsvLoader.TryToLoad("Configs/enemies", id);
            if (enemy == null) return;
            name = enemy["name"] as string;
            desc = enemy["desc"] as string;
            HpUpLimit = (int)enemy["hp"];
            attack = (int)enemy["attack"];
            imgPath = enemy["img_path"] as string;
            intentions = (enemy["action_pattern"] as string)!.Split(";")
                .Select(s => EnumUtility.GetValue<EnemyIntention>(s)).ToArray();
            var spStr = (enemy["special"] as string)!.Split(";");
            var className = spStr.First();
            if(className != "")
                special = Activator.CreateInstance(Type.GetType($"Model.EnemySpecialAttacks.{className}", true),
                    new object[]{ spStr[1..] }) as
                SpecialAttackBase;
            defend = (int)enemy["defend"];
            CurrentHp = HpUpLimit;
        }

        public EnemyIntention CurrentIntention => intentions[_nextActionInd];
        public int HpUpLimit{ set; get; }

        public int CurrentHp{
            set{
                _currentHp = value;
                if (value <= 0) Die();
            }
            get => _currentHp;
        }

        public int Armor{
            set{
                _armor = Math.Max(value, 0);
                OnArmorChanged?.Invoke(currentGame, this);
            }
            get => _armor;
        }

        public void TakeDamage(Damage damage){
            CurrentHp -= Math.Max(damage.totalPoint - Armor, 0);
            Armor = Math.Max(Armor - damage.totalPoint, 0);
            OnBeingAttacked?.Invoke(currentGame, this);
        }

        public event ModelEvent OnAttack;
        public event ModelEvent OnDefend;
        public event ModelEvent OnSpecial;
        public event ModelEvent OnBeingAttacked;
        public event ModelEvent OnDie;
        public event ModelEvent OnArmorChanged;
        public event ModelEvent OnIntentionChanged;

        private Damage GetDamage(){
            return new Damage(currentGame){
                totalPoint = attack,
                type = Damage.Type.Physics,
                target = currentGame.player,
                source = this
            };
        }

        private StageActionInfoEnemyAttack GetEnemyAttackInfo(){
            return new StageActionInfoEnemyAttack(this){
                damage = GetDamage()
            };
        }

        private StageActionInfoEnemyDefend GetEnemyDefendInfo(){
            return new StageActionInfoEnemyDefend(this){
                defend = defend
            };
        }

        private StageActionInfoEnemySpecial GetEnemySpecialInfo(){
            return new StageActionInfoEnemySpecial(this){
                special = special
            };
        }

        private void ForwardIntention(){
            _nextActionInd = (_nextActionInd + 1) % intentions.Length;
            OnIntentionChanged?.Invoke(currentGame, this);
        }

        public StageActionInfoBase GetCurrentStageAction(){
            StageActionInfoBase ret =  intentions[_nextActionInd] switch{
                EnemyIntention.Attack => GetEnemyAttackInfo(),
                EnemyIntention.Defend => GetEnemyDefendInfo(),
                EnemyIntention.SpecialAttack => GetEnemySpecialInfo(),
                _ => null
            };
            ForwardIntention();
            return ret;
        }

        public void Die(){
            // currentGame.OnTurnChanged -= DoAction;
            OnDie?.Invoke(currentGame, this);
        }

        public void BecomeCurrent(){
            // currentGame.OnTurnChanged += DoAction;
        }
        
        public Sprite GetSprite(){
            return Resources.Load<Sprite>(imgPath);
        }
    }
}