using System;
using System.Collections.Generic;
using System.Linq;
using BackendApi;
using Model.Buff;
using Model.GearEffects;
using Model.Mechanics.PlayableObjects;
using UI;
using Utility.Loader;
using UnityEngine;
using Utility;
using Utility.Extensions;

namespace Model{
    [Serializable]
    public class Player : Damageable{
        public List<Gear> gears;
        public int gearUpLimit;
        public int energy;
        public readonly List<Ball> hitBalls = new();
        public readonly List<Ball> circledBalls = new();
        public readonly List<Ball> hitDebuffBalls = new();
        public readonly List<Buff.Buff> buffs = new();
        // public readonly List<DebuffBall.DebuffBall> debuffBalls = new(); 
        
        private int _coin;
        public int Coin{
            set{
                _coin = Math.Max(value, 0);
                OnCoinChanged?.Invoke(currentGame, this);
            }
            get => _coin;
        }

        public Gear[] CurrentGears => gears.ToArray();

        public event ModelEvent OnHitBall;
        public event ModelEvent OnCircledBall;
        public event ModelEvent OnGearChanged;
        public event ModelEvent OnGearAdded;
        public event ModelEvent OnInit;
        public event ModelEvent OnCoinChanged;
        
        public Player(GameModel parent) : base(parent){
            Init();
        }
        
        public void Init(){
            HpUpLimit = (int)CsvLoader.GetConfig("player_init_hp");
            Coin = (int)CsvLoader.GetConfig("player_init_coin");
            CurrentHp = HpUpLimit;
            gears = ((string)CsvLoader.GetConfig("player_init_gears")).ParseAsParams(IntUtility.ParseString).Select(i => new Gear(this, i)).ToList();
            OnGearChanged?.Invoke(currentGame, this);
            OnInit?.Invoke(currentGame, this);
        }


        public void AddSlicedBall(Ball ball){
            if (ball.type == BallType.Debuff){
                hitDebuffBalls.Add(ball);
            } else{
                hitBalls.Add(ball);
            }
            OnHitBall?.Invoke(currentGame, ball);
        }

        public void AddCircledBall(Ball ball){
            if (ball.type == BallType.Debuff){
                hitDebuffBalls.Add(ball);
            } else{
                circledBalls.Add(ball);
            }
            OnCircledBall?.Invoke(currentGame, ball);
        }

        public int GetTotalAttackPoint(){
            return hitBalls.Sum(ball => ball.type != BallType.Defend ? ball.point : 0) +
                   circledBalls.Sum(ball => ball.type != BallType.Defend ? ball.point : 0);
        }

        public int GetTotalDefendPoint(){
            return hitBalls.Sum(ball => ball.type == BallType.Defend ? ball.point : 0) +
                   circledBalls.Sum(ball => ball.type == BallType.Defend ? ball.point : 0);
        }

        public GearEffectBase[] GetTriggeredEffects(){
            var ret = new List<GearEffectBase>();
            foreach (var gear in gears){
                if (gear.IsCharged()) ret.Add(gear.chargeEffect);

                if (gear.IsComboIng()) ret.Add(gear.comboEffect);
            }

            return ret.ToArray();
        }

        public Damage GetDamage(){
            return new Damage(currentGame, Damage.Type.Physics, GetTotalAttackPoint(), currentGame.CurrentEnemy){
                source = this
            };
        }

        public StageActionPlayerAction GetAction(){
            return new StageActionPlayerAction(
                this, 
                GetTriggeredEffects(),
                GetOnAttackBuffEffects()
                ){
                damage = GetDamage(),
                defend = GetTotalDefendPoint(),
                hitBalls = hitBalls.ToArray(),
                circledBalls = circledBalls.ToArray()
            };
        }

        public override void Die(){
            base.Die();
            currentGame.End();
        }
        
        public void AddGear(Gear gear){
            gear.parent = this;
            gears.Add(gear);
            OnGearAdded?.Invoke(currentGame, gear);
            OnGearChanged?.Invoke(currentGame, this);
        }
        
        public void RemoveGear(int id){
            var ind = gears.FindIndex(g => g.id == id);
            if (ind == -1) return;
            gears.RemoveAt(ind);
            OnGearChanged?.Invoke(currentGame, this);
        }


        public void ClearAllBalls(){
            hitBalls.Clear();
            circledBalls.Clear();
        }
        
        public Gear FindGearOfId(int id){
            return gears.Find(g => g.id == id);
        }
        
        private IBuffEffect<StageActionPlayerAction>[] GetOnAttackBuffEffects(){
            var triggers = Buff.Buff.GetBuffOfTriggerFrom<IBuffTriggerOnGetPlayerActionInfo>(this);
            return triggers?.Select(t => t.OnGetPlayerActionInfo()).ToArray();
        }
        
        public Ball[] GetAllBallsWithoutGridPosition(){
            var ret = new List<Ball>();
            ret.AddRange(GetAllGearBalls());
            // ret.AddRange(debuffBalls);
            return ret.ToArray();
        }

        public Ball[] GetAllGearBalls(){
            var ret = new List<Ball>();
            gears.ForEach(g => ret.AddRange(g.GetBalls()));
            return ret.ToArray();
        }
    }
}