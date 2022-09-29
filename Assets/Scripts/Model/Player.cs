﻿using System;
using System.Collections.Generic;
using System.Linq;

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
        public float armor;

        public List<Ball> hitBalls = new();
        public List<Ball> circledBalls = new();

        public Gear CurrentGear => gears.Count > 0? gears[^1] : null;

        public event ModelEvent OnHitBall;
        public event ModelEvent OnCircledBall;
        public event ModelEvent OnAttack;
        public event ModelEvent OnBeingAttacked;
        public event ModelEvent OnDie;
        public event ModelEvent OnGearChanged;
        public event ModelEvent OnInit;
        

        public Player(GameModel parent) : base(parent){
            Init();
        }

        public void Init(){
            HpUpLimit = 100;        // TODO
            CurrentHp = HpUpLimit;
            gears = new List<Gear>(){
                new Gear(this, id: -1)
            };
            OnGearChanged?.Invoke(currentGame, this);
            OnInit?.Invoke(currentGame, this);
        }

        
        public void TakeDamage(Damage damage){
            CurrentHp -= damage.point;
            OnBeingAttacked?.Invoke(currentGame, this);
        }

        
        public void AddHitBall(Ball ball){
            hitBalls.Add(ball);
            OnHitBall?.Invoke(currentGame, ball);
        }

        public void AddCircledBall(Ball ball){
            circledBalls.Add(ball);
            OnCircledBall?.Invoke(currentGame, ball);
        }

        public int GetTotalPoint(){
            return hitBalls.Sum(ball => ball.point) + circledBalls.Sum(ball => ball.point * 2);
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
            currentGame.currentStage.ProcessDamage(dmg);
        }
        
        private void Die() {
            OnDie?.Invoke(currentGame, this);
            currentGame.End();
        }

        public float ChargeEffect()
        {
            float points = 0;
            if (circledBalls.Count == 0) {
                return points;
            }
            else {
                Dictionary<Ball, int> res = new Dictionary<Ball, int>();
                for (int i = 0; i < circledBalls.Count; i++) {
                    if (res.ContainsKey(circledBalls[i])) {
                        res[circledBalls[i]] += 1;
                    }
                    else {
                        res.Add(circledBalls[i],1);
                    }
                }

                foreach (Ball circled in res.Keys) {
                    points += circled.charge * circled.point * res[circled];
                }
            }
            return points;
        }
        
        public float ComboEffect(){
            float points = 0;
            if(hitBalls.Count == 0) {
                return points;
            }
            else {
                Dictionary<Ball, int> record = new Dictionary<Ball, int>();
                for( int j = 0; j < hitBalls.Count; j++) {
                    if(record.ContainsKey(hitBalls[j])){
                        record[hitBalls[j]] += 1;
                    }else{
                        record.Add(hitBalls[j], 1);
                    }
                }
                foreach(Ball hitBall in record.Keys){
                    if(record[hitBall] >= 2){
                        if (hitBall.type != Ball.Type.Defend) {
                            points += hitBall.combo * hitBall.point * record[hitBall];
                        }
                        else {
                            points += hitBall.combo * hitBall.point * record[hitBall];
                            armor += points;
                        }
                    }
                }
                return points;
            }

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