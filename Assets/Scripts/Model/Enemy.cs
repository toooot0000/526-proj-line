using System;
using System.Collections.Generic;
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
        [Description("sp")] SpecialAttack,
        [Description("sp1")] Sp1,
        [Description("sp2")] Sp2,
        [Description("sp3")] Sp3
    }

    [Serializable]
    public sealed class Enemy : Damageable{
        public int attack = 1;
        public int id;
        public string name;
        public string desc;
        public EnemyIntention[] intentions;
        public int defend;
        public string imgPath;
        private int _nextActionInd;
        public SpecialAttackBase special;
        public SpecialAttackBase[] sps;
        public readonly List<Buff.Buff> buffs = new();

        public Enemy(GameModel parent) : base(parent){
            CurrentHp = HpUpLimit;
        }

        public bool IsDead => CurrentHp <= 0;

        private static readonly string[] SpHeaders = new string[3]{ "sp1", "sp2", "sp3" };
        private const int SpLen = 3;

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

            special = SpecialAttackBase.MakeSpecialAttack(enemy["special"] as string);
            sps = new SpecialAttackBase[SpLen];
            for (var i = 0; i < SpLen; i++){
                sps[i] = SpecialAttackBase.MakeSpecialAttack(enemy[SpHeaders[i]] as string);
            }
            
            defend = (int)enemy["defend"];
            CurrentHp = HpUpLimit;
        }
        
        

        public EnemyIntention CurrentIntention => intentions[_nextActionInd];
        public sealed override int HpUpLimit{ set; get; }
        public event ModelEvent OnIntentionChanged;

        private Damage GetDamage(){
            return new Damage(currentGame, Damage.Type.Physics, attack, currentGame.player){
                source = this
            };
        }

        private StageActionEnemyAttack GetEnemyAttackInfo(){
            return new StageActionEnemyAttack(this){
                damage = GetDamage()
            };
        }

        private StageActionEnemyDefend GetEnemyDefendInfo(){
            return new StageActionEnemyDefend(this){
                defend = defend
            };
        }

        private StageActionEnemySpecial GetEnemySpecialInfo(SpecialAttackBase sp){
            var ret = new StageActionEnemySpecial(this){
                damage = Damage.Default(currentGame.player),
                special = sp
            };
            ret.damage.source = this;
            return ret;
        }

        private void ForwardIntention(){
            _nextActionInd = (_nextActionInd + 1) % intentions.Length;
            OnIntentionChanged?.Invoke(currentGame, this);
        }

        public StageActionBase GetCurrentStageAction(){
            StageActionBase ret =  intentions[_nextActionInd] switch{
                EnemyIntention.Attack => GetEnemyAttackInfo(),
                EnemyIntention.Defend => GetEnemyDefendInfo(),
                EnemyIntention.SpecialAttack => GetEnemySpecialInfo(special),
                EnemyIntention.Sp1 => GetEnemySpecialInfo(sps[0]),
                EnemyIntention.Sp2 => GetEnemySpecialInfo(sps[1]),
                EnemyIntention.Sp3 => GetEnemySpecialInfo(sps[2]),
                _ => null
            };
            ForwardIntention();
            return ret;
        }
        
        
        public Sprite GetSprite(){
            return Resources.Load<Sprite>(imgPath);
        }
    }
}