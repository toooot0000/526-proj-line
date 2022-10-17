using System;
using System.Collections.Generic;
using System.Linq;
using BackendApi;
using Model.GearEffects;
using UI;
using Utility.Loader;
using UnityEngine;
using Utility;
using Utility.Extensions;

namespace Model{
    [Serializable]
    public class Player : GameModel, IDamageable{
        public List<Gear> gears;
        public int gearUpLimit;
        public int energy;

        public readonly List<Ball> hitBalls = new();
        public readonly List<Ball> circledBalls = new();
        public readonly List<Ball> hitDebuffBalls = new();
        public readonly List<Ball> activeDebuffBalls = new();
        
        private int _armor;

        private int _coin;

        private int _currentHp;


        public Player(GameModel parent) : base(parent){
            Init();
        }

        public int Coin{
            set{
                _coin = Math.Max(value, 0);
                OnCoinChanged?.Invoke(currentGame, this);
            }
            get => _coin;
        }

        [Obsolete("Use CurrentGears!")]
        public Gear CurrentGear => gears.Count > 0 ? gears[^1] : null;

        public Gear[] CurrentGears => gears.ToArray();

        public int HpUpLimit{ set; get; }

        public int CurrentHp{
            set{
                _currentHp = Math.Clamp(value, 0, HpUpLimit);
                if (value == 0) Die();
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
            Armor -= damage.totalPoint;
            OnBeingAttacked?.Invoke(currentGame, this);
        }

        public event ModelEvent OnHitBall;
        public event ModelEvent OnCircledBall;
        public event ModelEvent OnAttack;
        public event ModelEvent OnBeingAttacked;
        public event ModelEvent OnDie;
        public event ModelEvent OnGearChanged;
        public event ModelEvent OnInit;
        public event ModelEvent OnCoinChanged;
        public event ModelEvent OnArmorChanged;

        public event ModelEvent OnStageChanged;
        public void Init(){
            HpUpLimit = (int)CsvLoader.GetConfig("player_init_hp");
            Coin = (int)CsvLoader.GetConfig("player_init_coin");
            CurrentHp = HpUpLimit;
            gears = ((string)CsvLoader.GetConfig("player_init_gears")).ParseAsParams(IntUtility.ParseString).Select(i => new Gear(this, i)).ToList();
            OnGearChanged?.Invoke(currentGame, this);
            OnInit?.Invoke(currentGame, this);
        }


        public void AddHitBall(Ball ball){
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
            return new Damage(currentGame){
                totalPoint = GetTotalAttackPoint(),
                type = Damage.Type.Physics,
                target = currentGame.CurrentEnemy,
                source = this
            };
        }

        public StageActionInfoPlayerAttack GetAttackActionInfo(){
            return new StageActionInfoPlayerAttack(this, GetTriggeredEffects()){
                damage = GetDamage(),
                defend = GetTotalDefendPoint(),
                hitBalls = hitBalls.ToArray(),
                circledBalls = circledBalls.ToArray()
            };
        }

        private void Die(){
            OnDie?.Invoke(currentGame, this);
            currentGame.End();
        }
        public void AddGear(Gear gear){
            gear.parent = this;
            gears.Add(gear);
            OnGearChanged?.Invoke(currentGame, this);
        }
        
        public void RemoveGear(int id){
            var ind = gears.FindIndex(g => g.id == id);
            if (ind == -1) return;
            gears.RemoveAt(ind);
            OnGearChanged?.Invoke(currentGame, this);
        }
        
        
        //special event system related functions
        public void GetLife(int value)
        {
            int beforeHp = CurrentHp;
            CurrentHp += value;
            
            if (CurrentHp > HpUpLimit)
            {
                CurrentHp = HpUpLimit;
            }
            else if (CurrentHp < 0)
            {
                Debug.Log("Player Died");
                Die();
            }
            Debug.Log("Before:"+beforeHp.ToString()+ "After:" + CurrentHp.ToString());
        }

        public void GetCoin(int value)
        {
            Coin += value;
            
            if(Coin < 0)
            {
                Coin = 0;
            }
            Debug.Log("Coin:"+Coin.ToString());
        }
        
        
        public void ClearAllBalls(){
            hitBalls.Clear();
            circledBalls.Clear();
        }

        public void GainDebuffBall(Ball ball){
            activeDebuffBalls.Add(ball);
        }
    }
}