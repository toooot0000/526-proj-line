using System;
using System.Collections.Generic;
using System.Linq;
using BackendApi;
using Unity.Burst.Intrinsics;
using Utility.Loader;

namespace Model{
    [Serializable]
    public class Player: GameModel, IDamageable{

        public int HpUpLimit{ set; get; }

        private int _currentHp = 0;
        public int CurrentHp
        {
            set {
                _currentHp = value;
                if (value == 0) Die();
            }
            get => _currentHp;
        }

        public List<Gear> gears;
        public int gearUpLimit;
        public int energy;
        private int _armor = 0;

        public int Armor{
            set{
                _armor = value;
                OnArmorChanged?.Invoke(currentGame, this);
            }
            get => _armor;
        }

        private int _coin = 0;

        public int Coin{
            set{
                _coin = value;
                OnCoinChanged?.Invoke(currentGame, this);
            }
            get => _coin;
        }

        public List<Ball> hitBalls = new();
        public List<Ball> circledBalls = new();

        [Obsolete("Use CurrentGears!")]
        public Gear CurrentGear => gears.Count > 0? gears[^1] : null;

        public Gear[] CurrentGears => gears.ToArray();

        public event ModelEvent OnHitBall;
        public event ModelEvent OnCircledBall;
        public event ModelEvent OnAttack;
        public event ModelEvent OnBeingAttacked;
        public event ModelEvent OnDie;
        public event ModelEvent OnGearChanged;
        public event ModelEvent OnInit;
        public event ModelEvent OnCoinChanged;
        public event ModelEvent OnArmorChanged;

        public Player(GameModel parent) : base(parent){
            Init();
        }

        public void Init(){
            // HpUpLimit = 100;        // TODO

            HpUpLimit = (int)CsvLoader.GetConfig("player_init_hp");
            Coin = (int)CsvLoader.GetConfig("player_init_coin");
            CurrentHp = HpUpLimit;
            gears = new List<Gear>(){
                new Gear(this, id: -1)
            };
            OnGearChanged?.Invoke(currentGame, this);
            OnInit?.Invoke(currentGame, this);
        }

        
        public void TakeDamage(Damage damage){
            CurrentHp -= Math.Max(damage.point - Armor, 0);
            Armor -= damage.point;
            OnBeingAttacked?.Invoke(currentGame, this);
        }

        
        public void AddHitBall(Ball ball){
            hitBalls.Add(ball);
            OnHitBall?.Invoke(currentGame, ball);
            // Send Event
            var skillEvent = new EventSkillUses(){
                skillId = ball.id,
                uses = 1
            };
            EventLogger.Shared.Log(skillEvent);
        }

        public void AddCircledBall(Ball ball){
            circledBalls.Add(ball);
            OnCircledBall?.Invoke(currentGame, ball);
            // Send Event
            var skillEvent = new EventSkillUses(){
                skillId = ball.id,
                uses = 1
            };
            EventLogger.Shared.Log(skillEvent);
        }

        public int GetTotalPoint(){
            return hitBalls.Sum(ball => ball.point) + circledBalls.Sum(ball => ball.point * 2);
        }

        public int GetTotalDefend(){
            return hitBalls.Sum(ball => ball.type == Ball.Type.Defend ? ball.point : 0) +
                   circledBalls.Sum(ball => ball.type == Ball.Type.Defend ? ball.point * 2 : 0);
        }
        
        public void Attack() {
            if (currentGame.turn != Game.Turn.Player) return;
            var dmg = new Damage(currentGame){
                point = GetTotalPoint(),
                type = Damage.Type.Physics,
                target = currentGame.CurrentEnemy,
                source = this
            };
            OnAttack?.Invoke(currentGame, this);
            hitBalls.Clear();
            circledBalls.Clear();
            var playerAttackAction = new StageActionInfoPlayerAttack(this){
                damage = dmg,
                defend = GetTotalDefend()
            };
            currentGame.currentStage.ProcessStageAction(playerAttackAction);
        }
        
        private void Die() {
            OnDie?.Invoke(currentGame, this);
            currentGame.End();
        }

        public void AddGear(Gear gear){
            gears.Add(gear);
            OnGearChanged?.Invoke(currentGame, this);
        }

        public void RemoveGear(int id){
            var ind = gears.FindIndex(g => g.id == id);
            if (ind == -1) return;
            gears.RemoveAt(ind);
            OnGearChanged?.Invoke(currentGame, this);
        }
    }
}